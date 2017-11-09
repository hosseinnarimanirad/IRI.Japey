using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IRI.Test.CoordinateSystem
{
    [TestClass]
    public class ChangeDatumTest
    {
        [TestMethod]
        public void TestGeodeticToAT()
        {
            var ellipsoid = IRI.Ham.CoordinateSystem.Ellipsoids.WGS84;

            var phi = 35.123456;

            var lambda = 51.123456;

            var testPoint = new IRI.Ham.SpatialBase.Point(lambda, phi);

            var result1 = IRI.Ham.CoordinateSystem.Transformation.ToCartesian(testPoint, ellipsoid);

            var result2 =
                new IRI.Ham.CoordinateSystem.GeodeticPoint<Ham.MeasurementUnit.Meter, Ham.MeasurementUnit.Degree>(ellipsoid, new Ham.MeasurementUnit.Meter(0),
                new Ham.MeasurementUnit.Degree(lambda),
                new Ham.MeasurementUnit.Degree(phi)).ToCartesian<Ham.MeasurementUnit.Meter>();


            Assert.AreEqual(result2.X.Value, result1.X, 1E-9);
            Assert.AreEqual(result2.Y.Value, result1.Y, 1E-9);
            Assert.AreEqual(result2.Z.Value, result1.Z, 1E-9);


            var result3 = IRI.Ham.CoordinateSystem.Transformation.ToGeodetic(result1, ellipsoid);


            Assert.AreEqual(testPoint.X, result3.X, 1E-9);
            Assert.AreEqual(testPoint.Y, result3.Y, 1E-9);
            //Assert.AreEqual(result1.Z, result3.Z, 1E-9);
        }
    }
}
