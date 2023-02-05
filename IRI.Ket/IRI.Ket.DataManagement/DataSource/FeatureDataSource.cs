using IRI.Msh.Common.Primitives;
using IRI.Ket.DataManagement.DataSource;
using IRI.Ket.SpatialExtensions;
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
    public abstract class FeatureDataSource  : IDataSource
    {
        public abstract BoundingBox Extent { get; protected set; }

        public virtual int GetSrid()
        {
            return GetGeometries()?.SkipWhile(g => g.IsNotValidOrEmpty())?.FirstOrDefault()?.GetSrid() ?? 0;
        }

        #region Get Geometries

        public abstract List<Geometry<Point>> GetGeometries();

        public virtual List<Geometry<Point>> GetGeometries(BoundingBox boundingBox)
        {
            Geometry<Point> boundary = boundingBox.AsGeometry(this.GetSrid());

            return GetGeometries().Where(i => i.Intersects(boundary)).ToList();
        }

        public virtual List<Geometry<T>> GetGeometries(SqlGeometry geometry)
        {
            return GetGeometries().Where(i => i.STIntersects(geometry).IsTrue).ToList();
        }

        public virtual List<Geometry<T>> GetGeometriesForDisplay(double mapScale, BoundingBox boundingBox)
        {
            return GetGeometries(boundingBox);
        }

        #endregion

        #region Get Geometry Label Pairs

        public virtual List<NamedSqlGeometry> GetGeometryLabelPairs()
        {
            SqlGeometry geometry = null;

            return GetGeometryLabelPairs(geometry);
        }
         
        public virtual List<NamedSqlGeometry> GetGeometryLabelPairsForDisplay(BoundingBox boundary)
        {
            SqlGeometry geometry = boundary.AsSqlGeometry(GetSrid());

            return GetGeometryLabelPairs(geometry);
        }

        public abstract List<NamedSqlGeometry> GetGeometryLabelPairs(SqlGeometry geometry);

        #endregion
         
        #region GetEntireFeature

        public virtual DataTable GetEntireFeatures()
        {
            SqlGeometry geometry = null;

            return GetEntireFeatures(geometry);
        }
         
        public virtual DataTable GetEntireFeatures(BoundingBox boundary)
        {
            SqlGeometry geometry = boundary.AsSqlGeometry(GetSrid());

            return GetEntireFeatures(geometry);
        }

        public abstract DataTable GetEntireFeatures(SqlGeometry geometry);

        #endregion

        #region Async Methods

        public Task<List<SqlGeometry>> GetGeometriesAsync()
        {
            return Task.Run(GetGeometries);
        }
         
        public Task<List<SqlGeometry>> GetGeometriesAsync(SqlGeometry geometry)
        {
            return Task.Run(() => { return GetGeometries(geometry); });
        }

        public Task<List<SqlGeometry>> GetGeometriesAsync(BoundingBox boundingBox)
        {
            return Task.Run(() => { return GetGeometries(boundingBox); });
        }

        public Task<List<SqlGeometry>> GetGeometriesForDisplayAsync(double mapScale, BoundingBox boundingBox)
        {
            return Task.Run(() => { return GetGeometriesForDisplay(mapScale, boundingBox); });
        }


        public Task<List<NamedSqlGeometry>> GetGeometryLabelPairsAsync()
        {
            return Task.Run(GetGeometryLabelPairs);
        }
         
        public Task<List<NamedSqlGeometry>> GetGeometryLabelPairsForDisplayAsync(BoundingBox boundingBox)
        {
            return Task.Run(() => { return GetGeometryLabelPairsForDisplay(boundingBox); });
        }

        public Task<List<NamedSqlGeometry>> GetGeometryLabelPairsAsync(SqlGeometry geometry)
        {
            return Task.Run(() => { return GetGeometryLabelPairs(geometry); });
        }
         
        public Task<DataTable> GetEntireFeatureAsync(BoundingBox geometry)
        {
            return Task.Run(() => { return GetEntireFeatures(geometry); });
        }

        public Task<DataTable> GetEntireFeaturesWhereIntersectsAsync(SqlGeometry geometry)
        {
            return Task.Run(() => { return GetEntireFeatures(geometry); });
        }

        #endregion

        public abstract SqlFeatureSet GetSqlFeatures();

        public virtual SqlFeatureSet GetSqlFeatures(BoundingBox boundingBox)
        {
            SqlGeometry boundary = boundingBox.AsSqlGeometry(this.GetSrid()).MakeValid();

            var features = GetSqlFeatures().Features.Where(i => !i.TheSqlGeometry.IsNotValidOrEmpty() && i.TheSqlGeometry.STIntersects(boundary).IsTrue).ToList();

            return new SqlFeatureSet(this.GetSrid()) { Features = features };
        }
        public virtual SqlFeatureSet GetSqlFeatures(SqlGeometry geometry)
        {
            var features = GetSqlFeatures().Features.Where(i => !i.TheSqlGeometry.IsNotValidOrEmpty() && i.TheSqlGeometry.STIntersects(geometry).IsTrue).ToList();

            return new SqlFeatureSet(this.GetSrid()) { Features = features };
        }

        public abstract void Add(ISqlGeometryAware newValue);

        public abstract void Remove(ISqlGeometryAware value);

        public abstract void Update(ISqlGeometryAware newValue);

        public abstract void UpdateFeature(ISqlGeometryAware feature);

        public abstract void SaveChanges();
    }

    public abstract class FeatureDataSource<TGeometry, TPoint> : FeatureDataSource 
        where TGeometry : class, IGeometryAware<TPoint>
        where TPoint : IPoint, new()
    {
        const Geometry<TPoint> NullGeometry = null;

        public Func<List<TGeometry>, DataTable> ToDataTableMappingFunc;

        public FeatureDataSource()
        {

        }
         
        public virtual List<TGeometry> GetFeatures()
        {
            //SqlGeometry geometry = null;

            return GetFeatures(NullGeometry);
        }

        public abstract List<TGeometry> GetFeatures(Geometry<TPoint> geometry);

        public override DataTable GetEntireFeatures(Geometry<TPoint> geometry)
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

        public static DataTable SqlFeatureTypeMapping(List<SqlFeature> list)
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
