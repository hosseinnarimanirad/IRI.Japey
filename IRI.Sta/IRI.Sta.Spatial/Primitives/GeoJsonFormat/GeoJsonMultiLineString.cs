﻿using IRI.Sta.Spatial.Primitives;
using IRI.Sta.Common.Primitives;
using System.Text.Json.Serialization;
using IRI.Sta.SpatialReferenceSystem;
using IRI.Extensions;

namespace IRI.Sta.Spatial.GeoJsonFormat;

//[JsonConverter(typeof(GeoJsonGeometryConverter))]
public class GeoJsonMultiLineString : GeoJsonBase
{
    [JsonIgnore]
    //[JsonPropertyName("type")]
    public override string Type { get; set; }

    [JsonPropertyName("coordinates")]
    public double[][][] Coordinates { get; set; }

    [JsonIgnore]
    public override GeometryType GeometryType { get => GeometryType.MultiLineString; }


    public override bool IsNullOrEmpty()
    {
        return Coordinates == null || Coordinates.Length < 1;
    }

    public override int NumberOfGeometries()
    {
        return Coordinates == null ? 0 : Coordinates.Length;
    }

    public override int NumberOfPoints()
    {
        return Coordinates == null ? 0 : Coordinates.Sum(part => part == null ? 0 : part.Length);
    }


    public override Geometry<Point> Parse(bool isLongitudeFirst = true, int srid = SridHelper.GeodeticWGS84)
    {
        if (this.Coordinates.IsNullOrEmpty())
            return Geometry<Point>.CreateEmpty(GeometryType.MultiLineString, srid);

        return new Geometry<Point>(Coordinates!.Select(c => Geometry<Point>.ParseLineStringToGeometry(c, GeometryType.LineString, false, isLongitudeFirst, srid)).ToList(), this.GeometryType, srid);
    }

    //public string Serialize(bool indented, bool removeSpaces = false)
    //{
    //    return GeoJson.Serialize(this, indented, removeSpaces);
    //}

    //public Geometry<Point> TransformToWeMercator(bool isLongitudeFirst = true)
    //{
    //    return this.Parse(isLongitudeFirst, SridHelper.GeodeticWGS84)
    //                .Transform(MapProjects.GeodeticWgs84ToWebMercator, SridHelper.WebMercator);
    //}


    //public GeoJsonFeature AsFeature() => GeoJson.AsFeature(this);

    //public GeoJsonFeatureSet AsFeatureSet() => GeoJson.AsFeatureSet(this);

}
