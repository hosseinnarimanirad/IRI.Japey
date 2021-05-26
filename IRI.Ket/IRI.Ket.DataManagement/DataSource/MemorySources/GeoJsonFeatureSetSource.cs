//using IRI.Ket.SpatialExtensions;
//using IRI.Ket.SqlServerSpatialExtension.Model;
//using IRI.Msh.Common.Model.GeoJson;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace IRI.Ket.DataManagement.DataSource.MemorySources
//{
//    public class GeoJsonFeatureSetSource : MemoryDataSource<SqlFeature>
//    {
//        private GeoJsonFeatureSetSource(List<SqlFeature> features)
//        {
//            //this._labelFunc = labelFunc;

//            this._features = features;

//            this.Extent = GetGeometries().GetBoundingBox();

//            this._mapToFeatureFunc = f => f;
//        }

//        //public static GeoJsonFeatureSetSource CreateFromJsonString(string jsonString, Func<T, SqlFeature> mapToFeatureFunc, Func<T, string> labelFunc = null)
//        //{
//        //    var values = GeoJsonFeatureSet.Parse(jsonString);

//        //    var features = values.Features.Select(f => new SqlFeature()
//        //    {
//        //        Attributes = f.Properties,
//        //         TheSqlGeometry=f.Geometry.Parse()
//        //    })

//        //    return new GeoJsonFeatureSetSource(values, mapToFeatureFunc, labelFunc);
//        //}

//        //public static GeoJsonFeatureSetSource CreateFromFile(string fileName, Func<T, SqlFeature> mapToFeatureFunc, Func<T, string> labelFunc = null)
//        //{
//        //    var jsonString = System.IO.File.ReadAllText(fileName);

//        //    return CreateFromJsonString(jsonString, mapToFeatureFunc, labelFunc);
//        //}

//    }
//}
