﻿
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
    public interface IGeoJsonGeometry
    {
        string Type { get; set; }

        GeometryType GeometryType { get; }

        Geometry Parse(bool isLongitudeFirst = false, int srid = 0);

        bool IsNullOrEmpty();
    }
}