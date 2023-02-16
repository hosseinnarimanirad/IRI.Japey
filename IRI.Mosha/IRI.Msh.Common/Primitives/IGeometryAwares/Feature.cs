﻿using IRI.Msh.Common.Model.GeoJson;
using System;
using System.Collections.Generic;
using System.Text;
using IRI.Extensions;
using IRI.Msh.Common.Model;
using System.Linq;
using IRI.Msh.CoordinateSystem.MapProjection;
using System.Data;

namespace IRI.Msh.Common.Primitives;
public class Feature<T> : IGeometryAware<T> where T : IPoint, new()
{
    protected const string _defaultLabelAttributeName = "Label";

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

    public GeoJsonFeature AsGeoJsonFeature(Func<T, T> toWgs84Func, bool isLongitudeFirst)
    {
        return new GeoJsonFeature()
        {
            Geometry = this.TheGeometry.Transform(toWgs84Func, SridHelper.GeodeticWGS84).AsGeoJson(isLongitudeFirst),
            Id = this.Id.ToString(),
            Properties = this.Attributes/*.ToDictionary(k => k.Key, k => k.Value)*/,

        };
    }

    public override string ToString()
    {
        return $"Geometry: {TheGeometry?.Type}, Attributes: {Attributes?.Count}";
    }

}


public class Feature : Feature<Point>
{
    public Feature(Geometry<Point> geometry) : this(geometry, new Dictionary<string, object>())
    {

    }

    public Feature(Geometry<Point> geometry, string label) : this(geometry, new Dictionary<string, object>() { { _defaultLabelAttributeName, label } })
    {

    }

    public Feature(Geometry<Point> geometry, Dictionary<string, object> attributes) : base(geometry, attributes)
    {
    }

    public Feature()
    {

    }

    //public GeoJsonFeature AsGeoJsonFeature(Func<Point, Point> toWgs84Func, bool isLongitudeFirst)
    //{
    //    return new GeoJsonFeature()
    //    {
    //        Geometry = this.TheGeometry.Transform(toWgs84Func, SridHelper.GeodeticWGS84).AsGeoJson(isLongitudeFirst),
    //        Id = this.Id.ToString(),
    //        Properties = this.Attributes/*.ToDictionary(k => k.Key, k => k.Value)*/,

    //    };
    //}
}
