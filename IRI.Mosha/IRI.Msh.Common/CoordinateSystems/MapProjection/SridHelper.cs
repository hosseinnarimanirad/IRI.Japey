using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Msh.CoordinateSystem.MapProjection
{
    public static class SridHelper
    {
        public const int GeodeticWGS84 = 4326;

        public const int WebMercator = 3857;

        public const int UtmNorthZone38 = 32638;

        public const int UtmNorthZone39 = 32639;

        public const int UtmNorthZone40 = 32640;

        public const int UtmNorthZone41 = 32641;


        //public static CrsBase GetCrs(int srid)
        //{
        //    switch (srid)
        //    {
        //        case GeodeticWGS84:
        //            return DefaultMapProjections.
        //            break;

        //        case WebMercator:
        //            return new WebMercator();

        //        default:
        //            break;
        //    }
        //}
    }
}
