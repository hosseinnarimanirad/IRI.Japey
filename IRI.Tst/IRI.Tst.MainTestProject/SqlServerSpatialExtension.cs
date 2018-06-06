using System.Linq;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.SqlServer.Types;
using System.Data.SqlTypes;
using IRI.Msh.Common.Primitives;


using System.Reflection;
using System.IO;
using IRI.Ket.ShapefileFormat.Prj;
using IRI.Ket.SqlServerSpatialExtension;
using IRI.Ket.ShapefileFormat;
using IRI.Ket.SpatialExtensions;
using IRI.Test.MainTestProject.Assets;

namespace IRI.Test.MainTestProject.SqlServerSpatialExtension
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestExtractPoint()
        {
            var polygon = SqlGeometry.Parse(new SqlString("POLYGON( (0 0 9, 30 0 9, 30 30 9, 0 30 9, 0 0 9) )"));

            var linestring = SqlGeometry.Parse(new SqlString("LINESTRING( 4 4 4 4, 9 0 4 4 )"));

            var multiPolygon = SqlGeometry.Parse(new SqlString("MULTIPOLYGON(((0 0, 0 3, 3 3, 3 0, 0 0), (1 1, 1 2, 2 1, 1 1)), ((9 9, 9 10, 10 9, 9 9)))"));

            var polygonWithHole = SqlGeometry.Parse(new SqlString("POLYGON((-20 -20, -20 20, 20 20, 20 -20, -20 -20), (10 0, 0 10, 0 -10, 10 0))"));

            var geometry = polygon.ExtractPoints();

            var temp = geometry.AsSqlGeometry();

            Assert.AreEqual(polygon.AsWkt(), temp.AsWkt());

            Assert.AreEqual(4, geometry.Geometries.Sum(i => i.Points.Count()));


            geometry = linestring.ExtractPoints();

            temp = geometry.AsSqlGeometry();

            Assert.AreEqual(linestring.AsWkt(), temp.AsWkt());

            Assert.AreEqual(2, geometry.Points.Length);


            geometry = multiPolygon.ExtractPoints();

            temp = geometry.AsSqlGeometry();

            Assert.AreEqual(multiPolygon.AsWkt(), temp.AsWkt());

            Assert.AreEqual(4, geometry.Geometries[0].Geometries[0].Points.Length);

            Assert.AreEqual(3, geometry.Geometries[0].Geometries[1].Points.Length);

            Assert.AreEqual(3, geometry.Geometries[1].Geometries[0].Points.Length);


            geometry = polygonWithHole.ExtractPoints();

            temp = geometry.AsSqlGeometry();

            Assert.AreEqual(polygonWithHole.AsWkt(), temp.AsWkt());

            Assert.AreEqual(4, geometry.Geometries[0].Points.Length);

            Assert.AreEqual(3, geometry.Geometries[1].Points.Length);
        }

        [TestMethod]
        public void TestCreateSqlGeometryFromPointList()
        {

        }

        [TestMethod]
        [TestProperty("Author", "Hossein Narimani Rad")]
        public void TestCoordinateSystemExtensions()
        {
            var prjString = ReadFile("Clarke 1880 (RGS).prj");

            //var prjString = ReadFile("lccnioc.prj");

            EsriPrjFile prjFile = EsriPrjFile.Parse(prjString);

            var prjFile2 = prjFile.AsMapProjection().AsEsriPrj();
             
            Assert.AreEqual(prjFile.AsEsriCrsWkt(), prjFile2.AsEsriCrsWkt());
        }
         
        private string ReadFile(string fileName)
        {
            var assembly = Assembly.GetExecutingAssembly();

            var resourceName = $"IRI.Test.MainTestProject.Assets.PrjSamples.{fileName}";

            string result;

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    result = reader.ReadToEnd();
                }
            }

            return result;
        }

        [TestMethod]
        public void TestSqlSpatialToGml()
        {
            foreach (var geo in SqlGeometrySamples.AllGeometries)
            {
                GmlExtensions.ParseGML3(geo.AsGml3(), 3857);
            }
        }


        //[TestMethod]
        //public void TestSqlSpatialToMarkup()
        //{
        //    //var markup1 = SqlGeometrySamples.Linestring.ParseToPathMarkup();
        //    //var markup2 = SqlGeometrySamples.MultiLineString.ParseToPathMarkup();
        //    //var markup3 = SqlGeometrySamples.Polygon.ParseToPathMarkup();
        //    //var markup4 = SqlGeometrySamples.PolygonWithHole.ParseToPathMarkup();
        //    //var markup5 = SqlGeometrySamples.MultiPolygon01.ParseToPathMarkup();
        //    //var markup6 = SqlGeometrySamples.MultiPolygon02.ParseToPathMarkup();


        //}
    }
}
