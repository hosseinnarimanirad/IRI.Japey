using IRI.Ket.Common.Model.GeoJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.Common.Model
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
