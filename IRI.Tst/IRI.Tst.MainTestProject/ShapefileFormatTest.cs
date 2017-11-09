using System;
using IRI.Ket.ShapefileFormat;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using IRI.Ket.ShapefileFormat.EsriType;

namespace IRI.Test.MainTestProject
{
    [TestClass]
    public class ShapefileFormatTest
    {
        [TestMethod]
        public void TestD900ToWebMercatorProjection()
        {
            var d900Prj = IRI.Ket.ShapefileFormat.Prj.PrjFile.Parse(
                            IRI.Ket.Common.Helpers.ResourceHelper.ReadAllText(Assembly.GetExecutingAssembly(), "IRI.Test.MainTestProject.Assets.PrjSamples.d900.prj"))
                            .AsMapProjection();

            var webMercator = IRI.Ket.ShapefileFormat.Prj.PrjFile.Parse(
                            IRI.Ket.Common.Helpers.ResourceHelper.ReadAllText(Assembly.GetExecutingAssembly(), "IRI.Test.MainTestProject.Assets.PrjSamples.WGS 1984 Web Mercator (auxiliary sphere).prj"))
                            .AsMapProjection();
             
            var sourceShapes = Shapefile.Read($"Assets\\ShapefileSamples\\sourceD900.shp").ToList();  

            var targetShapes = Shapefile.Read($"Assets\\ShapefileSamples\\targetWebMercator.shp").ToList(); 

            var projected = Shapefile.Project(sourceShapes, d900Prj, webMercator); 

            for (int i = 0; i < targetShapes.Count - 1; i++)
            {
                Assert.AreEqual(((EsriPoint)targetShapes[i]).X, ((EsriPoint)projected[i]).X, 1E-4);
                Assert.AreEqual(((EsriPoint)targetShapes[i]).Y, ((EsriPoint)projected[i]).Y, 1E-4);
            }
        }
 
    }
}
