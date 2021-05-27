﻿using IRI.Msh.Common.Model.GeoJson;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Msh.Common.Model.GeoJson
{
    public static class GeoJsonExtensions
    {
        public static GeoJsonFeature AsFeature(this IGeoJsonGeometry geometry)
        {
            return GeoJsonFeature.Create(geometry);
        }

        public static GeoJsonFeatureSet AsFeatureSet(this IGeoJsonGeometry geometry)
        {
            return new GeoJsonFeatureSet() { Features = new List<GeoJsonFeature>() { geometry.AsFeature() }, TotalFeatures = 1 };
        }
    }
}
