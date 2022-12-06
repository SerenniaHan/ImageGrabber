using System.Collections.Generic;
using System.Linq;
using AForge.Video.DirectShow;

namespace ImageGrabber.Core.Camera;

// ReSharper disable once ClassNeverInstantiated.Global
/// <summary>
/// Camera manager inherited <see cref="ICameraManager"/>
/// </summary>
public class UsbCameraManager : ICameraManager
{
    public IEnumerable<ICamera> Cameras { get; set; }

    public void Buildup()
    {
        var infos = new FilterInfoCollection(FilterCategory.VideoInputDevice);

        if (infos.Count < 1) return;

        Cameras = (from FilterInfo info in infos select new UsbCamera(info.Name, info.MonikerString)).Cast<ICamera>().ToList();
    }
}