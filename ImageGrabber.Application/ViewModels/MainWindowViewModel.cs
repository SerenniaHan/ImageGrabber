using System;
using System.ComponentModel;
using System.Drawing;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using Prism.Commands;
using Prism.Mvvm;
using ImageGrabber.Application.Models;
using ImageGrabber.Core.CameraModule;
using ImageGrabber.Wpf.Extensions;
using ImageGrabber.Wpf.Utility;


namespace ImageGrabber.Application.ViewModels;

// ReSharper disable once ClassNeverInstantiated.Global
// ReSharper disable once MemberCanBePrivate.Global
public class MainWindowViewModel : BindableBase
{
    #region Private Fields

    private readonly string _title = "Image Grabber Application";
    private readonly ICameraModel _model;
    private GrabbedImageItem _selectedImageItem;
    private readonly ManualResetEvent _signalToFlush;
    #endregion

    #region Properties

    /// <summary>
    /// Window Title
    /// </summary>
    public string Title => _model?.Camera is null ? _title : $"{_title}_[{_model.Camera.Name}]";

    /// <summary>
    /// camera resources
    /// </summary>
    public ObservableCollection<CombBoxData<ICamera>> CameraResources
    {
        get
        {
            if (_model.CameraResources == null) return null;

            return new ObservableCollection<CombBoxData<ICamera>>(
                from camera in _model.CameraResources
                select new CombBoxData<ICamera>(camera.Name, camera)
            );
        }
    }

    /// <summary>
    /// Selected Camera
    /// </summary>
    public ICamera Camera
    {
        get => _model?.Camera;
        set
        {
            ICamera camera = _model?.Camera;
            if (SetProperty(ref camera, value))
                _model.Camera = value;
        }
    }

    public bool OpenCommandCanBeExecute => !Camera?.IsOpen ?? false;
    public bool CloseCommandCanBeExecute => Camera?.IsOpen ?? false;
    public bool StartGrabCommandCanBeExecute => ((Camera?.IsOpen ?? false) && (!Camera?.IsGrabbing ?? false));
    public bool StopGrabCommandCanBeExecute => ((Camera?.IsOpen ?? false) && (Camera?.IsGrabbing ?? false));

    public bool IsSaveCommandCanBeExecute => SelectedImageItem != null;

    /// <summary>
    /// grabbed image list 
    /// </summary>
    public ObservableCollection<GrabbedImageItem> ImageLists { get; }

    /// <summary>
    /// selected image item
    /// </summary>
    public GrabbedImageItem SelectedImageItem
    {
        get => _selectedImageItem;
        set
        {

            if (_selectedImageItem != null) _selectedImageItem.IsSelectedToSave = false;

            if (SetProperty(ref _selectedImageItem, value))
            {
                if (_selectedImageItem != null)
                    _selectedImageItem.IsSelectedToSave = true;
                if (_signalToFlush.WaitOne(1))
                {
                    _signalToFlush.Reset();
                }

                RaisePropertyChanged(nameof(IsSaveCommandCanBeExecute));
            }
        }
    }

    /// <summary>
    /// current grabbed image
    /// </summary>
    public Bitmap DisplayImage { get; set; }

    #endregion

    #region DelegateCommand

    public DelegateCommand OpenCommand { get; }
    public DelegateCommand CloseCommand { get; }
    public DelegateCommand StartGrabCommand { get; }
    public DelegateCommand StopGrabCommand { get; }
    public DelegateCommand SaveCommand { get; }

    public DelegateCommand<object> ApplicationMinimizeCommand { get; }
    public DelegateCommand<object> ApplicationCloseCommand { get; }

    #endregion

    #region Constructor

    /// <summary>
    /// Default Constructor
    /// </summary>
    public MainWindowViewModel(ICameraModel model)
    {
        _title = "Image Grabber Application";

        _model = model;
        _model.PropertyChanged += ModelPropertyChanged;

        //create a signal instance for pause the flush behavior
        _signalToFlush = new ManualResetEvent(false);
        //set this as default 
        _signalToFlush.Set();

        ImageLists = new ObservableCollection<GrabbedImageItem>();

        OpenCommand = new DelegateCommand(RelayOpenCommand, () => OpenCommandCanBeExecute).ObservesProperty(() => OpenCommandCanBeExecute);

        CloseCommand = new DelegateCommand(RelayCloseCommand, () => CloseCommandCanBeExecute).ObservesProperty(() => CloseCommandCanBeExecute);

        StartGrabCommand = new DelegateCommand(RelayStartGrabCommand, () => StartGrabCommandCanBeExecute).ObservesProperty(() => StartGrabCommandCanBeExecute);

        StopGrabCommand = new DelegateCommand(RelayStopGrabCommand, () => StopGrabCommandCanBeExecute).ObservesProperty(() => StopGrabCommandCanBeExecute);

        SaveCommand = new DelegateCommand(RelaySaveCommand, () => IsSaveCommandCanBeExecute).ObservesProperty(() => IsSaveCommandCanBeExecute);

        ApplicationMinimizeCommand = new DelegateCommand<object>(RelayMinimizeCommand);

        ApplicationCloseCommand = new DelegateCommand<object>(RelayCloseCommand);
    }

    #endregion

    #region Model Property Changed
    private void ModelPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(ICameraModel.Camera):
                RaisePropertyChanged(nameof(Title));
                RaisePropertyChanged(nameof(OpenCommandCanBeExecute));
                RaisePropertyChanged(nameof(CloseCommandCanBeExecute));
                RaisePropertyChanged(nameof(StartGrabCommandCanBeExecute));
                RaisePropertyChanged(nameof(StopGrabCommandCanBeExecute));
                break;

            case nameof(ICameraModel.IsOpen):
            case nameof(ICameraModel.IsGrabbing):
                RaisePropertyChanged(nameof(OpenCommandCanBeExecute));
                RaisePropertyChanged(nameof(CloseCommandCanBeExecute));
                RaisePropertyChanged(nameof(StartGrabCommandCanBeExecute));
                RaisePropertyChanged(nameof(StopGrabCommandCanBeExecute));
                break;

            case nameof(ICameraModel.GrabbedImage):
                //update image list on ui thread.
                new Action(() =>
                {
                    var image = new GrabbedImageItem(_model.GrabbedImage);


                    DisplayImage = image.Image;
                    RaisePropertyChanged(nameof(DisplayImage));

                    //check the signal status before raise property changed
                    if (_signalToFlush.WaitOne(1))
                    {
                        while (ImageLists.Count() > 4) ImageLists.RemoveAt(0);
                        ImageLists.Add(image);
                        RaisePropertyChanged(nameof(ImageLists));
                    }

                }).SafeInvoke();
                break;

            default:
                break;
        }
    }
    #endregion

    #region Private Methods

    private void RelaySaveCommand()
    {
        _selectedImageItem.Save();
        if (!_signalToFlush.WaitOne(1))
            _signalToFlush.Set();

        _selectedImageItem = null;

        RaisePropertyChanged(nameof(IsSaveCommandCanBeExecute));

    }

    private async void RelayOpenCommand()
    {
        await _model?.CameraOpen();
        this.ClearBuffer();
    }
    private async void RelayCloseCommand()
    {
        await _model?.CameraClose();
        this.ClearBuffer();
    }
    private async void RelayStartGrabCommand() => await _model.CameraStartGrab();
    private async void RelayStopGrabCommand() => await _model.CameraStopGrab();

    private void RelayMinimizeCommand(object sender)
    {
        if (sender is System.Windows.Window window)
        {
            window.WindowState = System.Windows.WindowState.Minimized;
        }
    }

    private void RelayCloseCommand(object sender)
    {
        if (sender is System.Windows.Window window)
        {
            if(_model.Camera?.IsGrabbing ?? false)
            {
                _model.Camera.StopGrab();
                _model.Camera.Close();
            }
            window.Close();
        }
    }

    /// <summary>
    /// reset the views buffer when open and close done
    /// </summary>
    private void ClearBuffer()
    {
        ImageLists?.Clear();
        DisplayImage = null;

        RaisePropertyChanged(nameof(ImageLists));
        RaisePropertyChanged(nameof(DisplayImage));
    }

    #endregion
}