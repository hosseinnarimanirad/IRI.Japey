using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Npgsql;
using System.Globalization;
using Microsoft.SqlServer.Types;
using System.Threading.Tasks;
using System.Data;
using IRI.Ket.DataManagement.Model;
using IRI.Msh.Common.Primitives;
using IRI.Ket.SqlServerSpatialExtension.Model;
using IRI.Ket.SpatialExtensions;

namespace IRI.Ket.DataManagement.DataSource
{
    public class PostGisDataSource : RelationalDbSource<SqlFeature>
    {
        public override BoundingBox Extent { get; protected set; }

        private string _connectionString;

        private string _tableName;

        private string _spatialColumnName;

        private string _schema;

        public PostGisDataSource(string server, string user, string password, string database, string port, string tableName, string spatialColumnName = null, string schema = "public")
            : this(Infrastructure.PostgreSqlInfrastructure.GetConnectionString(server, user, password, database, port), tableName, spatialColumnName, schema)
        {
        }

        public PostGisDataSource(string connectionString, string tableName, string spatialColumnName, string schema = "public")
        {
            this._connectionString = connectionString;
            //string.Format("Server={0}; UID={1}; PWD={2}; Database={3}; Port={4}", server, user, password, database, port);

            this._tableName = tableName;

            this._spatialColumnName = spatialColumnName;

            this._schema = schema;

            this.Extent = GetGeometries().GetBoundingBox();
        }

        private string GetProperSelectForSpatialColumn(string columnName)
        {
            return string.Format(System.Globalization.CultureInfo.InvariantCulture, "encode(ST_AsBinary({0}),'hex')", _spatialColumnName);
        }

        public DataTable ExecuteSql(string commandString)
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);

            NpgsqlCommand command = new NpgsqlCommand(commandString, connection);

            connection.Open();

            System.Data.DataTable result = new System.Data.DataTable();

            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command);

            adapter.Fill(result);

            connection.Close();

            return result;
        }

        public DataTable GetEntireFeaturesWhereIntersects(List<Point> region)
        {
            string wktRegion = string.Format(System.Globalization.CultureInfo.InvariantCulture, " POLYGON(({0})) ",
                string.Join(",", region.Select(i => string.Format(" {0} {1}", i.X, i.Y))));

            return GetEntireFeaturesWhereIntersects(wktRegion);
        }

        public DataTable GetAttributeColumns(string whereClause)
        {
            List<string> columns = Infrastructure.PostgreSqlInfrastructure.GetColumnNames(_connectionString, _schema ?? "public", _tableName);

            if (!columns.Contains(_spatialColumnName))
                throw new NotImplementedException();

            string command =
                string.Format(System.Globalization.CultureInfo.InvariantCulture, "SELECT {0} FROM {1} {2}",
                    string.Join(",", columns.Where(i => i != _spatialColumnName)),
                    _tableName,
                    string.IsNullOrEmpty(whereClause) ? string.Empty : whereClause);

            return ExecuteSql(command);
        }

        public DataTable GetAttributeColumnsWhereIntersects(string wktRegion)
        {
            string whereClause = string.Format(CultureInfo.InvariantCulture, " WHERE ST_INTERSECTS({0},'{1}'::geometry)", _spatialColumnName, wktRegion);

            return GetAttributeColumns(whereClause);
        }

        public DataTable GetAttributeColumnsWhereIntersects(List<Point> region)
        {
            string wktRegion = string.Format(System.Globalization.CultureInfo.InvariantCulture, " POLYGON(({0})) ",
                string.Join(",", region.Select(i => string.Format(" {0} {1}", i.X, i.Y))));

            return GetAttributeColumnsWhereIntersects(wktRegion);
        }

        public List<SqlGeometry> GetGeometriesWhereIntersects(string wktRegion)
        {
            string whereClause = string.Format(CultureInfo.InvariantCulture, " WHERE ST_INTERSECTS({0},'{1}'::geometry)", _spatialColumnName, wktRegion);

            return GetGeometries(whereClause);
        }

        public List<SqlGeometry> GetGeometriesWhereIntersects(List<Point> simpleRegion)
        {
            string wktRegion = string.Format(" POLYGON(({0})) ",
                string.Join(",", simpleRegion.Select(i => string.Format(System.Globalization.CultureInfo.InvariantCulture, " {0} {1}", i.X, i.Y))));

            return GetGeometriesWhereIntersects(wktRegion);
        }
         
        #region Override Methods

        public override List<SqlGeometry> GetGeometries()
        {
            return GetGeometries(string.Empty);
        }

        public override List<NamedSqlGeometry> GetGeometryLabelPairsForDisplay(BoundingBox boundingBox)
        {
            throw new NotImplementedException();
        }

        public override List<SqlGeometry> GetGeometries(string whereClause)
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);

            connection.Open();

            string commandString =
                string.Format(CultureInfo.InvariantCulture, "SELECT {0} FROM {1} {2}",
                    GetProperSelectForSpatialColumn(_spatialColumnName),
                    _tableName,
                    whereClause);

            NpgsqlCommand command = new NpgsqlCommand(commandString, connection);

            List<SqlGeometry> geometries = new List<SqlGeometry>();

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var obj = Common.Helpers.HexStringHelper.ToByteArray(reader[0].ToString());

                geometries.Add(SqlGeometry.STGeomFromWKB(new System.Data.SqlTypes.SqlBytes(obj), 0));
            }

            connection.Close();

            return geometries;
        }

        public override List<SqlGeometry> GetGeometries(BoundingBox boundingBox)
        {
            SqlGeometry boundary = boundingBox.AsSqlGeometry(GetSrid());

            return GetGeometries(boundary);
        }

        public override List<SqlGeometry> GetGeometries(SqlGeometry geometry)
        {
            return GetGeometries().Where(i => i.STIntersects(geometry).IsTrue).ToList();
        }

        //public override List<object> GetAttributes(string attributeColumn)
        //{
        //    throw new NotImplementedException();
        //}

        public override List<object> GetAttributes(string attributeColumn, string whereClause)
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

        public DataTable GetEntireFeaturesWhereIntersects(string wktRegion)
        {
            string whereClause = string.Format(System.Globalization.CultureInfo.InvariantCulture, " WHERE ST_INTERSECTS({0},'{1}'::geometry)", _spatialColumnName, wktRegion);

            return GetEntireFeatures(whereClause);
        }

        public override DataTable GetEntireFeatures(BoundingBox geometry)
        {
            var boundary = geometry.AsSqlGeometry(GetSrid());

            return GetEntireFeatures(boundary);
        }

        public override DataTable GetEntireFeatures()
        {
            string where = string.Empty;

            return GetEntireFeatures(where);
        }

        public override DataTable GetEntireFeatures(SqlGeometry geometry)
        {
            string wkt = new string(geometry.STAsText().Value);

            return GetEntireFeaturesWhereIntersects(wkt);
        }

        //WHERE ST_INTERSECTS(A,B)
        public override DataTable GetEntireFeatures(string whereClause)
        {
            List<string> columns = Infrastructure.PostgreSqlInfrastructure.GetColumnNames(_connectionString, _schema, _tableName);

            if (!columns.Contains(_spatialColumnName))
                throw new NotImplementedException();

            string command =
                string.Format(System.Globalization.CultureInfo.InvariantCulture, "SELECT {0} FROM {1} {2}",
                    string.Join(",", columns.Select(i => i == _spatialColumnName ? GetProperSelectForSpatialColumn(i) : i)),
                    _tableName,
                    string.IsNullOrEmpty(whereClause) ? string.Empty : whereClause);

            return ExecuteSql(command);
        }

        public override void Add(ISqlGeometryAware newValue)
        {
            throw new NotImplementedException();
        }

        public override void Remove(ISqlGeometryAware value)
        {
            throw new NotImplementedException();
        }

        public override void Update(ISqlGeometryAware newValue)
        {
            throw new NotImplementedException();
        }

        public override void UpdateFeature(ISqlGeometryAware feature)
        {
            throw new NotImplementedException();
        }

        public override void SaveChanges()
        {
            throw new NotImplementedException();
        }

        public override List<SqlFeature> GetFeatures(SqlGeometry geometry)
        {
            throw new NotImplementedException();
        }

        #endregion

        public override SqlFeatureSet GetSqlFeatures()
        {
            throw new NotImplementedException();
        }

        public override SqlFeatureSet GetSqlFeatures(SqlGeometry geometry)
        {
            throw new NotImplementedException();
        }
    }
}
