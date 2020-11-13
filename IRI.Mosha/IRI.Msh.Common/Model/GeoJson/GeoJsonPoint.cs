
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
    public class GeoJsonPoint : IGeoJsonGeometry
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("coordinates")]
        public double[] Coordinates { get; set; }

        public GeometryType GeometryType { get => GeometryType.Point; }

        public bool IsNullOrEmpty()
        {
            return Coordinates == null || Coordinates.Length < 1;
        }

        public Geometry<Point> Parse(bool isLongitudeFirst = false, int srid = 0)
        {
            return Geometry<Point>.ParsePointToGeometry(Coordinates, isLongitudeFirst, srid);
        }

        public string Serialize(bool indented, bool removeSpaces = false)
        {
            return GeoJson.Serialize(this, indented, removeSpaces);
        }
    }

}
