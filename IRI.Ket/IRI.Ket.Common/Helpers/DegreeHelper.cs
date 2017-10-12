using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.Common.Helpers
{
    public static class DegreeHelper
    {
        public static string ToDms(double degreeValue)
        {
            var degreePart = Math.Truncate(degreeValue);

            var minutePart = Math.Truncate((degreeValue - degreePart) * 60);

            var secondPart = (degreeValue - degreePart - minutePart / 60.0) * 3600;

            return $" {degreePart}° {minutePart}' {secondPart}'' ";
        }
    }
}
