using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using IRI.Ket.Common.Extensions;

namespace IRI.Jab.Common.Assets.Converters
{
    public class DateTimeToPersianEllapsedDateCoarseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !(value is DateTime))
            {
                return string.Empty;
            }

            var dateTime = (DateTime)value;

            return dateTime.GetPersianEllapsedDateCoarse();

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
