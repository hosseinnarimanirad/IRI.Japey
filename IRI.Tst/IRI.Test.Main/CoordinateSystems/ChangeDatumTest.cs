using System;
using IRI.Sta.Spatial.Primitives;
using IRI.Sta.SpatialReferenceSystem;
using IRI.Sta.Metrics;
using IRI.Sta.Common.Primitives;


namespace IRI.Test.CoordinateSystems;


public class ChangeDatumTest
{
    [Fact]
    public void TestGeodeticToAT()
    {
        var ellipsoid = Ellipsoids.WGS84;

        var phi = 35.123456;

        var lambda = 51.123456;

        var testPoint = new IRI.Sta.Common.Primitives.Point(lambda, phi);

        var result1 = Transformations.ToCartesian(testPoint, ellipsoid);

        var result2 =
            new GeodeticPoint<Meter, Degree>(ellipsoid, new Meter(0),
            new Degree(lambda),
            new Degree(phi)).ToCartesian<Meter>();


        Assert.Equal(result2.X.Value, result1.X, 9/* 1E-9*/);
        Assert.Equal(result2.Y.Value, result1.Y, 9 /*1E-9*/);
        Assert.Equal(result2.Z.Value, result1.Z, 9 /*1E-9*/);


        var result3 = Transformations.ToGeodetic<Point>(result1, ellipsoid);


        Assert.Equal(testPoint.X, result3.X, 9 /*1E-9*/);
        Assert.Equal(testPoint.Y, result3.Y, 9 /*1E-9*/);
        //Assert.Equal(result1.Z, result3.Z, 1E-9);
    }
}
