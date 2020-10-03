
using IRI.Msh.Common.Primitives;
using IRI.Msh.CoordinateSystem.MapProjection;
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

        public Geometry<Point> Parse(bool isLongitudeFirst = false, int srid = SridHelper.GeodeticWGS84)
        {
            return new Geometry<Point>(Coordinates?.Select(c => Geometry<Point>.ParseLineStringToGeometry(c, GeometryType.LineString, isLongitudeFirst, srid)).ToList(), this.GeometryType, srid);
        }

        public string Serialize(bool indented)
        {
            return GeoJson.Serialize(this, indented);
        }
    }
}
