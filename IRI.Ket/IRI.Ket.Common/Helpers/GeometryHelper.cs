using IRI.Ham.SpatialBase.CoordinateSystems.MapProjection;
using IRI.Ham.SpatialBase.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.Common.Helpers
{
    public static class GeometryHelper
    {
        public static Geometry ParseToGeometry(double[][] geoCoordinates, GeometryType geometryType, bool isLongFirst = false)
        {
            return new Geometry(geoCoordinates.Select(p => ParseToGeometry(p, isLongFirst)).ToArray(), geometryType, SridHelper.GeodeticWGS84);
        }

        private static Geometry ParseToGeometry(double[] values, bool isLongFirst)
        {
            if (values == null || values.Count() % 2 != 0)
            {
                throw new NotImplementedException();
            }

            List<IRI.Ham.SpatialBase.Point> result = new List<IRI.Ham.SpatialBase.Point>(values.Length / 2);

            if (isLongFirst)
            {
                for (int i = 0; i < values.Length - 1; i += 2)
                {
                    result.Add(new IRI.Ham.SpatialBase.Point(values[i], values[i + 1]));
                }
            }
            else
            {
                for (int i = 0; i < values.Length - 1; i += 2)
                {
                    result.Add(new IRI.Ham.SpatialBase.Point(values[i + 1], values[i]));
                }
            }

            return new Geometry(result.ToArray(), GeometryType.LineString, SridHelper.GeodeticWGS84);

        }


        public static Geometry ParseLineStringToGeometry(double[][] geoCoordinates, GeometryType geometryType, bool isLongFirst = false)
        {
            return new Geometry(geoCoordinates.Select(p => ParsePointToGeometry(p, isLongFirst)).ToArray(), geometryType, SridHelper.GeodeticWGS84);
        }

        private static Ham.SpatialBase.Point ParsePointToGeometry(double[] values, bool isLongFirst)
        {
            if (values == null || values.Count() != 2)
            {
                throw new NotImplementedException();
            }

            if (isLongFirst)
            {
                return new Ham.SpatialBase.Point(values[0], values[1]);
            }
            else
            {
                return new Ham.SpatialBase.Point(values[1], values[0]);
            }

        }

        public static Geometry ParsePolygonToGeometry(double[][][] rings, GeometryType geometryType, bool isLongFirst)
        {
            return new Geometry(rings.Select(p => ParseLineStringToGeometry(p, GeometryType.LineString, isLongFirst)).ToArray(), geometryType, SridHelper.GeodeticWGS84);
        }

    }
}
