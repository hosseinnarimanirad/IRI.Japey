using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IRI.Ket.ShapefileFormat;
using IRI.Ket.ShapefileFormat.EsriType;
using Microsoft.SqlServer.Types;
using System.Data.SqlTypes;


namespace IRI.Test.ShapefileFormatTest
{
    [TestClass]
    public class ConstructorTest
    {

        EsriPoint[][] points;

        [TestInitialize]
        public void Initialize()
        {
            this.points = new EsriPoint[3][];

            points[0] = new EsriPoint[] { new EsriPoint(0.65, 0), new EsriPoint(5, 5), new EsriPoint(3.04, 6.4) };

            points[1] = new EsriPoint[] { new EsriPoint(10, 0), new EsriPoint(5.43, 15.65), new EsriPoint(3.04, 6.4), new EsriPoint(10, 90.34), new EsriPoint(25.43, 15.65), new EsriPoint(73.04, 61.44) };

            points[2] = new EsriPoint[] { new EsriPoint(20, 10), new EsriPoint(15, 5), new EsriPoint(33.04, 64.4) };
        }

        [TestMethod]
        public void TestPolylineConstructor()
        {
            EsriPolyline polyLine = new EsriPolyline(this.points);

            Assert.AreEqual(polyLine.Parts[0], 0);

            Assert.AreEqual(polyLine.Parts[1], this.points[0].Length);

            Assert.AreEqual(polyLine.Parts[2], this.points[0].Length + this.points[1].Length);
        }

    }
}
