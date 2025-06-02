using IRI.Sta.Spatial.Primitives;
using IRI.Sta.Common.Primitives;
using System.Text.Json.Serialization;
using IRI.Sta.SpatialReferenceSystem;
using IRI.Sta.Spatial.GeoJsonFormat;

namespace IRI.Sta.Spatial.GeoJsonFormat;

//[JsonConverter(typeof(GeoJsonGeometryConverter))]
public class GeoJsonPoint : IGeoJsonGeometry
{
    [JsonIgnore]
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("coordinates")]
    public double[] Coordinates { get; set; }

    [JsonIgnore]
    public GeometryType GeometryType { get => GeometryType.Point; }

    public bool IsNullOrEmpty()
    {
        return Coordinates == null || Coordinates.Length < 1;
    }

    public Geometry<Point> Parse(bool isLongitudeFirst = true, int srid = 0)
    {
        return Geometry<Point>.ParsePointToGeometry(Coordinates, isLongitudeFirst, srid);
    }

    public string Serialize(bool indented, bool removeSpaces = false)
    {
        return GeoJson.Serialize(this, indented, removeSpaces);
    }

    public int NumberOfGeometries()
    {
        return 1;
    }

    public int NumberOfPoints()
    {
        return 1;
    }

    public static GeoJsonPoint Create(double longitude, double latitude)
    {
        return new GeoJsonPoint() { Coordinates = [longitude, latitude], Type = GeoJson.Point };
    }

    public Geometry<Point> TransformToWeMercator(bool isLongitudeFirst = true)
    {
        return this.Parse(isLongitudeFirst, SridHelper.GeodeticWGS84)
                    .Transform(MapProjects.GeodeticWgs84ToWebMercator, SridHelper.WebMercator);                          
    }
}
