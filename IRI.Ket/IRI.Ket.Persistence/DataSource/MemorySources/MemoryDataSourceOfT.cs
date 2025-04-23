using IRI.Msh.Common.Primitives;
using System.Data;
using IRI.Extensions;
using IRI.Msh.CoordinateSystem.MapProjection;
using System.Text;
using IRI.Sta.ShapefileFormat;
using IRI.Msh.Common.Mapping;
using System;

namespace IRI.Ket.Persistence.DataSources;

public class MemoryDataSource<TGeometryAware, TPoint> : VectorDataSource<TGeometryAware, TPoint>, IEditableVectorDataSource<TGeometryAware, TPoint>
    where TGeometryAware : class, IGeometryAware<TPoint>
    where TPoint : IPoint, new()
{
    protected List<TGeometryAware> _features;

    protected Func<TGeometryAware, string>? _labelFunc;

    private int _uniqueId = 0;

    protected Func<int, TGeometryAware>? _idFunc;

    protected Func<TGeometryAware, Feature<TPoint>> _mapToFeatureFunc;

    public override int Srid { get => GetSrid(); protected set => _ = value; }

    public MemoryDataSource()
    {

    }

    public MemoryDataSource(
        List<TGeometryAware> features,
        Func<TGeometryAware, string> labelingFunc,
        Func<int, TGeometryAware> idFunc,
        Func<TGeometryAware, Feature<TPoint>> mapToFeatureFunc)
    {
        this._features = features;
        this._labelFunc = labelingFunc;
        this._idFunc = idFunc;
        _mapToFeatureFunc = mapToFeatureFunc;
        this.GeometryType = features.First().TheGeometry.Type;
    }

    // todo: remove this method
    public int GetSrid()
    {
        return this._features?.SkipWhile(g => g is null || g.TheGeometry.IsNotValidOrEmpty())?.FirstOrDefault()?.TheGeometry.Srid ?? 0;
    }

    public override string ToString()
    {
        return $"MemoryDataSourceOfT {typeof(TGeometryAware).Name}";
    }

    protected int GetNewId()
    {
        return _uniqueId++;
    }

    protected void UpdateExtent()
    {
        this.WebMercatorExtent = _features.Select(f => f.TheGeometry).GetBoundingBox();
    }

    // Get GeometryAwares [GENERIC]
    public override List<TGeometryAware> GetGeometryAwares(BoundingBox boundingBox)
    {
        return this._features.Where(f => f.TheGeometry.Intersects(boundingBox)).ToList();
    }

    public override List<TGeometryAware> GetGeometryAwares(Geometry<TPoint>? geometry)
    {
        if (geometry.IsNotValidOrEmpty())
        {
            return this._features.ToList();
        }
        else
        {
            return this._features.Where(f => f.TheGeometry.Intersects(geometry)).ToList();
        }
    }

    // Get as FeatureSet of Point
    public override FeatureSet<Point> GetAsFeatureSetOfPoint(Geometry<Point>? geometry)
    {
        return new FeatureSet<Point>(GetGeometryAwares(geometry == null ? null : geometry.NeutralizeGenericPoint<TPoint>())
                .Select(ToFeatureMappingFunc)
                .Select(g => new Feature<Point>(g.TheGeometry.NeutralizeGenericPoint<Point>())
                {
                    Id = g.Id,
                    LabelAttribute = g.LabelAttribute,
                    Attributes = g.Attributes
                })
                .ToList());
    }

    public override FeatureSet<Point> GetAsFeatureSetOfPoint(BoundingBox boundingBox)
    {
        return new FeatureSet<Point>(GetGeometryAwares(boundingBox)
                .Select(ToFeatureMappingFunc)
                .Select(g => new Feature<Point>(g.TheGeometry.NeutralizeGenericPoint<Point>())
                {
                    LabelAttribute = g.LabelAttribute,
                    Id = g.Id,
                    Attributes = g.Attributes
                })
                .ToList());
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

    public virtual void Add(TGeometryAware newGeometry)
    {
        this._features.Add(newGeometry);

        UpdateExtent();
    }

    public virtual void Remove(TGeometryAware geometry)
    {
        this._features.Remove(geometry);

        UpdateExtent();
    }

    public virtual void Update(TGeometryAware newGeometry)
    {
        if (_idFunc == null)
            return;

        var geometry = _idFunc(newGeometry.Id);

        var index = this._features.IndexOf(geometry);

        //var index = newGeometry.Id;

        //if (index < 0)
        //{
        //    return;
        //}

        this._features[index] = newGeometry;

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

    protected override Feature<TPoint> ToFeatureMappingFunc(TGeometryAware geometryAware) => _mapToFeatureFunc(geometryAware);

}