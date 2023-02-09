using IRI.Msh.Common.Primitives;
using IRI.Ket.DataManagement.DataSource;
using IRI.Extensions;
using IRI.Ket.SqlServerSpatialExtension.Model;
using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.DataManagement.DataSource
{
    public abstract class FeatureDataSource : IDataSource
    {
        public abstract BoundingBox Extent { get; protected set; }

        public virtual int GetSrid()
        {
            //return GetGeometries()?.SkipWhile(g => g.IsNotValidOrEmpty())?.FirstOrDefault()?.GetSrid() ?? 0;
            return GetGeometries().FirstOrDefault()?.Srid ?? 0;
        }

        #region Get Geometries

        public abstract List<Geometry<Point>> GetGeometries();

        public virtual List<Geometry<Point>> GetGeometries(BoundingBox boundingBox)
        {
            Geometry<Point> boundary = boundingBox.AsGeometry(this.GetSrid());

            return GetGeometries().Where(i => i.Intersects(boundary)).ToList();
        }

        public virtual List<Geometry<Point>> GetGeometries(Geometry<Point> geometry)
        {
            return GetGeometries().Where(i => i.Intersects(geometry)).ToList();
        }

        public virtual List<Geometry<Point>> GetGeometriesForDisplay(double mapScale, BoundingBox boundingBox)
        {
            return GetGeometries(boundingBox);
        }

        #endregion


        #region Get Geometry Label Pairs

        public virtual List<NamedGeometry<Point>> GetGeometryLabelPairs()
        {
            Geometry<Point>? geometry = null;

            return GetGeometryLabelPairs(geometry);
        }

        public virtual List<NamedGeometry<Point>> GetGeometryLabelPairsForDisplay(BoundingBox boundary)
        {
            var geometry = boundary.AsGeometry(GetSrid());

            return GetGeometryLabelPairs(geometry);
        }

        public abstract List<NamedGeometry<Point>> GetGeometryLabelPairs(Geometry<Point>? geometry);

        #endregion

        #region GetEntireFeature

        public virtual DataTable GetEntireFeatures()
        {
            Geometry<Point>? geometry = null;

            return GetEntireFeatures(geometry);
        }

        public virtual DataTable GetEntireFeatures(BoundingBox boundary)
        {
            Geometry<Point> geometry = boundary.AsGeometry(GetSrid());

            return GetEntireFeatures(geometry);
        }

        public abstract DataTable GetEntireFeatures(Geometry<Point>? geometry);

        #endregion

        #region Async Methods

        public Task<List<Geometry<Point>>> GetGeometriesAsync()
        {
            return Task.Run(GetGeometries);
        }

        public Task<List<Geometry<Point>>> GetGeometriesAsync(Geometry<Point> geometry)
        {
            return Task.Run(() => { return GetGeometries(geometry); });
        }

        public Task<List<Geometry<Point>>> GetGeometriesAsync(BoundingBox boundingBox)
        {
            return Task.Run(() => { return GetGeometries(boundingBox); });
        }

        public Task<List<Geometry<Point>>> GetGeometriesForDisplayAsync(double mapScale, BoundingBox boundingBox)
        {
            return Task.Run(() => { return GetGeometriesForDisplay(mapScale, boundingBox); });
        }


        public Task<List<NamedGeometry<Point>>> GetGeometryLabelPairsAsync()
        {
            return Task.Run(GetGeometryLabelPairs);
        }

        public Task<List<NamedGeometry<Point>>> GetGeometryLabelPairsForDisplayAsync(BoundingBox boundingBox)
        {
            return Task.Run(() => { return GetGeometryLabelPairsForDisplay(boundingBox); });
        }

        public Task<List<NamedGeometry<Point>>> GetGeometryLabelPairsAsync(Geometry<Point> geometry)
        {
            return Task.Run(() => { return GetGeometryLabelPairs(geometry); });
        }

        public Task<DataTable> GetEntireFeatureAsync(BoundingBox geometry)
        {
            return Task.Run(() => { return GetEntireFeatures(geometry); });
        }

        public Task<DataTable> GetEntireFeaturesWhereIntersectsAsync(Geometry<Point> geometry)
        {
            return Task.Run(() => { return GetEntireFeatures(geometry); });
        }

        #endregion

        public abstract FeatureSet GetSqlFeatures();

        public virtual FeatureSet GetSqlFeatures(BoundingBox boundingBox)
        {
            Geometry<Point> boundary = boundingBox.AsGeometry(this.GetSrid());//.MakeValid();

            var features = GetSqlFeatures().Features.Where(i => !i.TheGeometry.IsNotValidOrEmpty() && i.TheGeometry.Intersects(boundary)).ToList();

            return new FeatureSet(this.GetSrid()) { Features = features };
        }
        public virtual FeatureSet GetSqlFeatures(Geometry<Point> geometry)
        {
            var features = GetSqlFeatures().Features.Where(i => !i.TheGeometry.IsNotValidOrEmpty() && i.TheGeometry.Intersects(geometry)).ToList();

            return new FeatureSet(this.GetSrid()) { Features = features };
        }

        public abstract void Add(IGeometryAware<Point> newValue);

        public abstract void Remove(IGeometryAware<Point> value);

        public abstract void Update(IGeometryAware<Point> newValue);

        public abstract void UpdateFeature(IGeometryAware<Point> feature);

        public abstract void SaveChanges();
    }

    public abstract class FeatureDataSource<TGeometry> : FeatureDataSource
        where TGeometry : class, IGeometryAware<Point> 
    {
        const Geometry<Point> NullGeometry = null;

        public Func<List<TGeometry>, DataTable> ToDataTableMappingFunc;

        public FeatureDataSource()
        {

        }

        public virtual List<TGeometry> GetFeatures()
        {
            //SqlGeometry geometry = null;

            return GetFeatures(NullGeometry);
        }

        public abstract List<TGeometry> GetFeatures(Geometry<Point> geometry);

        public override DataTable GetEntireFeatures(Geometry<Point>? geometry)
        {
            return ToDataTableMappingFunc(GetFeatures(geometry));
        }

        public override DataTable GetEntireFeatures()
        {
            return ToDataTableMappingFunc(GetFeatures());
        }
    }

    public static class ToDataTableDefaultMappings
    {
        public static Func<List<T>, DataTable> GenericTypeMapping<T>()
        {
            return list =>
            {
                var properties = typeof(T).GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

                var dataTable = new DataTable();

                if (list.Count == 0)
                    return dataTable;

                foreach (var property in properties)
                {
                    var type = (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ?
                                    Nullable.GetUnderlyingType(property.PropertyType) : property.PropertyType);

                    dataTable.Columns.Add(property.Name, type);
                }

                for (int row = 0; row < list.Count; row++)
                {
                    var dataRow = dataTable.NewRow();

                    for (int i = 0; i < properties.Length; i++)
                    {
                        var value = properties[i].GetValue(list[row]);

                        if (value == null)
                            continue;

                        dataRow[i] = value;
                    }

                    dataTable.Rows.Add(dataRow);
                }

                return dataTable;
            };
        }

        public static DataTable SqlFeatureTypeMapping(List<Feature<Point>> list)
        {
            var dataTable = new DataTable();

            if (list.Count == 0)
                return dataTable;

            var columnNames = list.First().Attributes.Select(dict => dict.Key.ToString()).Distinct().ToList();

            dataTable.Columns.AddRange(columnNames.Select(c => new DataColumn(c)).ToArray());

            for (int row = 0; row < list.Count; row++)
            {
                var dataRow = dataTable.NewRow();

                for (int col = 0; col < columnNames.Count; col++)
                {
                    var value = list[row].Attributes[columnNames[col]];

                    if (value == null)
                        continue;

                    dataRow[col] = value;
                }

                dataTable.Rows.Add(dataRow);
            }

            return dataTable;
        }

    }
}
