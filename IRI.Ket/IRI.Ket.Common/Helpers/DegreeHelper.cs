using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.Common.Helpers
{
    public static class DegreeHelper
    {
        public static string ToDms(double degreeValue, bool roundSecond = false)
        {
            var degreePart = Math.Truncate(degreeValue);

            var minutePart = Math.Truncate((degreeValue - degreePart) * 60);

            var secondPart = (degreeValue - degreePart - minutePart / 60.0) * 3600;

            var minuteSign = '\u2019'; var secondSign = '\u201d';

            if (roundSecond)
            {
                return FormattableString.Invariant($" {degreePart:000}° {minutePart:00}{minuteSign} {secondPart:00.00}{secondSign} ");
            }
            else
            {
                return FormattableString.Invariant($" {degreePart:000}° {minutePart:00}{minuteSign} {secondPart:00.#}{secondSign} ");
            }


        }
    }
}
