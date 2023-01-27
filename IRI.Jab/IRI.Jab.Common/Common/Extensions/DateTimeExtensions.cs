using IRI.Jab.Common.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Jab.Common.Extensions
{
    public static class DateTimeExtensions
    {
        public static SpecialDateTime AsSpecial(this DateTime dateTime)
        {
            return new SpecialDateTime(dateTime);
        }

        public static SpecialDateTime AsSpecial(this DateTime? dateTime)
        {
            return new SpecialDateTime(dateTime);
        }

        
    }
}
