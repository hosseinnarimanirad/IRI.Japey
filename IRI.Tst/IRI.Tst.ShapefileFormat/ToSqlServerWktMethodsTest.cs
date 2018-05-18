using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using IRI.Ket.ShapefileFormat.EsriType;
using Microsoft.SqlServer.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IRI.Test.ShapefileFormatTest
{
    [TestClass]
    public class AsSqlServerWktMethodsTest
    {
        double[] x = new double[10], y = new double[10], z = new double[10], m = new double[10];
        IRI.Ket.ShapefileFormat.EsriType.EsriPoint[] points = new IRI.Ket.ShapefileFormat.EsriType.EsriPoint[10];
        IRI.Ket.ShapefileFormat.EsriType.EsriPointM[] pointsM = new IRI.Ket.ShapefileFormat.EsriType.EsriPointM[10];
        IRI.Ket.ShapefileFormat.EsriType.EsriPointZ[] pointsZ = new IRI.Ket.ShapefileFormat.EsriType.EsriPointZ[10];

        IRI.Ket.ShapefileFormat.EsriType.EsriPoint[][] pointCollection;
        IRI.Ket.ShapefileFormat.EsriType.EsriPointM[][] pointCollectionM;
        IRI.Ket.ShapefileFormat.EsriType.EsriPointZ[][] pointCollectionZ;

        IShapeCollection polygons;

        [TestInitialize]
        public void InitializeTest()
        {
            for (int i = 0; i < 10; i++)
            {
                x[i] = Math.Round(Math.Sin(i) * 100, 5);
                y[i] = Math.Round(Math.Cos(i) * 70, 5);
                points[i] = new IRI.Ket.ShapefileFormat.EsriType.EsriPoint(x[i], y[i]);
                pointsM[i] = new IRI.Ket.ShapefileFormat.EsriType.EsriPointM(x[i], y[i], x[i] + y[i]);
                pointsZ[i] = new IRI.Ket.ShapefileFormat.EsriType.EsriPointZ(x[i], y[i], x[i] + y[i]);
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

        }

        [TestMethod]
        public void TestEsriPointAsSqlServerWkt()
        {
            //Test EsriPoint
            IRI.Ket.ShapefileFormat.EsriType.EsriPoint point = new IRI.Ket.ShapefileFormat.EsriType.EsriPoint(x[0], y[0]);
            SqlGeometry geoEsriPoint =
                SqlGeometry.STPointFromText(new SqlChars(new SqlString(point.AsSqlServerWkt())), 0);

            //Test EsriPointM
            IRI.Ket.ShapefileFormat.EsriType.EsriPointM pointM = new IRI.Ket.ShapefileFormat.EsriType.EsriPointM(x[0], y[0], m[0]);
            SqlGeometry geoEsriPointM =
                SqlGeometry.STPointFromText(new SqlChars(new SqlString(pointM.AsSqlServerWkt())), 0);

            //Test EsriPointZ
            IRI.Ket.ShapefileFormat.EsriType.EsriPointZ pointZ = new IRI.Ket.ShapefileFormat.EsriType.EsriPointZ(x[0], y[0], z[0]);
            SqlGeometry geoEsriPointZ =
                SqlGeometry.STPointFromText(new SqlChars(new SqlString(pointZ.AsSqlServerWkt())), 0);

            //Test EsriPointZM
            IRI.Ket.ShapefileFormat.EsriType.EsriPointZ pointZM = new IRI.Ket.ShapefileFormat.EsriType.EsriPointZ(x[0], y[0], z[0], m[0]);
            SqlGeometry geoEsriPointZM =
                SqlGeometry.STPointFromText(new SqlChars(new SqlString(pointZM.AsSqlServerWkt())), 0);

        }

        [TestMethod]
        public void TestEsriPointCollectionAsSqlServerWkt()
        {
            //Test MultiEsriPoint
            MultiPoint multiEsriPoint = new MultiPoint(points);
            SqlGeometry.STPointFromText(new SqlChars(new SqlString(multiEsriPoint.AsSqlServerWkt())), 0);

            //Test MultiEsriPointM
            MultiPointM multiEsriPointM = new MultiPointM(pointsM);
            SqlGeometry.STPointFromText(new SqlChars(new SqlString(multiEsriPointM.AsSqlServerWkt())), 0);

            //Test MultiEsriPointZ
            MultiPointZ multiEsriPointZ = new MultiPointZ(pointsZ);
            SqlGeometry.STPointFromText(new SqlChars(new SqlString(multiEsriPointZ.AsSqlServerWkt())), 0);
        }

        [TestMethod]
        public void TestPolylineAsSqlServerWktStringMethod()
        {
            PolyLine polyLine = new PolyLine(this.pointCollection);

            SqlGeometry sqlPolyline = SqlGeometry.STMLineFromText(new SqlChars(new SqlString(polyLine.AsSqlServerWkt())), 0);

            //PolyLineM polyLineM = new PolyLineM(this.pointCollectionM);

            //SqlGeometry sqlPolylineM = SqlGeometry.STMLineFromText(new SqlChars(new SqlString(polyLineM.AsSqlServerWkt())), 0);

            //Assert.AreEqual(true, sqlPolylineM.HasM);

            //PolyLineZ polyLineZ = new PolyLineZ(this.pointCollectionZ);

            //SqlGeometry sqlPolylineZ = SqlGeometry.STMLineFromText(new SqlChars(new SqlString(polyLineZ.AsSqlServerWkt())), 0);

            //Assert.AreEqual(true, sqlPolylineZ.HasZ);

        }

        [TestMethod]
        public void TestPolygonAsSqlServerWktStringMethod()
        {

            foreach (Polygon item in this.polygons)
            {
                SqlGeometry geometry = SqlGeometry.STPolyFromText(new SqlChars(new SqlString(item.AsSqlServerWkt())), 0);

                Assert.AreEqual(geometry.STNumPoints(), item.NumberOfPoints);

                Assert.AreEqual(geometry.STNumGeometries(), item.NumberOfParts);
            }

        }

    }
}
