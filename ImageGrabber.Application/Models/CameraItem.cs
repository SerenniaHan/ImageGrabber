using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using Prism.Mvvm;
using ImageGrabber.Core;
using ImageGrabber.Core.Camera;

namespace ImageGrabber.Application.Models
{
    /// <summary>
    /// Default model object inherited from <see cref="ICameraModel"/>
    /// </summary>
    public sealed class CameraItem : BindableBase, ICameraModel
    {

        #region Private Fields
        private readonly ICameraManager _cameraManager;
        private ICamera _camera;
        private GrabbedImageItem _grabbedImageItem;
        #endregion

        #region Constructor
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <remarks><see cref="ICameraManager"/> get from DI Contianer which is registed before</remarks>
        public CameraItem(ICameraManager cameraManager)
        {
            _cameraManager = cameraManager;
            _cameraManager.Buildup();
            _camera = null;
        }

        #endregion

        #region Properties

        public IEnumerable<ICamera> CameraResources
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

        public ICamera Camera
        {
            get => _camera;
            set => SetProperty(ref _camera, value);
        }

        public bool IsOpen => _camera?.IsOpen ?? false;

        public bool IsGrabbing => _camera?.IsGrabbing ?? false;

        public GrabbedImageItem GrabbedImage
        {
            get => _grabbedImageItem;
            set => SetProperty(ref _grabbedImageItem, value);
        }

        #endregion

        #region Public Methods

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

        #endregion

        #region Private Methods

        private void OnGrabbedImage(object sender, IGrabbedEvent e)
        {
            if (e.Image is Bitmap bmp)
            {
                this.GrabbedImage = new GrabbedImageItem(e.Name, e.GrabbedTime.ToString(), false, bmp);
            }
        }
        #endregion
    }
}
