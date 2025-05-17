using System;

using IRI.Sta.SpatialReferenceSystem;
using IRI.Sta.Common.Primitives;
using IRI.Sta.SpatialReferenceSystem.MapProjections;

namespace IRI.Test.CoordinateSystems;


public class FourInOneTest
{
    [Fact]
    public void MainTest()
    {
        var clarke = Ellipsoids.Clarke1880Rgs;
        double phi0 = 32.5;
        double phi1 = 29.65508274166;
        double phi2 = 35.31468809166;
        double lambda0 = 45.0;
        var niocLcc = new LambertConformalConic2P(clarke, phi1, phi2, lambda0, phi0, 1500000.0, 1166200.0, 0.9987864078);

        var xLccNioc = 2047473.33479;
        var yLccNioc = 912594.777238;

        var xWgs84 = 50.689721;
        var yWgs84 = 30.072906;

        var xWebMercator = 5642753.9243;
        var yWebMercator = 3512924.70491;

        var xClarke1880Rgs = 50.689721;
        var yClarke1880Rgs = 30.075637;


        var wgs84 = niocLcc.ToWgs84Geodetic(new Point(xLccNioc, yLccNioc));

        Assert.Equal(xWgs84, wgs84.X, 6);
        Assert.Equal(yWgs84, wgs84.Y, 6);

        var clarke1880 = niocLcc.ToGeodetic(new Point(xLccNioc, yLccNioc));

        Assert.Equal(xClarke1880Rgs, clarke1880.X, 6);
        Assert.Equal(yClarke1880Rgs, clarke1880.Y, 6);

        var webMercator = MapProjects.GeodeticWgs84ToWebMercator(wgs84);

        Assert.Equal(xWebMercator, webMercator.X, 2 /*0.05*/);
        Assert.Equal(yWebMercator, webMercator.Y, 2 /*0.05*/);

        var clarke1880_2 = Transformations.ChangeDatum(wgs84, Ellipsoids.WGS84, Ellipsoids.Clarke1880Rgs);

        Assert.Equal(xClarke1880Rgs, clarke1880_2.X, 6);
        Assert.Equal(yClarke1880Rgs, clarke1880_2.Y, 6);

    }
}
