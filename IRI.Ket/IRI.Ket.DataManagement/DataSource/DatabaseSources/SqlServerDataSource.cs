using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Types;
using System.Data.SqlClient;
using IRI.Ket.SqlServerSpatialExtension;

using System.Threading.Tasks;
using IRI.Sta.Common.Primitives;
using System.Data;
using IRI.Ket.DataManagement.Model;

using IRI.Ket.SqlServerSpatialExtension.Model;
using System.Globalization;
using IRI.Ket.SpatialExtensions;
using IRI.Sta.Common.Model;

namespace IRI.Ket.DataManagement.DataSource
{
    public class SqlServerDataSource : IFeatureDataSource
    {
        const string _outputSpatialAttribute = "_shape";

        protected BoundingBox _extent = BoundingBox.NaN;

        public override BoundingBox Extent
        {
            get
            {
                if (double.IsNaN(_extent.Width) || double.IsNaN(_extent.Height) && _spatialColumnName != null)
                {
                    //this._extent = GetGeometries().GetBoundingBox();
                    this._extent = GetBoundingBox();
                }

                return _extent;
            }
            protected set
            {
                _extent = value;
            }
        }

        protected string _connectionString;

        protected string _tableName;

        protected string _queryString;

        protected string _spatialColumnName;

        protected string _labelColumnName;

        protected SqlServerDataSource()
        {

        }

        public SqlServerDataSource(string connectionString, string tableName, string spatialColumnName = null, string labelColumnName = null)
        {
            this._connectionString = connectionString;

            this._tableName = tableName;

            this._spatialColumnName = spatialColumnName;

            this._labelColumnName = labelColumnName;

            if (spatialColumnName == null)
            {
                this.Extent = BoundingBox.NaN;
            }
            else
            {
                //IMPORTANT!
                //this.Extent = GetGeometries().GetBoundingBox();
            }

        }

        public static SqlServerDataSource CreateForQueryString(string connectionString, string queryString, string spatialColumnName, string labelColumnName = null)
        {
            SqlServerDataSource result = new SqlServerDataSource(connectionString, null, spatialColumnName, labelColumnName)
            {
                _queryString = queryString,
            };

            return result;
        }

        private string GetTable()
        {
            return this._tableName ?? $" ({this._queryString}) A";
        }

        protected List<T> Select<T>(string selectQuery, string connectionString = null)
        {
            if (connectionString == null)
            {
                connectionString = _connectionString;
            }

            SqlConnection connection = new SqlConnection(connectionString);

            var command = new SqlCommand(selectQuery, connection);

            connection.Open();

            List<T> result = new List<T>();

            SqlDataReader reader = command.ExecuteReader();

            if (!reader.HasRows)
            {
                return new List<T>();
            }

            while (reader.Read())
            {
                result.Add((T)reader[0]);//2565 ms
            }

            connection.Close();

            return result;
        }

        public List<T> Select<T>(string selectQuery, Func<IDataRecord, T> mapFunction)
        {
            SqlConnection connection = new SqlConnection(_connectionString);

            var command = new SqlCommand(selectQuery, connection);

            connection.Open();

            List<T> result = new List<T>();

            SqlDataReader reader = command.ExecuteReader();

            if (!reader.HasRows)
            {
                return new List<T>();
            }

            while (reader.Read())
            {
                result.Add(mapFunction(reader));
            }

            connection.Close();

            return result;
        }

        public List<Dictionary<string, object>> SelectFeatures(string selectQuery, bool returnWkt = false)
        {
            SqlConnection connection = new SqlConnection(_connectionString);

            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();

            try
            {

                var command = new SqlCommand(selectQuery, connection);

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (!reader.HasRows)
                {
                    return new List<Dictionary<string, object>>();
                }

                while (reader.Read())
                {
                    var dict = new Dictionary<string, object>();

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        var fieldName = reader.GetName(i);

                        //var columnOrdinal = reader.GetOrdinal(fields[i]);

                        //if (returnWkt && fieldName == _spatialColumnName)
                        //{
                        //    dict.Add(_outputSpatialAttribute, reader.IsDBNull(i) ? null : ((SqlGeometry)reader[i]).AsWkt());
                        //}
                        //else
                        //{
                        //    dict.Add(fieldName, reader.IsDBNull(i) ? null : reader[i]);
                        //}

                        while (dict.Keys.Contains(fieldName))
                        {
                            fieldName = $"{fieldName}_";
                        }

                        if (reader.IsDBNull(i))
                        {
                            dict.Add(fieldName, null);
                        }
                        else
                        {
                            if (returnWkt && reader[i] is SqlGeometry)
                            {
                                dict.Add(fieldName, ((SqlGeometry)reader[i]).AsWkt());
                            }
                            else
                            {
                                dict.Add(fieldName, reader[i]);
                            }
                        }
                    }

                    result.Add(dict);
                }

                connection.Close();
            }
            catch (Exception ex)
            {
                connection.Close();
            }

            return result;
        }

        public FeatureSet QueryFeaturesWhereIntersects(string wktGeometryFilter)
        {
            return QueryFeatures(GetCommandString(wktGeometryFilter, false));
        }

        public FeatureSet QueryFeatures(string selectQuery)
        {
            SqlConnection connection = new SqlConnection(_connectionString);

            FeatureSet result = new FeatureSet() { Fields = new List<Field>(), Features = new List<Feature>() };

            try
            {
                var command = new SqlCommand(selectQuery, connection);

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    var type = reader.GetFieldType(i);

                    if (type != typeof(SqlGeometry))
                    {
                        result.Fields.Add(new Field() { Name = reader.GetName(i), Type = type.ToString() });
                    }
                }

                if (!reader.HasRows)
                {
                    return result;
                }

                while (reader.Read())
                {
                    var dict = new Dictionary<string, object>();

                    var feature = new Feature();

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        var fieldName = reader.GetName(i);

                        while (dict.Keys.Contains(fieldName))
                        {
                            fieldName = $"{fieldName}_";
                        }

                        if (reader.IsDBNull(i))
                        {
                            dict.Add(fieldName, null);
                        }
                        else
                        {
                            if (reader[i] is SqlGeometry)
                            {
                                feature.Geometry = (SqlGeometry)reader[i];
                            }
                            else
                            {
                                dict.Add(fieldName, reader[i]);
                            }
                        }
                    }

                    feature.Attributes = dict;

                    result.Features.Add(feature);
                }

                connection.Close();
            }
            catch (Exception ex)
            {
                connection.Close();
            }

            return result;

        }

        public List<Dictionary<string, object>> GetFeaturesWhereIntersects(string wktGeometryFilter, bool returnWkt = false)
        {
            return SelectFeatures(GetCommandString(wktGeometryFilter, false), returnWkt);
        }

        public override int GetSrid()
        {
            SqlConnection connection = null;

            int srid;

            try
            {
                connection = new SqlConnection(_connectionString);

                //SqlCommand command = new SqlCommand(string.Format(CultureInfo.InvariantCulture, "SELECT TOP 1 {0}.STSrid FROM {1} ", _spatialColumnName, GetTable()), connection);
                SqlCommand command = new SqlCommand(FormattableString.Invariant($"SELECT TOP 1 {_spatialColumnName}.STSrid FROM {GetTable()} WHERE NOT {_spatialColumnName} IS NULL AND {_spatialColumnName}.STIsValid()=1"), connection);

                connection.Open();

                List<SqlGeometry> geometries = new List<SqlGeometry>();

                srid = (int)command.ExecuteScalar();

                connection.Close();

            }
            catch
            {
                srid = 0;
            }
            finally
            {
                connection.Close();
            }

            return srid;
        }

        public BoundingBox GetBoundingBox()
        {
            //var query = string.Format(CultureInfo.InvariantCulture, "SELECT {0}.STEnvelope() FROM {1} ", _spatialColumnName, GetTable());
            var query = FormattableString.Invariant($"SELECT {_spatialColumnName}.STEnvelope() FROM {GetTable()} ");

            var envelopes = SelectGeometries(query);

            return SqlSpatialExtensions.GetBoundingBoxFromEnvelopes(envelopes);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeColumn"></param>
        /// <param name="whereClause">Do not include the "WHERE", e.g. coulumn01 = someValue</param>
        /// <returns></returns>
        public override List<object> GetAttributes(string attributeColumn, string whereClause)
        {
            var selectQuery = FormattableString.Invariant($"SELECT {attributeColumn} FROM {GetTable()} {MakeWhereClause(whereClause)}");

            return Select<object>(selectQuery);

            //SqlConnection connection = new SqlConnection(_connectionString);

            //SqlCommand command = new SqlCommand(string.Format(CultureInfo.InvariantCulture, "SELECT {0} FROM {1} {2} ", attributeColumn, GetTable(), whereClause ?? string.Empty), connection);

            //connection.Open();

            //List<object> result = new List<object>();

            //SqlDataReader reader = command.ExecuteReader();

            //if (!reader.HasRows)
            //{
            //    return new List<object>();
            //}

            //while (reader.Read())
            //{
            //    result.Add(reader[0]);
            //}

            //connection.Close();

            //return result.Cast<object>().ToList();
        }

        protected static string GetWhereClause(string spatialColumnName, BoundingBox boundingBox, int srid)
        {
            return FormattableString.Invariant($" {spatialColumnName}.STIntersects(GEOMETRY::STPolyFromText('{boundingBox.AsWkt()}',{ srid})) = 1 ");
        }

        protected string GetCommandString(string wktGeometryFilter, bool returnOnlyGeometry = true)
        {
            var srid = GetSrid();

            var wkbArray = SqlSpatialExtensions.Parse(wktGeometryFilter).STAsBinary().Value;

            var wkbString = Common.Helpers.HexStringHelper.ByteToHexBitFiddle(wkbArray, true);

            if (returnOnlyGeometry)
            {
                return FormattableString.Invariant($@"
                DECLARE @filter GEOMETRY;
                SET @filter = GEOMETRY::STGeomFromWKB({wkbString},{srid});
                SELECT {_spatialColumnName} FROM {GetTable()} WHERE {_spatialColumnName}.STIntersects(@filter)=1");
            }
            else
            {
                return FormattableString.Invariant($@"
                DECLARE @filter GEOMETRY;
                SET @filter = GEOMETRY::STGeomFromWKB({wkbString},{srid});
                SELECT * FROM {GetTable()} WHERE {_spatialColumnName}.STIntersects(@filter)=1");
            }

        }

        #region Get Geometries

        //3857: web mercator; 102100: web mercator
        public override List<SqlGeometry> GetGeometries(BoundingBox boundingBox)
        {
            var srid = GetSrid();
            //if (string.IsNullOrWhiteSpace(whereClause))
            //{
            var whereClause = FormattableString.Invariant($" ({GetWhereClause(_spatialColumnName, boundingBox, srid)}) ");
            //}
            //else
            //{
            //    whereClause = FormattableString.Invariant($" ({whereClause}) AND ({GetWhereClause(_spatialColumnName, boundingBox, srid)}) ");
            //}

            return this.GetGeometries(whereClause);
        }

        ////3857: web mercator; 102100: web mercator
        //public List<SqlGeometry> GetGeometries(BoundingBox boundingBox, string whereClause, int srid = -1)
        //{
        //    if (srid == -1)
        //    {
        //        srid = GetSrid();
        //    }

        //    //if (string.IsNullOrWhiteSpace(whereClause))
        //    //{
        //    //    whereClause = $" WHERE ({this._spatialColumnName}.STIntersects(GEOMETRY::STPolyFromText('{boundingBox.AsWkt()}',{srid})) = 1) ";
        //    //}
        //    //else
        //    //{
        //    //    whereClause = $" {whereClause} AND ({this._spatialColumnName}.STIntersects(GEOMETRY::STPolyFromText('{boundingBox.AsWkt()}',{srid})) = 1) ";
        //    //}


        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="whereClause">Do not include the "WHERE", e.g. coulumn01 = someValue</param>
        /// <returns></returns>
        public override List<SqlGeometry> GetGeometries(string whereClause)
        {
            return SelectGeometries(FormattableString.Invariant($"SELECT {_spatialColumnName} FROM {GetTable()} {MakeWhereClause(whereClause)}"));
            //return SelectGeometries(string.Format(CultureInfo.InvariantCulture, "SELECT {0} FROM {1} {2} ", _spatialColumnName, GetTable(), whereClause ?? string.Empty));
        }

        protected List<SqlGeometry> SelectGeometries(string selectQuery, string connectionString = null)
        {
            if (connectionString == null)
            {
                connectionString = _connectionString;
            }

            SqlConnection connection = new SqlConnection(connectionString);

            connection.Open();

            var command = new SqlCommand(selectQuery, connection);

            List<SqlGeometry> geometries = new List<SqlGeometry>();

            using (var reader = command.ExecuteReader())
            {
                if (!reader.HasRows)
                {
                    return new List<SqlGeometry>();
                }

                while (reader.Read())
                {
                    //approach 1
                    //geometries.Add(SqlGeometry.STGeomFromWKB(new System.Data.SqlTypes.SqlBytes((byte[])reader[0]), srid).MakeValid()); //4100-4200 ms
                    //approach 2
                    //geometries.Add(SqlGeometry.Deserialize(reader.GetSqlBytes(0))); //3220 ms

                    //approach 3 
                     
                    geometries.Add(reader[0] as SqlGeometry);//2565 ms

                }
            }

            connection.Close();

            return geometries;
        }


        public List<SqlGeometry> GetGeometriesWhereIntersects(string wktGeometryFilter)
        {
            return SelectGeometries(GetCommandString(wktGeometryFilter));

            //return SelectGeometries(FormattableString.Invariant($"SELECT {_spatialColumnName} FROM {GetTable()} WHERE {_spatialColumnName}.STIntersects(GEOMETRY::STGeomFromWKB({wkbString},{srid}))=1"));
        }

        #endregion


        #region Get Geometry Label Pair

        public override List<NamedSqlGeometry> GetGeometryLabelPairs(string whereClause)
        {
            SqlConnection connection = new SqlConnection(_connectionString);

            //SqlCommand command =
            //    new SqlCommand(
            //        string.Format(CultureInfo.InvariantCulture, "SELECT {0}, {1} FROM {2} {3} ", _spatialColumnName, _labelColumnName, GetTable(), whereClause ?? string.Empty),
            //        connection);

            var commandText = string.IsNullOrWhiteSpace(_labelColumnName) ?
                FormattableString.Invariant($"SELECT {_spatialColumnName} FROM {GetTable()} {MakeWhereClause(whereClause)} ") :
                FormattableString.Invariant($"SELECT {_spatialColumnName}, {_labelColumnName} FROM {GetTable()} {MakeWhereClause(whereClause)} ");

            SqlCommand command = new SqlCommand(commandText, connection);

            connection.Open();

            var result = new List<NamedSqlGeometry>();

            using (SqlDataReader reader = command.ExecuteReader())
            {

                if (!reader.HasRows)
                {
                    return new List<NamedSqlGeometry>();
                }

                if (string.IsNullOrWhiteSpace(_labelColumnName))
                {
                    while (reader.Read())
                    {
                        result.Add(new NamedSqlGeometry((SqlGeometry)reader[0], string.Empty));
                    }
                }
                else
                {
                    while (reader.Read())
                    {
                        result.Add(new NamedSqlGeometry((SqlGeometry)reader[0], reader[1]?.ToString()));
                    }
                }
            }

            connection.Close();

            return result;
        }

        public override List<NamedSqlGeometry> GetGeometryLabelPairs(BoundingBox boundingBox)
        {
            //List<string> attributes = GetAttributes(_labelColumnName).Select(i => i.ToString()).ToList();

            //SqlGeometry boundary = boundingBox.ToSqlGeometry();

            //return GetGeometries().Zip(attributes, (a, b) => new NamedSqlGeometry(a, b)).Where(i => i.Geometry.STIntersects(boundary).Value).ToList();

            var whereClause = FormattableString.Invariant($" {GetWhereClause(_spatialColumnName, boundingBox, GetSrid())}");

            return GetGeometryLabelPairs(whereClause);
        }

        protected List<NamedSqlGeometry> SelectGeometryLabelPairs(string selectQuery, string connectionString = null)
        {
            if (connectionString == null)
            {
                connectionString = _connectionString;
            }

            SqlConnection connection = new SqlConnection(connectionString);

            connection.Open();

            var command = new SqlCommand(selectQuery, connection);

            List<NamedSqlGeometry> geometries = new List<NamedSqlGeometry>();

            using (var reader = command.ExecuteReader())
            {
                if (!reader.HasRows)
                {
                    return new List<NamedSqlGeometry>();
                }

                while (reader.Read())
                {
                    //approach 1
                    //geometries.Add(SqlGeometry.STGeomFromWKB(new System.Data.SqlTypes.SqlBytes((byte[])reader[0]), srid).MakeValid()); //4100-4200 ms
                    //approach 2
                    //geometries.Add(SqlGeometry.Deserialize(reader.GetSqlBytes(0))); //3220 ms

                    //approach 3 
                    geometries.Add(new NamedSqlGeometry((SqlGeometry)reader[0], string.Empty));//2565 ms

                }
            }

            connection.Close();

            return geometries;
        }

        #endregion

        public override List<SqlGeometry> GetGeometries(SqlGeometry geometry)
        {
            throw new NotImplementedException();
        }

        public override List<NamedSqlGeometry> GetGeometryLabelPairs()
        {
            throw new NotImplementedException();
        }

        public override List<NamedSqlGeometry> GetGeometryLabelPairs(SqlGeometry geometry)
        {
            throw new NotImplementedException();
        }


        #region Get Entire Feature

        public DataTable ExecuteSql(string commandString)
        {
            SqlConnection connection = new SqlConnection(_connectionString);

            SqlCommand command = new SqlCommand(commandString, connection);

            connection.Open();

            DataTable result = new DataTable();

            SqlDataAdapter adapter = new SqlDataAdapter(command);

            adapter.Fill(result);

            //result.Load(command.ExecuteReader());

            connection.Close();

            return result;
        }

        public DataTable GetEntireFeature()
        {
            //string query = string.Empty;

            //if (string.IsNullOrEmpty(_tableName))
            //{
            //    query = _queryString;
            //}
            //else
            //{
            //    query = string.Format(CultureInfo.InvariantCulture, "SELECT * FROM {0}", GetTable());
            //}
            //return ExecuteSql(query);
            return GetEntireFeatures(string.Empty);
        }

        public override DataTable GetEntireFeatures(string whereClause)
        {
            //return ExecuteSql(string.Format(CultureInfo.InvariantCulture, "SELECT * FROM {0} {1} ", GetTable(), whereClause ?? string.Empty));
            return ExecuteSql(FormattableString.Invariant($"SELECT * FROM {GetTable()} {MakeWhereClause(whereClause)} "));
        }

        public override DataTable GetEntireFeatures(SqlGeometry geometry)
        {
            throw new NotImplementedException();
        }

        public override DataTable GetEntireFeatures(BoundingBox geometry)
        {
            throw new NotImplementedException();
        }
         

        #endregion
    }
}