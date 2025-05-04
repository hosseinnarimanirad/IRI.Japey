using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Sta.CoordinateSystems.MapProjection
{
    public static class SridHelper
    {
        public const int GeodeticWGS84 = 4326;

        public const int WebMercator = 3857;

        public const int UtmNorthZone38 = 32638;

        public const int UtmNorthZone39 = 32639;

        public const int UtmNorthZone40 = 32640;

        public const int UtmNorthZone41 = 32641;

        // https://epsg.io/3395
        public const int Mercator = 3395;

        // https://epsg.io/54034
        public const int CylindricalEqualArea = 54034;

        public static SrsBase AsSrsBase(int srid)
        {
            switch (srid)
            {
                case SridHelper.GeodeticWGS84:
                    return new NoProjection("Wgs84", Ellipsoids.WGS84);// { DatumName = this.Geogcs.Values?.First() };

                case SridHelper.WebMercator:
                    return new WebMercator();

                case SridHelper.UtmNorthZone38:
                    return new UTM(Ellipsoids.WGS84, MapProjects.CalculateCentralMeridian(38));

                case SridHelper.UtmNorthZone39:
                    return new UTM(Ellipsoids.WGS84, MapProjects.CalculateCentralMeridian(39));

                case SridHelper.UtmNorthZone40:
                    return new UTM(Ellipsoids.WGS84, MapProjects.CalculateCentralMeridian(40));

                case SridHelper.UtmNorthZone41:
                    return new UTM(Ellipsoids.WGS84, MapProjects.CalculateCentralMeridian(41));

                case SridHelper.Mercator:
                    return new Mercator();

                case SridHelper.CylindricalEqualArea:
                    return new CylindricalEqualArea();

                default:
                    return null;
            }

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
}