﻿using IRI.Extensions;
using IRI.Sta.Spatial.Primitives;
using IRI.Sta.CoordinateSystems.MapProjection;
using IRI.Sta.Common.Primitives;
using System.Text.Json.Serialization;

namespace IRI.Sta.Spatial.Mapping;

public abstract class IndexBase : IGeometryAware<Point>
{
    public int Id { get; set; }

    [JsonPropertyName("shne")]
    public virtual string SheetNameEn { get; set; }

    [JsonPropertyName("shnf")]
    public virtual string SheetNameFa { get; set; }

    [JsonPropertyName("shno")]
    public virtual string SheetNumber { get; set; }

    [JsonPropertyName("minlat")]
    public virtual double MinLatitude { get; set; }

    [JsonPropertyName("minlng")]
    public virtual double MinLongitude { get; set; }

    [JsonIgnore]
    public abstract double Width { get; }

    [JsonIgnore]
    public abstract double Height { get; }

    public BoundingBox GetBoundingBox()
    {
        return new BoundingBox(MinLongitude, MinLatitude, MinLongitude + Width, MinLatitude + Height);
    }

    [JsonIgnore]
    public Geometry<Point> TheGeometry
    {
        //1397.03.21
        get => GetBoundingBox().Transform(MapProjects.GeodeticWgs84ToWebMercator).AsGeometry<Point>(SridHelper.WebMercator);
        set => throw new NotImplementedException();
    }
     
    public abstract Feature AsFeature();  
}
