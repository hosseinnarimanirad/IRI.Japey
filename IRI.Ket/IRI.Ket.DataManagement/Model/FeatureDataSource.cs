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

namespace IRI.Ket.DataManagement.Model
{
    public abstract class FeatureDataSource : IDataSource
    {
        public abstract BoundingBox Extent { get; protected set; }

        protected string MakeWhereClause(string whereClause)
        {
            return string.IsNullOrWhiteSpace(whereClause) ? string.Empty : FormattableString.Invariant($" WHERE ({whereClause}) ");
        }

        public virtual int GetSrid()
        {
            return GetGeometries()?.SkipWhile(g => g.IsNotValidOrEmpty())?.FirstOrDefault()?.GetSrid() ?? 0;
        }

        #region Get Geometries

        public abstract List<SqlGeometry> GetGeometries();

        public virtual List<SqlGeometry> GetGeometries(string whereClause)
        {
            throw new NotImplementedException();
        }

        public virtual List<SqlGeometry> GetGeometries(BoundingBox boundingBox)
        {
            SqlGeometry boundary = boundingBox.AsSqlGeometry(this.GetSrid()).MakeValid();

            return GetGeometries().Where(i => i.STIntersects(boundary).IsTrue).ToList();
        }

        public virtual List<SqlGeometry> GetGeometries(SqlGeometry geometry)
        {
            return GetGeometries().Where(i => i.STIntersects(geometry).IsTrue).ToList();
        }


        #endregion

        #region Get Geometry Label Pairs

        public virtual List<NamedSqlGeometry> GetGeometryLabelPairs()
        {
            SqlGeometry geometry = null;

            return GetGeometryLabelPairs(geometry);
        }

        public virtual List<NamedSqlGeometry> GetGeometryLabelPairs(string whereClause)
        {
            throw new NotImplementedException();
        }

        public virtual List<NamedSqlGeometry> GetGeometryLabelPairs(BoundingBox boundary)
        {
            SqlGeometry geometry = boundary.AsSqlGeometry(GetSrid());

            return GetGeometryLabelPairs(geometry);
        }

        public abstract List<NamedSqlGeometry> GetGeometryLabelPairs(SqlGeometry geometry);

        #endregion

        #region Get Attribute Methods

        public virtual List<object> GetAttributes(string attributeColumn)
        {
            return GetAttributes(attributeColumn, string.Empty);
        }

        public abstract List<object> GetAttributes(string attributeColumn, string whereClause);

        #endregion

        #region GetEntireFeature

        public virtual DataTable GetEntireFeatures()
        {
            SqlGeometry geometry = null;

            return GetEntireFeatures(geometry);
        }

        public virtual DataTable GetEntireFeatures(string whereClause)
        {
            throw new NotImplementedException();
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
            return Task.Run(() => { return GetGeometries(); });
        }

        public Task<List<SqlGeometry>> GetGeometriesAsync(string whereClause)
        {
            return Task.Run(() => { return GetGeometries(whereClause); });
        }

        public Task<List<SqlGeometry>> GetGeometriesAsync(SqlGeometry geometry)
        {
            return Task.Run(() => { return GetGeometries(geometry); });
        }

        public Task<List<SqlGeometry>> GetGeometriesAsync(BoundingBox boundingBox)
        {
            return Task.Run(() => { return GetGeometries(boundingBox); });
        }


        public Task<List<NamedSqlGeometry>> GetGeometryLabelPairsAsync()
        {
            return Task.Run(() => { return GetGeometryLabelPairs(); });
        }

        public Task<List<NamedSqlGeometry>> GetGeometryLabelPairsAsync(string whereClause)
        {
            return Task.Run(() => { return GetGeometryLabelPairs(whereClause); });
        }

        public Task<List<NamedSqlGeometry>> GetGeometryLabelPairsAsync(BoundingBox boundingBox)
        {
            return Task.Run(() => { return GetGeometryLabelPairs(boundingBox); });
        }

        public Task<List<NamedSqlGeometry>> GetGeometryLabelPairsAsync(SqlGeometry geometry)
        {
            return Task.Run(() => { return GetGeometryLabelPairs(geometry); });
        }


        public Task<DataTable> GetEntireFeatureAsync(string whereClause)
        {
            return Task.Run(() => { return GetEntireFeatures(whereClause); });
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
    }

    public abstract class FeatureDataSource<T> : FeatureDataSource where T : ISqlGeometryAware
    {
        public List<T> GetFeatures()
        {
            SqlGeometry geometry = null;

            return GetFeatures(geometry);
        }

        public abstract List<T> GetFeatures(SqlGeometry geometry);

        public abstract void Add(T newGeometry);

        public abstract void Remove(int geometryId);

        public abstract void Update(T newGeometry, int geometryId);
    }
}
