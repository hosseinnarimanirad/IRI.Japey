using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlServerCe;
using System.Data.SqlTypes;
using System.ComponentModel;

namespace IRI.Ket.DataManagement.Infrastructure
{
    //Remember; 
    //What about e.g. decimal(5,2)
    //What about geometies and geographies
    public class SqlServerCeInfrastructure : DataSourceInfrastructure
    {
        SqlCeConnection connection;

        public SqlServerCeInfrastructure(string sdfFilePath, string password = null)
            : this()
        {
            this.connection =
                new SqlCeConnection(
                    string.Format("Data Source = {0}; Max Database Size = 4091; Locale Identifier = 1256 {1}",
                        sdfFilePath,
                        string.IsNullOrEmpty(password) ? string.Empty : ";password=" + password));
        }

        public SqlServerCeInfrastructure()
        {
            InitializeTypeMappingSimple();

            InitializeTypeMappingForSqlCeTypes();
        }

        public string Connection
        {
            set { this.connection = new SqlCeConnection(value); }
        }

        private void InitializeTypeMappingForSqlCeTypes()
        {
            _typeMapping.Add(typeof(SqlInt64), "bigint");
            _typeMapping.Add(typeof(SqlBoolean), "bit");
            _typeMapping.Add(typeof(SqlDateTime), "datetime"); ;
            _typeMapping.Add(typeof(SqlDouble), "float");

            //note: varbinary has a 8000 length limit and does not support (MAX) in SQL Server CE, so I used image type insted
            _typeMapping.Add(typeof(Microsoft.SqlServer.Types.SqlGeography), "image");//----------bookmark

            //note: varbinary has a 8000 length limit and does not support (MAX) in SQL Server CE, so I used image type insted
            _typeMapping.Add(typeof(Microsoft.SqlServer.Types.SqlGeometry), "image");//-----------bookmark
            _typeMapping.Add(typeof(Microsoft.SqlServer.Types.SqlHierarchyId), null);//-----------Exception
            _typeMapping.Add(typeof(SqlInt32), "int");
            _typeMapping.Add(typeof(SqlMoney), "money");
            _typeMapping.Add(typeof(SqlDecimal), "float");
            _typeMapping.Add(typeof(SqlChars), "nvarchar");
            _typeMapping.Add(typeof(SqlSingle), "real");
            _typeMapping.Add(typeof(SqlInt16), "smallint");
            _typeMapping.Add(typeof(SqlString), "ntext");//---------------------------------------bookmark
            _typeMapping.Add(typeof(SqlByte), "tinyint");
            _typeMapping.Add(typeof(SqlGuid), "uniqueidentifier");

            //note: varbinary has a 8000 length limit and does not support (MAX) in SQL Server CE, so I used image type insted
            _typeMapping.Add(typeof(SqlBytes), "image");
            _typeMapping.Add(typeof(SqlXml), "ntext");//------------------------------------------bookmark
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

        public bool Exists(string tableName)
        {
            string command = string.Format("SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{0}'", tableName);

            connection.Open();

            var result = ((int)new SqlCeCommand(command, connection).ExecuteScalar()) > 0;

            connection.Close();

            return result;
        }

        public static List<string> GetAllTypes()
        {
            return new List<string>() 
            { 
                "bigint",
                "binary",
                "bit",
                "datetime",
                "float",
                "image",
                "int",
                "money",
                "nchar",
                "ntext",
                "numeric",
                "nvarchar",
                "real",
                "smallint",
                "tinyint",
                "uniqueidentifier",
                "varbinary",
                "rowversion"
            };
        }

        public void CreateTable(string createCommand)
        {
            ExecuteNonQuery(createCommand);
        }

        public void InsertDataTable(string tableName, System.Data.DataTable table)
        {
            if (Exists(tableName))
            {
                Delete(tableName);
            }

            SqlCeCommand command = new SqlCeCommand(GetProperCreateCommand(table.Columns.Cast<DataColumn>(), tableName), connection);

            connection.Open();

            command.ExecuteNonQuery();

            string insertCommandString = string.Format("INSERT INTO {0}({1}) VALUES({2})", tableName,
               string.Join(", ", table.Columns.Cast<DataColumn>().Select(i => i.ColumnName)),
               string.Join(", ", table.Columns.Cast<DataColumn>().Select(i => "@" + i.ColumnName)));

            foreach (DataRow item in table.Rows)
            {
                SqlCeCommand insertCommand = new SqlCeCommand(insertCommandString, connection);

                for (int i = 0; i < table.Columns.Count; i++)
                {
                    object value = item[i];

                    if (table.Columns[i].DataType == typeof(Microsoft.SqlServer.Types.SqlGeography))
                    {
                        value = ((Microsoft.SqlServer.Types.SqlGeography)value).STAsBinary().Buffer;
                    }
                    else if (table.Columns[i].DataType == typeof(Microsoft.SqlServer.Types.SqlGeometry))
                    {
                        value = ((Microsoft.SqlServer.Types.SqlGeometry)value).STAsBinary().Buffer;
                    }

                    insertCommand.Parameters.AddWithValue("@" + table.Columns[i].ColumnName, value);
                }

                insertCommand.ExecuteNonQuery();
            }

            connection.Close();
        }

        public void Insert(string tableName, System.Data.DataTable sourceTable, List<string> targetColumnNames, List<Func<DataRow, object>> mapFunctions)
        {
            if (!Exists(tableName))
            {
                throw new NotImplementedException();
            }

            connection.Open();

            string insertCommandString = string.Format("INSERT INTO {0}({1}) VALUES({2})", tableName,
               string.Join(", ", targetColumnNames),
               string.Join(", ", targetColumnNames.Select(i => "@" + i)));

            SqlCeCommand insertCommand = new SqlCeCommand(insertCommandString, connection);

            int count = sourceTable.Rows.Count;

            int numberOfColumns = targetColumnNames.Count;

            for (int n = 0; n < count; n++)
            {
                insertCommand.Parameters.Clear();

                for (int i = 0; i < numberOfColumns; i++)
                {
                    object value = mapFunctions[i](sourceTable.Rows[n]);

                    insertCommand.Parameters.AddWithValue("@" + targetColumnNames[i], value);
                }

                insertCommand.ExecuteNonQuery();
            }

            //foreach (DataRow item in sourceTable.Rows)
            //{
            //    insertCommand.Parameters.Clear();

            //    for (int i = 0; i < targetColumnNames.Count; i++)
            //    {
            //        object value = mapFunctions[i](item);

            //        insertCommand.Parameters.AddWithValue("@" + targetColumnNames[i], value);
            //    }

            //    insertCommand.ExecuteNonQuery();
            //}

            connection.Close();
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void ExecuteNonQuery(string commandString)
        {
            SqlCeCommand command = new SqlCeCommand(commandString, connection);

            connection.Open();

            command.ExecuteNonQuery();

            connection.Close();

        }

        public void Delete(string tableName)
        {
            ExecuteNonQuery("DROP TABLE " + tableName);
        }

        public static string TestConnection(string connectionString)
        {
            SqlCeConnection connection = new SqlCeConnection();

            try
            {
                connection = new SqlCeConnection(connectionString);

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

        public static void Delete(string tableName, string connectionString)
        {
            ExecuteNonQuery("DROP TABLE " + tableName, connectionString);
        }

        public static void ExecuteNonQuery(string commandString, string connectionString)
        {
            SqlCeConnection connection = new SqlCeConnection(connectionString);

            SqlCeCommand command = new SqlCeCommand(commandString, connection);

            connection.Open();

            command.ExecuteNonQuery();

            connection.Close();

        }


        public static bool Exists(string tableName, string connectionString)
        {
            var temp = new SqlServerCeInfrastructure(connectionString);

            return temp.Exists(tableName);
        }
    }
}
