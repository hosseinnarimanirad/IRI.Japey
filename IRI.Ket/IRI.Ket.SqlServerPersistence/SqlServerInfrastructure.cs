using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Types;
using IRI.Extensions;

namespace IRI.Ket.DataManagement.Infrastructure
{
    //Remember; 
    //What about e.g. decimal(5,2)
    //What about geometies and geographies
    public class SqlServerInfrastructure : DataSourceInfrastructure
    {
        public SqlServerInfrastructure()
        {
            InitializeTypeMappingSimple();

            InitializeTypeMappingForSqlTypes();
        }

        private void InitializeTypeMappingForSqlTypes()
        {
            _typeMapping.Add(typeof(SqlInt64), "bigint");
            ////_typeMapping.Add(typeof(SqlBytes), "binary");
            _typeMapping.Add(typeof(SqlBoolean), "bit");
            ////_typeMapping.Add(typeof(SqlChars), "char"); ;
            ////_typeMapping.Add(typeof(SqlDateTime), "date");
            _typeMapping.Add(typeof(SqlDateTime), "datetime"); ;
            //_typeMapping.Add(null, "datetime2");
            //_typeMapping.Add(null, "datetimeoffset");
            ////_typeMapping.Add(typeof(SqlDecimal), "decimal");
            _typeMapping.Add(typeof(SqlDouble), "float");
            _typeMapping.Add(typeof(Microsoft.SqlServer.Types.SqlGeography), "geography");
            _typeMapping.Add(typeof(Microsoft.SqlServer.Types.SqlGeometry), "geometry");
            _typeMapping.Add(typeof(Microsoft.SqlServer.Types.SqlHierarchyId), "hierarchyid");
            //_typeMapping.Add(null, "image");
            _typeMapping.Add(typeof(SqlInt32), "int");
            _typeMapping.Add(typeof(SqlMoney), "money");
            ////_typeMapping.Add(typeof(SqlChars), "nchar");
            //_typeMapping.Add(null, "ntext");
            _typeMapping.Add(typeof(SqlDecimal), "float");//------------------------------bookmark
            _typeMapping.Add(typeof(SqlChars), "nvarchar");
            _typeMapping.Add(typeof(SqlSingle), "real");
            //_typeMapping.Add(null, "rowversion");
            _typeMapping.Add(typeof(SqlInt16), "smallint");
            ////_typeMapping.Add(typeof(SqlMoney), "smallmoney");
            //_typeMapping.Add(null, "sql_variant");
            _typeMapping.Add(typeof(SqlString), "text");
            //_typeMapping.Add(null, "time");
            //_typeMapping.Add(null, "timestamp");
            _typeMapping.Add(typeof(SqlByte), "tinyint");
            _typeMapping.Add(typeof(SqlGuid), "uniqueidentifier");
            _typeMapping.Add(typeof(SqlBytes), "varbinary");
            ////_typeMapping.Add(typeof(SqlString), "varchar");
            _typeMapping.Add(typeof(SqlXml), "xml");
        }

        private void InitializeTypeMappingSimple()
        {
            _typeMapping.Add(typeof(string), "nvarchar");
            _typeMapping.Add(typeof(int), "int");
            _typeMapping.Add(typeof(double), "float");
            _typeMapping.Add(typeof(bool), "bit");

            //_typeMapping.Add(typeof(String), "nvarchar");
            //_typeMapping.Add(typeof(Int32), "int");
            //_typeMapping.Add(typeof(Double), "float");
            //_typeMapping.Add(typeof(Boolean), "bit");

        }

        private bool Exists(string tableName, SqlConnection connection)
        {
            string command = string.Format("SELECT COUNT(*) FROM sys.Tables WHERE name  = '{0}'", tableName);

            connection.Open();

            var result = ((int)new SqlCommand(command, connection).ExecuteScalar()) > 0;

            connection.Close();

            return result;
        }

        public static List<string> GetAllTypes()
        {
            return new List<string>()
            {
                "bigint",
                "binary",//(50)
                "bit",
                "char",//(10)
                "date",
                "datetime",
                "datetime2",//(7)
                "datetimeoffset",//(7)
                "decimal",//(18, 0)
                "float",
                "geography",
                "geometry",
                "hierarchyid",
                "image",
                "int",
                "money",
                "nchar",//(10)
                "ntext",
                "numeric",//(18, 0)
                "nvarchar",//(50)
                "real",
                "smalldatetime",
                "smallint",
                "smallmoney",
                "sql_variant",
                "text",
                "time",//(7)
                "timestamp",
                "tinyint",
                "uniqueidentifier",
                "varbinary",//(50)
                "varchar",//(50)
                "xml"
            };
        }

        public void InsertTable(string connectionString, System.Data.DataTable table, string tableName, bool createTable = false)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            connection.Open();

            if (createTable)
            {
                SqlCommand create = new SqlCommand(GetProperCreateCommand(table.Columns.OfType<DataColumn>(), tableName), connection);

                create.ExecuteNonQuery();
            }

            BulkInsertDataTable(connection, tableName, table);

            connection.Close();
        }

        public void Insert(string connectionString, string tableName, DataTable sourceTable, List<string> targetColumnNames, List<Func<DataRow, object>> mapFunctions)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            if (!Exists(tableName, connection))
            {
                throw new NotImplementedException();
            }

            connection.Open();

            string insertCommandString = string.Format("INSERT INTO {0}({1}) VALUES({2})", tableName,
               string.Join(", ", targetColumnNames),
               string.Join(", ", targetColumnNames.Select(i => "@" + i)));

            SqlCommand insertCommand = new SqlCommand(insertCommandString, connection);

            foreach (DataRow item in sourceTable.Rows)
            {
                insertCommand.Parameters.Clear();

                for (int i = 0; i < targetColumnNames.Count; i++)
                {
                    try
                    {
                        object value = mapFunctions[i](item);

                        SqlParameter parameter = new SqlParameter("@" + targetColumnNames[i], value);

                        if (value.GetType().FullName == typeof(Microsoft.SqlServer.Types.SqlGeometry).FullName)
                        {
                            parameter.UdtTypeName = "GEOMETRY";

                            parameter.SqlDbType = SqlDbType.Udt;

                            var geo = (Microsoft.SqlServer.Types.SqlGeometry)(value);

                            if (geo.STIsValid().IsFalse)
                            {

                            }
                        }

                        //insertCommand.Parameters.AddWithValue("@" + targetColumnNames[i], value);
                        insertCommand.Parameters.Add(parameter);
                    }
                    catch (Exception ex)
                    {

                    }
                }

                insertCommand.ExecuteNonQuery();
            }

            connection.Close();
        }

        private static void BulkInsertDataTable(SqlConnection connection, string tableName, System.Data.DataTable table)
        {
            //using (SqlConnection connection = new SqlConnection(connectionString))
            //{
            SqlBulkCopy bulkCopy =
                new SqlBulkCopy
                (
                connection,
                SqlBulkCopyOptions.TableLock |
                SqlBulkCopyOptions.FireTriggers |
                SqlBulkCopyOptions.UseInternalTransaction,
                null
                );

            bulkCopy.DestinationTableName = tableName;

            //connection.Open();

            bulkCopy.WriteToServer(table);

            //connection.Close();
            //}
        }

        public static void ExecuteNonQuery(string commandString, string connectionString)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            SqlCommand command = new SqlCommand(commandString, connection);

            connection.Open();

            command.ExecuteNonQuery();

            connection.Close();

        }

        public static bool Exists(string tableName, string connectionString)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            string command = string.Format("SELECT COUNT(*) FROM sys.Tables WHERE name  = '{0}'", tableName);

            connection.Open();

            var result = ((int)new SqlCommand(command, connection).ExecuteScalar()) > 0;

            connection.Close();

            return result;
        }

        public static void Delete(string tableName, string connectionString)
        {
            if (Exists(tableName, connectionString))
            {
                ExecuteNonQuery(string.Format("DROP TABLE {0}", tableName), connectionString);
            }
        }

        public static string TestConnection(string connectionString)
        {
            SqlConnection connection = new SqlConnection();

            try
            {
                connection = new SqlConnection(connectionString);

                connection.Open();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                connection.Close();
            }

            return "Connected Successfully";
        }

        public static List<Dictionary<string, object>> SelectFeatures(string connectionString, string selectQuery, bool returnWkt = false)
        {
            SqlConnection connection = new SqlConnection(connectionString);

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

        public static List<T> SelectFeatures<T>(string connectionString, string selectQuery, List<Action<T, object>> updateFieldFuncs, bool returnWkt = false) where T : new()
        {
            SqlConnection connection = new SqlConnection(connectionString);

            List<T> result = new List<T>();

            try
            {
                var command = new SqlCommand(selectQuery, connection);

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.FieldCount!=updateFieldFuncs.Count)
                {
                    throw new NotImplementedException("fieldCount must be equal to update functions");
                }

                if (!reader.HasRows)
                {
                    return new List<T>();
                }

                while (reader.Read())
                {
                    T newRecord = new T();

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        if (reader.IsDBNull(i))
                        {
                            updateFieldFuncs[i](newRecord, null);
                        }
                        else
                        {
                            updateFieldFuncs[i](newRecord, reader[i]);

                            //if (returnWkt && reader[i] is SqlGeometry)
                            //{
                            //    dict.Add(fieldName, ((SqlGeometry)reader[i]).AsWkt());
                            //}
                            //else
                            //{
                            //    dict.Add(fieldName, reader[i]);
                            //}
                        }
                    }

                    result.Add(newRecord);
                }

                connection.Close();
            }
            catch (Exception ex)
            {
                connection.Close();
            }

            return result;
        }

    }
}