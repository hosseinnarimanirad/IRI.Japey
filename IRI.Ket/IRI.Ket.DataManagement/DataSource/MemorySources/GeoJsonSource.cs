using IRI.Ket.SpatialExtensions;
using IRI.Ket.SqlServerSpatialExtension.Model;
using IRI.Msh.Common.Primitives;
using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.DataManagement.DataSource
{
    public class GeoJsonSource<T> : MemoryDataSource<T> where T : class, ISqlGeometryAware
    {

        private GeoJsonSource(List<T> features, Func<T, SqlFeature> mapToFeatureFunc) : this(features, mapToFeatureFunc, null)
        {

        }

        private GeoJsonSource(List<T> features, Func<T, SqlFeature> mapToFeatureFunc, Func<T, string> labelFunc)
        {
            this._labelFunc = labelFunc;

            this._features = features;

            this.Extent = GetGeometries().GetBoundingBox();

            this._mapToFeatureFunc = mapToFeatureFunc;
        }

        //public static GeoJsonSource<T> CreateFromJsonString(string jsonString)
        //{
        //    return CreateFromJsonString(jsonString, null);
        //}

        //public static GeoJsonSource<T> CreateFromFile(string fileName)
        //{
        //    return CreateFromFile(fileName, null);
        //}

        public static GeoJsonSource<T> CreateFromJsonString(string jsonString, Func<T, SqlFeature> mapToFeatureFunc, Func<T, string> labelFunc = null)
        {
            var values = Newtonsoft.Json.JsonConvert.DeserializeObject<List<T>>(jsonString);

            return new GeoJsonSource<T>(values, mapToFeatureFunc, labelFunc);
        }

        public static GeoJsonSource<T> CreateFromFile(string fileName, Func<T, SqlFeature> mapToFeatureFunc, Func<T, string> labelFunc = null)
        {
            var jsonString = System.IO.File.ReadAllText(fileName);

            return CreateFromJsonString(jsonString, mapToFeatureFunc, labelFunc);
        }

        //public static GeoJsonSource<SqlFeature> Create(List<SqlFeature> features)
        //{
        //    return new GeoJsonSource<SqlFeature>(features, f => f);
        //}
    }
}
