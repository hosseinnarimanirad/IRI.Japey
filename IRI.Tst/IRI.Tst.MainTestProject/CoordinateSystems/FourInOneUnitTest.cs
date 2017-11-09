using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IRI.Ham.CoordinateSystem;
using IRI.Ham.MeasurementUnit;
using IRI.Ham.SpatialBase;
using IRI.Ham.CoordinateSystem.MapProjection;

namespace IRI.Test.CoordinateSystem
{
    [TestClass]
    public class FourInOneUnitTest
    {
        [TestMethod]
        public void MainTest()
        {
            var clarke = IRI.Ham.CoordinateSystem.Ellipsoids.Clarke1880Rgs;
            double phi0 = 32.5;
            double phi1 = 29.65508274166;
            double phi2 = 35.31468809166;
            double lambda0 = 45.0;
            var niocLcc = new LambertConformalConic(clarke, phi1, phi2, lambda0, phi0, 1500000.0, 1166200.0, 0.9987864078);

            var xLccNioc = 2047473.33479;
            var yLccNioc = 912594.777238;

            var xWgs84 = 50.689721;
            var yWgs84 = 30.072906;

            var xWebMercator = 5642753.9243;
            var yWebMercator = 3512924.70491;

            var xClarke1880Rgs = 50.689721;
            var yClarke1880Rgs = 30.075637;


            var wgs84 = niocLcc.ToWgs84Geodetic(new Point(xLccNioc, yLccNioc));

            Assert.AreEqual(xWgs84, wgs84.X, 1E-6);
            Assert.AreEqual(yWgs84, wgs84.Y, 1E-6);

            var clarke1880 = niocLcc.ToGeodetic(new Point(xLccNioc, yLccNioc));

            Assert.AreEqual(xClarke1880Rgs, clarke1880.X, 1E-6);
            Assert.AreEqual(yClarke1880Rgs, clarke1880.Y, 1E-6);

            var webMercator = MapProjects.GeodeticWgs84ToWebMercator(wgs84);

            Assert.AreEqual(xWebMercator, webMercator.X, 0.05);
            Assert.AreEqual(yWebMercator, webMercator.Y, 0.05);

            var clarke1880_2 = Transformation.ChangeDatum(wgs84, Ellipsoids.WGS84, Ellipsoids.Clarke1880Rgs);

            Assert.AreEqual(xClarke1880Rgs, clarke1880_2.X, 1E-6);
            Assert.AreEqual(yClarke1880Rgs, clarke1880_2.Y, 1E-6);

        }
    }
}
