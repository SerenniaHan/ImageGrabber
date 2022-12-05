using System;
using System.Collections.Generic;

namespace ImageGrabber.Core.Camera
{
    public interface ICamera : IDevice, IDisposable
    {
        event GrabbedImageEventHandler OnGrabbedImageEvent;
        string Name { get; set; }
        string Description { get; set; }
        bool IsGrabbing { get; set; }
        bool ContinuousGrabbingMode { get; set; }
        void SetTriggerMode();
        void SetParameters(Dictionary<string, object> dict);
        void StartGrab();
        void StopGrab();
    }
}