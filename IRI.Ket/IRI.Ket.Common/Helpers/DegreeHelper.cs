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

            if (roundSecond)
            {
                return $" {degreePart:000}° {minutePart:00}' {Math.Round(secondPart, 2):00.00}'' ";
            }
            else
            {
                return $" {degreePart:000}° {minutePart:00}' {secondPart}'' ";
            }


        }
    }
}
