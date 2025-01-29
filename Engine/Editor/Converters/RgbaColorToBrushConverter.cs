using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;
using Ssit.CrossX;

namespace Editor.Converters;

public class RgbaColorToBrushConverter: IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is RgbaColor color)
        {
            return new SolidColorBrush(new Color(color.A, color.R, color.G, color.B));
        }
        return new SolidColorBrush(Colors.Magenta);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}