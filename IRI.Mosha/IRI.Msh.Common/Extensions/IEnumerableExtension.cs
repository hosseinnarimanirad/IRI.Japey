using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Msh.Common.Extensions
{
    public static class IEnumerableExtension
    { 
        public static bool IsNotNullNorEmpty<T>(this IEnumerable<T> enumerable)
        {
            return enumerable?.Any() == true;
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            return enumerable?.Any() != true;
        }
    }
}
