﻿using IRI.Msh.Common.Primitives;
using System.Data;
using IRI.Extensions;
using IRI.Msh.CoordinateSystem.MapProjection;
using System.Text;
using IRI.Sta.ShapefileFormat;
using IRI.Msh.Common.Mapping;
using System; 

namespace IRI.Ket.Persistence.DataSources;


public class MemoryDataSource : MemoryDataSource<Feature<Point>, Point>
{
    public MemoryDataSource() : this(new List<Feature<Point>>(), null, null)
    {

    }

    public MemoryDataSource(List<Geometry<Point>> geometries)
    {
        var features = geometries.Select(g => new Feature<Point>(g) { Id = GetNewId() }).ToList();

        Func<int, Feature<Point>> idFunc = id => features.Single(f => f.Id == id);

        Initialize(features, null, idFunc);
    }

    public MemoryDataSource(List<Feature<Point>> features, Func<Feature<Point>, string>? labelFunc, Func<int, Feature<Point>>? idFunc)
    {
        Initialize(features, labelFunc, idFunc);
    }

    private void Initialize(List<Feature<Point>> features, Func<Feature<Point>, string>? labelFunc, Func<int, Feature<Point>>? idFunc)
    { 
        foreach (var item in features)
            item.Id = GetNewId();

        this._features = features;
        this._labelFunc = labelFunc;
        this._idFunc = idFunc;
        //this.WebMercatorExtent = features.Select(f => f.TheGeometry).GetBoundingBox();
        this._mapToFeatureFunc = f => f;
        this.GeometryType = features.First().TheGeometry.Type;

        this.UpdateExtent();
    }

    public override FeatureSet<Point> GetAsFeatureSetOfPoint(Geometry<Point>? geometry)
    {
        return new FeatureSet<Point>(GetGeometryAwares(geometry)
                .Select(ToFeatureMappingFunc)
                .ToList());
    }

    public override FeatureSet<Point> GetAsFeatureSetOfPoint(BoundingBox boundingBox)
    {
        return new FeatureSet<Point>(GetGeometryAwares(boundingBox)
                .Select(ToFeatureMappingFunc)
                .ToList());
    }
     
    public static MemoryDataSource CreateFromShapefile(string shpFileName, string label, SrsBase targetSrs = null, bool correctFarsiCharacters = true, Encoding dataEncoding = null, Encoding headerEncoding = null)
    {
        //var features = ShapefileHelper.ReadAsSqlFeature(shpFileName, dataEncoding, targetSrs, headerEncoding, correctFarsiCharacters, label);
        var features = Shapefile.ReadAsFeature(shpFileName, dataEncoding, targetSrs, headerEncoding, correctFarsiCharacters, label);

        var result = new MemoryDataSource(features, f => f.Label, i => (Feature)features.Single(tt => tt.Id == i));

        //result.ToDataTableMappingFunc = sqlFeatureToDataTableMapping;

        return result;
    }

    public static async Task<MemoryDataSource> CreateFromShapefileAsync(string shpFileName, string label, Encoding dataEncoding = null, SrsBase targetSrs = null, Encoding headerEncoding = null, bool correctFarsiCharacters = true)
    {
        var features = await Shapefile.ReadAsFeatureAsync(shpFileName, dataEncoding, targetSrs, headerEncoding, correctFarsiCharacters, label);

        var result = new MemoryDataSource(features, i => i.Label, i => (Feature)features.Single(tt => tt.Id == i));

        //result.ToDataTableMappingFunc = sqlFeatureToDataTableMapping;

        return result;
    }

    //public override List<Geometry<Point>> GetGeometries()
    //{
    //    return this._geometries;
    //}

    //public override DataTable GetEntireFeatures(Geometry<Point> geometry)
    //{
    //    DataTable result = new DataTable();

    //    result.Columns.Add(new DataColumn(_geoColumnName));

    //    IEnumerable<Geometry<Point>> geometries;

    //    if (geometry == null)
    //    {
    //        geometries = GetGeometries();
    //    }
    //    else
    //    {
    //        if (geometry?.Srid != this.GetSrid())
    //        {
    //            throw new ArgumentException("srid mismatch");
    //        }

    //        geometries = GetGeometries()?.Where(i => i.Intersects(geometry));
    //    }

    //    foreach (var item in geometries)
    //    {
    //        result.Rows.Add(item);
    //    }

    //    return result;
    //}

    //public override List<NamedGeometry> GetGeometryLabelPairs(Geometry<Point> geometry)
    //{
    //    return GetGeometries().Select(g => new NamedGeometry(g, string.Empty)).ToList();
    //}


    //public override FeatureSet GetSqlFeatures()
    //{
    //    var features = GetGeometries().Select(g => new Feature<Point>(g, string.Empty) { Attributes = new Dictionary<string, object>() }).ToList();

    //    return new FeatureSet(this.GetSrid()) { Features = features };
    //}
}

