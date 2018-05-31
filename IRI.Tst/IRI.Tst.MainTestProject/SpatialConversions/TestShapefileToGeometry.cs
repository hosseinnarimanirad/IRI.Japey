using IRI.Ket.SpatialExtensions;
using IRI.Test.MainTestProject.Assets;
using Microsoft.SqlServer.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Test.MainTestProject.SpatialConversions
{
    [TestClass]
    public class TestShapefileToGeometry
    {
        [TestMethod]
        public void TestShpToGeometry()
        {
            Test(SqlGeometrySamples.Point);
            Test(SqlGeometrySamples.PointZ);
            Test(SqlGeometrySamples.PointZM);
            Test(SqlGeometrySamples.Multipoint);
            Test(SqlGeometrySamples.MultipointComplex);
            Test(SqlGeometrySamples.Linestring);
            Test(SqlGeometrySamples.LinestringMZ);
            Test(SqlGeometrySamples.MultiLineString);

            Test(SqlGeometrySamples.Polygon);
            Test(SqlGeometrySamples.PolygonWithHole);
            //Test(SqlGeometrySamples.MultiPolygon01);
            //Test(SqlGeometrySamples.MultiPolygon02);
        }

        private void Test(SqlGeometry sqlGeometry)
        {
            var esriShape = sqlGeometry.ParseToEsriShape();

            var geometry = esriShape.AsGeometry();

            var geometry2 = sqlGeometry.ExtractPoints();

            Assert.AreEqual(geometry2.AsSqlGeometry().AsWkt(), geometry.AsSqlGeometry().AsWkt());
        }

    }
}
