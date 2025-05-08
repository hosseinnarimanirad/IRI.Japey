
using IRI.Sta.Spatial.Primitives;
using IRI.Sta.CoordinateSystems.MapProjection;
using IRI.Sta.Common.Primitives;
using System.Text.Json.Serialization;

namespace IRI.Sta.Spatial.Model.GeoJsonFormat;

//[JsonConverter(typeof(GeoJsonGeometryConverter))]
public class GeoJsonMultiPoint : IGeoJsonGeometry
{
    private static readonly GeoJsonMultiPoint _empty = new GeoJsonMultiPoint() { Coordinates = [] };

    [JsonIgnore]
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("coordinates")]
    public double[][] Coordinates { get; set; }

    [JsonIgnore]
    public GeometryType GeometryType { get => GeometryType.MultiPoint; }

    public GeoJsonMultiPoint()
    {
        Type = GeoJson.MultiPoint;
    }

    public bool IsNullOrEmpty()
    {
        return Coordinates == null || Coordinates.Length < 1;
    }

    public Geometry<Point> Parse(bool isLongitudeFirst = true, int srid = 0)
    {
        return new Geometry<Point>(Coordinates?.Select(c => Geometry<Point>.ParsePointToGeometry(c, isLongitudeFirst)).ToList(), this.GeometryType, srid);
    }

    public string Serialize(bool indented, bool removeSpaces = false)
    {
        return GeoJson.Serialize(this, indented, removeSpaces);
    }

    public int NumberOfGeometries()
    {
        return Coordinates == null ? 0 : Coordinates.Length;
    }

    public int NumberOfPoints()
    {
        // 1400.02.03
        // number of parts equals number of points
        return NumberOfGeometries();
    }

    public Geometry<Point> TransformToWeMercator(bool isLongitudeFirst = true)
    {
        return this.Parse(isLongitudeFirst, SridHelper.GeodeticWGS84)
                    .Transform(MapProjects.GeodeticWgs84ToWebMercator, SridHelper.WebMercator);
    }

    public static GeoJsonMultiPoint Empty => _empty;
}
