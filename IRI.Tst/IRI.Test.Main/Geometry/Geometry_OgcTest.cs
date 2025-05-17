using IRI.Extensions;
using IRI.Sta.Spatial.Primitives;
using IRI.Sta.Common.Primitives;
using Microsoft.SqlServer.Types;
using IRI.Sta.Spatial.IO.OgcSFA;


namespace IRI.Test.Main.TheGeometry;


public class Geometry_OgcTest
{
    public Geometry_OgcTest()
    {
        SqlServerTypes.Utilities.LoadNativeAssembliesv14();
    }

    #region Wkb

    [Fact]
    public void TestGeometryToWkb()
    {
        var Point = WktParser.Parse("POINT(1 2)");
        var MULTIPOINT1 = WktParser.Parse("MULTIPOINT((0 0), (0 3), (3 3), (3 0), (1 1), (9 9), (9 10), (10 9))");
        var MULTIPOINT2 = WktParser.Parse("MULTIPOINT((2 3), (7 8))");
        var LINESTRING1 = WktParser.Parse("LINESTRING(1 1, 2 0, 2 4, 3 3)");
        var LINESTRING2 = WktParser.Parse("LINESTRING(4 4, 9 0)");
        var MULTILINESTRING = WktParser.Parse("MULTILINESTRING((1 1, 3 5), (-5 3, -8 -2))");
        var POLYGON1 = WktParser.Parse("POLYGON((0 0, 30 0, 30 30, 0 30, 0 0))");
        var POLYGON2 = WktParser.Parse("POLYGON((-20 -20, -20 20, 20 20, 20 -20, -20 -20), (10 0, 0 10, 0 -10, 10 0))");
        var POLYGON3 = WktParser.Parse("POLYGON((-20 -20, -20 20, 20 20, 20 -20, -20 -20), (10 0, 0 10, 0 -10, 10 0), (-10 0, -10 10, -15 0, -10 0))");
        var MULTIPOLYGON1 = WktParser.Parse("MULTIPOLYGON(((0 0, 0 3, 3 3, 3 0, 0 0), (2 1, 1 2, 1 1, 2 1)), ((9 9, 9 10, 10 9, 9 9)))");
        var MULTIPOLYGON2 = WktParser.Parse("MULTIPOLYGON(((0 0, 0 6, 6 6, 6 0, 0 0), (5 5, 5 1, 1 1, 1 5, 5 5)), ((2 4, 2 2, 4 2, 4 4, 2 4),(3.5 3.5, 2.5 3.5, 2.5 2.5, 3.5 2.5, 3.5 3.5)))");

        Assert.Equal(Point.AsWkt(), FromWkb(Point.AsWkb()).AsWkt());
        Assert.Equal(MULTIPOINT1.AsWkt(), FromWkb(MULTIPOINT1.AsWkb()).AsWkt());
        Assert.Equal(MULTIPOINT2.AsWkt(), FromWkb(MULTIPOINT2.AsWkb()).AsWkt());
        Assert.Equal(LINESTRING1.AsWkt(), FromWkb(LINESTRING1.AsWkb()).AsWkt());
        Assert.Equal(LINESTRING2.AsWkt(), FromWkb(LINESTRING2.AsWkb()).AsWkt());
        Assert.Equal(MULTILINESTRING.AsWkt(), FromWkb(MULTILINESTRING.AsWkb()).AsWkt());
        Assert.Equal(POLYGON1.AsWkt(), FromWkb(POLYGON1.AsWkb()).AsWkt());
        Assert.Equal(POLYGON2.AsWkt(), FromWkb(POLYGON2.AsWkb()).AsWkt());
        Assert.Equal(POLYGON3.AsWkt(), FromWkb(POLYGON3.AsWkb()).AsWkt());
        Assert.Equal(MULTIPOLYGON1.AsWkt(), FromWkb(MULTIPOLYGON1.AsWkb()).AsWkt());
        Assert.Equal(MULTIPOLYGON2.AsWkt(), FromWkb(MULTIPOLYGON2.AsWkb()).AsWkt());
    }

    private Geometry<Point> FromWkb(byte[] data)
    {
        return SqlGeometry.STGeomFromWKB(new System.Data.SqlTypes.SqlBytes(data), 0).AsGeometry();
    }


    //[Fact]
    [Theory]
    [InlineData("POINT(1 2)")]
    [InlineData("MULTIPOINT((0 0), (0 3), (3 3), (3 0), (1 1), (9 9), (9 10), (10 9))")]
    [InlineData("MULTIPOINT((2 3), (7 8))")]
    [InlineData("LINESTRING(1 1, 2 0, 2 4, 3 3)")]
    [InlineData("LINESTRING(4 4, 9 0)")]
    [InlineData("MULTILINESTRING((1 1, 3 5), (-5 3, -8 -2))")]
    [InlineData("POLYGON((0 0, 30 0, 30 30, 0 30, 0 0))")]
    [InlineData("POLYGON((-20 -20, -20 20, 20 20, 20 -20, -20 -20), (10 0, 0 10, 0 -10, 10 0))")]
    [InlineData("POLYGON((-20 -20, -20 20, 20 20, 20 -20, -20 -20), (10 0, 0 10, 0 -10, 10 0), (-10 0, -10 10, -15 0, -10 0))")]
    [InlineData("MULTIPOLYGON(((0 0, 0 3, 3 3, 3 0, 0 0), (2 1, 1 2, 1 1, 2 1)), ((9 9, 9 10, 10 9, 9 9)))")]
    [InlineData("MULTIPOLYGON(((0 0, 0 6, 6 6, 6 0, 0 0), (1 5, 1 1, 5 1, 5 5, 1 5)), ((4 4, 4 2, 2 2, 2 4, 4 4),(3.5 3.5, 2.5 3.5, 2.5 2.5, 3.5 2.5, 3.5 3.5)))")]
    public void TestWkbToGeometry(string wktGeometry)
    {
        // ARRANGE
        //var Point = WktParser.Parse("POINT(1 2)");
        //var MULTIPOINT1 = WktParser.Parse("MULTIPOINT((0 0), (0 3), (3 3), (3 0), (1 1), (9 9), (9 10), (10 9))");
        //var MULTIPOINT2 = WktParser.Parse("MULTIPOINT((2 3), (7 8))");
        //var LINESTRING1 = WktParser.Parse("LINESTRING(1 1, 2 0, 2 4, 3 3)");
        //var LINESTRING2 = WktParser.Parse("LINESTRING(4 4, 9 0)");
        //var MULTILINESTRING = WktParser.Parse("MULTILINESTRING((1 1, 3 5), (-5 3, -8 -2))");
        //var POLYGON1 = WktParser.Parse("POLYGON((0 0, 30 0, 30 30, 0 30, 0 0))");
        //var POLYGON2 = WktParser.Parse("POLYGON((-20 -20, -20 20, 20 20, 20 -20, -20 -20), (10 0, 0 10, 0 -10, 10 0))");
        //var POLYGON3 = WktParser.Parse("POLYGON((-20 -20, -20 20, 20 20, 20 -20, -20 -20), (10 0, 0 10, 0 -10, 10 0), (-10 0, -10 10, -15 0, -10 0))");
        //var MULTIPOLYGON1 = WktParser.Parse("MULTIPOLYGON(((0 0, 0 3, 3 3, 3 0, 0 0), (2 1, 1 2, 1 1, 2 1)), ((9 9, 9 10, 10 9, 9 9)))");
        //var MULTIPOLYGON2 = WktParser.Parse("MULTIPOLYGON(((0 0, 0 6, 6 6, 6 0, 0 0), (1 5, 1 1, 5 1, 5 5, 1 5)), ((4 4, 4 2, 2 2, 2 4, 4 4),(3.5 3.5, 2.5 3.5, 2.5 2.5, 3.5 2.5, 3.5 3.5)))");

        var geometry = WktParser.Parse(wktGeometry);

        // ACT
        var actualWkt = Geometry<Point>.FromWkb(geometry.AsWkb(), 0).AsWkt();

        // ASSERT
        Assert.Equal(geometry.AsWkt(), actualWkt);

        //Assert.Equal(Point.AsWkt(), Geometry<Point>.FromWkb(Point.AsWkb(), 0).AsWkt());
        //Assert.Equal(MULTIPOINT1.AsWkt(), Geometry<Point>.FromWkb(MULTIPOINT1.AsWkb(), 0).AsWkt());
        //Assert.Equal(MULTIPOINT2.AsWkt(), Geometry<Point>.FromWkb(MULTIPOINT2.AsWkb(), 0).AsWkt());
        //Assert.Equal(LINESTRING1.AsWkt(), Geometry<Point>.FromWkb(LINESTRING1.AsWkb(), 0).AsWkt());
        //Assert.Equal(LINESTRING2.AsWkt(), Geometry<Point>.FromWkb(LINESTRING2.AsWkb(), 0).AsWkt());
        //Assert.Equal(MULTILINESTRING.AsWkt(), Geometry<Point>.FromWkb(MULTILINESTRING.AsWkb(), 0).AsWkt());
        //Assert.Equal(POLYGON1.AsWkt(), Geometry<Point>.FromWkb(POLYGON1.AsWkb(), 0).AsWkt());
        //Assert.Equal(POLYGON2.AsWkt(), Geometry<Point>.FromWkb(POLYGON2.AsWkb(), 0).AsWkt());
        //Assert.Equal(POLYGON3.AsWkt(), Geometry<Point>.FromWkb(POLYGON3.AsWkb(), 0).AsWkt());
        //Assert.Equal(MULTIPOLYGON1.AsWkt(), Geometry<Point>.FromWkb(MULTIPOLYGON1.AsWkb(), 0).AsWkt());
        //Assert.Equal(MULTIPOLYGON2.AsWkt(), Geometry<Point>.FromWkb(MULTIPOLYGON2.AsWkb(), 0).AsWkt());
    }

    #endregion


    #region Wkt

    [Fact]
    public void TestWktToGeometry()
    {
        var Point = "POINT(1 2)";
        var MULTIPOINT1 = "MULTIPOINT((0 0), (0 3), (3 3), (3 0), (1 1), (9 9), (9 10), (10 9))";
        var MULTIPOINT2 = "MULTIPOINT((2 3), (7 8))";
        var LINESTRING1 = "LINESTRING(1 1, 2 0, 2 4, 3 3)";
        var LINESTRING2 = "LINESTRING(4 4, 9 0)";
        var MULTILINESTRING = "MULTILINESTRING((1 1, 3 5), (-5 3, -8 -2))";
        var POLYGON1 = "POLYGON((0 0, 30 0, 30 30, 0 30, 0 0))";
        var POLYGON2 = "POLYGON((-20 -20, -20 20, 20 20, 20 -20, -20 -20), (10 0, 0 10, 0 -10, 10 0))";
        var POLYGON3 = "POLYGON((-20 -20, -20 20, 20 20, 20 -20, -20 -20), (10 0, 0 10, 0 -10, 10 0), (-10 0, -10 10, -15 0, -10 0))";
        var MULTIPOLYGON1 = "MULTIPOLYGON(((0 0, 0 3, 3 3, 3 0, 0 0), (2 1, 1 2, 1 1, 2 1)), ((9 9, 9 10, 10 9, 9 9)))";
        var MULTIPOLYGON2 = "MULTIPOLYGON(((0 0, 0 6, 6 6, 6 0, 0 0), (5 5, 5 1, 1 1, 1 5, 5 5)), ((2 4, 2 2, 4 2, 4 4, 2 4),(3.5 3.5, 2.5 3.5, 2.5 2.5, 3.5 2.5, 3.5 3.5)))";

        Assert.Equal(Point, WktParser.Parse(Point).AsWkt());
        Assert.Equal(MULTIPOINT1.Replace(", ", ","), WktParser.Parse(MULTIPOINT1).AsWkt());
        Assert.Equal(MULTIPOINT2.Replace(", ", ","), WktParser.Parse(MULTIPOINT2).AsWkt());
        Assert.Equal(LINESTRING1.Replace(", ", ","), WktParser.Parse(LINESTRING1).AsWkt());
        Assert.Equal(LINESTRING2.Replace(", ", ","), WktParser.Parse(LINESTRING2).AsWkt());
        Assert.Equal(MULTILINESTRING.Replace(", ", ","), WktParser.Parse(MULTILINESTRING).AsWkt());
        Assert.Equal(POLYGON1.Replace(", ", ","), WktParser.Parse(POLYGON1).AsWkt());
        Assert.Equal(POLYGON2.Replace(", ", ","), WktParser.Parse(POLYGON2).AsWkt());
        Assert.Equal(POLYGON3.Replace(", ", ","), WktParser.Parse(POLYGON3).AsWkt());
        Assert.Equal(MULTIPOLYGON1.Replace(", ", ","), WktParser.Parse(MULTIPOLYGON1).AsWkt());
        Assert.Equal(MULTIPOLYGON2.Replace(", ", ","), WktParser.Parse(MULTIPOLYGON2).AsWkt());
    }

    #endregion



}
