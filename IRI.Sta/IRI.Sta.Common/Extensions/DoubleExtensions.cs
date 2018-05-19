using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace System
{
    public static class DoubleExtensions
    {
        public static bool IsNormal(this double value)
        {
            return !(double.IsNaN(value) || double.IsInfinity(value));
        }

        public static double AsDouble(this double? value)
        {
            return value.HasValue ? value.Value : double.NaN;
        }

        public static string ToInvariantString(this double value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        public static string AsExactString(this double value)
        {
            return value.ToString("G17", System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
