
using IRI.Jab.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace IRI.Jab.Common.Assets.Converters
{
    public class ArabicStringToFarsiStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value.ToString().ArabicToFarsi();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value.ToString().ArabicToFarsi();
        }
    }
}
