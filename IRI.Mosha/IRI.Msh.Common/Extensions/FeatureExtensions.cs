using IRI.Msh.Common.Model.GeoJson;
using IRI.Msh.Common.Primitives;
using IRI.Msh.CoordinateSystem.MapProjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Extensions
{
    public static class FeatureExtensions
    {

        public static GeoJsonFeature AsGeoJsonFeature<T>(this Feature<T> feature, Func<T, T> toWgs84Func, bool isLongitudeFirst) where T : IPoint, new()
        {
            return new GeoJsonFeature()
            {
                Geometry = feature.TheGeometry.Transform(toWgs84Func, SridHelper.GeodeticWGS84).AsGeoJson(isLongitudeFirst),
                Id = feature.Id.ToString(),
                Properties = feature.Attributes/*.ToDictionary(k => k.Key, k => k.Value)*/,

            };
        }

    }
}
