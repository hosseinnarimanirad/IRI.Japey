using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IRI.Sta.MeasurementUnit;
using IRI.Sta.CoordinateSystem;
using IRI.Msh.Common.Primitives;
using IRI.Sta.CoordinateSystem.MapProjection;

namespace IRI.Test.CoordinateSystem
{
    [TestClass]
    public class LccUnitTest
    {
        [TestMethod]
        public void TestGeographicToLcc()
        {

            var result = new LambertConformalConic(Ellipsoids.Clarke1866, 33.0, 45.0, -96.0, 23.0).FromGeodetic(new Point(-75.0, 35.0));

            Assert.AreEqual(result.X, 1894410.9, .1);
            Assert.AreEqual(result.Y, 1564649.5, .1);


            var ellipse = IRI.Sta.CoordinateSystem.Ellipsoids.GRS80;

            double phi0 = 52;
            double phi1 = 35;
            double phi2 = 65;

            double lambda0 = 10;

            var projection = new LambertConformalConic(ellipse, phi1, phi2, lambda0, phi0);


            //Iran
            double lambda = 53;
            double phi = 33;

            result = projection.FromGeodetic(new Point(lambda, phi));

            Assert.AreEqual(result.X + 4000000, 7830046.77, .01);
            Assert.AreEqual(result.Y + 2800000, 1879902.99, .01);

            //Canada
            lambda = -105;
            phi = 54;

            result = projection.FromGeodetic(new Point(lambda, phi));

            Assert.AreEqual(result.X + 4000000, -685838.46, .01);
            Assert.AreEqual(result.Y + 2800000, 7633442.68, .01);


            //Brazil
            lambda = -53;
            phi = -12;

            result = projection.FromGeodetic(new Point(lambda, phi));

            Assert.AreEqual(result.X + 4000000, -5883916.57, .01);
            Assert.AreEqual(result.Y + 2800000, -936363.79, .01);


            //Autralia
            lambda = 134;
            phi = -23;

            result = projection.FromGeodetic(new Point(lambda, phi));

            Assert.AreEqual(result.X + 4000000, 19245528.93, .01);
            Assert.AreEqual(result.Y + 2800000, 9343431.52, .01);
        }


        [TestMethod]
        public void TestLccToGeographic()
        {
            double expectedPhi = 35.0;
            double expectedLambda = -75.0;

            var result = new LambertConformalConic(
                            IRI.Sta.CoordinateSystem.Ellipsoids.Clarke1866,
                            33.0,
                            45.0,
                            -96.0,
                            23.0).ToGeodetic(new Point(1894410.9, 1564649.5));

            Assert.AreEqual(result.X, expectedLambda, .00001);
            Assert.AreEqual(result.Y, expectedPhi, .00001);

            var ellipse = IRI.Sta.CoordinateSystem.Ellipsoids.GRS80;

            double phi0 = 52;
            double phi1 = 35;
            double phi2 = 65;

            double lambda0 = 10;

            var projection = new LambertConformalConic(ellipse, phi1, phi2, lambda0, phi0);


            //China
            double x = 10000000 - 4000000;
            double y = 4000000 - 2800000;

            result = projection.ToGeodetic(new Point(x, y));

            Assert.AreEqual(result.X, 85.22674468408002, 0.000001);
            Assert.AreEqual(result.Y, 32.27382257394294, 0.000001);

            //Canada
            x = -800000 - 4000000;
            y = 7000000 - 2800000;

            result = projection.ToGeodetic(new Point(x, y));

            Assert.AreEqual(result.X, -95.34976832367953, 0.000001);
            Assert.AreEqual(result.Y, 52.469308415860205, 0.000001);

            //Brazil
            x = -5000000 - 4000000;
            y = -350000 - 2800000;

            result = projection.ToGeodetic(new Point(x, y));

            Assert.AreEqual(result.X, -52.14245756518865, 0.000001);
            Assert.AreEqual(result.Y, -5.873476680963928, 0.000001);


            //Autralia
            x = 19000000 - 4000000;
            y = 9000000 - 2800000;

            result = projection.ToGeodetic(new Point(x, y));

            Assert.AreEqual(result.X, 132.45309686067716, 0.000001);
            Assert.AreEqual(result.Y, -21.744457361769854, 0.000001);
        }

        [TestMethod]
        public void TestLccToGeographicIterative()
        {
            double phi1 = 33.0;
            double phi2 = 45.0;
            double phi0 = 23.0;

            double lambda0 = -96.0;

            double x = 1894410.9;
            double y = 1564649.5;

            double expectedPhi = 35.0;
            double expectedLambda = -75.0;

            var projection = new LambertConformalConic(
                IRI.Sta.CoordinateSystem.Ellipsoids.Clarke1866,
                phi1,
                phi2,
                lambda0,
                phi0);

            var result = projection.LCCToGeodeticIterative(new Point(x, y));

            Assert.AreEqual(result.X, expectedLambda, .00001);
            Assert.AreEqual(result.Y, expectedPhi, .00001);
        }


        //[TestMethod]
        //public void TestLCCDirect()
        //{


        //}

        //[TestMethod]
        //public void TestLCCInverse()
        //{

        //}

        [TestMethod]
        public void TestNiocLcc()
        {
            var niocLcc = DefaultMapProjections.LccNiocWithClarcke1880Rgs;

            double wgsPhi, wgsLambda, x_1, y_1;

            //lat/long are in WGS84
            //x_1 = 1780017.307; y_1 = 1225546.350; wgsLambda = 48.003831; wgsPhi = 32.997924;
            //x_1 = 1686682.568; y_1 = 1223335.127; wgsLambda = 47.00245; wgsPhi = 32.997744;
            x_1 = 1687728.029; y_1 = 1167963.306; wgsLambda = 47.002509; wgsPhi = 32.497152;

            var lccPoint = niocLcc.FromWgs84Geodetic(new Point(wgsLambda, wgsPhi));

            Assert.AreEqual(lccPoint.X, x_1, 0.05);
            Assert.AreEqual(lccPoint.Y, y_1, 0.05);

            var geographicPoint = niocLcc.ToWgs84Geodetic(new Point(x_1, y_1));

            Assert.AreEqual(geographicPoint.X, wgsLambda, 1E-6);
            Assert.AreEqual(geographicPoint.Y, wgsPhi, 1E-6);

        }

        [TestMethod]
        public void TestNiocLcc2()
        {
            var niocLcc = DefaultMapProjections.LccNiocWithClarcke1880Rgs;

            double wgsPhi, wgsLambda, x_1, y_1;

            //lat/long are in WGS84
            //x_1 = 1780017.307; y_1 = 1225546.350; wgsLambda = 48.003831; wgsPhi = 32.997924;
            //x_1 = 1686682.568; y_1 = 1223335.127; wgsLambda = 47.00245; wgsPhi = 32.997744;
            x_1 = 2047473.33479; y_1 = 912594.777238; wgsLambda = 50.689721; wgsPhi = 30.072906;

            var lccPoint = niocLcc.FromWgs84Geodetic(new Point(wgsLambda, wgsPhi));

            Assert.AreEqual(lccPoint.X, x_1, 0.05);
            Assert.AreEqual(lccPoint.Y, y_1, 0.05);

            var geographicPoint = niocLcc.ToWgs84Geodetic(new Point(x_1, y_1));

            Assert.AreEqual(geographicPoint.X, wgsLambda, 1E-6);
            Assert.AreEqual(geographicPoint.Y, wgsPhi, 1E-6);

        }

    }
}
