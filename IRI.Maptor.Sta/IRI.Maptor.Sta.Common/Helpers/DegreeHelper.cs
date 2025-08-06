using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Maptor.Sta.Common.Helpers;

public static class DegreeHelper
{

    public const char minuteSign = '\u2019';

    public const char secondSign = '\u201d';

    public static string ToDms(double degreeValue, bool roundSecond = false)
    {
        int degreePart, minutePart;

        double secondPart;

        ToDms(degreeValue, roundSecond, out degreePart, out minutePart, out secondPart);

        if (roundSecond)
        {
            return FormattableString.Invariant($" {degreePart:000}° {minutePart:00}{minuteSign} {secondPart:00.00}{secondSign} ");
        }
        else
        {
            return FormattableString.Invariant($" {degreePart:000}° {minutePart:00}{minuteSign} {secondPart:00.#}{secondSign} ");
        }

    }

    public static void ToDms(double degreeValue, bool roundSecond, out int degree, out int minute, out double second)
    {
        degree = (int)Math.Truncate(degreeValue);

        minute = (int)Math.Truncate((degreeValue - degree) * 60);

        second = (degreeValue - degree - minute / 60.0) * 3600;
    }
}
