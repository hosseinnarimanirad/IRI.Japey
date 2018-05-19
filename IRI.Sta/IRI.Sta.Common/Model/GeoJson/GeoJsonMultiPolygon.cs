
using IRI.Ham.SpatialBase.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Sta.Common.Model.GeoJson
{
    [JsonConverter(typeof(GeoJsonGeometryConverter))]
    public class GeoJsonMultiPolygon : IGeoJsonGeometry
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("coordinates")]
        public double[][][][] Coordinates { get; set; }

        public GeometryType GeometryType { get => GeometryType.MultiPolygon; }

        public bool IsNullOrEmpty()
        {
            return Coordinates == null || Coordinates.Length < 1;
        }

        public Geometry Parse(bool isLongitudeFirst = false, int srid = 0)
        {
            return new Geometry(Coordinates?.Select(c => Geometry.ParsePolygonToGeometry(c, GeometryType.Polygon, isLongitudeFirst, srid)).ToArray(), this.GeometryType, srid);
        }
    }
}
