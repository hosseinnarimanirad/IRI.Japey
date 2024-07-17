//using System;
//using System.Collections.Generic;
//using System.Data.OleDb;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace IRI.Ket.PersonalGdbPersistence;
//public static class FileName
//{
//    public static void test()
//    {
//        //"C:\Users\Hossein\Desktop\ab_proj\ab_proj\dbf971030\dbfs\ab_zirp.dbf"
//        string connectionString = "Provider=VFPOLEDB;Data Source=C:\\Users\\Hossein\\Desktop\\ab_proj\\ab_proj\\dbf971030\\dbfs\\;Collating Sequence=MACHINE;";
//        string query = "SELECT * FROM ab_zirp";

//        using (OleDbConnection connection = new OleDbConnection(connectionString))
//        {
//            using (OleDbCommand command = new OleDbCommand(query, connection))
//            {
//                connection.Open();

//                using (OleDbDataReader reader = command.ExecuteReader())
//                {
//                    while (reader.Read())
//                    {
//                        // Access columns using reader["ColumnName"]
//                        string column1Value = reader["Column1"].ToString();
//                        int column2Value = Convert.ToInt32(reader["Column2"]);

//                        Console.WriteLine($"Column1: {column1Value}, Column2: {column2Value}");
//                    }
//                }
//            }
//        }
//    }

//}
