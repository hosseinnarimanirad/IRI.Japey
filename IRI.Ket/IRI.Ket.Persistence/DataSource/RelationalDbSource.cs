using IRI.Msh.Common.Primitives;
using System.Data;

namespace IRI.Ket.DataManagement.DataSource
{

    public abstract class RelationalDbSource<T> : FeatureDataSource<T> where T : class, IGeometryAware<Point>
    {
        protected string MakeWhereClause(string whereClause)
        {
            return string.IsNullOrWhiteSpace(whereClause) ? string.Empty : FormattableString.Invariant($" WHERE ({whereClause}) ");
        }

        public virtual List<Geometry<Point>> GetGeometries(string whereClause)
        {
            throw new NotImplementedException();
        }

        public virtual List<NamedGeometry<Point>> GetGeometryLabelPairs(string whereClause)
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


        public Task<List<Geometry<Point>>> GetGeometriesAsync(string whereClause)
        {
            return Task.Run(() => { return GetGeometries(whereClause); });
        }

        public Task<List<NamedGeometry<Point>>> GetGeometryLabelPairsAsync(string whereClause)
        {
            return Task.Run(() => { return GetGeometryLabelPairs(whereClause); });
        }

        public Task<DataTable> GetEntireFeatureAsync(string whereClause)
        {
            return Task.Run(() => { return GetEntireFeatures(whereClause); });
        }
    }
}
