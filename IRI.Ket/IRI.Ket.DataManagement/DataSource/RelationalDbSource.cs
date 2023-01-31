using IRI.Ket.DataManagement.Model;
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
    //public abstract class RelationalDbSource : FeatureDataSource
    //{

    //    public virtual List<SqlGeometry> GetGeometries(string whereClause)
    //    {
    //        throw new NotImplementedException();
    //    }

    //}


    public abstract class RelationalDbSource<T> : FeatureDataSource<T> where T : class, ISqlGeometryAware
    {
       

        protected string MakeWhereClause(string whereClause)
        {
            return string.IsNullOrWhiteSpace(whereClause) ? string.Empty : FormattableString.Invariant($" WHERE ({whereClause}) ");
        }

        public virtual List<SqlGeometry> GetGeometries(string whereClause)
        {
            throw new NotImplementedException();
        }

        public virtual List<NamedSqlGeometry> GetGeometryLabelPairs(string whereClause)
        {
            throw new NotImplementedException();
        }

        //public virtual List<object> GetAttributes(string attributeColumn)
        //{
        //    return GetAttributes(attributeColumn, string.Empty);
        //}

        //public abstract List<object> GetAttributes(string attributeColumn, string whereClause);

        public virtual DataTable GetEntireFeatures(string whereClause)
        {
            throw new NotImplementedException();
        }


        public Task<List<SqlGeometry>> GetGeometriesAsync(string whereClause)
        {
            return Task.Run(() => { return GetGeometries(whereClause); });
        }

        public Task<List<NamedSqlGeometry>> GetGeometryLabelPairsAsync(string whereClause)
        {
            return Task.Run(() => { return GetGeometryLabelPairs(whereClause); });
        }

        public Task<DataTable> GetEntireFeatureAsync(string whereClause)
        {
            return Task.Run(() => { return GetEntireFeatures(whereClause); });
        }
    }
}
