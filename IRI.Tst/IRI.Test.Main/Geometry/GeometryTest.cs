using IRI.Extensions;
using IRI.Sta.Common.Primitives;
using IRI.Sta.Spatial.Primitives;
using IRI.Test.NetFrameworkTest.Assets;
using Xunit;

namespace IRI.Test.Main.TheGeometry;

public class GeometryTests
{
    public GeometryTests()
    {
        SqlServerTypes.Utilities.LoadNativeAssembliesv14();
    }

    [Fact]
    public void LineString_CalculateEuclideanLength_ShouldMatchSqlLength()
    {
        // Arrange
        var geometry = SqlGeometrySamples.Linestring.AsGeometry();
        var expected = geometry.CalculateEuclideanLength();

        // Act
        var actual = geometry.AsSqlGeometry().STLength().Value;

        // Assert
        Assert.Equal(expected, actual, precision: 5);
    }

    [Fact]
    public void Polygon_CalculateUnsignedArea_ShouldMatchSqlArea()
    {
        // Arrange
        var geometry = SqlGeometrySamples.Polygon.AsGeometry();
        var expected = geometry.CalculateUnsignedEuclideanArea();

        // Act
        var actual = geometry.AsSqlGeometry().STArea().Value;

        // Assert
        Assert.Equal(expected, actual, precision: 5);
    }

    [Fact]
    public void LineString_CalculateMeanAngularChange_ShouldMatchExpected()
    {
        // Arrange
        var geometry = SqlGeometrySamples.LineString_ForAngularChange.AsGeometry();
        var expectedDegrees = 72.1864;

        // Act
        var actualDegrees = geometry.CalculateMeanAngularChange() * 180 / Math.PI;

        // Assert
        Assert.Equal(expectedDegrees, actualDegrees, precision: 4);
    }

    [Fact]
    public void Polygon_CalculateMeanAngularChange_ShouldMatchExpected()
    {
        // Arrange
        var geometry = SqlGeometrySamples.Polygon_ForAngularChange.AsGeometry();
        var expectedDegrees = 75.687;

        // Act
        var actualDegrees = geometry.CalculateMeanAngularChange() * 180 / Math.PI;

        // Assert
        Assert.Equal(expectedDegrees, actualDegrees, precision: 4);
    }

    [Fact]
    public void VectorDisplacement_ShouldMatchExpected()
    {
        // Arrange
        var original = SqlGeometrySamples.LineString_ForVectorDisplacement_Original.AsGeometry();
        var simplified = SqlGeometrySamples.LineString_ForVectorDisplacement_Simplified.AsGeometry();
        var expectedDisplacement = 6.324;

        // Act
        var actualDisplacement = original.CalculateTotalVectorDisplacement(simplified);

        // Assert
        Assert.Equal(expectedDisplacement, actualDisplacement, precision: 3);
    }
}
