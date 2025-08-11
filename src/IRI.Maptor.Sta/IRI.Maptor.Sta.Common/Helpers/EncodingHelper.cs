using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Maptor.Sta.Common.Helpers;

public static class EncodingHelper
{
    private static int _arabicWindowsEncoding = 1256;

    public static Encoding ArabicEncoding { get => Encoding.GetEncoding(_arabicWindowsEncoding); }
}
