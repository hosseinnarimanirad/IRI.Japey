using System;
using IRI.Ket.ShapefileFormat;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using IRI.Ket.ShapefileFormat.EsriType;
using IRI.Ket.ShapefileFormat.Prj;
using IRI.Ket.Common.Helpers;

namespace IRI.Test.MainTestProject
{
    [TestClass]
    public class ShapefileFormatTest
    {
        [TestMethod]
        public void TestD900ToWebMercatorProjection()
        {
            var d900Prj = IRI.Ket.ShapefileFormat.Prj.EsriPrjFile.Parse(
                            IRI.Ket.Common.Helpers.ResourceHelper.ReadAllText(Assembly.GetExecutingAssembly(), "IRI.Test.MainTestProject.Assets.PrjSamples.d900.prj"))
                            .AsMapProjection();

            var webMercator = IRI.Ket.ShapefileFormat.Prj.EsriPrjFile.Parse(
                            IRI.Ket.Common.Helpers.ResourceHelper.ReadAllText(Assembly.GetExecutingAssembly(), "IRI.Test.MainTestProject.Assets.PrjSamples.WGS 1984 Web Mercator (auxiliary sphere).prj"))
                            .AsMapProjection();

            var sourceShapes = Shapefile.ReadShapes($"Assets\\ShapefileSamples\\sourceD900.shp").ToList();

            var targetShapes = Shapefile.ReadShapes($"Assets\\ShapefileSamples\\targetWebMercator.shp").ToList();

            var projected = Shapefile.Project(sourceShapes, d900Prj, webMercator);

            for (int i = 0; i < targetShapes.Count - 1; i++)
            {
                Assert.AreEqual(((EsriPoint)targetShapes[i]).X, ((EsriPoint)projected[i]).X, 1E-4);
                Assert.AreEqual(((EsriPoint)targetShapes[i]).Y, ((EsriPoint)projected[i]).Y, 1E-4);
            }
        }

        [TestMethod]
        public void TestPrjFile()
        {
            var prj1 = EsriPrjFile.Parse(ResourceHelper.ReadAllText(Assembly.GetExecutingAssembly(), "IRI.Test.MainTestProject.Assets.PrjSamples.Clarke 1880 (RGS).prj"));
            var prj2 = EsriPrjFile.Parse(ResourceHelper.ReadAllText(Assembly.GetExecutingAssembly(), "IRI.Test.MainTestProject.Assets.PrjSamples.Cylindrical Equal Area (world).prj"));
            var prj3 = EsriPrjFile.Parse(ResourceHelper.ReadAllText(Assembly.GetExecutingAssembly(), "IRI.Test.MainTestProject.Assets.PrjSamples.d900.prj"));
            var prj4 = EsriPrjFile.Parse(ResourceHelper.ReadAllText(Assembly.GetExecutingAssembly(), "IRI.Test.MainTestProject.Assets.PrjSamples.lccnioc.prj"));
            var prj5 = EsriPrjFile.Parse(ResourceHelper.ReadAllText(Assembly.GetExecutingAssembly(), "IRI.Test.MainTestProject.Assets.PrjSamples.Mercator (sphere).prj"));
            var prj6 = EsriPrjFile.Parse(ResourceHelper.ReadAllText(Assembly.GetExecutingAssembly(), "IRI.Test.MainTestProject.Assets.PrjSamples.Mercator (world).prj"));
            var prj7 = EsriPrjFile.Parse(ResourceHelper.ReadAllText(Assembly.GetExecutingAssembly(), "IRI.Test.MainTestProject.Assets.PrjSamples.WGS 1984 UTM Zone 39N.prj"));
            var prj8 = EsriPrjFile.Parse(ResourceHelper.ReadAllText(Assembly.GetExecutingAssembly(), "IRI.Test.MainTestProject.Assets.PrjSamples.WGS 1984 Web Mercator (auxiliary sphere).prj"));
            var prj9 = EsriPrjFile.Parse(ResourceHelper.ReadAllText(Assembly.GetExecutingAssembly(), "IRI.Test.MainTestProject.Assets.PrjSamples.WGS 1984 World Mercator.prj"));
            var prj10 = EsriPrjFile.Parse(ResourceHelper.ReadAllText(Assembly.GetExecutingAssembly(), "IRI.Test.MainTestProject.Assets.PrjSamples.WGS 1984.prj"));
            var prj11 = EsriPrjFile.Parse(ResourceHelper.ReadAllText(Assembly.GetExecutingAssembly(), "IRI.Test.MainTestProject.Assets.PrjSamples.World_Mercator.prj"));

            Assert.AreEqual(4012, prj1.Srid);
            Assert.AreEqual(32639, prj7.Srid);
            Assert.AreEqual(3857, prj8.Srid);
            Assert.AreEqual(3395, prj9.Srid);
            Assert.AreEqual(4326, prj10.Srid);
             
        }

    }
}
