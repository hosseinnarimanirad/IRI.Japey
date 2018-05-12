using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace IRI.Jab.Common.Assets.Converters
{
    public class BoolToGridLengthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isTrue = (bool)value;

            if (isTrue && parameter != null)
            {
                if (parameter.ToString().ToLower() == "auto")
                {
                    return new GridLength(0, GridUnitType.Auto);
                }
                if (parameter.ToString().EndsWith("*"))
                {
                    return new GridLength(double.Parse(parameter.ToString().TrimEnd('*')), GridUnitType.Star);
                }
                else
                {
                    return new GridLength(double.Parse(parameter.ToString()), GridUnitType.Pixel);
                }
            }
            else
            {
                return new GridLength(0);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
