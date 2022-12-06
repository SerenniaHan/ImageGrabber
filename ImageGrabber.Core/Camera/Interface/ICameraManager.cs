using System.Collections.Generic;

namespace ImageGrabber.Core.Camera;

public interface ICameraManager
{
    /// <summary>
    /// Camera reousces
    /// </summary>
    IEnumerable<ICamera> Cameras { get; set; }
    /// <summary>
    /// Build up all camera resources
    /// </summary>
    void Buildup();
}