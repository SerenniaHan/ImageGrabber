using ImageGrabber.Core.Camera;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace ImageGrabber.Application.Models
{
    /// <summary>
    /// ICameraModel provides the generic interface for implement model object
    /// </summary>
    public interface ICameraModel : INotifyPropertyChanged
    {
        /// <summary>
        /// camera resources gets from <see cref="ICameraManager"/> object
        /// </summary>
        IEnumerable<ICamera> CameraResources { get; }
        /// <summary>
        /// selected camera object which inherit from <see cref="ICamera"/>
        /// </summary>
        ICamera Camera { get; set; }
        /// <summary>
        /// if camera is open then return <see langword="true"/>, otherwise return <see langword="false"/>
        /// </summary>
        bool IsOpen { get; }
        /// <summary>
        /// if camera is grabbin image then return <see langword="false"/>, otherwise return <see langword="false"/>
        /// </summary>
        bool IsGrabbing { get; }
        /// <summary>
        /// grabbed image object <see cref="GrabbedImageItem"/> contains basic information
        /// </summary>
        GrabbedImageItem GrabbedImage { get; set; }
        /// <summary>
        /// camera open task
        /// </summary>
        Task CameraOpen();
        /// <summary>
        /// camera close task
        /// </summary>
        Task CameraClose();
        /// <summary>
        /// camera start grabbing image task
        /// </summary>
        Task CameraStartGrab();
        /// <summary>
        /// camera stop grabbing image task
        /// </summary>
        Task CameraStopGrab();
    }
}
