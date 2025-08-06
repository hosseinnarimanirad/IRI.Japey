using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;

using IRI.Extensions;
using IRI.Maptor.Sta.Common.Helpers;
using IRI.Maptor.Sta.Common.Primitives;
using IRI.Maptor.Sta.Spatial.Primitives;

namespace IRI.Maptor.Sta.Persistence.DataSources;

public class OrdinaryJsonListSource/*<TGeometryAware>*/ : MemoryDataSource//<TGeometryAware> where TGeometryAware : class, IGeometryAware<Point>
{
    public override GeometryType? GeometryType
    {
        get; protected set;
    }

    private OrdinaryJsonListSource(List<Feature<Point>> features/*, Func<TGeometryAware, Feature<Point>> mapToFeatureFunc, Func<TGeometryAware, string>? labelFunc = null*/)
        : base(features)
    {
        //_labelFunc = labelFunc;

        //_features = features;

        //WebMercatorExtent = features.Extent;//.Select(f => f.TheGeometry).GetBoundingBox();

        //_mapToFeatureFunc = mapToFeatureFunc;

        // 1401.11.29
        //GeometryType = features.First().TheGeometry.Type;

    }

    public override string ToString()
    {
        return $"OrdinaryJsonListSource";
    }

    public static OrdinaryJsonListSource/*<TGeometryAware>*/ CreateFromJsonString<TGeometryAware>(
        string jsonString,
        Func<TGeometryAware, Feature<Point>> mapToFeatureFunc/*, */
        /*Func<TGeometryAware, string> labelFunc = null*/) where TGeometryAware : class, IGeometryAware<Point>
    {
        var values = JsonHelper.Deserialize<List<TGeometryAware>>(jsonString);

        return new OrdinaryJsonListSource(values.Select(v => mapToFeatureFunc(v)).ToList());//, mapToFeatureFunc, labelFunc);
    }

    public static OrdinaryJsonListSource/*<TGeometryAware>*/ CreateFromFile<TGeometryAware>(
        string fileName, 
        Func<TGeometryAware, Feature<Point>> mapToFeatureFunc
        /*, Func<TGeometryAware, string> labelFunc = null*/) where TGeometryAware : class, IGeometryAware<Point>
    {
        var jsonString = System.IO.File.ReadAllText(fileName);

        return CreateFromJsonString(jsonString, mapToFeatureFunc/*, labelFunc*/);
    }

    //public static GeoJsonSource<SqlFeature> Create(List<SqlFeature> features)
    //{
    //    return new GeoJsonSource<SqlFeature>(features, f => f);
    //}
}
