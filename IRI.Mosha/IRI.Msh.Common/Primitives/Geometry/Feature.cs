using IRI.Msh.Common.Model.GeoJson;
using System;
using System.Collections.Generic;
using System.Text;
using IRI.Msh.Common.Extensions;

namespace IRI.Msh.Common.Primitives
{
    public class Feature<T> : IGeometryAware<T> where T : IPoint, new()
    {
        public int Id { get; set; }

        public Geometry<T> TheGeometry { get; set; }

        public Dictionary<string, object> Attributes { get; set; }

        public GeoJsonFeature AsGeoJsonFeature()
        {
            return new GeoJsonFeature() { Geometry = TheGeometry.AsGeoJson(), Id = Id.ToString(), Properties = Attributes };
        }
    }
}
