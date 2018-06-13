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
    public class GeoJsonSource<T> : MemoryDataSource<T> where T : ISqlGeometryAware
    {

        private GeoJsonSource(List<T> features) : this(features, null)
        {

        }

        private GeoJsonSource(List<T> features, Func<T, string> labelFunc)
        {
            this._labelFunc = labelFunc;

            this._features = features;

            this.Extent = GetGeometries().GetBoundingBox();
        }

        public static GeoJsonSource<T> CreateFromJsonString(string jsonString)
        {
            var values = Newtonsoft.Json.JsonConvert.DeserializeObject<List<T>>(jsonString);

            return new GeoJsonSource<T>(values);
        }

        public static GeoJsonSource<T> CreateFromFile(string fileName)
        {
            var jsonString = System.IO.File.ReadAllText(fileName);

            return CreateFromJsonString(jsonString);
        }

        public static GeoJsonSource<T> CreateFromJsonString(string jsonString, Func<T, string> labelFunc)
        {
            var values = Newtonsoft.Json.JsonConvert.DeserializeObject<List<T>>(jsonString);

            return new GeoJsonSource<T>(values, labelFunc);
        }

        public static GeoJsonSource<T> CreateFromFile(string fileName, Func<T, string> labelFunc)
        {
            var jsonString = System.IO.File.ReadAllText(fileName);

            return CreateFromJsonString(jsonString, labelFunc);
        }

    }
}
