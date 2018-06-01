using IRI.Ket.SpatialExtensions;
using IRI.Ket.SqlServerSpatialExtension.Model;
using IRI.Sta.Common.Primitives;
using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.DataManagement.DataSource
{
    public class JsonSource<T> : MemoryDataSource<T> where T : IGeometryAware
    {

        private JsonSource(IEnumerable<T> features)
        {
            //if (attributes == null || labelFunc == null)
            //{
            //    this._geometries = geometries;
            //}
            //else
            //{
            //    this.geometryAttributePairs = geometries.Zip(attributes, (a, b) => new NamedSqlGeometry(a, labelFunc(b))).ToList();
            //}

            //this._attributes = attributes;

            //this._labelFunc = labelFunc;

            var sqlGeometries = features.Select(g => g.Geometry.AsSqlGeometry()).ToList();

            this._geometries = sqlGeometries;

            this.Extent = sqlGeometries.GetBoundingBox();
        }

        public static JsonSource<T> CreateFromJsonString(string jsonString)
        {
            var values = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<T>>(jsonString);

            return new JsonSource<T>(values);
        }

        public static JsonSource<T> CreateFromFile(string fileName)
        {
            var jsonString = System.IO.File.ReadAllText(fileName);

            return CreateFromJsonString(jsonString);
        }


    }
}
