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
    public abstract class FeatureDataSource : IDataSource
    {
        public abstract BoundingBox Extent { get; protected set; }

        public virtual int GetSrid()
        {
            return GetGeometries()?.SkipWhile(g => g.IsNotValidOrEmpty())?.FirstOrDefault()?.GetSrid() ?? 0;
        }

        #region Get Geometries

        public abstract List<SqlGeometry> GetGeometries();

        //public virtual List<SqlGeometry> GetGeometries(string whereClause)
        //{
        //    throw new NotImplementedException();
        //}

        public virtual List<SqlGeometry> GetGeometries(BoundingBox boundingBox)
        {
            SqlGeometry boundary = boundingBox.AsSqlGeometry(this.GetSrid()).MakeValid();

            return GetGeometries().Where(i => i.STIntersects(boundary).IsTrue).ToList();
        }

        public virtual List<SqlGeometry> GetGeometries(SqlGeometry geometry)
        {
            return GetGeometries().Where(i => i.STIntersects(geometry).IsTrue).ToList();
        }

        public virtual List<SqlGeometry> GetGeometriesForDisplay(double mapScale, BoundingBox boundingBox)
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

        //public virtual List<NamedSqlGeometry> GetGeometryLabelPairs(string whereClause)
        //{
        //    throw new NotImplementedException();
        //}

        public virtual List<NamedSqlGeometry> GetGeometryLabelPairsForDisplay(BoundingBox boundary)
        {
            SqlGeometry geometry = boundary.AsSqlGeometry(GetSrid());

            return GetGeometryLabelPairs(geometry);
        }

        public abstract List<NamedSqlGeometry> GetGeometryLabelPairs(SqlGeometry geometry);

        #endregion

        //#region Get Attribute Methods

        ////public virtual List<object> GetAttributes(string attributeColumn)
        ////{
        ////    return GetAttributes(attributeColumn, string.Empty);
        ////}

        ////public abstract List<object> GetAttributes(string attributeColumn, string whereClause);

        //#endregion

        #region GetEntireFeature

        public virtual DataTable GetEntireFeatures()
        {
            SqlGeometry geometry = null;

            return GetEntireFeatures(geometry);
        }

        //public virtual DataTable GetEntireFeatures(string whereClause)
        //{
        //    throw new NotImplementedException();
        //}

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
            return Task.Run(() => { return GetGeometries(); });
        }

        //public Task<List<SqlGeometry>> GetGeometriesAsync(string whereClause)
        //{
        //    return Task.Run(() => { return GetGeometries(whereClause); });
        //}

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
            return Task.Run(() => { return GetGeometryLabelPairs(); });
        }

        //public Task<List<NamedSqlGeometry>> GetGeometryLabelPairsAsync(string whereClause)
        //{
        //    return Task.Run(() => { return GetGeometryLabelPairs(whereClause); });
        //}

        public Task<List<NamedSqlGeometry>> GetGeometryLabelPairsForDisplayAsync(BoundingBox boundingBox)
        {
            return Task.Run(() => { return GetGeometryLabelPairsForDisplay(boundingBox); });
        }

        public Task<List<NamedSqlGeometry>> GetGeometryLabelPairsAsync(SqlGeometry geometry)
        {
            return Task.Run(() => { return GetGeometryLabelPairs(geometry); });
        }


        //public Task<DataTable> GetEntireFeatureAsync(string whereClause)
        //{
        //    return Task.Run(() => { return GetEntireFeatures(whereClause); });
        //}

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

    public abstract class FeatureDataSource<T> : FeatureDataSource where T : class, ISqlGeometryAware
    {
        const SqlGeometry NullGeometry = null;

        public Func<List<T>, DataTable> ToDataTableMappingFunc;

        public FeatureDataSource()
        {

        }



        public virtual List<T> GetFeatures()
        {
            //SqlGeometry geometry = null;

            return GetFeatures(NullGeometry);
        }

        public abstract List<T> GetFeatures(SqlGeometry geometry);

        public override DataTable GetEntireFeatures(SqlGeometry geometry)
        {

            return ToDataTableMappingFunc(GetFeatures(geometry));
            //return ToDataTableMappingFunc(this._features.Where(i => i.TheSqlGeometry?.STIntersects(geometry).IsTrue == true).ToList()); 
        }

        public override DataTable GetEntireFeatures()
        {
            return ToDataTableMappingFunc(GetFeatures());
        }

        //public List<Feature<Point>> GetFeatures(Func<T, Feature<Point>> map)
        //{
        //    return GetFeatures(map, NullGeometry);
        //}

        //public List<Feature<Point>> GetFeatures(Func<T, Feature<Point>> map, SqlGeometry geometry)
        //{
        //    return GetFeatures(geometry).Select(f => map(f)).ToList();
        //}

        //public abstract void Add(ISqlGeometryAware newGeometry);

        //public abstract void Remove(ISqlGeometryAware feature);

        //public abstract void Update(ISqlGeometryAware newGeometry, int geometryId);


        //public override void Add(ISqlGeometryAware newValue)
        //{
        //    //Add(newValue as T);
        //    throw new NotImplementedException();
        //}

        //public override void Remove(ISqlGeometryAware value)
        //{
        //    //Remove(value as T);
        //    throw new NotImplementedException();
        //}

        //public override void Update(ISqlGeometryAware newValue)
        //{
        //    //Update(newValue as T, valueId);
        //    throw new NotImplementedException();
        //}

        //public override void UpdateFeature(ISqlGeometryAware newValue)
        //{
        //    //Update(newValue as T, valueId);
        //    throw new NotImplementedException();
        //}

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
