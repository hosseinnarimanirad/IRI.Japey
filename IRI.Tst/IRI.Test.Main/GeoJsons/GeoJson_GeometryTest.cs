using IRI.Extensions;
using IRI.Sta.Common.Primitives;
using IRI.Sta.Spatial.Primitives;
using IRI.Sta.Spatial.GeoJsonFormat;

namespace IRI.Test.GeoJsons;

public class GeoJson_GeometryTest
{
    [Fact]
    public void TestGeoJson_GeometryConversion()
    {
        // ---------- Test Point ----------
        // ARRANGE
        var geoJsonPoint = Geometry<Point>.Create(30, 10).AsGeoJson();

        // ACT
        var point = geoJsonPoint.Parse(true);

        // ASSERT
        Assert.Equal(30, point.Points[0].X);
        Assert.Equal(10, point.Points[0].Y);

        // ---------- Test MultiPoint ----------
        // ARRANGE
        var multiPointString = "{\"type\": \"MultiPoint\", \"coordinates\": [[10.1, 40.1], [40.1, 30.1], [20.1, 20.1], [30.1, 10.1]]}".Replace(" ", string.Empty);

        // ACT
        IGeoJsonGeometry geoJsonMultiPoint = GeoJson.Deserialize(multiPointString);
        var parsedMultiPoint = geoJsonMultiPoint.Parse(true).AsGeoJson(true).Serialize(false, true);

        // ASSERT
        Assert.Equal(multiPointString, parsedMultiPoint);

        // ---------- Test LineString ----------
        // ARRANGE
        var lineStringString = "{\"type\": \"LineString\", \"coordinates\": [[30.1, 10.1], [10.1, 30.1], [40.1, 40.1]]}".Replace(" ", string.Empty);

        // ACT
        IGeoJsonGeometry lineString = GeoJson.Deserialize(lineStringString);
        var parsedLineString = lineString.Parse(true).AsGeoJson(true).Serialize(false, true);

        // ASSERT
        Assert.Equal(lineStringString, parsedLineString);

        // ---------- Test MultiLineString ----------
        // ARRANGE
        var multiLineStringString = "{\"type\": \"MultiLineString\",\"coordinates\": [[[10.1, 10.1], [20.1, 20.1], [10.1, 40.1]],[[40.1, 40.1], [30.1, 30.1], [40.1, 20.1], [30.1, 10.1]]]}".Replace(" ", string.Empty);

        // ACT
        IGeoJsonGeometry multiLineString = GeoJson.Deserialize(multiLineStringString);
        var parsedMultiLineString = multiLineString.Parse(true).AsGeoJson(true).Serialize(false, true);

        // ASSERT
        Assert.Equal(multiLineStringString, parsedMultiLineString);

        // ---------- Test Polygon (1 ring) ----------
        // ARRANGE
        var polygonString1 = "{\"type\": \"Polygon\",\"coordinates\": [[[30.1, 10.1], [40.1, 40.1], [20.1, 40.1], [10.1, 20.1], [30.1, 10.1]]]}".Replace(" ", string.Empty);

        // ACT
        IGeoJsonGeometry polygon1 = GeoJson.Deserialize(polygonString1);
        var parsedPolygon1 = polygon1.Parse(true).AsGeoJson(true).Serialize(false, true);

        // ASSERT
        Assert.Equal(polygonString1, parsedPolygon1);

        // ---------- Test Polygon (1 outer ring + 1 inner ring) ----------
        // ARRANGE
        var polygonString2 = "{\"type\": \"Polygon\",\"coordinates\": [[[35.1, 10.1], [45.1, 45.1], [15.1, 40.1], [10.1, 20.1], [35.1, 10.1]],[[20.1, 30.1], [35.1, 35.1], [30.1, 20.1], [20.1, 30.1]]]}".Replace(" ", string.Empty);

        // ACT
        IGeoJsonGeometry polygon2 = GeoJson.Deserialize(polygonString2);
        var parsedPolygon2 = polygon2.Parse(true).AsGeoJson(true).Serialize(false, true);

        // ASSERT
        Assert.Equal(polygonString2, parsedPolygon2);

        // ---------- Test MultiPolygon (2 polygons with 1 ring each) ----------
        // ARRANGE
        var multiPolygonString1 = "{\"type\": \"MultiPolygon\",\"coordinates\": [[[[30.1, 20.1], [45.1, 40.1], [10.1, 40.1], [30.1, 20.1]]],[[[15.1, 5.1], [40.1, 10.1], [10.1, 20.1], [5.1, 10.1], [15.1, 5.1]]]]}".Replace(" ", string.Empty);

        // ACT
        IGeoJsonGeometry multiPolygon1 = GeoJson.Deserialize(multiPolygonString1);
        var parsedMultiPolygon1 = multiPolygon1.Parse(true).AsGeoJson(true).Serialize(false, true);

        // ASSERT
        Assert.Equal(multiPolygonString1, parsedMultiPolygon1);

        // ---------- Test MultiPolygon (1 polygon with 1 ring + 1 polygon with 2 rings) ----------
        // ARRANGE
        var multiPolygonString2 = "{\"type\": \"MultiPolygon\",\"coordinates\": [[[[40.1, 40.1], [20.1, 45.1], [45.1, 30.1], [40.1, 40.1]]],[[[20.1, 35.1], [10.1, 30.1], [10.1, 10.1], [30.1, 5.1], [45.1, 20.1], [20.1, 35.1]],[[30.1, 20.1], [20.1, 15.1], [20.1, 25.1], [30.1, 20.1]]]]}".Replace(" ", string.Empty);

        // ACT
        IGeoJsonGeometry multiPolygon2 = GeoJson.Deserialize(multiPolygonString2);
        var parsedMultiPolygon2 = multiPolygon2.Parse(true).AsGeoJson(true).Serialize(false, true);

        // ASSERT
        Assert.Equal(multiPolygonString2, parsedMultiPolygon2);
    }
}
