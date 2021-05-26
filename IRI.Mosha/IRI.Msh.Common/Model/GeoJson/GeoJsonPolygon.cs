
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

        public Geometry<Point> Parse(bool isLongitudeFirst = true, int srid = 0)
        {
            return Geometry<Point>.ParsePolygonToGeometry(Coordinates, this.GeometryType, isLongitudeFirst, srid);
        }

        public string Serialize(bool indented, bool removeSpaces = false)
        {
            return GeoJson.Serialize(this, indented, removeSpaces);
        }

        public int NumberOfGeometries()
        {
            return Coordinates == null ? 0 : Coordinates.Length;
        }

        public int NumberOfPoints()
        {
            return Coordinates == null ? 0 : Coordinates.Sum(ring => ring == null ? 0 : ring.Length);
        }
    }
}
