﻿using System;
using System.Collections.Generic;

using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace IRI.Jab.Common.Assets.Converters
{
    public class StreamGeometryToPathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Path result;
             
            try
            {
                result = new Path() { Data = value as StreamGeometry, Stretch = Stretch.Uniform };
            }
            catch
            {
                result = new Path();
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
