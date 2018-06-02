using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Msh.Common.Model.GeoJson
{

    public class GeoJsonFeatureCollection
    {
        public string type { get; set; }
        public int totalFeatures { get; set; }
        public GeoJsonFeature[] features { get; set; }
        public Crs crs { get; set; }
    }

    public class Crs
    {
        public string type { get; set; }
        public Properties properties { get; set; }
    }

    public class Properties
    {
        public string name { get; set; }
    }
     

}
