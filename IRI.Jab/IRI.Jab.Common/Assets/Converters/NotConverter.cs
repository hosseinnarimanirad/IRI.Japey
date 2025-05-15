using System;
using System.Windows.Data;
using System.Globalization;

namespace IRI.Jab.Common.Assets.Converters;

public class NotConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return !((bool)value);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return !((bool)value);
    }
}
