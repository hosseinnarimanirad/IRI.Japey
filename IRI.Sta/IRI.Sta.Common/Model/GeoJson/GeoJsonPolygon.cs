
using IRI.Sta.Common.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Sta.Common.Model.GeoJson
{

    [JsonConverter(typeof(GeoJsonGeometryConverter))]
    public class GeoJsonPolygon : IGeoJsonGeometry
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("coordinates")]
        public double[][][] Coordinates { get; set; }

        public GeometryType GeometryType { get => GeometryType.Polygon; }

        public bool IsNullOrEmpty()
        {
            return Coordinates == null || Coordinates.Length < 1;
        }

        public Geometry Parse(bool isLongitudeFirst = false, int srid = 0)
        {
            return Geometry.ParsePolygonToGeometry(Coordinates, this.GeometryType, isLongitudeFirst, srid);
        }
    }
}
