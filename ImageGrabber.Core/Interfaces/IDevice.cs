namespace ImageGrabber.Core
{
    public interface IDevice
    {
        bool IsOpen { get; set; }
        void Open();
        void Close();
        bool TryOpen();
    }
}