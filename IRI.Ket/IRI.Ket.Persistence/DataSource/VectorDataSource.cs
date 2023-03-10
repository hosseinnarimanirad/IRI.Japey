using IRI.Msh.Common.Primitives;

using IRI.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRI.Msh.CoordinateSystem.MapProjection;

namespace IRI.Ket.Persistence.DataSources
{
    public abstract class VectorDataSource<TGeometryAware, TPoint> : IVectorDataSource
        where TGeometryAware : class, IGeometryAware<TPoint>
        where TPoint : IPoint, new()
    {
        protected abstract Feature<TPoint> ToFeatureMappingFunc(TGeometryAware geometryAware);

        public virtual BoundingBox Extent { get; protected set; }

        public abstract int Srid { get; protected set; }

        public virtual GeometryType? GeometryType { get; protected set; }

        #region Get Geometries

        public virtual List<Geometry<Point>> GetGeometries()
        {
            return GetAsFeatureSet().Features.Select(f => f.TheGeometry).ToList();
        }

        public virtual List<Geometry<Point>> GetGeometries(BoundingBox webMercatorBoundingBox)
        {
            Geometry<Point> boundary = webMercatorBoundingBox.AsGeometry<Point>(SridHelper.WebMercator);

            return GetGeometries().Where(i => i.Intersects(boundary)).ToList();
        }

        public virtual List<Geometry<Point>> GetGeometries(Geometry<Point> geometry)
        {
            return GetGeometries().Where(i => i.Intersects(geometry)).ToList();
        }

        public Task<List<Geometry<Point>>> GetGeometriesAsync()
        {
            return Task.Run(GetGeometries);
        }

        public virtual Task<List<Geometry<Point>>> GetGeometriesAsync(BoundingBox boundingBox)
        {
            return Task.Run(() => GetGeometries(boundingBox));
        }

        public virtual Task<List<Geometry<Point>>> GetGeometriesAsync(Geometry<Point> geometry)
        {
            return Task.Run(() => GetGeometries(geometry));
        }

        #endregion


        #region Get Geometry Label Pairs

        public virtual List<NamedGeometry> GetNamedGeometries()
        {
            Geometry<Point>? geometry = null;

            return GetNamedGeometries(geometry);
        }

        public virtual List<NamedGeometry> GetNamedGeometries(BoundingBox webMercatorBoundingBox)
        {
            var geometry = webMercatorBoundingBox.AsGeometry<Point>(SridHelper.WebMercator);

            return GetNamedGeometries(geometry);
        }

        public virtual List<NamedGeometry> GetNamedGeometries(Geometry<Point>? geometry)
        {
            return GetAsFeatureSet(geometry).Features.Select(f => new NamedGeometry(f.TheGeometry, f.Label)).ToList();
        }

        public virtual List<NamedGeometry> GetNamedGeometriesForDisplay(double mapScale, BoundingBox boundingBox)
        {
            return GetNamedGeometries(boundingBox);
        }

        public Task<List<NamedGeometry>> GetNamedGeometriesAsync() => Task.Run(GetNamedGeometries);

        public Task<List<NamedGeometry>> GetNamedGeometriesAsync(Geometry<Point> geometry)
        {
            return Task.Run(() => { return GetNamedGeometries(geometry); });
        }

        public Task<List<NamedGeometry>> GetNamedGeometriesAsync(BoundingBox boundingBox)
        {
            return Task.Run(() => { return GetNamedGeometries(boundingBox); });
        }

        public Task<List<NamedGeometry>> GetNamedGeometriesForDisplayAsync(BoundingBox boundingBox)
        {
            return Task.Run(() => { return GetNamedGeometries(boundingBox); });
        }

        #endregion


        #region GetAsFeatureSet

        public FeatureSet<Point> GetAsFeatureSet()
        {
            return GetAsFeatureSet(Geometry<Point>.Empty);
        }

        public virtual FeatureSet<Point> GetAsFeatureSet(BoundingBox webMercatorBoundingBox)
        {
            Geometry<Point> geometry = webMercatorBoundingBox.AsGeometry<Point>(SridHelper.WebMercator);

            return GetAsFeatureSet(geometry);
        }

        public abstract FeatureSet<Point> GetAsFeatureSet(Geometry<Point>? geometry);

        public Task<FeatureSet<Point>> GetAsFeatureSetAsync(BoundingBox boundingBox)
        {
            return Task.Run(() => { return GetAsFeatureSet(boundingBox); });
        }

        public Task<FeatureSet<Point>> GetAsFeatureSetAsync(Geometry<Point> geometry)
        {
            return Task.Run(() => GetAsFeatureSet(geometry));
        }

        public virtual FeatureSet<Point> GetAsFeatureSetForDisplay(double mapScale, BoundingBox boundingBox)
        {
            return GetAsFeatureSet(boundingBox);
        }

        public Task<FeatureSet<Point>> GetAsFeatureSetForDisplayAsync(double mapScale, BoundingBox boundingBox)
        {
            return Task.Run(() => { return GetAsFeatureSetForDisplay(mapScale, boundingBox); });
        }

        #endregion


        #region GetGeometryAwares

        public virtual List<TGeometryAware> GetGeometryAwares()
        {
            //SqlGeometry geometry = null;

            return GetGeometryAwares(Geometry<TPoint>.Empty);
        }

        public abstract List<TGeometryAware> GetGeometryAwares(Geometry<TPoint>? geometry);

        #endregion


        #region GetFeatures

        public virtual FeatureSet<TPoint> GetFeatures()
        {
            return new FeatureSet<TPoint>(GetGeometryAwares().Select(ToFeatureMappingFunc).ToList());
        }

        public virtual FeatureSet<TPoint> GetFeatures(BoundingBox webMercatorBoundingBox)
        {
            Geometry<TPoint> boundary = webMercatorBoundingBox.AsGeometry<TPoint>(SridHelper.WebMercator);//.MakeValid();

            return GetFeatures(boundary);
        }

        public virtual FeatureSet<TPoint> GetFeatures(Geometry<TPoint>? geometry)
        {
            if (geometry.IsNullOrEmpty())
            {
                return new FeatureSet<TPoint>(GetFeatures().Features);
            }
            else
            {
                var features = GetFeatures().Features.Where(i => !i.TheGeometry.IsNotValidOrEmpty() && i.TheGeometry.Intersects(geometry)).ToList();

                return new FeatureSet<TPoint>(features);
            }
        }


        #endregion

        public abstract FeatureSet<Point> Search(string searchText);
        

        #region CRUD
        public virtual void Add(IGeometryAware<Point> newValue)
        {
            Add(newValue as TGeometryAware);
        }

        public virtual void Remove(IGeometryAware<Point> newValue)
        {
            Remove(newValue as TGeometryAware);
        }

        public virtual void Update(IGeometryAware<Point> newValue)
        {
            Update(newValue as TGeometryAware);
        }

        public abstract void Add(TGeometryAware newValue);

        public abstract void Remove(TGeometryAware value);

        public abstract void Update(TGeometryAware newValue);

        //public abstract void UpdateFeature(TGeometryAware feature);

        public abstract void SaveChanges();

     


        #endregion
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


}