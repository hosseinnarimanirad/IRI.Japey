using IRI.Ham.SpatialBase.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.Common.Model.GeoJson
{
    [JsonConverter(typeof(GeoJsonGeometryConverter))]
    public interface IGeoJsonGeometry
    {
        string Type { get; set; }

        GeoJsonType GeometryType { get; }

        Geometry Parse();
    }
}
