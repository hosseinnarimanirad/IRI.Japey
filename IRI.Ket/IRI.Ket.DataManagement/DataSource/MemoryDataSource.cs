using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SqlServer.Types;
using IRI.Msh.Common.Primitives;
using System.Data;
using IRI.Ket.DataManagement.Model;
using IRI.Ket.SqlServerSpatialExtension.Model;
using IRI.Ket.SpatialExtensions;
using System.Threading.Tasks;
using IRI.Msh.CoordinateSystem.MapProjection;
using System.Text;
using IRI.Ket.SqlServerSpatialExtension.Helpers;

namespace IRI.Ket.DataManagement.DataSource
{
    public class MemoryDataSource : FeatureDataSource
    {
        const string _geoColumnName = "Geo";

        public override BoundingBox Extent { get; protected set; }

        protected List<SqlGeometry> _geometries;

        public MemoryDataSource()
        {

        }

        public MemoryDataSource(List<SqlGeometry> geometries)
        {
            this._geometries = geometries;

            this.Extent = geometries.GetBoundingBox();
        }

        //GetGeometries
        public override List<SqlGeometry> GetGeometries()
        {
            return this._geometries;
        }

        public override List<object> GetAttributes(string attributeColumn, string whereClause)
        {
            throw new NotImplementedException();
        }

        public override DataTable GetEntireFeatures(SqlGeometry geometry)
        {
            DataTable result = new DataTable();

            result.Columns.Add(new DataColumn(_geoColumnName));

            IEnumerable<SqlGeometry> geometries;

            if (geometry == null)
            {
                geometries = GetGeometries();
            }
            else
            {
                if (geometry?.STSrid.Value != this.GetSrid())
                {
                    throw new ArgumentException("srid mismatch");
                }

                geometries = GetGeometries()?.Where(i => i.STIntersects(geometry).IsTrue);
            }

            foreach (var item in geometries)
            {
                result.Rows.Add(item);
            }

            return result;
        }

        public override List<NamedSqlGeometry> GetGeometryLabelPairs(SqlGeometry geometry)
        {
            return GetGeometries().Select(g => new NamedSqlGeometry(g, string.Empty)).ToList();
        }


        private static Func<List<SqlFeature>, DataTable> sqlFeatureToDataTableMapping = (list) =>
         {
             if (!(list?.Count > 0))
             {
                 return null;
             }

             DataTable table = new DataTable();

             foreach (var col in list?.First().Attributes)
             {
                 table.Columns.Add(col.Key);
             }

             table.Columns.Add(new DataColumn("Geo"));

             foreach (var item in list)
             {
                 var newRow = table.NewRow();

                 foreach (var col in list.First().Attributes)
                 {
                     newRow[col.Key] = col.Value;
                 }

                 newRow["Geo"] = item.TheSqlGeometry;

                 table.Rows.Add(newRow);
             }

             return table;
         };

        public static MemoryDataSource<SqlFeature> Create(string shpFileName, Encoding dataEncoding, Encoding headerEncoding, bool correctFarsiCharacters, SrsBase targetSrs = null)
        {
            var features = ShapefileHelper.ReadShapefile(shpFileName, dataEncoding, headerEncoding, correctFarsiCharacters, targetSrs);

            var result = new MemoryDataSource<SqlFeature>(features, i => i.Label);

            result.MappingFunc = sqlFeatureToDataTableMapping;

            return result;
        }

        public static async Task<MemoryDataSource<SqlFeature>> CreateAsync(string shpFileName, Encoding dataEncoding, Encoding headerEncoding, bool correctFarsiCharacters, SrsBase targetSrs = null)
        {
            var features = await ShapefileHelper.ReadShapefileAsync(shpFileName, dataEncoding, headerEncoding, correctFarsiCharacters, targetSrs);

            var result = new MemoryDataSource<SqlFeature>(features, i => i.Label);

            result.MappingFunc = sqlFeatureToDataTableMapping;

            return result;
        }
    }

    public class MemoryDataSource<T> : FeatureDataSource<T> where T : ISqlGeometryAware
    {
        public override BoundingBox Extent { get; protected set; }

        protected List<T> _features;

        protected Func<T, string> _labelFunc;

        protected Func<int, T> _idFunc;

        public Func<List<T>, DataTable> MappingFunc;

        public override int GetSrid()
        {
            return this._features?.SkipWhile(g => g == null || g.TheSqlGeometry == null || g.TheSqlGeometry.IsNotValidOrEmpty())?.FirstOrDefault()?.TheSqlGeometry.GetSrid() ?? 0;
        }

        public MemoryDataSource() : this(new List<SqlFeature>().Cast<T>().ToList(), null)
        {

        }

        public MemoryDataSource(List<SqlGeometry> geometries)
        {
            this._features = geometries.Select(g => new SqlFeature(g)).Cast<T>().ToList<T>();

            this._labelFunc = null;

            this.Extent = GetGeometries().GetBoundingBox();
        }

        public MemoryDataSource(List<T> features, Func<T, string> labelFunc, Func<int, T> idFunc = null)
        {
            this._features = features;

            this._labelFunc = labelFunc;

            this._idFunc = idFunc;

            this.Extent = GetGeometries().GetBoundingBox();
        }

        //GetGeometries
        //public override List<SqlGeometry> GetGeometries()
        //{
        //    return this._features?.Select(f => f.TheSqlGeometry)?.ToList();
        //}

        //public override List<SqlGeometry> GetGeometries(string whereClause)
        //{
        //    throw new NotImplementedException();
        //}

        //public override List<SqlGeometry> GetGeometries(BoundingBox boundingBox)
        //{
        //    SqlGeometry boundary = boundingBox.AsSqlGeometry(this.GetSrid()).MakeValid();

        //    return GetGeometries().Where(i => i.STIntersects(boundary).IsTrue).ToList();
        //}

        public override List<SqlGeometry> GetGeometries()
        {
            SqlGeometry geometry = null;

            return GetGeometries(geometry);
        }

        public override List<SqlGeometry> GetGeometries(SqlGeometry geometry)
        {
            if (geometry == null)
            {
                return this._features.Select(f => f.TheSqlGeometry).ToList();
            }
            else
            {
                return this._features.Where(i => i?.TheSqlGeometry?.STIntersects(geometry).IsTrue == true).Select(f => f.TheSqlGeometry).ToList();
            }

        }

        //GetAttributes
        public override List<T> GetFeatures(SqlGeometry geometry)
        {
            if (geometry == null)
            {
                return this._features.ToList();
            }
            else
            {
                return this._features.Where(i => i?.TheSqlGeometry?.STIntersects(geometry).IsTrue == true).ToList();
            }
        }

        public override List<object> GetAttributes(string attributeColumn, string whereClause)
        {
            throw new NotImplementedException();
        }

        //GetEntireFeatures
        //public override DataTable GetEntireFeatures(string whereClause)
        //{
        //    throw new NotImplementedException();
        //}

        //public override DataTable GetEntireFeatures(BoundingBox boundary)
        //{
        //    SqlGeometry geometry = boundary.AsSqlGeometry(GetSrid());

        //    return GetEntireFeatures(geometry);
        //}

        public override DataTable GetEntireFeatures(SqlGeometry geometry)
        {
            if (geometry?.STSrid.Value != this.GetSrid())
            {
                throw new ArgumentException("srid mismatch");
            }

            return MappingFunc(this._features.Where(i => i.TheSqlGeometry?.STIntersects(geometry).IsTrue == true).ToList());

            //DataTable result = new DataTable();

            //result.Columns.Add(new DataColumn("Geo"));

            //if (_labelFunc != null)
            //{
            //    var geometries = GetGeometryLabelPairs(geometry);

            //    result.Columns.Add(new DataColumn("Attribute", typeof(string)));

            //    foreach (var item in geometries)
            //    {
            //        result.Rows.Add(item.Geometry, item.Label);
            //    }
            //}
            //else
            //{
            //    var geometries = GetGeometries().Where(i => i.STIntersects(geometry).IsTrue);

            //    foreach (var item in geometries)
            //    {
            //        result.Rows.Add(item);
            //    }
            //}

            //return result;
        }

        public override DataTable GetEntireFeatures()
        {
            return MappingFunc(_features);
        }

        //GetGeometryLabelPairs
        public override List<NamedSqlGeometry> GetGeometryLabelPairs()
        {
            return this._features.Select(f => new NamedSqlGeometry(f.TheSqlGeometry, _labelFunc(f))).ToList();
        }

        //public override List<NamedSqlGeometry> GetGeometryLabelPairs(BoundingBox boundingBox)
        //{
        //    SqlGeometry boundary = boundingBox.AsSqlGeometry(GetSrid());

        //    return GetGeometryLabelPairs(boundary);
        //}

        public override List<NamedSqlGeometry> GetGeometryLabelPairs(SqlGeometry geometry)
        {
            return this._features.Where(i => i?.TheSqlGeometry?.STIntersects(geometry).IsTrue == true)
                                    .Select(f => new NamedSqlGeometry(f.TheSqlGeometry, _labelFunc(f))).ToList();
        }

        public override void Add(T newGeometry)
        {
            this._features.Add(newGeometry);
        }

        public override void Remove(int geometryId)
        {
            if (_idFunc == null)
            {
                return;
            }

            var geometry = _idFunc(geometryId);

            this.Remove(geometry);
        }

        public void Remove(T geometry)
        {
            this._features.Remove(geometry);
        }

        public override void Update(T newGeometry, int geometryId)
        {
            if (_idFunc == null)
            {
                return;
            }

            var geometry = _idFunc(geometryId);

            var index = this._features.IndexOf(geometry);

            if (index < 0)
            {
                return;
            }

            this._features[index] = newGeometry; ;
        }
    }
}