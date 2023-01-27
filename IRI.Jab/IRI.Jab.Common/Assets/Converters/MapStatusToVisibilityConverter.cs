﻿using IRI.Jab.Common.Model;
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
    public class MapStatusToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        { 
            MapStatus expectedStatus;

            if (value is MapStatus && parameter != null)
            { 
                if (Enum.TryParse(parameter.ToString(), out expectedStatus))
                {
                    return (MapStatus)value == expectedStatus ? Visibility.Visible : Visibility.Collapsed;
                }
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
