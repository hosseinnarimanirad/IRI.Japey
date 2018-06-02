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
            var ellipsoid = IRI.Sta.CoordinateSystem.Ellipsoids.WGS84;

            var phi = 35.123456;

            var lambda = 51.123456;

            var testPoint = new IRI.Msh.Common.Primitives.Point(lambda, phi);

            var result1 = IRI.Sta.CoordinateSystem.Transformation.ToCartesian(testPoint, ellipsoid);

            var result2 =
                new IRI.Sta.CoordinateSystem.GeodeticPoint<Sta.MeasurementUnit.Meter, Sta.MeasurementUnit.Degree>(ellipsoid, new Sta.MeasurementUnit.Meter(0),
                new Sta.MeasurementUnit.Degree(lambda),
                new Sta.MeasurementUnit.Degree(phi)).ToCartesian<Sta.MeasurementUnit.Meter>();


            Assert.AreEqual(result2.X.Value, result1.X, 1E-9);
            Assert.AreEqual(result2.Y.Value, result1.Y, 1E-9);
            Assert.AreEqual(result2.Z.Value, result1.Z, 1E-9);


            var result3 = IRI.Sta.CoordinateSystem.Transformation.ToGeodetic(result1, ellipsoid);


            Assert.AreEqual(testPoint.X, result3.X, 1E-9);
            Assert.AreEqual(testPoint.Y, result3.Y, 1E-9);
            //Assert.AreEqual(result1.Z, result3.Z, 1E-9);
        }
    }
}
