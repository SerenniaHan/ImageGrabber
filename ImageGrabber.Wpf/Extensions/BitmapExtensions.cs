using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ImageGrabber.Wpf.Extensions;

/// <summary>
/// Bitmap Extensions
/// </summary>
public static class BitmapExtensions
{
    [System.Runtime.InteropServices.DllImport("gdi32.dll")]
    private static extern bool DeleteObject(IntPtr ptr);

    public static ImageSource ToImageSource(this System.Drawing.Bitmap bitmap)
    {
        IntPtr image = IntPtr.Zero;
        ImageSource source = null;


        try
        {
            image = bitmap.GetHbitmap();
            source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                image,
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());
        }
        catch (Exception e)
        {
            source = null;
            System.Diagnostics.Debug.WriteLine($"BitmapToImageSource Failed\r\n{{{e.Message}}}");
        }
        finally
        {
            _ = DeleteObject(image);
        }

        return source;
    }
}