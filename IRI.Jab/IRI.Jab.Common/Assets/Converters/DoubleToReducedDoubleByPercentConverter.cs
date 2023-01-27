using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace IRI.Jab.Common.Assets.Converters
{
    public class DoubleToReducedDoubleByPercentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double doubleValue;

            if (!double.TryParse(value?.ToString(), out doubleValue))
            {
                doubleValue = 0;
            }

            double percent;

            if (!double.TryParse(parameter?.ToString(), out percent))
            {
                percent = 0;
            }

            return percent / 100.0 * doubleValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
