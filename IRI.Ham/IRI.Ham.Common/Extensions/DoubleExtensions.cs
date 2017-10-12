using System;
using System.Collections.Generic;
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
    }
}
