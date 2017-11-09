using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Xml;

namespace IRI.Test.FileFormats
{
    [TestClass]
    public class TestShapefileToKml
    {
        [TestMethod]
        public void TestParseToString()
        {
            string shpFileName = @"D:\Data\0. UnitTestData\Geographic Shapefiles\ArcMap Output\Point.shp";

            var shapes = IRI.Ket.ShapefileFormat.Shapefile.Read(shpFileName);

            var IShape = shapes[0];

            string value = IRI.Ket.Common.Helpers.XmlHelper.Parse(IShape.AsPlacemark());
        }

        [TestMethod]
        public void TestAsKmlForPoint()
        {
            string shpFileName = @"D:\Data\0. UnitTestData\Geographic Shapefiles\ArcMap Output\Point.shp";

            string kmlFileName = @"D:\Data\0. UnitTestData\Geographic Shapefiles\My Output\Point.kml";

            string computedKml = IRI.Ket.ShapefileFormat.Shapefile.Read(shpFileName).AsKml();

            string actualKml = System.IO.File.ReadAllText(kmlFileName);

            Assert.AreEqual(computedKml, actualKml);
        }

        [TestMethod]
        public void TestAsKmlForLineString()
        {
            string shpFileName = @"D:\Data\0. UnitTestData\Geographic Shapefiles\ArcMap Output\MultipartLinestring.shp";

            string kmlFileName = @"D:\Data\0. UnitTestData\Geographic Shapefiles\My Output\MultipartLinestring.kml";

            string computedKml = IRI.Ket.ShapefileFormat.Shapefile.Read(shpFileName).AsKml();

            string actualKml = System.IO.File.ReadAllText(kmlFileName);

            Assert.AreEqual(computedKml, actualKml);
        }


    }
}
