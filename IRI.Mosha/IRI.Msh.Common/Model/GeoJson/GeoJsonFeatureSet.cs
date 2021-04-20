using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Msh.Common.Model.GeoJson
{

    public class GeoJsonFeatureSet
    {
        public string type { get; set; }
        public int totalFeatures { get; set; }
        public List<GeoJsonFeature>  features { get; set; }
        public GeoJsonCrs crs { get; set; }
    }

    public class GeoJsonCrs
    {
        public string type { get; set; }
        public Properties properties { get; set; }
    }

    public class Properties
    {
        public string name { get; set; }
    }
     

}
