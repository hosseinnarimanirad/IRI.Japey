﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Types;
using System.Data.SqlClient;
using IRI.Ket.SqlServerSpatialExtension;

using System.Threading.Tasks;
using IRI.Msh.Common.Primitives;
using System.Data;
using IRI.Ket.DataManagement.Model;

using IRI.Ket.SqlServerSpatialExtension.Model;
using System.Globalization;
using IRI.Ket.SpatialExtensions;
using IRI.Msh.Common.Model;

namespace IRI.Ket.DataManagement.DataSource
{
    public class SqlServerDataSource : RelationalDbSource<SqlFeature>
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

        public Action<ISqlGeometryAware> AddAction;

        public Action<int> RemoveAction;

        public Action<ISqlGeometryAware> UpdateAction;

        public string IdColumnName { get; set; }

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

        protected static string GetWhereClause(string spatialColumnName, BoundingBox boundingBox, int srid)
        {
            return FormattableString.Invariant($" {spatialColumnName}.STIntersects(GEOMETRY::STPolyFromText('{boundingBox.AsWkt()}',{srid})) = 1 ");
        }

        //protected string GetCommandString(string wktGeometryFilter, bool returnOnlyGeometry = true)
        //{
        //    if (string.IsNullOrWhiteSpace(wktGeometryFilter))
        //    {
        //        return GetCommandString(null, returnOnlyGeometry);
        //    }
        //    else
        //    {
        //        var wkbArray = IRI.Ket.SqlServerSpatialExtension.Helpers.SqlSpatialHelper.Parse(wktGeometryFilter).STAsBinary().Value;

        //        return MakeSelectCommandWithWkb(wkbArray, returnOnlyGeometry);
        //    }
        //}

        //protected string MakeSelectCommand(string whereClause)
        //{
        //    return FormattableString.Invariant($"SELECT {_spatialColumnName} FROM {GetTable()} {MakeWhereClause(whereClause)}");
        //}

        protected string MakeSelectCommand(string whereClause, bool returnOnlyGeometry)
        {
            if (returnOnlyGeometry)
            {
                return FormattableString.Invariant($"SELECT {_spatialColumnName} FROM {GetTable()} {MakeWhereClause(whereClause)}");
            }
            else
            {
                return FormattableString.Invariant($"SELECT * FROM {GetTable()} {MakeWhereClause(whereClause)}");
            }
        }

        protected string MakeSelectCommandWithWkt(string wktGeometryFilter, bool returnOnlyGeometry)
        {
            if (string.IsNullOrWhiteSpace(wktGeometryFilter))
            {
                return MakeSelectCommand(string.Empty, returnOnlyGeometry);
            }

            var srid = GetSrid();

            if (returnOnlyGeometry)
            {
                return FormattableString.Invariant($@"
                DECLARE @filter GEOMETRY;
                SET @filter = GEOMETRY::STGeomFromText({wktGeometryFilter},{srid});
                SELECT {_spatialColumnName} FROM {GetTable()} WHERE {_spatialColumnName}.STIntersects(@filter)=1");
            }
            else
            {
                return FormattableString.Invariant($@"
                DECLARE @filter GEOMETRY;
                SET @filter = GEOMETRY::STGeomFromText({wktGeometryFilter},{srid});
                SELECT * FROM {GetTable()} WHERE {_spatialColumnName}.STIntersects(@filter)=1");
            }
        }

        protected string MakeSelectCommandWithWkb(byte[] wkbGeometryFilter, bool returnOnlyGeometry)
        {
            if (wkbGeometryFilter == null)
            {
                return MakeSelectCommand(string.Empty, returnOnlyGeometry);
            }

            var srid = GetSrid();

            var wkbString = Common.Helpers.HexStringHelper.ByteToHexBitFiddle(wkbGeometryFilter, true);

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

        public override int GetSrid()
        {
            SqlConnection connection = null;

            int srid;

            try
            {
                connection = new SqlConnection(_connectionString);

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

            return IRI.Ket.SqlServerSpatialExtension.Helpers.SqlSpatialHelper.GetBoundingBoxFromEnvelopes(envelopes);
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
            return Infrastructure.SqlServerInfrastructure.SelectFeatures(_connectionString, selectQuery, returnWkt);

            //SqlConnection connection = new SqlConnection(_connectionString);

            //List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();

            //try
            //{

            //    var command = new SqlCommand(selectQuery, connection);

            //    connection.Open();

            //    SqlDataReader reader = command.ExecuteReader();

            //    if (!reader.HasRows)
            //    {
            //        return new List<Dictionary<string, object>>();
            //    }

            //    while (reader.Read())
            //    {
            //        var dict = new Dictionary<string, object>();

            //        for (int i = 0; i < reader.FieldCount; i++)
            //        {
            //            var fieldName = reader.GetName(i);

            //            //var columnOrdinal = reader.GetOrdinal(fields[i]);

            //            //if (returnWkt && fieldName == _spatialColumnName)
            //            //{
            //            //    dict.Add(_outputSpatialAttribute, reader.IsDBNull(i) ? null : ((SqlGeometry)reader[i]).AsWkt());
            //            //}
            //            //else
            //            //{
            //            //    dict.Add(fieldName, reader.IsDBNull(i) ? null : reader[i]);
            //            //}

            //            while (dict.Keys.Contains(fieldName))
            //            {
            //                fieldName = $"{fieldName}_";
            //            }

            //            if (reader.IsDBNull(i))
            //            {
            //                dict.Add(fieldName, null);
            //            }
            //            else
            //            {
            //                if (returnWkt && reader[i] is SqlGeometry)
            //                {
            //                    dict.Add(fieldName, ((SqlGeometry)reader[i]).AsWkt());
            //                }
            //                else
            //                {
            //                    dict.Add(fieldName, reader[i]);
            //                }
            //            }
            //        }

            //        result.Add(dict);
            //    }

            //    connection.Close();
            //}
            //catch (Exception ex)
            //{
            //    connection.Close();
            //}

            //return result;
        }

        public List<Dictionary<string, object>> GetFeaturesWhereIntersects(string wktGeometryFilter, bool returnGeometryAsWktForm = false)
        {
            //return SelectFeatures(GetCommandString(wktGeometryFilter, false), returnWkt);
            return SelectFeatures(MakeSelectCommandWithWkt(wktGeometryFilter, false), returnGeometryAsWktForm);
        }

        #region Get Geometries

        public override List<SqlGeometry> GetGeometries()
        {
            return GetGeometries(string.Empty);
        }

        //3857: web mercator; 102100: web mercator
        public override List<SqlGeometry> GetGeometries(BoundingBox boundingBox)
        {
            var srid = GetSrid();

            var whereClause = GetWhereClause(_spatialColumnName, boundingBox, srid);

            return this.GetGeometries(whereClause);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="whereClause">Do not include the "WHERE", e.g. coulumn01 = someValue</param>
        /// <returns></returns>
        public override List<SqlGeometry> GetGeometries(string whereClause)
        {
            //return SelectGeometries(FormattableString.Invariant($"SELECT {_spatialColumnName} FROM {GetTable()} {MakeWhereClause(whereClause)}"));
            return SelectGeometries(MakeSelectCommand(whereClause, true));
        }

        public override List<SqlGeometry> GetGeometries(SqlGeometry geometry)
        {
            if (geometry == null)
            {
                return GetGeometries();
            }

            return GetGeometriesWhereIntersects(geometry.AsWkt());
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
            //if (wktGeometryFilter == null)
            //{
            //    return GetGeometries();
            //}

            //return SelectGeometries(GetCommandStringForGeometryOnly(wktGeometryFilter));
            return SelectGeometries(MakeSelectCommandWithWkt(wktGeometryFilter, true));
        }

        public List<SqlGeometry> GetGeometriesWhereIntersects(byte[] wkbGeometryFilter)
        {
            if (wkbGeometryFilter == null)
            {
                return GetGeometries();
            }

            return SelectGeometries(MakeSelectCommandWithWkb(wkbGeometryFilter, true));
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
                MakeSelectCommand(whereClause, true) :
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

        public override List<NamedSqlGeometry> GetGeometryLabelPairsForDisplay(BoundingBox boundingBox)
        {
            //List<string> attributes = GetAttributes(_labelColumnName).Select(i => i.ToString()).ToList();

            //SqlGeometry boundary = boundingBox.ToSqlGeometry();

            //return GetGeometries().Zip(attributes, (a, b) => new NamedSqlGeometry(a, b)).Where(i => i.Geometry.STIntersects(boundary).Value).ToList();

            var whereClause = GetWhereClause(_spatialColumnName, boundingBox, GetSrid());

            return GetGeometryLabelPairs(whereClause);
        }

        public override List<NamedSqlGeometry> GetGeometryLabelPairs()
        {
            throw new NotImplementedException();
        }

        public override List<NamedSqlGeometry> GetGeometryLabelPairs(SqlGeometry geometry)
        {
            throw new NotImplementedException();
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

        public override DataTable GetEntireFeatures()
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


        #region Get FeatureSet

        public SqlFeatureSet QueryFeatures()
        {
            return QueryFeatures(MakeSelectCommand(null, false));
        }

        public SqlFeatureSet QueryFeaturesWhereIntersects(string wktGeometryFilter)
        {
            //return QueryFeatures(GetCommandString(wktGeometryFilter, false));
            return QueryFeatures(MakeSelectCommandWithWkt(wktGeometryFilter, false));
        }

        private SqlFeatureSet QueryFeatures(string selectQuery)
        {
            SqlConnection connection = new SqlConnection(_connectionString);

            SqlFeatureSet result = new SqlFeatureSet(this.GetSrid()) { Fields = new List<Field>(), Features = new List<SqlFeature>() };

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

                    var feature = new SqlFeature();

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
                                feature.TheSqlGeometry = (SqlGeometry)reader[i];
                            }
                            else
                            {
                                dict.Add(fieldName, reader[i]);
                            }
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(IdColumnName))
                    {
                        feature.Id = (int)dict[IdColumnName];
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

        public SqlFeatureSet QueryFeaturesWhereIntersects(BoundingBox boundingBox)
        {
            var whereClause = GetWhereClause(_spatialColumnName, boundingBox, GetSrid());

            return QueryFeatures(MakeSelectCommand(whereClause, false));
        }

        #endregion

        public override List<SqlFeature> GetFeatures(SqlGeometry geometry)
        {
            //var selectQuery = GetCommandString(geometry?.AsWkt(), false);
            var selectQuery = MakeSelectCommandWithWkb(geometry?.AsWkb(), false);

            var featureSet = QueryFeatures(selectQuery);

            return featureSet?.Features;
        }

        public override void Add(ISqlGeometryAware newFeature)
        {
            this.AddAction?.Invoke(newFeature);
        }

        public void Remove(int featureId)
        {
            this.RemoveAction?.Invoke(featureId);
        }

        public override void Update(ISqlGeometryAware newFeature)
        {
            this.UpdateAction?.Invoke(newFeature);
        }

        public override void UpdateFeature(ISqlGeometryAware feature)
        {
            throw new NotImplementedException();
        }

        public override void Remove(ISqlGeometryAware feature)
        {
            Remove(feature.Id);
        }

        public override void SaveChanges()
        {
            throw new NotImplementedException();
        }

        public override SqlFeatureSet GetSqlFeatures()
        {
            return QueryFeatures();
        }

        public override SqlFeatureSet GetSqlFeatures(SqlGeometry geometry)
        {
            var boundingBox = geometry.GetBoundingBox();

            var featureSet = QueryFeaturesWhereIntersects(boundingBox);
            //.Features
            //.Where(s => s.TheSqlGeometry?.STIntersects(geometry).IsTrue == true)
            //.ToList();

            featureSet.Features = featureSet.Features.Where(s => s.TheSqlGeometry?.STIntersects(geometry).IsTrue == true).ToList();

            return featureSet;
        }
    }
}