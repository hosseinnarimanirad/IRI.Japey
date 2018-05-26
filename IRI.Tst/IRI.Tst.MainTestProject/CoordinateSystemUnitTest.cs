using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IRI.Sta.Common.Primitives;
using IRI.Sta.CoordinateSystem;

namespace IRI.Test.MainTestProject
{
    [TestClass]
    public class CoordinateSystemUnitTest
    {

        [TestMethod]
        [TestProperty("Author", "Hossein Narimani Rad")]
        public void TestGeodeticToAlbersEqualAreaConic()
        {
            double phi1 = 29.5;     //Degree
            double phi2 = 45.5;     //Degree
            double phi0 = 23;       //Degree    
            double lambda0 = -96;   //Degree
            double phi = 35;        //Degree
            double lambda = -75;    //Degree
            double expectedResultForX = 1885472.7;     //Meter
            double expectedResultForY = 1535925.0;     //Meter
            const double delta = 0.1;

            double[][] actualResult = IRI.Sta.CoordinateSystem.MapProjection.MapProjects.GeodeticToAlbersEqualAreaConic(
                new double[] { lambda },
                new double[] { phi },
                Ellipsoids.Clarke1866,
                lambda0,
                phi0,
                phi1,
                phi2);


            Assert.AreEqual(expectedResultForX, actualResult[0][0], delta);
            Assert.AreEqual(expectedResultForY, actualResult[1][0], delta);

        }

        //[TestMethod]
        //[TestProperty("Author", "Hossein Narimani Rad")]
        //public void TestMercatorToWebMercator()
        //{
        //    //Point mercator = new Point(5723402.4442, 3421545.5449);

        //    Point geodetic = new Point(51.414199, 29.524690);

        //    var calculatedMercator = Projection.GeodeticToMercator(geodetic);

        //    var geocentric = Transformation.ChangeDatum(geodetic, Ellipsoids.WGS84, Ellipsoids.Sphere);

        //    var calculatedWebMercator = Projection.GeodeticToMercator(geocentric, Ellipsoids.Sphere);

        //    //var webMercator = Projection.MercatorToWebMercator(mercator);
        //}


        [TestMethod]
        public void TestLccNahrawan()
        {
            var wgsGeodeticPoint = new Point(51 + 22.0 / 60.0 + 12.72 / 3600.0, 29 + 14.0 / 60.0 + 14.68 / 3600.0);
            var nahrawanGeodeticPoint = new Point(51 + 22.0 / 60.0 + 09.42 / 3600.0, 29 + 14.0 / 60.0 + 08.65 / 3600.0);

            var lccNahrawanPoint = new Point(2119090.03, 823058.15);


            var result = IRI.Sta.CoordinateSystem.MapProjection.DefaultMapProjections.LccNahrawan.FromGeodetic(nahrawanGeodeticPoint);

            Assert.AreEqual(lccNahrawanPoint.X, result.X, 1);
            Assert.AreEqual(lccNahrawanPoint.Y, result.Y, 2);
        }


        [TestMethod]
        public void TestChangeDatum()
        {
            var wgsGeodeticPoint = new Point(51 + 22.0 / 60.0 + 12.72 / 3600.0, 29 + 14.0 / 60.0 + 14.68 / 3600.0);
            var nahrawanGeodeticPoint = new Point(51 + 22.0 / 60.0 + 09.42 / 3600.0, 29 + 14.0 / 60.0 + 08.65 / 3600.0);

            var result = Transformation.ChangeDatum(wgsGeodeticPoint, Ellipsoids.WGS84, Ellipsoids.FD58);
             
            Assert.AreEqual(nahrawanGeodeticPoint.X, result.X, 1E-4);
            Assert.AreEqual(nahrawanGeodeticPoint.Y, result.Y, 1E-3);
        }
    }
}
