using System;
using System.Drawing;

namespace ImageGrabber.Core.CameraModule;

public class BitmapGrabbedEvent : GrabbedBaseEvent<Bitmap>
{
    public BitmapGrabbedEvent(string name, DateTime grabbedTime, Bitmap image) : base(name, grabbedTime, image)
    {
    }
}
