using Newtonsoft.Json;
using System.Collections.Generic;

namespace IRI.Msh.Common.Model.GeoJson
{
    public class GeoJsonFeature
    {
        [JsonProperty("type")]
        public string Type { get; set; } = "Feature";

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("geometry")]
        public IGeoJsonGeometry Geometry { get; set; }

        [JsonProperty("geometry_name")]
        public string Geometry_name { get; set; }

        [JsonProperty("properties")]
        public Dictionary<string, object> Properties { get; set; }
    }

}
