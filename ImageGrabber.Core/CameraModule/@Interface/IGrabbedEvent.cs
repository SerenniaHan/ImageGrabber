using System;

namespace ImageGrabber.Core.CameraModule;

public interface IGrabbedEvent
{
    string Name { get; }
    object Image { get; }
    DateTime GrabbedTime { get; }
}
public interface IGrabbedEvent<out T> : IGrabbedEvent
{
    new T Image { get; }
}