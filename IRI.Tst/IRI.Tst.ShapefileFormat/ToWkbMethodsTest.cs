using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
//using IRI.Ket.ShapefileFormat.EsriType;
using Microsoft.SqlServer.Types;
using System.Data.SqlTypes;
using IRI.Ket.ShapefileFormat.EsriType;

namespace IRI.Test.ShapefileFormat
{
    [TestClass]
    public class AsWkbMethodsTest
    {
        double[] x = new double[10], y = new double[10], z = new double[10], m = new double[10];
        EsriPoint[] points = new EsriPoint[10];
        EsriPointM[] pointsM = new EsriPointM[10];
        EsriPointZ[] pointsZ = new EsriPointZ[10];

        EsriPoint[][] pointCollection;
        EsriPointM[][] pointCollectionM;
        EsriPointZ[][] pointCollectionZ;

        IEsriShapeCollection polygons;
        IEsriShapeCollection polygonsZ;

        [TestInitialize]
        public void InitializeTest()
        {
            for (int i = 0; i < 10; i++)
            {
                x[i] = Math.Round(Math.Sin(i) * 100, 5);
                y[i] = Math.Round(Math.Cos(i) * 70, 5);
                points[i] = new EsriPoint(x[i], y[i]);
                pointsM[i] = new EsriPointM(x[i], y[i], x[i] + y[i]);
                pointsZ[i] = new EsriPointZ(x[i], y[i], x[i] + y[i]);
            }

            this.pointCollection = new EsriPoint[3][];
            pointCollection[0] = new EsriPoint[] { new EsriPoint(0.65, 0), new EsriPoint(5, 5), new EsriPoint(3.04, 6.4) };
            pointCollection[1] = new EsriPoint[] { new EsriPoint(10, 0), new EsriPoint(5.43, 15.65), new EsriPoint(3.04, 6.4), new EsriPoint(10, 90.34), new EsriPoint(25.43, 15.65), new EsriPoint(73.04, 61.44) };
            pointCollection[2] = new EsriPoint[] { new EsriPoint(20, 10), new EsriPoint(15, 5), new EsriPoint(33.04, 64.4) };

            this.pointCollectionM = new EsriPointM[4][];
            pointCollectionM[0] = new EsriPointM[] { pointsM[0], pointsM[1], pointsM[2] };
            pointCollectionM[1] = new EsriPointM[] { pointsM[3], pointsM[4], pointsM[5], pointsM[5], pointsM[6] };
            pointCollectionM[2] = new EsriPointM[] { pointsM[0], pointsM[7], pointsM[8] };
            pointCollectionM[3] = new EsriPointM[] { pointsM[0], pointsM[7], pointsM[8] };


            this.pointCollectionZ = new EsriPointZ[5][];
            pointCollectionZ[0] = new EsriPointZ[] { pointsZ[0], pointsZ[1], pointsZ[2] };
            pointCollectionZ[1] = new EsriPointZ[] { pointsZ[3], pointsZ[4], pointsZ[5], pointsZ[5], pointsZ[6] };
            pointCollectionZ[2] = new EsriPointZ[] { pointsZ[1], pointsZ[7] };
            pointCollectionZ[3] = new EsriPointZ[] { pointsZ[3], pointsZ[5], pointsZ[8] };
            pointCollectionZ[4] = new EsriPointZ[] { pointsZ[9], pointsZ[7], pointsZ[8] };

            System.IO.DirectoryInfo info = new System.IO.DirectoryInfo(System.Environment.CurrentDirectory);

            string fileName = string.Join("\\", info.Parent.Parent.FullName, "Resources\\regions.shp");

            this.polygons = IRI.Ket.ShapefileFormat.Shapefile.Read(fileName);

            string fileName2 = string.Join("\\", info.Parent.Parent.FullName, "Resources\\buildings_oops.shp");

            this.polygonsZ = IRI.Ket.ShapefileFormat.Shapefile.Read(fileName2);

        }


        [TestMethod]
        public void TestEsriPointToSqlServerWkb()
        {
            double x = 4.543, y = -42.342, z = 4234.5235, m = 3423.34;

            //EsriPoint
            EsriPoint point = new EsriPoint(x, y);
            SqlGeometry sqlEsriPoint =
                SqlGeometry.STPointFromText(
                new System.Data.SqlTypes.SqlChars(
                    new System.Data.SqlTypes.SqlString(string.Format("POINT({0} {1})", x, y))), 0);
            CollectionAssert.AreEqual(point.AsWkb(), sqlEsriPoint.STAsBinary().Buffer);

            //EsriPointM
            EsriPointM pointM = new EsriPointM(x, y, m);
            SqlGeometry sqlEsriPointM =
                SqlGeometry.STPointFromText(
                new System.Data.SqlTypes.SqlChars(
                    new System.Data.SqlTypes.SqlString(string.Format("POINT({0} {1} NULL {2})", x, y, m))), 0);
            CollectionAssert.AreEqual(pointM.AsWkb(), sqlEsriPointM.AsBinaryZM().Buffer);

            //EsriPointZ
            EsriPointZ pointZ = new EsriPointZ(x, y, z, m);
            SqlGeometry sqlEsriPointZ =
                SqlGeometry.STPointFromText(
                new System.Data.SqlTypes.SqlChars(
                    new System.Data.SqlTypes.SqlString(string.Format("POINT({0} {1} {2} {3})", x, y, z, m))), 0);
            CollectionAssert.AreEqual(pointZ.AsWkb(), sqlEsriPointZ.AsBinaryZM().Buffer);


            //byte[] b1 = pointM.ToSqlServerWkb();
            //byte[] b2 = sqlEsriPointM.AsBinaryZM().Buffer;

        }

        [TestMethod]
        public void TestEsriMultiPointToSqlServerWkb()
        {
            //Test MultiPoint
            EsriMultiPoint multiEsriPoint = new IRI.Ket.ShapefileFormat.EsriType.EsriMultiPoint(points);
            SqlGeometry sqlEsriPoint =
               SqlGeometry.STPointFromText(
               new System.Data.SqlTypes.SqlChars(
                   new System.Data.SqlTypes.SqlString(multiEsriPoint.AsSqlServerWkt())), 0);

            CollectionAssert.AreEqual(multiEsriPoint.AsWkb(), sqlEsriPoint.STAsBinary().Buffer);

            //Test MultiPointM
            IRI.Ket.ShapefileFormat.EsriType.EsriMultiPointM multiEsriPointM = new IRI.Ket.ShapefileFormat.EsriType.EsriMultiPointM(pointsM);
            SqlGeometry sqlMultiPointM =
               SqlGeometry.STPointFromText(
               new System.Data.SqlTypes.SqlChars(
                   new System.Data.SqlTypes.SqlString(multiEsriPointM.AsSqlServerWkt())), 0);

            CollectionAssert.AreEqual(multiEsriPointM.AsWkb(), sqlMultiPointM.STAsBinary().Buffer);

            //Test EsriPointZ
            IRI.Ket.ShapefileFormat.EsriType.EsriMultiPointZ multiEsriPointZ = new EsriMultiPointZ(pointsZ);
            SqlGeometry sqlEsriPointZ =
               SqlGeometry.STPointFromText(
               new System.Data.SqlTypes.SqlChars(
                   new System.Data.SqlTypes.SqlString(multiEsriPointZ.AsSqlServerWkt())), 0);

            CollectionAssert.AreEqual(multiEsriPointZ.AsWkb(), sqlEsriPointZ.STAsBinary().Buffer);

        }

        [TestMethod]
        public void TestPolylineToSqlServerWkb()
        {
            //Test Polyline
            EsriPolyLine polyline = new EsriPolyLine(pointCollection);
            SqlGeometry sqlPolyline =
               SqlGeometry.STMLineFromText(
               new System.Data.SqlTypes.SqlChars(
                   new System.Data.SqlTypes.SqlString(polyline.AsSqlServerWkt())), 0);

            CollectionAssert.AreEqual(polyline.AsWkb(), sqlPolyline.STAsBinary().Buffer);

            ////Test PolylineM
            //PolyLineM polylineM = new PolyLineM(this.pointCollectionM);
            //SqlGeometry sqlPolylineM =
            //   SqlGeometry.STMLineFromText(
            //   new System.Data.SqlTypes.SqlChars(
            //       new System.Data.SqlTypes.SqlString(polylineM.AsSqlServerWkt())), 0);

            //CollectionAssert.AreEqual(polylineM.AsWkb(), sqlPolylineM.AsBinaryZM().Buffer);

            ////Test PolylineZ
            //PolyLineZ polylineZ = new PolyLineZ(pointCollectionZ);
            //SqlGeometry sqlPolylineZ = SqlGeometry.STMLineFromWKB(new SqlBytes(polylineZ.AsWkb()), 0);

            //CollectionAssert.AreEqual(polylineZ.AsWkb(), sqlPolylineZ.AsBinaryZM().Buffer);

        }

        [TestMethod]
        public void TestPolygonToSqlServerWkb()
        {

            foreach (EsriPolygon item in this.polygons)
            {
                SqlGeometry geometry = SqlGeometry.STPolyFromWKB(new SqlBytes(item.AsWkb()), 0);

                CollectionAssert.AreEqual(geometry.AsBinaryZM().Buffer, item.AsWkb());
            }

            //foreach (PolygonZ item in this.polygonsZ)
            //{
            //    SqlGeometry geometry = SqlGeometry.STPolyFromWKB(new SqlBytes(item.AsWkb()), 0);

            //    CollectionAssert.AreEqual(geometry.AsBinaryZM().Buffer, item.AsWkb());
            //}
        }

    }
}
