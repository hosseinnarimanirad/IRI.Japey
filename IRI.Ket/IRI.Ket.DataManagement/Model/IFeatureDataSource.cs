using IRI.Ham.SpatialBase;
using IRI.Ket.DataManagement.DataSource;
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

        #region Get Geometries

        public virtual List<SqlGeometry> GetGeometries()
        {
            return GetGeometries(string.Empty);
        }

        public abstract List<SqlGeometry> GetGeometries(string whereClause);

        public abstract List<SqlGeometry> GetGeometries(BoundingBox boundingBox);

        #endregion


        #region Get Geometry Label Pairs

        public abstract List<NamedSqlGeometry> GetGeometryLabelPairs(BoundingBox boundingBox);

        #endregion


        #region Get Attribute Methods

        public virtual List<object> GetAttributes(string attributeColumn)
        {
            return GetAttributes(attributeColumn, string.Empty);
        }

        public abstract List<object> GetAttributes(string attributeColumn, string whereClause);

        #endregion


        #region GetEntireFeature

        public abstract DataTable GetEntireFeature(string whereClause);

        public abstract DataTable GetEntireFeaturesWhereIntersects(SqlGeometry geometry);

        #endregion


        #region Async Methods
        public Task<List<SqlGeometry>> GetGeometriesAsync()
        {
            return Task.Run(() => { return GetGeometries(); });
        }

        public Task<List<SqlGeometry>> GetGeometriesAsync(BoundingBox boundingBox)
        {
            return Task.Run(() => { return GetGeometries(boundingBox); });
        }

        public Task<DataTable> GetEntireFeatureAsync(string whereClause)
        {
            return Task.Run(() => { return GetEntireFeature(whereClause); });
        }

        public Task<List<NamedSqlGeometry>> GetGeometryLabelPairsAsync(BoundingBox boundingBox)
        {
            return Task.Run(() => { return GetGeometryLabelPairs(boundingBox); });
        }

        #endregion
    }
}
