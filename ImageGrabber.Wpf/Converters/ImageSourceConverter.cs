using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Data;
using ImageGrabber.Wpf.Extensions;

namespace ImageGrabber.Wpf.Converters;

public class ImageSourceConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not Bitmap bmp) return null;
        return bmp.ToImageSource();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
