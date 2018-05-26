using IRI.Sta.Common.Primitives;
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
    public abstract class IFeatureDataSource : IDataSource
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

        public virtual List<SqlGeometry> GetGeometries()
        {
            return GetGeometries(string.Empty);
        }

        public abstract List<SqlGeometry> GetGeometries(string whereClause);

        public abstract List<SqlGeometry> GetGeometries(BoundingBox boundingBox);

        public abstract List<SqlGeometry> GetGeometries(SqlGeometry geometry);

        #endregion


        #region Get Geometry Label Pairs

        public abstract List<NamedSqlGeometry> GetGeometryLabelPairs();

        public abstract List<NamedSqlGeometry> GetGeometryLabelPairs(string whereClause);

        public abstract List<NamedSqlGeometry> GetGeometryLabelPairs(BoundingBox boundingBox);

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

        public abstract DataTable GetEntireFeatures(string whereClause);

        public abstract DataTable GetEntireFeatures(BoundingBox geometry);

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
}
