using System;
using System.Collections.Generic;

namespace ImageGrabber.Core.CameraModule;
/// <summary>
/// ICamera interface inherited from <see cref="IDevice"/>
/// </summary>
public interface ICamera : IDevice, IDisposable
{
    /// <summary>
    /// fire this event when grabbed image
    /// </summary>
    event GrabbedImageEventHandler OnGrabbedImageEvent;
    /// <summary>
    /// camera name
    /// </summary>
    string Name { get; set; }
    /// <summary>
    /// camera description
    /// </summary>
    string Description { get; set; }
    /// <summary>
    /// if camera is grabbing then return <see langword="true"/>, otherwise return <see langword="false"/>
    /// </summary>
    bool IsGrabbing { get; set; }
    /// <summary>
    /// if camera is continuous grabbing mode then return <see langword="true"/>, otherwise return <see langword="false"/>
    /// </summary>
    bool ContinuousGrabbingMode { get; set; }
    void SetTriggerMode();
    void SetParameters(Dictionary<string, object> dict);
    /// <summary>
    /// camera start grab
    /// </summary>
    void StartGrab();
    /// <summary>
    /// camera stop grab
    /// </summary>
    void StopGrab();
}
