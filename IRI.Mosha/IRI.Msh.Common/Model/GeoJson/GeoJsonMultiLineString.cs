
using IRI.Msh.Common.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Msh.Common.Model.GeoJson
{
    [JsonConverter(typeof(GeoJsonGeometryConverter))]
    public class GeoJsonMultiLineString : IGeoJsonGeometry
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("coordinates")]
        public double[][][] Coordinates { get; set; }

        public GeometryType GeometryType { get => GeometryType.MultiLineString; }


        public bool IsNullOrEmpty()
        {
            return Coordinates == null || Coordinates.Length < 1;
        }

        public Geometry Parse(bool isLongitudeFirst = false, int srid = 0)
        {
            return new Geometry(Coordinates?.Select(c => Geometry.ParseLineStringToGeometry(c, GeometryType.LineString, isLongitudeFirst, srid)).ToArray(), this.GeometryType, srid);
        }
    }
}
