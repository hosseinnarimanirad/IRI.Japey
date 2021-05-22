using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

namespace IRI.Msh.Common.Model.GeoJson
{

    public class GeoJsonFeatureSet
    {
        [JsonProperty("type")]
        public string Type { get; set; } = "FeatureCollection";

        [JsonProperty("totalFeatures")]
        public int TotalFeatures { get; set; }

        [JsonProperty("features")]
        public List<GeoJsonFeature> Features { get; set; }

        [JsonProperty("crs")]
        public GeoJsonCrs Crs { get; set; }

        public void Save(string fileName, bool indented, bool removeSpaces = false)
        {
            var result = Newtonsoft.Json.JsonConvert.SerializeObject(this, indented ? Formatting.Indented : Formatting.None);

            System.IO.File.WriteAllText(fileName, removeSpaces ? result.Replace(" ", string.Empty) : result);
        }

        public static GeoJsonFeatureSet Load(string fileName)
        {
            return GeoJson.ParseToGeoJsonFeatureCollection(fileName);
        }
    }

    public class GeoJsonCrs
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("properties")]
        public Properties Properties { get; set; }
    }

    public class Properties
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }


}
