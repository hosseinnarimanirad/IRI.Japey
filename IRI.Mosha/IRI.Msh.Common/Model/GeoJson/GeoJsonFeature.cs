using System.Collections.Generic;

namespace IRI.Msh.Common.Model.GeoJson
{
    public class GeoJsonFeature
    {
        public string type { get; set; }
        public string id { get; set; }
        public IGeoJsonGeometry geometry { get; set; }
        public string geometry_name { get; set; }
        public Dictionary<string, string> properties { get; set; }
    }

}
