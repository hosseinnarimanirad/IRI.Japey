using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.Common.Extensions
{
    public static class StringExtensions
    {

        //This method cannot be implemented in Portable Class!
        public static bool EqualsIgnoreCase(this string theString, string value)
        {
            return theString.Equals(value, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
