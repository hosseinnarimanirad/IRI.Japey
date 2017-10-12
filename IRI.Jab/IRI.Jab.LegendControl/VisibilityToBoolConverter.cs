using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace IRI.Jab.LegendControl
{
    class VisibilityToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            System.Windows.Visibility visibility = (System.Windows.Visibility)value;

            return visibility == System.Windows.Visibility.Visible ? true : false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (bool)value ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
        }
    }
}
