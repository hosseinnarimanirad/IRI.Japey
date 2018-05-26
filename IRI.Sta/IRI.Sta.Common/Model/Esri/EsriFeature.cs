using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Sta.Common.Model.Esri
{
    public class EsriFeature
    {
        public EsriJsonGeometry Geometry { get; set; }

        public Dictionary<string, string> Attributes { get; set; }
    }
}
