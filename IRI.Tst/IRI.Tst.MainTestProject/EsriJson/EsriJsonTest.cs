using IRI.Ham.SpatialBase.Model.Esri;

using IRI.Ket.SpatialExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Test.MainTestProject.EsriJson
{
    [TestClass]
    public class EsriJsonTest
    {
        const string point2DJson = "{\"x\" : -118.15, \"y\" : 33.80, \"type\" : \"point\", \"spatialReference\" : {\"wkid\" : 4326}}";
        const string point3DJson = "{\"x\" : -118.15, \"y\" : 33.80, \"z\" : 10.0, \"type\" : \"point\", \"spatialReference\" : {\"wkid\" : 4326}}";
        const string pointNullJson = "{\"x\" : null, \"type\" : \"point\", \"spatialReference\" : {\"wkid\" : 4326}}";
        const string pointNaNJson = "{\"x\" : \"NaN\", \"y\" : 22.2, \"type\" : \"point\", \"spatialReference\" : {\"wkid\" : 4326}}";

        const string multipoint2DJson = "{\"points\" : [[-97.06138,32.837],[-97.06133,32.836],[-97.06124,32.834],[-97.06127,32.832]],\"type\" : \"multipoint \", \"spatialReference\" : {\"wkid\" : 4326}}";
        const string multipoint3DJson = "{\"hasZ\" : true,\"points\" : [[-97.06138,32.837,35.0], [-97.06133,32.836,35.1], [-97.06124,32.834,35.2]],\"type\" : \"multipoint \", \"spatialReference\" : {\"wkid\" : 4326}}";
        const string multipointEmptyJson = "{\"points\" : [  ], \"type\" : \"multipoint \", \"spatialReference\" : {\"wkid\" : 4326}}";

        const string polyline2DJson = "{\"paths\" : [[[-97.06138,32.837],[-97.06133,32.836],[-97.06124,32.834],[-97.06127,32.832]], [[-97.06326,32.759],[-97.06298,32.755]]],\"type\" : \"polyline\", \"spatialReference\" : {\"wkid\" : 4326}}";
        const string polylineMJson = "{\"hasM\" : true,\"paths\" : [[[-97.06138,32.837,5],[-97.06133,32.836,6],[-97.06124,32.834,7],[-97.06127,32.832,8]],[[-97.06326,32.759],[-97.06298,32.755]]],\"type\" : \"polyline\", \"spatialReference\" : {\"wkid\" : 4326}}";
        const string polylineEmptyJson = "{\"paths\" : [ ],\"type\" : \"polyline\" }";

        const string polygon2DJson = "{\"rings\" : [[[-97.06138,32.837],[-97.06133,32.836],[-97.06124,32.834],[-97.06127,32.832],[-97.06138,32.837]],[[-97.06326,32.759],[-97.06298,32.755],[-97.06153,32.749],[-97.06326,32.759]]],\"spatialReference\" : {\"wkid\" : 4326},\"type\" : \"polygon \"}";
        const string polygon3DJson = "{\"hasZ\" : true,\"hasM\" : true,\"rings\" : [[[-97.06138,32.837,35.1,4],[-97.06133,32.836,35.2,4.1],[-97.06124,32.834,35.3,4.2],[-97.06127,32.832, 35.2,44.3],[-97.06138,32.837,35.1,4]],[[-97.06326,32.759,35.4],[-97.06298,32.755,35.5],[-97.06153,32.749,35.6],[-97.06326,32.759,35.4]]],\"spatialReference\" : {\"wkid\" : 4326},\"type\" : \"polygon \"}";
        const string polygonEmptyJson = "{\"rings\" : [ ],\"type\" : \"polygon \"}";

        [TestMethod]
        public void TestEsriJson()
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


                Assert.AreEqual("POINT(-118.15 33.8)", point2D.AsWkt());
                Assert.AreEqual("POINT EMPTY", pointNull.AsWkt());
                Assert.AreEqual("POINT EMPTY", pointNaN.AsWkt());

                Assert.AreEqual("MULTIPOINT(-97.06138 32.837,-97.06133 32.836,-97.06124 32.834,-97.06127 32.832)", multipoint2D.AsWkt());

                IRI.Ket.SpatialExtensions.SqlSpatialExtensions.Parse(point2D.AsWkt());
                IRI.Ket.SpatialExtensions.SqlSpatialExtensions.Parse(point3D.AsWkt());
                IRI.Ket.SpatialExtensions.SqlSpatialExtensions.Parse(pointNull.AsWkt());
                IRI.Ket.SpatialExtensions.SqlSpatialExtensions.Parse(pointNaN.AsWkt());

                IRI.Ket.SpatialExtensions.SqlSpatialExtensions.Parse(multipoint2D.AsWkt());
                IRI.Ket.SpatialExtensions.SqlSpatialExtensions.Parse(multipoint3D.AsWkt());
                IRI.Ket.SpatialExtensions.SqlSpatialExtensions.Parse(multipointEmpty.AsWkt());

                IRI.Ket.SpatialExtensions.SqlSpatialExtensions.Parse(polyline2D.AsWkt());
                IRI.Ket.SpatialExtensions.SqlSpatialExtensions.Parse(polyline3D.AsWkt());
                IRI.Ket.SpatialExtensions.SqlSpatialExtensions.Parse(polylineEmpty.AsWkt());

                IRI.Ket.SpatialExtensions.SqlSpatialExtensions.Parse(polygon2D.AsWkt());
                IRI.Ket.SpatialExtensions.SqlSpatialExtensions.Parse(polygon3D.AsWkt());
                IRI.Ket.SpatialExtensions.SqlSpatialExtensions.Parse(polygonEmpty.AsWkt());

            }
            catch (Exception ex)
            {
                Assert.Fail();
            }
        }


        [TestMethod]
        public void TestEsriJsonSqlGeometryConversion()
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

                SqlSpatialExtensions.Parse(point2D.AsWkt()).ParseToEsriJsonGeometry();
                SqlSpatialExtensions.Parse(point3D.AsWkt()).ParseToEsriJsonGeometry();
                SqlSpatialExtensions.Parse(pointNull.AsWkt()).ParseToEsriJsonGeometry();
                SqlSpatialExtensions.Parse(pointNaN.AsWkt()).ParseToEsriJsonGeometry();

                SqlSpatialExtensions.Parse(multipoint2D.AsWkt()).ParseToEsriJsonGeometry();
                SqlSpatialExtensions.Parse(multipoint3D.AsWkt()).ParseToEsriJsonGeometry();
                SqlSpatialExtensions.Parse(multipointEmpty.AsWkt()).ParseToEsriJsonGeometry();

                SqlSpatialExtensions.Parse(polyline2D.AsWkt()).ParseToEsriJsonGeometry();
                SqlSpatialExtensions.Parse(polyline3D.AsWkt()).ParseToEsriJsonGeometry();
                SqlSpatialExtensions.Parse(polylineEmpty.AsWkt()).ParseToEsriJsonGeometry();

                SqlSpatialExtensions.Parse(polygon2D.AsWkt()).ParseToEsriJsonGeometry();
                SqlSpatialExtensions.Parse(polygon3D.AsWkt()).ParseToEsriJsonGeometry();
                SqlSpatialExtensions.Parse(polygonEmpty.AsWkt()).ParseToEsriJsonGeometry();
            }
            catch (Exception ex)
            {
                Assert.Fail();
            }
        }
    }
}
