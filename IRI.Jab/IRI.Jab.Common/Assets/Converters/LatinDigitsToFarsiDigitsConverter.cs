using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using IRI.Extensions;

namespace IRI.Jab.Common.Assets.Converters
{
    class LatinDigitsToFarsiDigitsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                return value.ToString().LatinNumbersToFarsiNumbers();
            }
            else
            {
                return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                return value.ToString().FarsiNumbersToLatinNumbers();
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
