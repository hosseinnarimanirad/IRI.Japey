using System;
using System.Windows.Data;
using System.Windows.Media;

namespace IRI.Jab.Common.Assets.Converters;

public class SolidColorBrushToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        if (value == null)
        {
            return null;
        }
        else
        {
            return ((SolidColorBrush)value).Color;
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        if (value == null)
        {
            return null;
        }
        else
        {
            return new SolidColorBrush((Color)value);
        }
    }
}
