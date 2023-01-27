﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace IRI.Jab.Common.Assets.Converters
{
    public class IsAuthorizedToBool : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            IPrincipal principal = value as IPrincipal;

            if (value == null || parameter == null)
            {
                return false;
            }

            return principal.IsInRole(parameter.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

