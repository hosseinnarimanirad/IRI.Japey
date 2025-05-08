using IRI.Sta.Common.Helpers;
using System.Text.Json.Serialization;

namespace IRI.Sta.Spatial.Model.GeoJsonFormat;


public class GeoJsonFeatureSet
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "FeatureCollection";

    [JsonPropertyName("totalFeatures")]
    public int TotalFeatures { get; set; }

    [JsonPropertyName("features")]
    public List<GeoJsonFeature> Features { get; set; }

    [JsonPropertyName("crs")]
    public GeoJsonCrs Crs { get; set; }

    public void Save(string fileName, bool indented, bool removeSpaces = false)
    {
        //var result = Newtonsoft.Json.JsonConvert.SerializeObject(this, indented ? Formatting.Indented : Formatting.None);
        var result = JsonHelper.Serialize(this, indented);

        System.IO.File.WriteAllText(fileName, removeSpaces ? result.Replace(" ", string.Empty) : result);
    }

    public static GeoJsonFeatureSet Load(string fileName)
    {
        return Parse(System.IO.File.ReadAllText(fileName));
    }

    public static GeoJsonFeatureSet Parse(string geoJsonFeaturesSetString)
    {
        return JsonHelper.Deserialize<GeoJsonFeatureSet>(geoJsonFeaturesSetString);
    }
}

public class GeoJsonCrs
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("properties")]
    public Properties Properties { get; set; }
}

public class Properties
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
}
