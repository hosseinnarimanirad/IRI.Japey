using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;

using IRI.Extensions;
using IRI.Sta.Common.Primitives;
using IRI.Sta.Spatial.Primitives;
using IRI.Sta.Persistence.Abstractions;
using IRI.Sta.ShapefileFormat;
using IRI.Sta.SpatialReferenceSystem.MapProjections;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Sta.Persistence.DataSources;

public class MemoryDataSource/*<TGeometryAware>*/ : VectorDataSource/*<TGeometryAware>*/, IEditableVectorDataSource/*<TGeometryAware, Point>*/
//where TGeometryAware : class, IGeometryAware<Point>
//where TPoint : IPoint, new()
{
    protected FeatureSet<Point> _features;

    //protected Func<FeatureSet<Point>, string>? _labelFunc;

    private int _uniqueId = 0;

    //protected Func<int, FeatureSet<Point>>? _idFunc;

    //protected Func<TGeometryAware, Feature<Point>> _mapToFeatureFunc;

    public override int Srid { get => GetSrid(); protected set => _ = value; }

    public MemoryDataSource()
    {

    }

    public MemoryDataSource(List<Geometry<Point>> geometries)
    {
        var features = geometries.Select(g => new Feature<Point>(g) { Id = GetNewId() }).ToList();

        Func<int, Feature<Point>> idFunc = id => features.Single(f => f.Id == id);

        Initialize(features/*, null, idFunc*/);
    }

    public MemoryDataSource(List<Feature<Point>> features/*, Func<Feature<Point>, string>? labelFunc, Func<int, Feature<Point>>? idFunc*/)
    {
        Initialize(features/*, labelFunc, idFunc*/);
    }

    private void Initialize(List<Feature<Point>> features/*, Func<Feature<Point>, string>? labelFunc, Func<int, Feature<Point>>? idFunc*/)
    {
        foreach (var item in features)
            item.Id = GetNewId();

        _features = FeatureSet<Point>.Create(string.Empty, features);
        //_labelFunc = labelFunc;
        //_idFunc = idFunc;
        //this.WebMercatorExtent = features.Select(f => f.TheGeometry).GetBoundingBox();
        //_mapToFeatureFunc = f => f;
        GeometryType = features.First().TheGeometry.Type;

        UpdateExtent();
    }

    //public MemoryDataSource(
    //    List<TGeometryAware> features,
    //    Func<TGeometryAware, string> labelingFunc,
    //    Func<int, TGeometryAware> idFunc,
    //    Func<TGeometryAware, Feature<Point>> mapToFeatureFunc)
    //{
    //    _features = features;
    //    _labelFunc = labelingFunc;
    //    _idFunc = idFunc;
    //    _mapToFeatureFunc = mapToFeatureFunc;
    //    GeometryType = features.First().TheGeometry.Type;
    //}

    // todo: remove this method
    public int GetSrid()
    {
        //return _features?.SkipWhile(g => g is null || g.TheGeometry.IsNotValidOrEmpty())?.FirstOrDefault()?.TheGeometry.Srid ?? 0;
        return _features.Srid;
    }

    public override string ToString()
    {
        //return $"MemoryDataSourceOfT {typeof(TGeometryAware).Name}";
        return $"MemoryDataSource";
    }

    protected int GetNewId()
    {
        return _uniqueId++;
    }

    protected void UpdateExtent()
    {
        WebMercatorExtent = _features.Extent;// _features.Select(f => f.TheGeometry).GetBoundingBox();
    }

    // Get GeometryAwares [GENERIC]
    //public List<TGeometryAware> GetGeometryAwares(BoundingBox boundingBox)
    //{
    //    return _features.Where(f => f.TheGeometry.Intersects(boundingBox)).ToList();
    //}

    //public List<TGeometryAware> GetGeometryAwares(Geometry<Point>? geometry)
    //{
    //    if (geometry.IsNotValidOrEmpty())
    //    {
    //        return _features.ToList();
    //    }
    //    else
    //    {
    //        return _features.Where(f => f.TheGeometry.Intersects(geometry)).ToList();
    //    }
    //}

    // Get as FeatureSet of Point
    public override FeatureSet<Point> GetAsFeatureSet(Geometry<Point>? geometry)
    {
        if (geometry.IsNullOrEmpty())
        {
            return _features;
        }
        else
        {
            return _features.FilterByGeometry(f => f.Intersects(geometry));

            //var result = new FeatureSet<Point>(_features.Features.Where(f => f.TheGeometry.Intersects(geometry)).ToList());

            //result.Fields = this._features.Fields;
        }

        //return new FeatureSet<Point>(GetGeometryAwares(geometry == null ? null : geometry.NeutralizeGenericPoint<Point>())
        //        .Select(ToFeatureMappingFunc)
        //        .Select(g => new Feature<Point>(g.TheGeometry.NeutralizeGenericPoint<Point>())
        //        {
        //            Id = g.Id,
        //            LabelAttribute = g.LabelAttribute,
        //            Attributes = g.Attributes
        //        })
        //        .ToList());
    }

    public override FeatureSet<Point> GetAsFeatureSet(BoundingBox boundingBox)
    {
        return _features.FilterByGeometry(f => f.Intersects(boundingBox));

        //return new FeatureSet<Point>(_features.Features.Where(f => f.TheGeometry.Intersects(boundingBox)).ToList());

        //return new FeatureSet<Point>(GetGeometryAwares(boundingBox)
        //        .Select(ToFeatureMappingFunc)
        //        .Select(g => new Feature<Point>(g.TheGeometry.NeutralizeGenericPoint<Point>())
        //        {
        //            LabelAttribute = g.LabelAttribute,
        //            Id = g.Id,
        //            Attributes = g.Attributes
        //        })
        //        .ToList());
    }

    public static MemoryDataSource CreateFromShapefile(string shpFileName, string label, SrsBase targetSrs = null, bool correctFarsiCharacters = true, Encoding dataEncoding = null, Encoding headerEncoding = null)
    {
        //var features = ShapefileHelper.ReadAsSqlFeature(shpFileName, dataEncoding, targetSrs, headerEncoding, correctFarsiCharacters, label);
        var features = Shapefile.ReadAsFeature(shpFileName, dataEncoding, targetSrs, headerEncoding, correctFarsiCharacters, label);

        var result = new MemoryDataSource(features/*, f => f.Label, i => (Feature)features.Single(tt => tt.Id == i)*/);


        //result.ToDataTableMappingFunc = sqlFeatureToDataTableMapping;

        return result;
    }

    public static async Task<MemoryDataSource> CreateFromShapefileAsync(string shpFileName, string label, Encoding dataEncoding = null, SrsBase targetSrs = null, Encoding headerEncoding = null, bool correctFarsiCharacters = true)
    {
        var features = await Shapefile.ReadAsFeatureAsync(shpFileName, dataEncoding, targetSrs, headerEncoding, correctFarsiCharacters, label);

        var result = new MemoryDataSource(features/*, i => i.Label, i => (Feature)features.Single(tt => tt.Id == i)*/);

        //result.ToDataTableMappingFunc = sqlFeatureToDataTableMapping;

        return result;
    }


    #region CRUD

    //public override void UpdateFeature(TGeometryAware newGeometry)
    //{
    //    if (_idFunc == null)
    //        return;

    //    var geometry = _idFunc(newGeometry.Id);

    //    var index = this._features.IndexOf(geometry);

    //    //var index = newGeometry.Id;

    //    //if (index < 0)
    //    //{
    //    //    return;
    //    //}

    //    _features[index] = newGeometry as T;
    //}

    //public virtual void Add(IGeometryAware<Point> newValue)
    //{
    //    Add(newValue as TGeometryAware);
    //}

    //public virtual void Remove(IGeometryAware<Point> newValue)
    //{
    //    Remove(newValue as TGeometryAware);
    //}

    //public virtual void Update(IGeometryAware<Point> newValue)
    //{
    //    Update(newValue as TGeometryAware);
    //}

    public virtual void Add(Feature<Point> newGeometry)
    {
        _features.Add(newGeometry);

        UpdateExtent();
    }

    public virtual void Remove(Feature<Point> geometry)
    {
        _features.Remove(geometry);

        UpdateExtent();
    }

    public virtual void Update(Feature<Point> newGeometry)
    {
        //if (_idFunc == null)
        //    return;

        //var geometry = _idFunc(newGeometry.Id);

        //var index = _features.IndexOf(geometry);

        ////var index = newGeometry.Id;

        ////if (index < 0)
        ////{
        ////    return;
        ////}

        //_features[index] = newGeometry;
        
        _features.Update(newGeometry);

        UpdateExtent();
    }

    public virtual void SaveChanges()
    {
        return;
    }

    #endregion


    public override FeatureSet<Point> Search(string searchText)
    {
        throw new NotImplementedException();
    }

    //protected override Feature<Point> ToFeatureMappingFunc(TGeometryAware geometryAware) => _mapToFeatureFunc(geometryAware);

}