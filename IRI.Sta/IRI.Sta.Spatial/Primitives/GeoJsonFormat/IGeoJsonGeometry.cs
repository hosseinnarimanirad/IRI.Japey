using IRI.Sta.Common.Primitives;
using IRI.Sta.Spatial.Primitives; 
using System.Text.Json.Serialization;

namespace IRI.Sta.Spatial.GeoJsonFormat;

//[JsonConverter(typeof(GeoJsonGeometryConverter))]
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(GeoJsonPoint), "Point")]
[JsonDerivedType(typeof(GeoJsonMultiPoint), "MultiPoint")]
[JsonDerivedType(typeof(GeoJsonLineString), "LineString")]
[JsonDerivedType(typeof(GeoJsonMultiLineString), "MultiLineString")]
[JsonDerivedType(typeof(GeoJsonPolygon), "Polygon")]
[JsonDerivedType(typeof(GeoJsonMultiPolygon), "MultiPolygon")]
public interface IGeoJsonGeometry
{
    string Type { get; set; }
     
    GeometryType GeometryType { get; }

    Geometry<Point> Parse(bool isLongitudeFirst = true, int srid = 0);

    bool IsNullOrEmpty();

    string Serialize(bool indented, bool removeSpaces = false);

    int NumberOfGeometries();

    int NumberOfPoints();

    Geometry<Point> TransformToWeMercator(bool isLongitudeFirst = true);
     
}
