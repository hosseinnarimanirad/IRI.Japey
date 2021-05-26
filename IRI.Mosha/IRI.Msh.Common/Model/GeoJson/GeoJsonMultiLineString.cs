
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

        public Geometry<Point> Parse(bool isLongitudeFirst = true, int srid = SridHelper.GeodeticWGS84)
        {
            return new Geometry<Point>(Coordinates?.Select(c => Geometry<Point>.ParseLineStringToGeometry(c, GeometryType.LineString, false, isLongitudeFirst, srid)).ToList(), this.GeometryType, srid);
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
            return Coordinates == null ? 0 : Coordinates.Sum(part => part == null ? 0 : part.Length);
        }
    }
}
