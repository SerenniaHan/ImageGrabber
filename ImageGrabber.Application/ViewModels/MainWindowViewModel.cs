using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using ImageGrabber.Application.Models;
using Prism.Commands;
using Prism.Mvvm;
using ImageGrabber.Core.Camera;
using ImageGrabber.Wpf.Extensions;
using ImageGrabber.Wpf.Utility;
using System.Windows.Navigation;
using System.ComponentModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Collections.Concurrent;
using System.Drawing;

namespace ImageGrabber.Application.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    // ReSharper disable once MemberCanBePrivate.Global
    public class MainWindowViewModel : BindableBase
    {
        #region Private Fields

        private string _title = "Image Grabber Application";
        private readonly ICameraModel _model;

        #endregion

        #region Properties

        /// <summary>
        /// Window Title
        /// </summary>
        public string Title
        {
            get
            {
                if (_model?.Camera is null) return _title;
                else return $"{_title}_[{_model.Camera.Name}]";
            }
        }

        /// <summary>
        /// camera source
        /// </summary>
        public ObservableCollection<CombBoxData<ICamera>> CameraResources
        {
            get
            {
                if (_model.CameraResource == null) return null;

                return new ObservableCollection<CombBoxData<ICamera>>(
                    from camera in _model.CameraResource
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


        public ObservableCollection<GrabbedImageItem> ImageLists
        {
            get;
        }

        public Bitmap DisplayImage
        {
            get;set;
        }

        #endregion

        #region DelegateCommand

        public DelegateCommand OpenCommand { get; }
        public DelegateCommand CloseCommand { get;  }
        public DelegateCommand StartGrabCommand { get;  }
        public DelegateCommand StopGrabCommand { get;  }
        public DelegateCommand SaveCommand { get;  }

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

            ImageLists = new ObservableCollection<GrabbedImageItem>();

            OpenCommand = new DelegateCommand(RelayOpenCommand, () => OpenCommandCanBeExecute).ObservesProperty(() => OpenCommandCanBeExecute);
            CloseCommand = new DelegateCommand(RelayCloseCommand, () => CloseCommandCanBeExecute).ObservesProperty(() => CloseCommandCanBeExecute);
            StartGrabCommand = new DelegateCommand(RelayStartGrabCommand, () => StartGrabCommandCanBeExecute).ObservesProperty(() => StartGrabCommandCanBeExecute);
            StopGrabCommand = new DelegateCommand(RelayStopGrabCommand, () => StopGrabCommandCanBeExecute).ObservesProperty(() => StopGrabCommandCanBeExecute);
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

                        while (ImageLists.Count() > 4) ImageLists.RemoveAt(0);
                        ImageLists.Add(image);
                        DisplayImage = image.Image;
                        RaisePropertyChanged(nameof(ImageLists));
                        RaisePropertyChanged(nameof(DisplayImage));
                    }).SafeInvoke();
                    break;

                default:
                    break;
            }
        }
        #endregion

        #region Private Methods

        private void SaveExecute()
        {
        }

        private async void RelayOpenCommand() => await _model?.CameraOpen();
        private async void RelayCloseCommand() => await _model?.CameraClose();
        private async void RelayStartGrabCommand() => await _model.CameraStartGrab();
        private async void RelayStopGrabCommand() => await _model.CameraStopGrab();
        #endregion
    }
}