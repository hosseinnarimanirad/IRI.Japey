using IRI.Ket.SpatialExtensions;
using IRI.Ket.SqlServerSpatialExtension.Model;
using IRI.Msh.Common.Primitives;
using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRI.Msh.Common.Extensions;

namespace IRI.Ket.DataManagement.DataSource
{
    public class OrdinaryJsonListSource<T> : MemoryDataSource<T> where T : class, IGeometryAware<Point>
    {
        private OrdinaryJsonListSource(List<T> features, Func<T, Feature<Point>> mapToFeatureFunc) : this(features, mapToFeatureFunc, null)
        {

        }

        private OrdinaryJsonListSource(List<T> features, Func<T, Feature<Point>> mapToFeatureFunc, Func<T, string> labelFunc)
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

        public static OrdinaryJsonListSource<T> CreateFromJsonString(string jsonString, Func<T, Feature<Point>> mapToFeatureFunc, Func<T, string> labelFunc = null)
        {
            var values = Newtonsoft.Json.JsonConvert.DeserializeObject<List<T>>(jsonString);

            return new OrdinaryJsonListSource<T>(values, mapToFeatureFunc, labelFunc);
        }

        public static OrdinaryJsonListSource<T> CreateFromFile(string fileName, Func<T, Feature<Point>> mapToFeatureFunc, Func<T, string> labelFunc = null)
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
