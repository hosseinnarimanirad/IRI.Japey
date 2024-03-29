﻿using IRI.Msh.Common.Primitives;
using IRI.Msh.Common.Primitives;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Sta.Common.Service.Google
{
    public static class GoogleServiceUtility
    {
        static DateTime _baseDateTime = new DateTime(1970, 1, 1, 0, 0, 0);

        public static string ParseToString(Point geodeticPoint)
        {
            return $"{geodeticPoint.Y.ToString(CultureInfo.InvariantCulture)},{geodeticPoint.X.ToString(CultureInfo.InvariantCulture)}";
        }

        public static int GetTime(DateTime dateTime)
        {
            return (int)(dateTime - _baseDateTime).TotalSeconds;
        }

        public static int GetNow()
        {
            return GetTime(DateTime.Now);
        }
    }
}
