using System;
using System.Drawing;

namespace ImageGrabber.Core
{
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

    public class BitmapGrabbedEvent : GrabbedBaseEvent<Bitmap>
    {
        public BitmapGrabbedEvent(string name, DateTime grabbedTime, Bitmap image) : base(name, grabbedTime, image)
        {
        }
    }
}