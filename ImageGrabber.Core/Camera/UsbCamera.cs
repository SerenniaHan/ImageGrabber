using System;
using System.Collections.Generic;
using AForge.Video;
using AForge.Video.DirectShow;

namespace ImageGrabber.Core.Camera
{
    /// <summary>
    /// UsbCamera
    /// </summary>
    public class UsbCamera : CameraBase
    {
        #region Private Fields

        private IVideoSource _camera;

        #endregion

        #region Properties



        #endregion

        #region Constructor

        public UsbCamera()
        {
        }

        public UsbCamera(string name) : base(name)
        {

        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="name">Name of camera device</param>
        /// <param name="description">Description of camera device</param>
        public UsbCamera(string name, string description) : base(name, description)
        {
        }

        #endregion

        public override void Open()
        {
            if (_camera is not null) return;
            _camera = new VideoCaptureDevice(Description);
            IsOpen = (_camera != null);
        }

        public override void Close()
        {
            IsOpen = false;
            if (_camera == null) return;
            _camera = null;
        }

        public override bool TryOpen()
        {
            Open();
            return _camera != null;
        }

        public override void Dispose()
        {
        }

        public override bool ContinuousGrabbingMode { get; set; }
        public override void SetTriggerMode()
        {
        }

        public override void SetParameters(Dictionary<string, object> dict)
        {
        }

        public override void StartGrab()
        {
            IsGrabbing = true;
            _camera.NewFrame += OnNewFrameGrabbed;
            _camera.Start();
        }

        public override void StopGrab()
        {
            IsGrabbing = false;
            _camera.NewFrame -= OnNewFrameGrabbed;
#if NETCOREAPP3_1_OR_GREATER
            //framework problem
            _camera.SignalToStop();
            //this.Close();
#else
            _camera.Stop();
#endif

        }

        private void OnNewFrameGrabbed(object sender, NewFrameEventArgs e)
        {
            e.Frame.RotateFlip(System.Drawing.RotateFlipType.Rotate90FlipY);

            base.OnGrabbedImageChanged(new BitmapGrabbedEvent($"{this.Name}", DateTime.Now, e.Frame));
        }
    }
}