using System;
using System.Windows.Data;

namespace IRI.Jab.Common.Assets.Converters;

class MultiBooleanToVisibilityConverter : IMultiValueConverter
{
    public object Convert(object[] values,
                            Type targetType,
                            object parameter,
                            System.Globalization.CultureInfo culture)
    {
        bool visible = true;

        foreach (object value in values)
            if (value is bool)
            {
                visible = visible && (bool)value;
            }

        if (visible)
            return System.Windows.Visibility.Visible;
        else
            return System.Windows.Visibility.Collapsed;
    }

    public object[] ConvertBack(object value,
                                Type[] targetTypes,
                                object parameter,
                                System.Globalization.CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}