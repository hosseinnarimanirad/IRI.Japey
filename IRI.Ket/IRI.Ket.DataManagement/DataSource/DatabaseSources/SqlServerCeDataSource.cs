using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Types;
using System.Data.SqlServerCe;
using System.IO;
using IRI.Ket.SqlServerSpatialExtension;
using System.Threading.Tasks;
using IRI.Ham.SpatialBase;
using System.Data;
using IRI.Ket.DataManagement.Model;
using IRI.Ket.SqlServerSpatialExtension.Model;
using IRI.Ket.SpatialExtensions;

namespace IRI.Ket.DataManagement.DataSource
{
    public class SqlServerCeDataSource : IFeatureDataSource
    {
        public override BoundingBox Extent { get; protected set; }

        //private string _connectionString;

        private SqlCeConnection _connection;

        private string _tableName;

        private string _indexName;

        private string _spatialColumnName, _labelColumnName;

        private bool _wktMode;
        //private readonly string _getAllGeometriesCommand;

        public SqlServerCeDataSource(string connectionString, string tableName, string spatialColumnName = null, bool wktMode = false, string labelColumnName = null)
        {
            //this._connectionString = connectionString;

            this._connection = new SqlCeConnection(connectionString);

            this._connection.Open();

            this._tableName = tableName;

            this._spatialColumnName = spatialColumnName;

            this._labelColumnName = labelColumnName;

            this._wktMode = wktMode;

            //this._indexName = indexName;

            if (spatialColumnName == null)
            {
                this.Extent = BoundingBox.NaN;
            }
            else
            {
                this.Extent = GetGeometries().GetBoundingBox();
            }

        }

        //public List<SqlGeometry> GetGeometries()
        //{
        //    return GetGeometries(string.Empty);
        //}

        //public override List<object> GetAttributes(string attributeColumn)
        //{
        //    return GetAttributes(attributeColumn, string.Empty);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="whereClause"> forget the "WHERE", e.g.   coulumn01 = someValue</param>
        /// <returns></returns>
        public override List<SqlGeometry> GetGeometries(string whereClause)
        {
            //SqlCeConnection connection = new SqlCeConnection(_connectionString);

            //_connection.Open();

            List<Microsoft.SqlServer.Types.SqlGeometry> geometries = new List<Microsoft.SqlServer.Types.SqlGeometry>();

            SqlCeCommand command =
                new SqlCeCommand(
                    string.Format(System.Globalization.CultureInfo.InvariantCulture, "SELECT {0} FROM {1} {2} ", _spatialColumnName, _tableName, MakeWhereClause(whereClause)),
                    _connection);

            command.CommandType = System.Data.CommandType.Text;

            SqlCeResultSet resultSet = command.ExecuteResultSet(ResultSetOptions.Scrollable);

            if (resultSet.HasRows)
            {
                int columnIndex = resultSet.GetOrdinal(_spatialColumnName);

                if (_wktMode)
                {
                    while (resultSet.Read())
                    {
                        geometries.Add(SqlGeometry.Parse(resultSet.GetString(columnIndex)).MakeValid());
                    }
                }
                else
                {
                    while (resultSet.Read())
                    {
                        geometries.Add(SqlGeometry.STGeomFromWKB(
                            new System.Data.SqlTypes.SqlBytes((byte[])resultSet.GetValue(columnIndex)), 0).MakeValid());
                    }
                }
            }

            //connection.Close();

            return geometries;
        }

        public override List<SqlGeometry> GetGeometries(BoundingBox boundingBox)
        {
            //SqlGeometry boundary =
            //  SqlGeometry.Parse(
            //      string.Format(System.Globalization.CultureInfo.InvariantCulture,
            //      "POLYGON(({0} {1}, {0} {2}, {3} {2}, {3} {1}, {0} {1}))", boundingBox.XMin, boundingBox.YMin, boundingBox.YMax, boundingBox.XMax));

            //return GetGeometries().Where(i => i.STIntersects(boundary).IsTrue).ToList();
            SqlGeometry boundary = boundingBox.AsSqlGeometry();

            return GetGeometries().Where(i => i.STIntersects(boundary).IsTrue).ToList();
        }

        //public Task<List<SqlGeometry>> GetGeometriesAsync(BoundingBox boundingBox)
        //{
        //    return Task.Run<List<SqlGeometry>>(() => { return GetGeometries(boundingBox); });
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeColumn"></param>
        /// <param name="whereClause"> forget the "WHERE", e.g.  coulumn01 = someValue</param>
        /// <returns></returns>
        public override List<object> GetAttributes(string attributeColumn, string whereClause)
        {
            //SqlCeConnection connection = new SqlCeConnection(_connectionString);

            SqlCeCommand command = new SqlCeCommand(string.Format(System.Globalization.CultureInfo.InvariantCulture, "SELECT {0} FROM {1} {2}", attributeColumn, _tableName, MakeWhereClause(whereClause)), _connection);

            command.CommandType = System.Data.CommandType.Text;

            //connection.Open();

            List<object> result = new List<object>();

            SqlCeResultSet resultSet = command.ExecuteResultSet(ResultSetOptions.Scrollable);

            if (!resultSet.HasRows)
            {
                return new List<object>();
            }

            int columnIndex = resultSet.GetOrdinal(attributeColumn);

            while (resultSet.Read())
            {
                result.Add(resultSet.GetValue(columnIndex));
            }

            //connection.Close();

            return result.Cast<object>().ToList();
        }

        public System.Data.DataTable ExecuteSql(string commandString)
        {
            //SqlCeConnection connection = new SqlCeConnection(_connectionString);

            SqlCeCommand command = new SqlCeCommand(commandString, _connection);

            command.CommandType = System.Data.CommandType.Text;

            //connection.Open();

            System.Data.DataTable result = new System.Data.DataTable();

            SqlCeDataAdapter adapter = new SqlCeDataAdapter(command);

            adapter.Fill(result);

            //result.Load(command.ExecuteReader());
            //command.ExecuteResultSet(ResultSetOptions.Scrollable);

            //connection.Close();

            return result;
        }

        public System.Data.DataTable GetEntireFeature()
        {
            return ExecuteSql(string.Format(System.Globalization.CultureInfo.InvariantCulture, "SELECT * FROM {0}", this._tableName));
        }

        public override System.Data.DataTable GetEntireFeatures(string whereClause)
        {
            return ExecuteSql(string.Format(System.Globalization.CultureInfo.InvariantCulture, "SELECT * FROM {0} {1}", this._tableName, MakeWhereClause(whereClause)));
        }

        //public   Task<DataTable> GetEntireFeatureAsync(string whereClause)
        //{
        //    return Task.Run(() => { return GetEntireFeature(whereClause); });
        //}

        //public List<string> GetLabels()
        //{
        //    return GetAttributes(this._labelColumnName).Select(i => i.ToString()).ToList();
        //}

        //public IEnumerable<Tuple<SqlGeometry, string>> GetGeometryLabelPairs()
        //{
        //    throw new NotImplementedException();
        //}

        //public IEnumerable<Tuple<SqlGeometry, string>> GetGeometryLabelPairs(string whereClause)
        //{
        //    throw new NotImplementedException();
        //}

        public override List<NamedSqlGeometry> GetGeometryLabelPairs(BoundingBox boundingBox)
        {
            List<string> attributes = GetAttributes(this._labelColumnName).Select(i => i.ToString()).ToList();

            SqlGeometry boundary = boundingBox.AsSqlGeometry();

            return GetGeometries().Zip(attributes, (a, b) => new NamedSqlGeometry(a, b)).Where(i => i.Geometry.STIntersects(boundary).Value).ToList();

        }

        public override DataTable GetEntireFeaturesWhereIntersects(SqlGeometry geometry)
        {
            throw new NotImplementedException();
        }

        public override List<SqlGeometry> GetGeometries(SqlGeometry geometry)
        {
            throw new NotImplementedException();
        }

        public override List<NamedSqlGeometry> GetGeometryLabelPairs()
        {
            throw new NotImplementedException();
        }

        public override List<NamedSqlGeometry> GetGeometryLabelPairs(string whereClause)
        {
            throw new NotImplementedException();
        }

        public override List<NamedSqlGeometry> GetGeometryLabelPairs(SqlGeometry geometry)
        {
            throw new NotImplementedException();
        }

        public override DataTable GetEntireFeatures(BoundingBox geometry)
        {
            throw new NotImplementedException();
        }

        //public Task<IEnumerable<NamedSqlGeometry>> GetGeometryLabelPairsAsync(BoundingBox boundingBox)
        //{
        //    throw new NotImplementedException();
        //}

        ~SqlServerCeDataSource()
        {
            if (this._connection != null)
            {
                this._connection.Close();
                this._connection.Dispose();
            }
        }


    }
}
