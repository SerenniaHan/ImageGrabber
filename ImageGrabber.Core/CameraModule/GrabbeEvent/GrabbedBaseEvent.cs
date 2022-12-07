using System;

namespace ImageGrabber.Core.CameraModule;

/// <summary>
/// grabbed image event handler
/// </summary>
/// <param name="sender">object inherited from <see cref="ICamera"/> interface</param>
/// <param name="e">grabbed image event object inherited from <see cref="IGrabbedEvent"/></param>
public delegate void GrabbedImageEventHandler(object sender, IGrabbedEvent e);

public abstract class GrabbedBaseEvent<T> : IGrabbedEvent<T>
{
    public string Name { get; }

    object IGrabbedEvent.Image => (object)Image ?? null;
    public DateTime GrabbedTime { get; }
    public T Image { get; }

    public GrabbedBaseEvent(string name, DateTime grabbedTime, T image)
    {
        this.Name = name;
        this.GrabbedTime = grabbedTime;
        this.Image = image;
    }
}

