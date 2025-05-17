using System.Linq;
using System.Reflection;
using IRI.Sta.Common.Helpers;
using IRI.Sta.ShapefileFormat;
using IRI.Sta.ShapefileFormat.EsriType;
using IRI.Sta.ShapefileFormat.Prj;


namespace IRI.Test.Esri;


public class PrjFileTest
{
    [Fact]
    public void TestPrjFile()
    {
        var prj1 = EsriPrjFile.Parse(ResourceHelper.ReadAllText(Assembly.GetExecutingAssembly(), "IRI.Test.Main.Assets.PrjSamples.Clarke 1880 (RGS).prj"));
        var prj2 = EsriPrjFile.Parse(ResourceHelper.ReadAllText(Assembly.GetExecutingAssembly(), "IRI.Test.Main.Assets.PrjSamples.Cylindrical Equal Area (world).prj"));
        var prj3 = EsriPrjFile.Parse(ResourceHelper.ReadAllText(Assembly.GetExecutingAssembly(), "IRI.Test.Main.Assets.PrjSamples.d900.prj"));
        var prj4 = EsriPrjFile.Parse(ResourceHelper.ReadAllText(Assembly.GetExecutingAssembly(), "IRI.Test.Main.Assets.PrjSamples.lccnioc.prj"));
        var prj5 = EsriPrjFile.Parse(ResourceHelper.ReadAllText(Assembly.GetExecutingAssembly(), "IRI.Test.Main.Assets.PrjSamples.Mercator (sphere).prj"));
        var prj6 = EsriPrjFile.Parse(ResourceHelper.ReadAllText(Assembly.GetExecutingAssembly(), "IRI.Test.Main.Assets.PrjSamples.Mercator (world).prj"));
        var prj7 = EsriPrjFile.Parse(ResourceHelper.ReadAllText(Assembly.GetExecutingAssembly(), "IRI.Test.Main.Assets.PrjSamples.WGS 1984 UTM Zone 39N.prj"));
        var prj8 = EsriPrjFile.Parse(ResourceHelper.ReadAllText(Assembly.GetExecutingAssembly(), "IRI.Test.Main.Assets.PrjSamples.WGS 1984 Web Mercator (auxiliary sphere).prj"));
        var prj9 = EsriPrjFile.Parse(ResourceHelper.ReadAllText(Assembly.GetExecutingAssembly(), "IRI.Test.Main.Assets.PrjSamples.WGS 1984 World Mercator.prj"));
        var prj10 = EsriPrjFile.Parse(ResourceHelper.ReadAllText(Assembly.GetExecutingAssembly(), "IRI.Test.Main.Assets.PrjSamples.WGS 1984.prj"));
        var prj11 = EsriPrjFile.Parse(ResourceHelper.ReadAllText(Assembly.GetExecutingAssembly(), "IRI.Test.Main.Assets.PrjSamples.World_Mercator.prj"));

        Assert.Equal(4012, prj1.Srid);
        Assert.Equal(32639, prj7.Srid);
        Assert.Equal(3857, prj8.Srid);
        Assert.Equal(3395, prj9.Srid);
        Assert.Equal(4326, prj10.Srid);
    }


    [Fact]
    public void TestD900ToWebMercatorProjection()
    {
        var d900Prj = EsriPrjFile.Parse(ResourceHelper.ReadAllText(Assembly.GetExecutingAssembly(), "IRI.Test.Main.Assets.PrjSamples.d900.prj"))
                        .AsMapProjection();

        var webMercator = EsriPrjFile.Parse(ResourceHelper.ReadAllText(Assembly.GetExecutingAssembly(), "IRI.Test.Main.Assets.PrjSamples.WGS 1984 Web Mercator (auxiliary sphere).prj"))
                        .AsMapProjection();

        var sourceShapes = Shapefile.ReadShapes($"Assets\\ShapefileSamples\\sourceD900.shp").ToList();

        var targetShapes = Shapefile.ReadShapes($"Assets\\ShapefileSamples\\targetWebMercator.shp").ToList();

        var projected = Shapefile.Project(sourceShapes, d900Prj, webMercator);

        for (int i = 0; i < targetShapes.Count - 1; i++)
        {
            Assert.Equal(((EsriPoint)targetShapes[i]).X, ((EsriPoint)projected[i]).X,4 /*1E-4*/);
            Assert.Equal(((EsriPoint)targetShapes[i]).Y, ((EsriPoint)projected[i]).Y,4 /*1E-4*/);
        }
    }
}
