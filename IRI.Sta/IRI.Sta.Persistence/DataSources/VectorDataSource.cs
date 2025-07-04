using System.Linq;
using System.Data;
using System.Threading.Tasks;
using System.Collections.Generic;

using IRI.Sta.Common.Primitives; 
using IRI.Sta.Spatial.Primitives;
using IRI.Sta.Persistence.Abstractions;

namespace IRI.Sta.Persistence.DataSources;

public abstract class VectorDataSource<TGeometryAware> : IVectorDataSource
    where TGeometryAware : class, IGeometryAware<Point>
{
    protected abstract Feature<Point> ToFeatureMappingFunc(TGeometryAware geometryAware);

    public virtual BoundingBox WebMercatorExtent { get; protected set; }

    public abstract int Srid { get; protected set; }

    public virtual GeometryType? GeometryType { get; protected set; }

    public List<Field>? Fields { get; set; }

    #region Get Geometries

    //public virtual List<Geometry<Point>> GetGeometries() => GetGeometries(null);

    //public virtual List<Geometry<Point>> GetGeometries(BoundingBox boundingBox)
    //{
    //    return GetAsFeatureSet(boundingBox).Features.Select(f => f.TheGeometry).ToList();
    //}

    //public virtual List<Geometry<Point>> GetGeometries(Geometry<Point>? geometry)
    //{
    //    return GetAsFeatureSet(geometry).Features.Select(f => f.TheGeometry).ToList();
    //}


    //public virtual Task<List<Geometry<Point>>> GetGeometriesAsync() => Task.Run(GetGeometries);

    //public virtual Task<List<Geometry<Point>>> GetGeometriesAsync(BoundingBox boundingBox) => Task.Run(() => GetGeometries(boundingBox));

    //public virtual Task<List<Geometry<Point>>> GetGeometriesAsync(Geometry<Point>? geometry) => Task.Run(() => GetGeometries(geometry));

    #endregion


    //#region Get Named-Geometries

    //public virtual List<NamedGeometry> GetNamedGeometries()
    //{
    //    return GetNamedGeometries(Geometry<Point>.Empty);
    //}

    //public virtual List<NamedGeometry> GetNamedGeometries(BoundingBox boundingBox)
    //{
    //    return GetAsFeatureSet(boundingBox).Features.Select(f => new NamedGeometry(f.TheGeometry, f.Label)).ToList();
    //}

    //public virtual List<NamedGeometry> GetNamedGeometries(Geometry<Point>? geometry)
    //{
    //    return GetAsFeatureSet(geometry).Features.Select(f => new NamedGeometry(f.TheGeometry, f.Label)).ToList();
    //}

    //public virtual List<NamedGeometry> GetNamedGeometries(double mapScale, BoundingBox boundingBox)
    //{
    //    return GetNamedGeometries(boundingBox);
    //}


    //public virtual Task<List<NamedGeometry>> GetNamedGeometriesAsync() => Task.Run(GetNamedGeometries);

    //public virtual Task<List<NamedGeometry>> GetNamedGeometriesAsync(BoundingBox boundingBox)
    //{
    //    return Task.Run(() => GetNamedGeometries(boundingBox));
    //}

    //public virtual Task<List<NamedGeometry>> GetNamedGeometriesAsync(Geometry<Point>? geometry)
    //{
    //    return Task.Run(() => GetNamedGeometries(geometry));
    //}

    //public virtual Task<List<NamedGeometry>> GetNamedGeometriesAsync(double mapScale, BoundingBox boundingBox)
    //{
    //    return Task.Run(() => GetNamedGeometries(mapScale, boundingBox));
    //}

    //#endregion


    #region Get as FeatureSet

    public virtual FeatureSet<Point> GetAsFeatureSet() => GetAsFeatureSet(Geometry<Point>.Empty);

    public abstract FeatureSet<Point> GetAsFeatureSet(BoundingBox boundingBox);

    public abstract FeatureSet<Point> GetAsFeatureSet(Geometry<Point>? geometry);

    public virtual FeatureSet<Point> GetAsFeatureSet(double mapScale, BoundingBox boundingBox) => GetAsFeatureSet(boundingBox);


    public virtual Task<FeatureSet<Point>> GetAsFeatureSetAsync() => Task.Run(GetAsFeatureSet);

    public virtual Task<FeatureSet<Point>> GetAsFeatureSetAsync(BoundingBox boundingBox) => Task.Run(() => GetAsFeatureSet(boundingBox));

    public virtual Task<FeatureSet<Point>> GetAsFeatureSetAsync(Geometry<Point>? geometry) => Task.Run(() => GetAsFeatureSet(geometry));

    public virtual Task<FeatureSet<Point>> GetAsFeatureSetAsync(double mapScale, BoundingBox boundingBox) => Task.Run(() => GetAsFeatureSet(mapScale, boundingBox));

    #endregion


    //#region Get GeometryAwares [GENERIC]

    //public virtual List<TGeometryAware> GetGeometryAwares() => GetGeometryAwares(Geometry<Point>.Empty);

    //public abstract List<TGeometryAware> GetGeometryAwares(BoundingBox boundingBox);

    //public abstract List<TGeometryAware> GetGeometryAwares(Geometry<Point>? geometry);


    //public virtual Task<List<TGeometryAware>> GetGeometryAwaresAsync() => Task.Run(GetGeometryAwares);

    //public virtual Task<List<TGeometryAware>> GetGeometryAwaresAsync(BoundingBox boundingBox)
    //{
    //    return Task.Run(() => GetGeometryAwares(boundingBox));
    //}

    //public virtual Task<List<TGeometryAware>> GetGeometryAwaresAsync(Geometry<Point>? geometry)
    //{
    //    return Task.Run(() => GetGeometryAwares(geometry));
    //}

    //#endregion


    //#region Get Features [Generic]

    //public virtual FeatureSet<Point> GetAsFeatureSet() => GetAsFeatureSet(Geometry<Point>.Empty);
    ////{
    ////return new FeatureSet<TPoint>(GetGeometryAwares().Select(ToFeatureMappingFunc).ToList());            
    ////}

    //public virtual FeatureSet<Point> GetAsFeatureSet(BoundingBox boundingBox)
    //{
    //    //var features = GetAsFeatureSet().Features.Where(i => !i.TheGeometry.IsNotValidOrEmpty() && i.TheGeometry.Intersects(boundingBox)).ToList();
    //    return new FeatureSet<Point>(GetGeometryAwares(boundingBox).Select(ToFeatureMappingFunc).ToList());
    //}

    //public virtual FeatureSet<Point> GetAsFeatureSet(Geometry<Point>? geometry)
    //{
    //    return new FeatureSet<Point>(GetGeometryAwares(geometry).Select(ToFeatureMappingFunc).ToList());
    //}

    //#endregion


    #region Other

    public abstract FeatureSet<Point> Search(string searchText);

    #endregion


    //#region CRUD

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


    //public abstract void Add(TGeometryAware? newValue);

    //public abstract void Remove(TGeometryAware? value);

    //public abstract void Update(TGeometryAware? newValue);

    //public abstract void SaveChanges();

    //#endregion
}

//public static class ToDataTableDefaultMappings
//{
//    public static Func<List<T>, DataTable> GenericTypeMapping<T>()
//    {
//        return list =>
//        {
//            var properties = typeof(T).GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

//            var dataTable = new DataTable();

//            if (list.Count == 0)
//                return dataTable;

//            foreach (var property in properties)
//            {
//                var type = (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ?
//                                Nullable.GetUnderlyingType(property.PropertyType) : property.PropertyType);

//                dataTable.Columns.Add(property.Name, type);
//            }

//            for (int row = 0; row < list.Count; row++)
//            {
//                var dataRow = dataTable.NewRow();

//                for (int i = 0; i < properties.Length; i++)
//                {
//                    var value = properties[i].GetValue(list[row]);

//                    if (value == null)
//                        continue;

//                    dataRow[i] = value;
//                }

//                dataTable.Rows.Add(dataRow);
//            }

//            return dataTable;
//        };
//    }

//    public static DataTable SqlFeatureTypeMapping(List<Feature> list)
//    {
//        var dataTable = new DataTable();

//        if (list.Count == 0)
//            return dataTable;

//        var columnNames = list.First().Attributes.Select(dict => dict.Key.ToString()).Distinct().ToList();

//        dataTable.Columns.AddRange(columnNames.Select(c => new DataColumn(c)).ToArray());

//        for (int row = 0; row < list.Count; row++)
//        {
//            var dataRow = dataTable.NewRow();

//            for (int col = 0; col < columnNames.Count; col++)
//            {
//                var value = list[row].Attributes[columnNames[col]];

//                if (value == null)
//                    continue;

//                dataRow[col] = value;
//            }

//            dataTable.Rows.Add(dataRow);
//        }

//        return dataTable;
//    }
//}