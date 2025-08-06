using System;

using System.Reflection;

namespace IRI.Maptor.Tst.CoordinateSystems;


public class ProjectTest
{
    [Fact]
    public void TestD900ToWebMercator()
    {
        var d900Prj = IRI.Maptor.Sta.Common.Helpers.ResourceHelper.ReadAllText(Assembly.GetExecutingAssembly(), "IRI.Maptor.Tst.CoordinateSystem.Data.d900.txt");

        var webMercator = IRI.Maptor.Sta.Common.Helpers.ResourceHelper.ReadAllText(Assembly.GetExecutingAssembly(), "IRI.Maptor.Tst.CoordinateSystem.Data.WGS 1984 Web Mercator (auxiliary sphere).txt");
         

    }
}
