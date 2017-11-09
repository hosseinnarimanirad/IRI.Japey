using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.SqlServer.Types;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.InteropServices;
using System.Data.SqlTypes;

namespace IRI.Test.Standards.OGC.SFA
{
    [TestClass]
    public class GeometryTypesTest
    {
        double x = -10.87453087523;

        double y = 10.2344245235;

        double z = 324.98743523;

        double m = Math.Sin(23) / Math.Pow(Math.PI, 3);

        [TestMethod]
        public void TestPoint()
        {
            SqlGeometry geometry = SqlGeometry.STPointFromText(
                                        new SqlChars(new SqlString(string.Format("POINT({0} {1})", x, y))), 0);
            
            IRI.Standards.OGC.SFA.OgcPoint<IRI.Standards.OGC.SFA.Point> point =
                new IRI.Standards.OGC.SFA.OgcPoint<IRI.Standards.OGC.SFA.Point>(geometry.AsBinaryZM().Buffer);
            Assert.AreEqual(point.X, geometry.STX);
            Assert.AreEqual(point.Y, geometry.STY);


            SqlGeometry geometryZ = SqlGeometry.STPointFromText(
                                        new SqlChars(new SqlString(string.Format("POINT({0} {1} {2})", x, y, z))), 0);
            IRI.Standards.OGC.SFA.OgcPoint<IRI.Standards.OGC.SFA.PointZ> pointZ =
                new IRI.Standards.OGC.SFA.OgcPoint<IRI.Standards.OGC.SFA.PointZ>(geometryZ.AsBinaryZM().Buffer);
            Assert.AreEqual(pointZ.X, geometryZ.STX);
            Assert.AreEqual(pointZ.Y, geometryZ.STY);


            SqlGeometry geometryZM = SqlGeometry.STPointFromText(
                                        new SqlChars(new SqlString(string.Format("POINT({0} {1} {2} {3})", x, y, z, m))), 0);
            IRI.Standards.OGC.SFA.OgcPoint<IRI.Standards.OGC.SFA.PointZM> pointZM =
                new IRI.Standards.OGC.SFA.OgcPoint<IRI.Standards.OGC.SFA.PointZM>(geometryZM.AsBinaryZM().Buffer);
            Assert.AreEqual(pointZM.X, geometryZM.STX);
            Assert.AreEqual(pointZM.Y, geometryZM.STY);
        }

        [TestMethod]
        public void TestLineString()
        {
            SqlGeometry geometry =
                SqlGeometry.STLineFromText(
                    new SqlChars(
                        new SqlString(
                            string.Format("LINESTRING({0} {1}, {2} {3}, {0} {3}, {1} {2}, {3} {1})", x, y, z, m))), 0);
            IRI.Standards.OGC.SFA.OgcLineString<IRI.Standards.OGC.SFA.Point> linestring =
                new IRI.Standards.OGC.SFA.OgcLineString<IRI.Standards.OGC.SFA.Point>(geometry.AsBinaryZM().Buffer);
            CollectionAssert.AreEqual(linestring.ToWkb(), geometry.AsBinaryZM().Buffer);


            SqlGeometry geometryZ =
                SqlGeometry.STLineFromText(
                    new SqlChars(
                        new SqlString(
                            string.Format("LINESTRING({0} {1} {1}, {2} {3} {1}, {0} {3} {2}, {1} {2} {2}, {3} {1} {2})", x, y, z, m))), 0);
            IRI.Standards.OGC.SFA.OgcLineString<IRI.Standards.OGC.SFA.PointZ> linestringZ =
                new IRI.Standards.OGC.SFA.OgcLineString<IRI.Standards.OGC.SFA.PointZ>(geometryZ.AsBinaryZM().Buffer);
            CollectionAssert.AreEqual(linestringZ.ToWkb(), geometryZ.AsBinaryZM().Buffer);

            SqlGeometry geometryZM =
                SqlGeometry.STLineFromText(
                    new SqlChars(
                        new SqlString(
                            string.Format("LINESTRING({0} {1} {1} {0}, {2} {3} {1} {0}, {0} {3} {2} {0}, {1} {2} {2} {0}, {3} {1} {2} {2})", x, y, z, m))), 0);
            IRI.Standards.OGC.SFA.OgcLineString<IRI.Standards.OGC.SFA.PointZM> linestringZM =
                new IRI.Standards.OGC.SFA.OgcLineString<IRI.Standards.OGC.SFA.PointZM>(geometryZM.AsBinaryZM().Buffer);
            CollectionAssert.AreEqual(linestringZM.ToWkb(), geometryZM.AsBinaryZM().Buffer);
        }

        [TestMethod]
        public void TestPolygon()
        {
            SqlGeometry geometry =
                SqlGeometry.STPolyFromText(
                    new SqlChars(
                        new SqlString(
                            string.Format("POLYGON(({0} {1}, {2} {3}, {0} {3}, {1} {2}, {3} {1}, {0} {1}))", x, y, z, m))), 0);
            IRI.Standards.OGC.SFA.OgcPolygon<IRI.Standards.OGC.SFA.Point> polygon =
                new IRI.Standards.OGC.SFA.OgcPolygon<IRI.Standards.OGC.SFA.Point>(geometry.AsBinaryZM().Buffer);
            CollectionAssert.AreEqual(polygon.ToWkb(), geometry.AsBinaryZM().Buffer);


            SqlGeometry geometryZ =
                SqlGeometry.STPolyFromText(
                    new SqlChars(
                        new SqlString(
                            string.Format("POLYGON(({0} {1} {1}, {2} {3} {1}, {0} {3} {2}, {1} {2} {2}, {3} {1} {2}, {0} {1} {1}))", x, y, z, m))), 0);
            IRI.Standards.OGC.SFA.OgcPolygon<IRI.Standards.OGC.SFA.PointZ> polygonZ =
                new IRI.Standards.OGC.SFA.OgcPolygon<IRI.Standards.OGC.SFA.PointZ>(geometryZ.AsBinaryZM().Buffer);
            CollectionAssert.AreEqual(polygonZ.ToWkb(), geometryZ.AsBinaryZM().Buffer);


            SqlGeometry geometryZM =
                SqlGeometry.STPolyFromText(
                    new SqlChars(
                        new SqlString(
                            string.Format("POLYGON(({0} {1} {1} {0}, {2} {3} {1} {0}, {0} {3} {2} {0}, {1} {2} {2} {0}, {3} {1} {2} {2}, {0} {1} {1} {0}))", x, y, z, m))), 0);
            IRI.Standards.OGC.SFA.OgcPolygon<IRI.Standards.OGC.SFA.PointZM> polygonZM =
                new IRI.Standards.OGC.SFA.OgcPolygon<IRI.Standards.OGC.SFA.PointZM>(geometryZM.AsBinaryZM().Buffer);
            CollectionAssert.AreEqual(polygonZM.ToWkb(), geometryZM.AsBinaryZM().Buffer);
        }


    }
}
