using System;
using System.Collections.Generic;
using System.Drawing;

namespace ImageGrabber.Core.Camera
{
    public delegate void GrabbedImageEventHandler(object sender, IGrabbedEvent e);

    /// <summary>
    /// Abstract camera base inherited from <see cref="ICamera"/> interface
    /// </summary>
    public abstract class CameraBase : ICamera
    {
        #region DelegateCommand

        public event GrabbedImageEventHandler OnGrabbedImageEvent;

        protected void OnGrabbedImageChanged(IGrabbedEvent e)
        {
            OnGrabbedImageEvent ?.Invoke(this, e);
        }
        
        #endregion
        
        #region Properties

        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsOpen { get; set; }
        public bool IsGrabbing { get; set; }

        #endregion

        #region Constructor

        public CameraBase()
        {
            
        }

        protected CameraBase(string name, string description)
        {
            this.Name = name;
            this.Description = description;
        }

        #endregion

        #region Abstract Methods
        public abstract void Open();
        public abstract void Close();
        public abstract bool TryOpen();
        public abstract void Dispose();
        public abstract bool ContinuousGrabbingMode { get; set; }
        public abstract void SetTriggerMode();
        public abstract void SetParameters(Dictionary<string, object> dict);
        public abstract void StartGrab();
        public abstract void StopGrab();
        #endregion
    }
}