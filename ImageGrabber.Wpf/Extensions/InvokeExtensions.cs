using System;
using System.Windows;

namespace ImageGrabber.Wpf.Extensions
{
    public static class InvokeExtensions
    {
        public static void SafeInvoke(this Action action)
        {
            try
            {
                Application.Current?.Dispatcher.Invoke(action);
            }
            catch(Exception e)
            {
                throw e;
            }
        }
    }
}
