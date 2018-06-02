using IRI.Ket.SpatialExtensions;
using IRI.Ket.SqlServerSpatialExtension.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Test.MainTestProject.SpatialConversions
{
    [TestClass]
    public class TestGeoJsonToSqlGeometry
    {
        [TestMethod]
        public void TestParseToString()
        { 
            var networkblocks = IRI.Msh.Common.Model.GeoJson.GeoJson.ParseToGeoJsonFeatures(ReadFile("networkblock.json"));

            var checkSqlGeometryAndGeometryConversions = networkblocks.Select(f => f.geometry.AsSqlGeometry().AsWkt() == f.geometry.Parse().AsSqlGeometry().AsWkt());

            if (checkSqlGeometryAndGeometryConversions.Any(r => r == false))
            {
                Assert.Fail();
            }

            var checkIShapeConversions = networkblocks.Select(f => f.geometry.AsToShapefileShape().AsSqlGeometry(0).AsWkt() == f.geometry.AsSqlGeometry().AsWkt());
 
            if (checkIShapeConversions.Any(r => r == false))
            {
                Assert.Fail();
            }



            var stations = IRI.Msh.Common.Model.GeoJson.GeoJson.ParseToGeoJsonFeatures(ReadFile("stations.json"));

            var checkSqlGeometryAndGeometryConversions2 = stations.Select(f => f.geometry.AsSqlGeometry().AsWkt() == f.geometry.Parse().AsSqlGeometry().AsWkt());

            if (checkSqlGeometryAndGeometryConversions2.Any(r => r == false))
            {
                Assert.Fail();
            }

            var checkIShapeConversions2 = stations.Select(f => f.geometry.AsToShapefileShape().AsSqlGeometry(0).AsWkt() == f.geometry.AsSqlGeometry().AsWkt());

            if (checkIShapeConversions2.Any(r => r == false))
            {
                Assert.Fail();
            }
        }

        private string ReadFile(string fileName)
        {
            var assembly = Assembly.GetExecutingAssembly();

            var resourceName = $"IRI.Test.MainTestProject.Assets.GeoJsonSamples.{fileName}";

            string result;

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    result = reader.ReadToEnd();
                }
            }

            return result;
        }
    }
}
