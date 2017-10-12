using IRI.Ham.SpatialBase;
using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.SpatialExtensions
{
    public static class BoundingBoxExtensions
    {
        public static string AsWkt(this BoundingBox boundingBox)
        {
            return
                     string.Format(
                            System.Globalization.CultureInfo.InvariantCulture,
                            "POLYGON(({0} {1}, {0} {2}, {3} {2}, {3} {1}, {0} {1}))",
                            boundingBox.XMin,
                            boundingBox.YMin,
                            boundingBox.YMax,
                            boundingBox.XMax);
        }

        public static SqlGeometry AsSqlGeometry(this BoundingBox boundingBox, int srid = 0)
        {
            //var result =
            //    SqlGeometry.Parse(
            //         string.Format(
            //                System.Globalization.CultureInfo.InvariantCulture,
            //                "POLYGON(({0} {1}, {0} {2}, {3} {2}, {3} {1}, {0} {1}))",
            //                boundingBox.XMin,
            //                boundingBox.YMin,
            //                boundingBox.YMax,
            //                boundingBox.XMax));

            //result.STSrid = srid;

            //return result;

            return SqlGeometry.STPolyFromText(new System.Data.SqlTypes.SqlChars(boundingBox.AsWkt()), srid);
        }

        public static bool Intersects(this BoundingBox boundingBox, SqlGeometry geometry)
        {
            SqlGeometry boundary = boundingBox.AsSqlGeometry(geometry.GetSrid());

            return geometry.STIntersects(boundary).IsTrue;
        }
    }
}
