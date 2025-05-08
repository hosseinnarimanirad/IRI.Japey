using IRI.Sta.Spatial.Primitives;
using IRI.Sta.CoordinateSystems.MapProjection;
using IRI.Sta.Common.Primitives;
using System.Text.Json.Serialization;
//using IRI.Sta.Spatial.Primitives.GeoJson.Converters;

namespace IRI.Sta.Spatial.Model.GeoJsonFormat;

//[JsonConverter(typeof(GeoJsonGeometryConverter))]
public class GeoJsonLineString : IGeoJsonGeometry
{
    [JsonIgnore]
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("coordinates")]
    public double[][] Coordinates { get; set; }

    [JsonIgnore]
    public GeometryType GeometryType { get => GeometryType.LineString; }

    public bool IsNullOrEmpty()
    {
        return Coordinates == null || Coordinates.Length < 1;
    }

    public Geometry<Point> Parse(bool isLongitudeFirst = true, int srid = 0)
    {
        return Geometry<Point>.ParseLineStringToGeometry(Coordinates, this.GeometryType, false, isLongitudeFirst, srid);
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
        return Coordinates == null ? 0 : Coordinates.Length;
    }

    public Geometry<Point> TransformToWeMercator(bool isLongitudeFirst = true)
    {
        return this.Parse(isLongitudeFirst, SridHelper.GeodeticWGS84)
                    .Transform(MapProjects.GeodeticWgs84ToWebMercator, SridHelper.WebMercator);
    }
}
