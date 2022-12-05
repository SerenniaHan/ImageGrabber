using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Prism.Mvvm;
using ImageGrabber.Core;
using ImageGrabber.Core.Camera;
using System.Drawing;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Timers;
using System.Windows.Media;
using ImageGrabber.Wpf.Extensions;
using System.Windows.Controls.Primitives;

namespace ImageGrabber.Application.Models
{
    public interface ICameraModel : INotifyPropertyChanged
    {
        IEnumerable<ICamera> CameraResource { get; }
        ICamera Camera { get; set; }
        bool IsOpen { get; }
        bool IsGrabbing { get; }
        GrabbedImageItem GrabbedImage { get; set; }
        Task CameraOpen();
        Task CameraClose();
        Task CameraStartGrab();
        Task CameraStopGrab();
    }

    public sealed class GrabbedImageItem
    {
        #region Properties

        public string CameraName { get; private set; }
        public string GrabbedTime { get; private set; }
        public bool IsSelectedToSave { get; private set; }
        public Bitmap Image { get; private set; }
        public ImageSource ShowIamge { get; private set; }

        #endregion

        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        public GrabbedImageItem(string cameraName, string grabbedTime, bool isSelectedToSave, Bitmap image)
        {
            CameraName = cameraName;
            GrabbedTime = grabbedTime;
            IsSelectedToSave = isSelectedToSave;
            Image = image;
            ShowIamge = image.ToImageSource();
        }

        public GrabbedImageItem(GrabbedImageItem item)
        {
            this.CameraName = item.CameraName;
            this.GrabbedTime = item.GrabbedTime;
            this.IsSelectedToSave = item.IsSelectedToSave;
            this.Image = item.Image;
            this.ShowIamge = Image.ToImageSource();
        }
        #endregion
    }

    public class CameraItem : BindableBase, ICameraModel
    {

        #region Private Fields
        private readonly ICameraManager _cameraManager;
        private ICamera _camera;
        private int _flushTimeSpan = 1000;
        private System.Timers.Timer _flushTimer;
        #endregion

        #region Constructor
        /// <summary>
        /// Default Constructor
        /// </summary>
        public CameraItem(ICameraManager cameraManager)
        {
            _cameraManager = cameraManager;
            _cameraManager.Buildup();
            _camera = null;
        }

        #endregion

        #region Properties

        public IEnumerable<ICamera> CameraResource
        {
            get
            {
                if (_cameraManager == null || !_cameraManager.Cameras.Any()) yield return null;

                foreach (ICamera camera in _cameraManager.Cameras)
                {
                    yield return camera;
                }
            }
        }

        /// <summary>
        /// Selected camera instance default value is set to null
        /// </summary>
        public ICamera Camera
        {
            get => _camera;
            set => SetProperty(ref _camera, value);
        }

        public bool IsOpen => _camera?.IsOpen ?? false;

        public bool IsGrabbing => _camera?.IsGrabbing ?? false;

        /// <summary>
        /// Time span for flushing to videmodel
        /// </summary>
        public int FlushTimeSpan
        {
            get => _flushTimeSpan;
            set => SetProperty(ref _flushTimeSpan, value);
        }

        private GrabbedImageItem _item;
        public GrabbedImageItem GrabbedImage
        {
            get => _item;
            set => SetProperty(ref _item, value);
        }

        private Bitmap _img;
        public Bitmap NewFrame
        {
            get => _img;
            set => SetProperty(ref _img, value);
        }


        #endregion

        public Task CameraOpen() => Task.Run(() =>
        {
            _camera?.Open();
            RaisePropertyChanged(nameof(IsOpen));
        });

        public Task CameraClose() => Task.Run(() =>
        {
            _camera?.Close();
            RaisePropertyChanged(nameof(IsOpen));
        });

        public Task CameraStartGrab() => Task.Run(() =>
        {
            _camera.OnGrabbedImageEvent += OnGrabbedImage;
            _camera.StartGrab();
            RaisePropertyChanged(nameof(IsGrabbing));
        });


        public Task CameraStopGrab() => Task.Run(() =>
        {
            _camera.OnGrabbedImageEvent -= OnGrabbedImage;
            _camera.StopGrab();
            RaisePropertyChanged(nameof(IsGrabbing));
        });

        private void OnGrabbedImage(object sender, IGrabbedEvent e)
        {
            if (e.Image is Bitmap bmp)
            {
                this.GrabbedImage = new GrabbedImageItem(e.Name, e.GrabbedTime.ToString(), false, bmp);
            }
        }
    }
}
