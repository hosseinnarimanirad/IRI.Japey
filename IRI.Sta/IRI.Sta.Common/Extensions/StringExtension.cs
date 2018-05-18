using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    public static class StringExtension
    {
        public static string AsExactString(this double value)
        {
            return value.ToString("G17", System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
