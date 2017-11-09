using IRI.Ham.SpatialBase;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Test.CoordinateSystem
{
    [TestClass]
    public class MercatorUnitTest
    {
        [TestMethod]
        public void TestGeodeticToMercator()
        {
            //WGS84(51.190512, 35.817032)
            Point wgs84Point = new Point(51.190512, 35.817032);

            var mercator = IRI.Ham.CoordinateSystem.MapProjection.MapProjects.GeodeticToMercator(wgs84Point);

            Assert.AreEqual(5698501.75902, mercator.X, .1);
            Assert.AreEqual(4250468.61959, mercator.Y, .1);
        }

        [TestMethod]
        public void TestWebMercator()
        {
            //WGS84(51.190512, 35.817032)
            Point wgs84Point = new Point(51.190512, 35.817032);

            //var a = Ham.CoordinateSystem.Ellipsoids.WGS84.SemiMajorAxis.Value;

            //Web Mercator has Sphere-based calculation but the resuling phi,lambda is assumend to be based on WGS84

            var webMercator = Ham.CoordinateSystem.MapProjection.MapProjects.GeodeticWgs84ToWebMercator(wgs84Point);

            Assert.AreEqual(5698501.75902, webMercator.X, .1);
            Assert.AreEqual(4275474.36482, webMercator.Y, .1);

            var mercator = Ham.CoordinateSystem.MapProjection.MapProjects.GeodeticToMercator(wgs84Point);

            var calculatedMercator = Ham.CoordinateSystem.MapProjection.MapProjects.WebMercatorToMercatorWgs84(webMercator);

            Assert.AreEqual(mercator.X, calculatedMercator.X, .1);
            Assert.AreEqual(mercator.Y, calculatedMercator.Y, .1);

            var calculatedWgs84Point = Ham.CoordinateSystem.MapProjection.MapProjects.WebMercatorToGeodeticWgs84(webMercator);

            Assert.AreEqual(wgs84Point.X, calculatedWgs84Point.X, .000001);
            Assert.AreEqual(wgs84Point.Y, calculatedWgs84Point.Y, .000001);

        }

    }
}
