﻿
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
    public class GeoJsonMultiPoint : IGeoJsonGeometry
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("coordinates")]
        public double[][] Coordinates { get; set; }

        public GeometryType GeometryType { get => GeometryType.MultiPoint; }

        public bool IsNullOrEmpty()
        {
            return Coordinates == null || Coordinates.Length < 1;
        }

        public Geometry<Point> Parse(bool isLongitudeFirst = false, int srid = 0)
        {
            return new Geometry<Point>(Coordinates?.Select(c => Geometry<Point>.ParsePointToGeometry(c, isLongitudeFirst)).ToList(), this.GeometryType, srid);
        }

        public string Serialize(bool indented, bool removeSpaces = false)
        {
            return GeoJson.Serialize(this, indented, removeSpaces);
        }
    }
}
