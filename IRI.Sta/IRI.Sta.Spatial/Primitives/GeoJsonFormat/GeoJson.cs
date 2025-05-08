using IRI.Sta.Common.Helpers;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace IRI.Sta.Spatial.Model.GeoJsonFormat;

public static class GeoJson
{
    public static IEnumerable<GeoJsonFeature> ReadFeatures(string fileName)
    {
        var geoJsonString = System.IO.File.ReadAllText(fileName);

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


}
