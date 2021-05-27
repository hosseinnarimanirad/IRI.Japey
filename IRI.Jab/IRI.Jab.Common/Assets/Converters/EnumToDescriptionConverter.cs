using IRI.Ket.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using IRI.Msh.Common.Extensions;

namespace IRI.Jab.Common.Assets.Converters
{
    class EnumToDescriptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return DependencyProperty.UnsetValue;

            try
            {
                return ((Enum)value).GetDescription();
            }
            catch (Exception ex)
            {
                return DependencyProperty.UnsetValue;
            }            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Enum.ToObject(targetType, value);
        }
    }
}
