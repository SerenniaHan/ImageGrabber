using System.Collections.Generic;

namespace ImageGrabber.Core.Camera;

public interface ICameraManager
{
    IEnumerable<ICamera> Cameras { get; set; }
    void Buildup();
}