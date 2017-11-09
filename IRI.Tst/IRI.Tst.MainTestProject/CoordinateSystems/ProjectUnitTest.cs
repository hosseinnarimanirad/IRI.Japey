using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;

namespace IRI.Test.CoordinateSystem
{
    [TestClass]
    public class ProjectUnitTest
    {
        [TestMethod]
        public void TestD900ToWebMercator()
        {
            var d900Prj = IRI.Ket.Common.Helpers.ResourceHelper.ReadAllText(Assembly.GetExecutingAssembly(), "IRI.Test.CoordinateSystem.Data.d900.txt");

            var webMercator = IRI.Ket.Common.Helpers.ResourceHelper.ReadAllText(Assembly.GetExecutingAssembly(), "IRI.Test.CoordinateSystem.Data.WGS 1984 Web Mercator (auxiliary sphere).txt");
             

        }
    }
}
