namespace ImageGrabber.Core
{
    public interface IDevice
    {
        /// <summary>
        /// if device is opened then return <see langword="true"/>, otherwise return <see langword="false"/>
        /// </summary>
        bool IsOpen { get; set; }
        /// <summary>
        /// open device
        /// </summary>
        void Open();
        /// <summary>
        /// close device
        /// </summary>
        void Close();
        /// <summary>
        /// try open device, and return the result
        /// </summary>
        /// <returns>if device is opened then return <see langword="true"/>, otherwise return <see langword="false"/></returns>
        bool TryOpen();
    }
}