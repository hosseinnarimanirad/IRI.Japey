using System.Text.Json.Nodes;
using IRI.Sta.Common.Helpers;

namespace IRI.Sta.Spatial.GeoJsonFormat;

public static class GeoJson
{
    public static IEnumerable<GeoJsonFeature> ReadFeatures(string fileName)
    {
        var geoJsonString = File.ReadAllText(fileName);

        //var parsedObject = Newtonsoft.Json.Linq.JObject.Parse(geoJsonString);
        //return parsedObject["features"].Select(f => JsonConvert.DeserializeObject<GeoJsonFeature>(f.ToString()));

        var parsedObject = JsonNode.Parse(geoJsonString);

        return parsedObject?["features"]?.AsArray().Select(f => JsonHelper.Deserialize<GeoJsonFeature>(f.ToString())) ?? Enumerable.Empty<GeoJsonFeature>();
    }


    public static IEnumerable<GeoJsonFeature> ParseToGeoJsonFeatures(string geoJsonFeatureSet)
    {
        return GeoJsonFeatureSet.Parse(geoJsonFeatureSet).Features;
    }

    public const string Point = "Point";
    public const string MultiPoint = "MultiPoint";
    public const string LineString = "LineString";
    public const string MultiLineString = "MultiLineString";
    public const string Polygon = "Polygon";
    public const string MultiPolygon = "MultiPolygon";

    internal static string Serialize(IGeoJsonGeometry geoJson, bool indented, bool removeSpaces = false)
    {
        var result = JsonHelper.Serialize(geoJson, indented);

        return removeSpaces ? result.Replace(" ", string.Empty) : result;
    }

    public static IGeoJsonGeometry Deserialize(string geoJsonString)
    {
        return JsonHelper.Deserialize<IGeoJsonGeometry>(geoJsonString);
    }

    internal static GeoJsonFeature AsFeature(IGeoJsonGeometry geometry)
    {
        return GeoJsonFeature.Create(geometry);
    }

    internal static GeoJsonFeatureSet AsFeatureSet(IGeoJsonGeometry geometry)
    {
        return new GeoJsonFeatureSet() { Features = new List<GeoJsonFeature>() { AsFeature(geometry) }, TotalFeatures = 1 };
    }

}
