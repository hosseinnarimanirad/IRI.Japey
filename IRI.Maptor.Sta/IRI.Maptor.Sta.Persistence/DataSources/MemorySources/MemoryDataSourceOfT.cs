﻿using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;

using IRI.Extensions;
using IRI.Maptor.Sta.Common.Primitives;
using IRI.Maptor.Sta.Spatial.Primitives;
using IRI.Maptor.Sta.Persistence.Abstractions;
using IRI.Maptor.Sta.ShapefileFormat;
using IRI.Maptor.Sta.SpatialReferenceSystem.MapProjections;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Maptor.Sta.Persistence.DataSources;

public class MemoryDataSource/*<TGeometryAware>*/ : VectorDataSource/*<TGeometryAware>*/, IEditableVectorDataSource/*<TGeometryAware, Point>*/
//where TGeometryAware : class, IGeometryAware<Point>
//where TPoint : IPoint, new()
{
    protected FeatureSet<Point> _features;
     
    private int _uniqueId = 0;
     
    public override int Srid { get => GetSrid(); protected set => _ = value; }

    public MemoryDataSource()
    {
    }

    public MemoryDataSource(List<Geometry<Point>> geometries)
    {
        var features = geometries.Select(g => new Feature<Point>(g) { Id = GetNewId() }).ToList();

        Initialize(features);
    }

    public MemoryDataSource(List<Feature<Point>> features)
    {
        Initialize(features);
    }

    private void Initialize(List<Feature<Point>> features)
    {
        foreach (var item in features)
            item.Id = GetNewId();

        _features = FeatureSet<Point>.Create(string.Empty, features);
        
        GeometryType = features.First().TheGeometry.Type;

        UpdateExtent();
    }
     

    // todo: remove this method
    public int GetSrid()
    {
        //return _features?.SkipWhile(g => g is null || g.TheGeometry.IsNotValidOrEmpty())?.FirstOrDefault()?.TheGeometry.Srid ?? 0;
        return _features.Srid;
    }

    public override string ToString()
    {
        return $"MemoryDataSource";
    }

    protected int GetNewId()
    {
        return _uniqueId++;
    }

    protected void UpdateExtent()
    {
        WebMercatorExtent = _features.Extent; 
    }
     
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
    }

    public override FeatureSet<Point> GetAsFeatureSet(BoundingBox boundingBox)
    {
        return _features.FilterByGeometry(f => f.Intersects(boundingBox));
    }

    public static MemoryDataSource CreateFromShapefile(string shpFileName, string label, SrsBase targetSrs = null, bool correctFarsiCharacters = true, Encoding dataEncoding = null, Encoding headerEncoding = null)
    {
        var features = Shapefile.ReadAsFeature(shpFileName, dataEncoding, targetSrs, headerEncoding, correctFarsiCharacters, label);

        var result = new MemoryDataSource(features);

        return result;
    }

    public static async Task<MemoryDataSource> CreateFromShapefileAsync(string shpFileName, string label, Encoding dataEncoding = null, SrsBase targetSrs = null, Encoding headerEncoding = null, bool correctFarsiCharacters = true)
    {
        var features = await Shapefile.ReadAsFeatureAsync(shpFileName, dataEncoding, targetSrs, headerEncoding, correctFarsiCharacters, label);

        var result = new MemoryDataSource(features/*, i => i.Label, i => (Feature)features.Single(tt => tt.Id == i)*/);

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
}