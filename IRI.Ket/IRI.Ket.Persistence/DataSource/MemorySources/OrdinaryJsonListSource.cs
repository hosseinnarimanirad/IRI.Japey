using IRI.Extensions;
using IRI.Msh.Common.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.DataManagement.DataSource
{
    public class OrdinaryJsonListSource<TGeometryAware> : MemoryDataSource<TGeometryAware, Point> where TGeometryAware : class, IGeometryAware<Point>
    {
        private OrdinaryJsonListSource(List<TGeometryAware> features, Func<TGeometryAware, Feature<Point>> mapToFeatureFunc) : this(features, mapToFeatureFunc, null)
        {

        }

        private OrdinaryJsonListSource(List<TGeometryAware> features, Func<TGeometryAware, Feature<Point>> mapToFeatureFunc, Func<TGeometryAware, string> labelFunc)
        {
            this._labelFunc = labelFunc;

            this._features = features;

            this.Extent = features.Select(f => f.TheGeometry).GetBoundingBox();

            this._mapToFeatureFunc = mapToFeatureFunc;
        }
         
        public static OrdinaryJsonListSource<TGeometryAware> CreateFromJsonString(string jsonString, Func<TGeometryAware, Feature<Point>> mapToFeatureFunc, Func<TGeometryAware, string> labelFunc = null)
        {
            var values = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TGeometryAware>>(jsonString);

            return new OrdinaryJsonListSource<TGeometryAware>(values, mapToFeatureFunc, labelFunc);
        }

        public static OrdinaryJsonListSource<TGeometryAware> CreateFromFile(string fileName, Func<TGeometryAware, Feature<Point>> mapToFeatureFunc, Func<TGeometryAware, string> labelFunc = null)
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
