using System;
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
using IRI.Msh.Common.Extensions;

namespace IRI.Ket.DataManagement.DataSource
{
    public class SqlServerDataSource : RelationalDbSource<Feature<Point>>
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

        public Action<IGeometryAware<Point>> AddAction;

        public Action<int> RemoveAction;

        public Action<IGeometryAware<Point>> UpdateAction;

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

                List<Geometry<Point>> geometries = new List<Geometry<Point>>();

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

            //return IRI.Ket.SqlServerSpatialExtension.Helpers.SqlSpatialHelper.GetBoundingBoxFromEnvelopes(envelopes);
            return envelopes.GetBoundingBox();
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="attributeColumn"></param>
        ///// <param name="whereClause">Do not include the "WHERE", e.g. coulumn01 = someValue</param>
        ///// <returns></returns>
        //public override List<object> GetAttributes(string attributeColumn, string whereClause)
        //{
        //    var selectQuery = FormattableString.Invariant($"SELECT {attributeColumn} FROM {GetTable()} {MakeWhereClause(whereClause)}");

        //    return Select<object>(selectQuery);
        //}

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
        }

        public List<Dictionary<string, object>> GetFeaturesWhereIntersects(string wktGeometryFilter, bool returnGeometryAsWktForm = false)
        {
            return SelectFeatures(MakeSelectCommandWithWkt(wktGeometryFilter, false), returnGeometryAsWktForm);
        }

        #region Get Geometries

        public override List<Geometry<Point>> GetGeometries()
        {
            return GetGeometries(string.Empty);
        }

        //3857: web mercator; 102100: web mercator
        public override List<Geometry<Point>> GetGeometries(BoundingBox boundingBox)
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
        public override List<Geometry<Point>> GetGeometries(string whereClause)
        {
            //return SelectGeometries(FormattableString.Invariant($"SELECT {_spatialColumnName} FROM {GetTable()} {MakeWhereClause(whereClause)}"));
            return SelectGeometries(MakeSelectCommand(whereClause, true));
        }

        public override List<Geometry<Point>> GetGeometries(Geometry<Point> geometry)
        {
            if (geometry == null)
            {
                return GetGeometries();
            }

            return GetGeometriesWhereIntersects(geometry.AsWkt());
        }

        protected List<Geometry<Point>> SelectGeometries(string selectQuery, string connectionString = null)
        {
            if (connectionString == null)
            {
                connectionString = _connectionString;
            }

            SqlConnection connection = new SqlConnection(connectionString);

            connection.Open();

            var command = new SqlCommand(selectQuery, connection);

            List<Geometry<Point>> geometries = new List<Geometry<Point>>();

            using (var reader = command.ExecuteReader())
            {
                if (!reader.HasRows)
                {
                    return new List<Geometry<Point>>();
                }

                while (reader.Read())
                {
                    //approach 1
                    //geometries.Add(SqlGeometry.STGeomFromWKB(new System.Data.SqlTypes.SqlBytes((byte[])reader[0]), srid).MakeValid()); //4100-4200 ms
                    //approach 2
                    //geometries.Add(SqlGeometry.Deserialize(reader.GetSqlBytes(0))); //3220 ms

                    //approach 3 

                    //geometries.Add(reader[0] as SqlGeometry);//2565 ms

                    geometries.Add((reader[0] as SqlGeometry).AsGeometry());//2565 ms

                }
            }

            connection.Close();

            return geometries;
        }


        public List<Geometry<Point>> GetGeometriesWhereIntersects(string wktGeometryFilter)
        {
            return SelectGeometries(MakeSelectCommandWithWkt(wktGeometryFilter, true));
        }

        public List<Geometry<Point>> GetGeometriesWhereIntersects(byte[] wkbGeometryFilter)
        {
            if (wkbGeometryFilter == null)
            {
                return GetGeometries();
            }

            return SelectGeometries(MakeSelectCommandWithWkb(wkbGeometryFilter, true));
        }

        #endregion


        #region Get Geometry Label Pair

        public override List<NamedGeometry<Point>> GetGeometryLabelPairs(string whereClause)
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

            var result = new List<NamedGeometry<Point>>();

            using (SqlDataReader reader = command.ExecuteReader())
            {

                if (!reader.HasRows)
                {
                    return new List<NamedGeometry<Point>>();
                }

                if (string.IsNullOrWhiteSpace(_labelColumnName))
                {
                    while (reader.Read())
                    {
                        result.Add(new NamedGeometry<Point>(((SqlGeometry)reader[0]).AsGeometry(), string.Empty));
                    }
                }
                else
                {
                    while (reader.Read())
                    {
                        result.Add(new NamedGeometry<Point>(((SqlGeometry)reader[0]).AsGeometry(), reader[1]?.ToString()));
                    }
                }
            }

            connection.Close();

            return result;
        }

        public override List<NamedGeometry<Point>> GetGeometryLabelPairsForDisplay(BoundingBox boundingBox)
        {
            //List<string> attributes = GetAttributes(_labelColumnName).Select(i => i.ToString()).ToList();

            //Geometry<Point> boundary = boundingBox.ToSqlGeometry();

            //return GetGeometries().Zip(attributes, (a, b) => new NamedSqlGeometry(a, b)).Where(i => i.Geometry.STIntersects(boundary).Value).ToList();

            var whereClause = GetWhereClause(_spatialColumnName, boundingBox, GetSrid());

            return GetGeometryLabelPairs(whereClause);
        }

        public override List<NamedGeometry<Point>> GetGeometryLabelPairs()
        {
            throw new NotImplementedException();
        }

        public override List<NamedGeometry<Point>> GetGeometryLabelPairs(Geometry<Point> geometry)
        {
            throw new NotImplementedException();
        }

        protected List<NamedGeometry<Point>> SelectGeometryLabelPairs(string selectQuery, string connectionString = null)
        {
            if (connectionString == null)
            {
                connectionString = _connectionString;
            }

            SqlConnection connection = new SqlConnection(connectionString);

            connection.Open();

            var command = new SqlCommand(selectQuery, connection);

            List<NamedGeometry<Point>> geometries = new List<NamedGeometry<Point>>();

            using (var reader = command.ExecuteReader())
            {
                if (!reader.HasRows)
                {
                    return new List<NamedGeometry<Point>>();
                }

                while (reader.Read())
                {
                    //approach 1
                    //geometries.Add(SqlGeometry.STGeomFromWKB(new System.Data.SqlTypes.SqlBytes((byte[])reader[0]), srid).MakeValid()); //4100-4200 ms
                    //approach 2
                    //geometries.Add(SqlGeometry.Deserialize(reader.GetSqlBytes(0))); //3220 ms

                    //approach 3 
                    geometries.Add(new NamedGeometry<Point>(((SqlGeometry)reader[0]).AsGeometry(), string.Empty));//2565 ms

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

        public override DataTable GetEntireFeatures(Geometry<Point> geometry)
        {
            throw new NotImplementedException();
        }

        public override DataTable GetEntireFeatures(BoundingBox geometry)
        {
            throw new NotImplementedException();
        }

        #endregion


        #region Get FeatureSet

        public FeatureSet QueryFeatures()
        {
            return QueryFeatures(MakeSelectCommand(null, false));
        }

        public FeatureSet QueryFeaturesWhereIntersects(string wktGeometryFilter)
        {
            //return QueryFeatures(GetCommandString(wktGeometryFilter, false));
            return QueryFeatures(MakeSelectCommandWithWkt(wktGeometryFilter, false));
        }

        private FeatureSet QueryFeatures(string selectQuery)
        {
            SqlConnection connection = new SqlConnection(_connectionString);

            FeatureSet result = new FeatureSet(this.GetSrid()) { Fields = new List<Field>(), Features = new List<Feature<Point>>() };

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

                    var feature = new Feature<Point>();

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
                                feature.TheGeometry = ((SqlGeometry)reader[i]).AsGeometry();
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

        public FeatureSet QueryFeaturesWhereIntersects(BoundingBox boundingBox)
        {
            var whereClause = GetWhereClause(_spatialColumnName, boundingBox, GetSrid());

            return QueryFeatures(MakeSelectCommand(whereClause, false));
        }

        #endregion

        public override List<Feature<Point>> GetFeatures(Geometry<Point> geometry)
        {
            //var selectQuery = GetCommandString(geometry?.AsWkt(), false);
            var selectQuery = MakeSelectCommandWithWkb(geometry?.AsWkb(), false);

            var featureSet = QueryFeatures(selectQuery);

            return featureSet?.Features;
        }

        public override void Add(IGeometryAware<Point> newFeature)
        {
            this.AddAction?.Invoke(newFeature);
        }

        public void Remove(int featureId)
        {
            this.RemoveAction?.Invoke(featureId);
        }

        public override void Update(IGeometryAware<Point> newFeature)
        {
            this.UpdateAction?.Invoke(newFeature);
        }

        public override void UpdateFeature(IGeometryAware<Point> feature)
        {
            throw new NotImplementedException();
        }

        public override void Remove(IGeometryAware<Point> feature)
        {
            Remove(feature.Id);
        }

        public override void SaveChanges()
        {
            throw new NotImplementedException();
        }

        public override FeatureSet GetSqlFeatures()
        {
            return QueryFeatures();
        }

        public override FeatureSet GetSqlFeatures(Geometry<Point> geometry)
        {
            var boundingBox = geometry.GetBoundingBox();

            var featureSet = QueryFeaturesWhereIntersects(boundingBox);
            //.Features
            //.Where(s => s.TheSqlGeometry?.STIntersects(geometry).IsTrue == true)
            //.ToList();

            featureSet.Features = featureSet.Features.Where(s => s.TheGeometry?.Intersects(geometry) == true).ToList();

            return featureSet;
        }
    }
}