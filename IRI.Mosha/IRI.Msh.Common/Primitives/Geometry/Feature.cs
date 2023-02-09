using IRI.Msh.Common.Model.GeoJson;
using System;
using System.Collections.Generic;
using System.Text;
using IRI.Extensions;
using IRI.Msh.Common.Model;
using System.Linq;
using IRI.Msh.CoordinateSystem.MapProjection;

namespace IRI.Msh.Common.Primitives
{
    public class Feature<T> : IGeometryAware<T> where T : IPoint, new()
    {
        const string _defaultLabelAttributeName = "Label";

        public int Id { get; set; }

        public Geometry<T> TheGeometry { get; set; }

        public Dictionary<string, object> Attributes { get; set; }

        public string LabelAttribute { get; set; } = _defaultLabelAttributeName;

        public string Label
        {
            get
            {
                if (Attributes?.Keys.Any(k => k == LabelAttribute) == true)
                {
                    return Attributes[LabelAttribute]?.ToString();
                }

                return string.Empty;
            }
        }

        public Feature(Geometry<T> geometry) : this(geometry, new Dictionary<string, object>())
        {

        }

        public Feature(Geometry<T> geometry, string label) : this(geometry, new Dictionary<string, object>() { { _defaultLabelAttributeName, label } })
        {

        }

        public Feature(Geometry<T> geometry, Dictionary<string, object> attributes)
        {
            this.TheGeometry = geometry;

            this.Attributes = attributes;
        }

        public Feature()
        {

        }

        public GeoJsonFeature AsGeoJsonFeature()
        {
            return new GeoJsonFeature() { Geometry = TheGeometry.AsGeoJson(), Id = Id.ToString(), Properties = Attributes };
        }

        public override string ToString()
        {
            return $"Geometry: {TheGeometry?.Type}, Attributes: {Attributes?.Count}";
        }


    }


}
