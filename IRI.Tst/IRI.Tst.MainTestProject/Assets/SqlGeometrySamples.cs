using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Test.MainTestProject.Assets
{
    public static class SqlGeometrySamples
    {
        public static SqlGeometry Point = SqlGeometry.Parse(new SqlString("Point(1 1)"));

        public static SqlGeometry PointZ = SqlGeometry.Parse(new SqlString("Point(1 1 1)"));

        public static SqlGeometry PointZM = SqlGeometry.Parse(new SqlString("Point(1 1 2 3)"));

        public static SqlGeometry MultipointComplex = SqlGeometry.Parse(new SqlString("MULTIPOINT((0 0), (0 3), (3 3), (3 0), (1 1 1), (9 9 1 2), (9 10), (10 9))"));

        public static SqlGeometry Multipoint = SqlGeometry.Parse(new SqlString("MULTIPOINT((2 3), (7 8))"));


        public static SqlGeometry Linestring = SqlGeometry.Parse(new SqlString("LINESTRING(1 1, 2 0,  2 4, 3 3)"));

        public static SqlGeometry LinestringMZ = SqlGeometry.Parse(new SqlString("LINESTRING( 4 4 4 4, 9 0 4 4 )"));

        public static SqlGeometry MultiLineString = SqlGeometry.Parse(new SqlString("MULTILINESTRING((1 1, 3 5), (-5 3, -8 -2))"));

        public static SqlGeometry Polygon = SqlGeometry.Parse(new SqlString("POLYGON( (0 0 9, 30 0 9, 30 30 9, 0 30 9, 0 0 9))"));

        public static SqlGeometry PolygonWithHole = SqlGeometry.Parse(new SqlString("POLYGON((-20 -20, -20 20, 20 20, 20 -20, -20 -20), (10 0, 0 10, 0 -10, 10 0))"));

        public static SqlGeometry MultiPolygon01 = SqlGeometry.Parse(new SqlString("MULTIPOLYGON(((0 0, 0 3, 3 3, 3 0, 0 0), (1 1, 1 2, 2 1, 1 1)), ((9 9, 9 10, 10 9, 9 9)))"));

        public static SqlGeometry MultiPolygon02 = SqlGeometry.Parse(new SqlString("MULTIPOLYGON(((0 0, 0 6, 6 6, 6 0, 0 0), (5 5, 5 1, 1 1, 1 5, 5 5)), ((4 4, 2 4, 2 2, 4 2, 4 4),(3.5 3.5, 2.5 3.5, 2.5 2.5, 3.5 2.5, 3.5 3.5)))"));

        public static List<SqlGeometry> AllGeometries = new List<SqlGeometry>()
        {
            Point,PointZ, PointZM,MultipointComplex,Multipoint,Linestring,LinestringMZ,MultiLineString,Polygon,PolygonWithHole,MultiPolygon01,MultiPolygon02
        };
    }
}
