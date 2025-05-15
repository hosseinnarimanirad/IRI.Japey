using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Extensions;

public static class IntExtension
{
    public static string Number2String(this int number, bool isCaps)
    {
        Char c = (Char)((isCaps ? 65 : 97) + (number));

        return c.ToString();
    }
}
