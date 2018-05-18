using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IRI.Test.SpatialDataManagement
{
    [TestClass]
    public class SpatialDataProviderTest
    {
        System.IO.DirectoryInfo currentDirectory = new System.IO.DirectoryInfo(System.Environment.CurrentDirectory);

        string ResourcesDirectory
        {
            get { return string.Join("\\", this.currentDirectory.Parent.Parent.FullName, "Resources"); }
        }

        string SampleShapefileArea
        {
            get { return string.Join("\\", this.ResourcesDirectory, "regions.shp"); }
        }

        string SampleShapefileLine
        {
            get { return string.Join("\\", this.ResourcesDirectory, "mjrRoad.shp"); }
        }


        [TestMethod]
        public void TestShapefileToSqlSpatial()
        {
            //string connectionString = @"server = YA-MORTAZA\MSSQLSERVER2012; integrated security = true;user id=sa; password=sa123456; database = TestSpatialDatabase";

            //IRI.Ket.SpatialDataManagement.SpatialSqlDataProvider p = new IRI.Ket.SpatialDataManagement.SpatialSqlDataProvider(connectionString);
            //p.ImportDataFromShapefile(SampleShapefileLine, "geoLine");
        }


        [TestMethod]
        public void TestSqlSpatial()
        {
            //string connectionString = @"server = YA-MORTAZA\MSSQLSERVER2012; integrated security = true;user id=sa; password=sa123456; database = TestSpatialDatabase";

            //IRI.Ket.SpatialDataManagement.SpatialSqlDataProvider p = new IRI.Ket.SpatialDataManagement.SpatialSqlDataProvider(connectionString);
            //p.ImportDataFromShapefile(SampleShapefileLine, "geoLine");
        }
    }
}
