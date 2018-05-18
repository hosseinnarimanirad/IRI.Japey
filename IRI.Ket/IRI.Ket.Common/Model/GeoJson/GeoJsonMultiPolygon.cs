
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
    public class GeoJsonMultiPolygon : IGeoJsonGeometry
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("coordinates")]
        public double[][][][] Coordinates { get; set; }

        public GeoJsonType GeometryType { get => GeoJsonType.MultiPolygon; }
         
        public Geometry Parse()
        {
            throw new NotImplementedException();
        }
    }
}
