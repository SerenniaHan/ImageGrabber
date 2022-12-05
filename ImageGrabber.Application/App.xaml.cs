using System.Windows;
using Unity;
using Prism.Ioc;
using ImageGrabber.Core.Camera;
using ImageGrabber.Application.Views;
using ImageGrabber.Application.Models;

namespace ImageGrabber.Application
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<ICameraManager, UsbCameraManager>();
            containerRegistry.Register<ICameraModel, CameraItem>();
        }
    }
}