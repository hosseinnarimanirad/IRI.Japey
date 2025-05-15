using System;
using System.Globalization;
using System.Windows.Data;

using IRI.Jab.Common.Assets.ShapeStrings;

namespace IRI.Jab.Common.Assets.Converters;

public class BoolToGeometryConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (bool)value ? Appbar.appbarCheckGeometry : Appbar.appbarCloseGeometry;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
