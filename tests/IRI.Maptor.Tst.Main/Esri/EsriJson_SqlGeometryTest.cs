using IRI.Maptor.Extensions;
using IRI.Maptor.Sta.Spatial.Primitives.Esri;
using static IRI.Maptor.Tst.NetFrameworkTest.Assets.EsriJsonSamples;

namespace IRI.Maptor.Tst.Esri;


public class EsriJson_SqlGeometryTest
{
    public EsriJson_SqlGeometryTest()
    {
        //SqlServerTypes.Utilities.LoadNativeAssembliesv14();
    }


    [Fact]
    public void TestEsriJson_SqlGeometryConversion()
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

            IRI.Maptor.Ket.SqlServerSpatialExtension.Helpers.SqlSpatialHelper.Parse(point2D.AsWkt()).AsEsriJsonGeometry();
            IRI.Maptor.Ket.SqlServerSpatialExtension.Helpers.SqlSpatialHelper.Parse(point3D.AsWkt()).AsEsriJsonGeometry();
            IRI.Maptor.Ket.SqlServerSpatialExtension.Helpers.SqlSpatialHelper.Parse(pointNull.AsWkt()).AsEsriJsonGeometry();
            IRI.Maptor.Ket.SqlServerSpatialExtension.Helpers.SqlSpatialHelper.Parse(pointNaN.AsWkt()).AsEsriJsonGeometry();

            IRI.Maptor.Ket.SqlServerSpatialExtension.Helpers.SqlSpatialHelper.Parse(multipoint2D.AsWkt()).AsEsriJsonGeometry();
            IRI.Maptor.Ket.SqlServerSpatialExtension.Helpers.SqlSpatialHelper.Parse(multipoint3D.AsWkt()).AsEsriJsonGeometry();
            IRI.Maptor.Ket.SqlServerSpatialExtension.Helpers.SqlSpatialHelper.Parse(multipointEmpty.AsWkt()).AsEsriJsonGeometry();

            IRI.Maptor.Ket.SqlServerSpatialExtension.Helpers.SqlSpatialHelper.Parse(polyline2D.AsWkt()).AsEsriJsonGeometry();
            IRI.Maptor.Ket.SqlServerSpatialExtension.Helpers.SqlSpatialHelper.Parse(polyline3D.AsWkt()).AsEsriJsonGeometry();
            IRI.Maptor.Ket.SqlServerSpatialExtension.Helpers.SqlSpatialHelper.Parse(polylineEmpty.AsWkt()).AsEsriJsonGeometry();

            IRI.Maptor.Ket.SqlServerSpatialExtension.Helpers.SqlSpatialHelper.Parse(polygon2D.AsWkt()).AsEsriJsonGeometry();
            IRI.Maptor.Ket.SqlServerSpatialExtension.Helpers.SqlSpatialHelper.Parse(polygon3D.AsWkt()).AsEsriJsonGeometry();
            IRI.Maptor.Ket.SqlServerSpatialExtension.Helpers.SqlSpatialHelper.Parse(polygonEmpty.AsWkt()).AsEsriJsonGeometry();
        }
        catch (Exception ex)
        {
            throw new NotImplementedException();
            
        }
    }

}
