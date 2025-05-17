using System;

using System.Reflection;

namespace IRI.Test.CoordinateSystems;


public class ProjectTest
{
    [Fact]
    public void TestD900ToWebMercator()
    {
        var d900Prj = IRI.Sta.Common.Helpers.ResourceHelper.ReadAllText(Assembly.GetExecutingAssembly(), "IRI.Test.CoordinateSystem.Data.d900.txt");

        var webMercator = IRI.Sta.Common.Helpers.ResourceHelper.ReadAllText(Assembly.GetExecutingAssembly(), "IRI.Test.CoordinateSystem.Data.WGS 1984 Web Mercator (auxiliary sphere).txt");
         

    }
}
