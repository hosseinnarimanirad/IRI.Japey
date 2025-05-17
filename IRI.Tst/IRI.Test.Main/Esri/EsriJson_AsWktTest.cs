using IRI.Sta.Spatial.Primitives.Esri;
using static IRI.Test.NetFrameworkTest.Assets.EsriJsonSamples;

namespace IRI.Test.Esri;


public class EsriJson_AsWktTest
{
    public EsriJson_AsWktTest()
    {
        SqlServerTypes.Utilities.LoadNativeAssembliesv14();
    }

    [Fact]
    public void TestEsriJson_Wkt()
    {
        try
        {
            var point2D = Newtonsoft.Json.JsonConvert.DeserializeObject<EsriJsonGeometry>(point2DJson);
            var point3D = Newtonsoft.Json.JsonConvert.DeserializeObject<EsriJsonGeometry>(point3DJson);
            var pointNull = Newtonsoft.Json.JsonConvert.DeserializeObject<EsriJsonGeometry>(pointNullJson);
            var pointNaN = Newtonsoft.Json.JsonConvert.DeserializeObject<EsriJsonGeometry>(pointNaNJson);

            var multipoint2D = Newtonsoft.Json.JsonConvert.DeserializeObject<EsriJsonGeometry>(multipoint2DJson);
            var multipoint3D = Newtonsoft.Json.JsonConvert.DeserializeObject<EsriJsonGeometry>(multipoint3DJson);
            var multipointEmpty = Newtonsoft.Json.JsonConvert.DeserializeObject<EsriJsonGeometry>(multipointEmptyJson);

            var polyline2D = Newtonsoft.Json.JsonConvert.DeserializeObject<EsriJsonGeometry>(polyline2DJson);
            var polyline3D = Newtonsoft.Json.JsonConvert.DeserializeObject<EsriJsonGeometry>(polylineMJson);
            var polylineEmpty = Newtonsoft.Json.JsonConvert.DeserializeObject<EsriJsonGeometry>(polylineEmptyJson);

            var polygon2D = Newtonsoft.Json.JsonConvert.DeserializeObject<EsriJsonGeometry>(polygon2DJson);
            var polygon3D = Newtonsoft.Json.JsonConvert.DeserializeObject<EsriJsonGeometry>(polygon3DJson);
            var polygonEmpty = Newtonsoft.Json.JsonConvert.DeserializeObject<EsriJsonGeometry>(polygonEmptyJson);


            Assert.Equal("POINT(-118.15 33.8)", point2D.AsWkt());
            Assert.Equal("POINT EMPTY", pointNull.AsWkt());
            Assert.Equal("POINT EMPTY", pointNaN.AsWkt());

            Assert.Equal("MULTIPOINT(-97.06138 32.837,-97.06133 32.836,-97.06124 32.834,-97.06127 32.832)", multipoint2D.AsWkt());

            IRI.Ket.SqlServerSpatialExtension.Helpers.SqlSpatialHelper.Parse(point2D.AsWkt());
            IRI.Ket.SqlServerSpatialExtension.Helpers.SqlSpatialHelper.Parse(point3D.AsWkt());
            IRI.Ket.SqlServerSpatialExtension.Helpers.SqlSpatialHelper.Parse(pointNull.AsWkt());
            IRI.Ket.SqlServerSpatialExtension.Helpers.SqlSpatialHelper.Parse(pointNaN.AsWkt());

            IRI.Ket.SqlServerSpatialExtension.Helpers.SqlSpatialHelper.Parse(multipoint2D.AsWkt());
            IRI.Ket.SqlServerSpatialExtension.Helpers.SqlSpatialHelper.Parse(multipoint3D.AsWkt());
            IRI.Ket.SqlServerSpatialExtension.Helpers.SqlSpatialHelper.Parse(multipointEmpty.AsWkt());

            IRI.Ket.SqlServerSpatialExtension.Helpers.SqlSpatialHelper.Parse(polyline2D.AsWkt());
            IRI.Ket.SqlServerSpatialExtension.Helpers.SqlSpatialHelper.Parse(polyline3D.AsWkt());
            IRI.Ket.SqlServerSpatialExtension.Helpers.SqlSpatialHelper.Parse(polylineEmpty.AsWkt());

            IRI.Ket.SqlServerSpatialExtension.Helpers.SqlSpatialHelper.Parse(polygon2D.AsWkt());
            IRI.Ket.SqlServerSpatialExtension.Helpers.SqlSpatialHelper.Parse(polygon3D.AsWkt());
            IRI.Ket.SqlServerSpatialExtension.Helpers.SqlSpatialHelper.Parse(polygonEmpty.AsWkt());

        }
        catch (Exception ex)
        {
            throw new NotImplementedException();
        }
    }


}
