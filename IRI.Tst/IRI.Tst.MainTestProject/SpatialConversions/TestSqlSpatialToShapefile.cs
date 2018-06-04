using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.SqlServer.Types;
using IRI.Ket.SqlServerSpatialExtension;

using IRI.Ket.ShapefileFormat.EsriType;
using IRI.Ket.SpatialExtensions;

namespace IRI.Test.FileFormats
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class TestSqlSpatialToShapefile
    {
        public TestSqlSpatialToShapefile()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        /// <summary>
        /// This method test both AsSqlServerWkt and ParseToEsriShapefile functionalities
        /// </summary>
        [TestMethod]
        public void TestSqlSpatialToEsriShapefile()
        {
            //
            string pointString = "POINT(-20 -20)";

            Assert.AreEqual(pointString, GetWktRepresentation(pointString));

            //
            string multiPointString = "MULTIPOINT((2 3),(7 8))";

            Assert.AreEqual(multiPointString, GetWktRepresentation(multiPointString));

            //
            string lineStringString = "LINESTRING(1 1,3 5,5 10,-70 34)";

            Assert.AreEqual(lineStringString, GetWktRepresentation(lineStringString));

            //
            string multiLineStringString = "MULTILINESTRING((1 1,3 5),(-5 3,-8 -2))";

            Assert.AreEqual(multiLineStringString, GetWktRepresentation(multiLineStringString));

            //
            string polygonString = "POLYGON((-20 -20,-20 20,20 20,20 -20,-20 -20))";

            Assert.AreEqual(polygonString, GetWktRepresentation(polygonString));


            //string multiPolygonString = "MULTIPOLYGON(((0 0, 0 3, 3 3, 3 0, 0 0), (1 1, 1 2, 2 1, 1 1)), ((9 9, 9 10, 10 9, 9 9)))";

            //SqlGeometry polygon = SqlGeometry.Parse(
            // new System.Data.SqlTypes.SqlString(multiPolygonString));

            //var shape = polygon.ParseToEsriShapefile();

            //string NewMultiPolygonString = SqlGeometry.Parse(
            // new System.Data.SqlTypes.SqlString(shape.AsSqlServerWkt())).MakeValid().STAsText().ToSqlString().Value;

            //Assert.AreEqual(multiPolygonString, NewMultiPolygonString);
        }

        private string GetWktRepresentation(string wktRepresentation)
        {
            SqlGeometry polygon = SqlGeometry.Parse(
                new System.Data.SqlTypes.SqlString(wktRepresentation));

            var shape = polygon.AsEsriShape();

            return shape.AsSqlServerWkt();
        }

        /// <summary>
        /// test whether arcmap can open polygons with wrong cw/ccw orientation
        /// </summary>
        [TestMethod]
        public void TestPolygonWrite()
        {
            string polygonString = "MULTIPOLYGON(((0 0, 0 6, 6 6, 6 0, 0 0), (5 5, 5 1, 1 1, 1 5, 5 5)), ((4 4, 2 4, 2 2, 4 2, 4 4),(3.5 3.5, 2.5 3.5, 2.5 2.5, 3.5 2.5, 3.5 3.5)))";

            SqlGeometry polygon = SqlGeometry.Parse(
                           new System.Data.SqlTypes.SqlString(polygonString));

            var shape = polygon.AsEsriShape();

            IRI.Ket.ShapefileFormat.Shapefile.Save(@"D:\test5.shp", new EsriShapeCollection<EsriPolygon>(
                new List<EsriPolygon>() { (EsriPolygon)shape }), true);
        }
    }
}
