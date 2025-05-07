using IRI.Sta.Spatial.Mapping;
using IRI.Jab.Common.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace IRI.Jab.Common.Assets.Converters
{
    public class ScaleToGoogleScaleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var mapScale = (double)value;

            //var scale = GoogleScale.Scales.FirstOrDefault(i => i.Scale > googleScale);

            return GoogleScale.GetNearestScale(mapScale);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return 1;
            }

            return 1.0 / (value as GoogleScale).InverseScale;

        }
    }
}
