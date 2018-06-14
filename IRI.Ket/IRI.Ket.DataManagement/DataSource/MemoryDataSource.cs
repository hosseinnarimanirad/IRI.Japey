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

namespace IRI.Ket.DataManagement.DataSource
{
    public class MemoryDataSource<T> : IFeatureDataSource where T : ISqlGeometryAware
    {
        public override BoundingBox Extent { get; protected set; }

        //protected List<SqlGeometry> _geometries;

        protected List<T> _features;

        protected Func<T, string> _labelFunc;

        public Func<List<T>, DataTable> MappingFunc;
        //protected List<NamedSqlGeometry> geometryAttributePairs;
         
        public override int GetSrid()
        {
            return this._features?.SkipWhile(g => g == null || g.Geometry == null || g.Geometry.IsNotValidOrEmpty())?.FirstOrDefault()?.Geometry.GetSrid() ?? 0;
        }

        //private bool HasAttribute
        //{
        //    get { return _attributes != null && _labelFunc != null && _attributes.Count == _geometries?.Count; }
        //}

        //public static MemoryDataSource<SqlFeature> Create(SqlGeometry geometry)
        //{
        //    return new MemoryDataSource<SqlFeature>(new List<SqlFeature>() { new SqlFeature(geometry) });
        //}

        //public MemoryDataSource()
        //{
        //    List<SqlGeometry> geometries = new List<SqlGeometry>();
        //    var tmep = new MemoryDataSource<SqlFeature>(geometries.Select(g => (new SqlFeature(g))).ToList(), null);
        //}

        public MemoryDataSource() : this(new List<SqlFeature>().Cast<T>().ToList(), null)
        {

        }

        public MemoryDataSource(List<SqlGeometry> geometries)
        {
            this._features = geometries.Select(g => new SqlFeature(g)).Cast<T>().ToList<T>();

            this._labelFunc = null;

            this.Extent = GetGeometries().GetBoundingBox();
        }

        //public MemoryDataSource(List<T> attributes)
        //    : this(geometries, attributes, null)
        //{ }

        public MemoryDataSource(List<T> features, Func<T, string> labelFunc)
        {
            //if (attributes == null || labelFunc == null)
            //{
            //    this._geometries = geometries;
            //}
            //else
            //{
            //    //this.geometryAttributePairs = geometries.Zip(attributes, (a, b) =>  Tuple.Create(a, labelFunc(b))).ToList();
            //    this.geometryAttributePairs = geometries.Zip(attributes, (a, b) => new NamedSqlGeometry(a, labelFunc(b))).ToList();
            //}
            //this._geometries = geometries;

            this._features = features;

            //this._attributes = attributes;

            this._labelFunc = labelFunc;

            this.Extent = GetGeometries().GetBoundingBox();
        }

        //GetGeometries
        public override List<SqlGeometry> GetGeometries()
        {
            return this._features?.Select(f => f.Geometry)?.ToList();
        }

        public override List<SqlGeometry> GetGeometries(string whereClause)
        {
            throw new NotImplementedException();
        }

        public override List<SqlGeometry> GetGeometries(BoundingBox boundingBox)
        {
            SqlGeometry boundary = boundingBox.AsSqlGeometry(this.GetSrid()).MakeValid();

            return GetGeometries().Where(i => i.STIntersects(boundary).IsTrue).ToList();
        }

        public override List<SqlGeometry> GetGeometries(SqlGeometry geometry)
        {
            return GetGeometries().Where(i => i.STIntersects(geometry).IsTrue).ToList();
        }

        //GetAttributes
        public List<T> GetFeatures(SqlGeometry geometry)
        {
            return this._features.Where(i => i.Geometry.STIntersects(geometry).IsTrue).ToList();
        }

        public override List<object> GetAttributes(string attributeColumn, string whereClause)
        {
            throw new NotImplementedException();
        }

        //GetEntireFeatures
        public override DataTable GetEntireFeatures(string whereClause)
        {
            throw new NotImplementedException();
        }

        public override DataTable GetEntireFeatures(BoundingBox boundary)
        {
            SqlGeometry geometry = boundary.AsSqlGeometry(GetSrid());

            return GetEntireFeatures(geometry);
        }

        public override DataTable GetEntireFeatures(SqlGeometry geometry)
        {
            if (geometry?.STSrid.Value != this.GetSrid())
            {
                throw new NotImplementedException();
            }

            DataTable result = new DataTable();

            result.Columns.Add(new DataColumn("Geo"));

            if (_labelFunc != null)
            {
                var geometries = GetGeometryLabelPairs(geometry);

                result.Columns.Add(new DataColumn("Attribute", typeof(string)));

                foreach (var item in geometries)
                {
                    result.Rows.Add(item.Geometry, item.Label);
                }
            }
            else
            {
                var geometries = GetGeometries().Where(i => i.STIntersects(geometry).IsTrue);

                foreach (var item in geometries)
                {
                    result.Rows.Add(item);
                }
            }

            return result;
        }

        public override DataTable GetEntireFeatures()
        {
            return MappingFunc(_features);
        }

        //GetGeometryLabelPairs
        public override List<NamedSqlGeometry> GetGeometryLabelPairs()
        {
            return this._features.Select(f => new NamedSqlGeometry(f.Geometry, _labelFunc(f))).ToList();
        }

        public override List<NamedSqlGeometry> GetGeometryLabelPairs(BoundingBox boundingBox)
        {
            //SqlGeometry boundary =
            //    SqlGeometry.Parse(
            //        string.Format("POLYGON(({0} {1}, {0} {2}, {3} {2}, {3} {1}, {0} {1}))", boundingBox.XMin, boundingBox.YMin, boundingBox.YMax, boundingBox.XMax));

            //return _geometries.Zip(_attributes, (a, b) => Tuple.Create(a, _labelFunc(b))).Where(i => i.Item1.STWithin(boundary).Value).ToList();
            SqlGeometry boundary = boundingBox.AsSqlGeometry(GetSrid());

            //93.01.18
            //return _geometries.Zip(_attributes, (a, b) => Tuple.Create(a, _labelFunc(b))).Where(i => i.Item1.STIntersects(boundary).Value).ToList();

            //return this.geometryAttributePairs.Where(i => i.Geometry.STIntersects(boundary).IsTrue).ToList();
            return GetGeometryLabelPairs(boundary);
        }

        public override List<NamedSqlGeometry> GetGeometryLabelPairs(SqlGeometry geometry)
        {
            return this._features.Where(i => i.Geometry.STIntersects(geometry).IsTrue).Select(f => new NamedSqlGeometry(f.Geometry, _labelFunc(f))).ToList();
        }

        public override List<NamedSqlGeometry> GetGeometryLabelPairs(string whereClause)
        {
            throw new NotImplementedException();
        }

    }



    public class MemoryDataSource : IFeatureDataSource
    {
        public override BoundingBox Extent { get; protected set; }

        protected List<SqlGeometry> _geometries;


        public override int GetSrid()
        {
            return this._geometries?.SkipWhile(g => g.IsNotValidOrEmpty())?.FirstOrDefault().GetSrid() ?? 0;
        }


        //public static MemoryDataSource  Create(SqlGeometry geometry)
        //{
        //    return new MemoryDataSource (new List<SqlGeometry>() { geometry });
        //}

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

        public override List<SqlGeometry> GetGeometries(string whereClause)
        {
            throw new NotImplementedException();
        }

        public override List<SqlGeometry> GetGeometries(BoundingBox boundingBox)
        {
            //SqlGeometry boundary =
            //     SqlGeometry.Parse(
            //         string.Format("POLYGON(({0} {1}, {0} {2}, {3} {2}, {3} {1}, {0} {1}))", boundingBox.XMin, boundingBox.YMin, boundingBox.YMax, boundingBox.XMax));

            //return GetGeometries().Where(i => i.STWithin(boundary).Value).ToList();
            SqlGeometry boundary = boundingBox.AsSqlGeometry(this.GetSrid()).MakeValid();

            return GetGeometries().Where(i => i.STIntersects(boundary).IsTrue).ToList();
        }

        public override List<SqlGeometry> GetGeometries(SqlGeometry geometry)
        {
            return GetGeometries().Where(i => i.STIntersects(geometry).IsTrue).ToList();
        }

        //GetAttributes
        public List<object> GetAttributes()
        {
            throw new NotImplementedException();
        }

        public override List<object> GetAttributes(string attributeColumn, string whereClause)
        {
            throw new NotImplementedException();
        }

        //GetEntireFeatures
        public override DataTable GetEntireFeatures(string whereClause)
        {
            throw new NotImplementedException();
        }

        public override DataTable GetEntireFeatures(BoundingBox boundary)
        {
            SqlGeometry geometry = boundary.AsSqlGeometry(GetSrid());

            return GetEntireFeatures(geometry);
        }

        public override DataTable GetEntireFeatures(SqlGeometry geometry)
        {
            if (geometry?.STSrid.Value != this.GetSrid())
            {
                throw new NotImplementedException();
            }

            DataTable result = new DataTable();

            result.Columns.Add(new DataColumn("Geo"));


            var geometries = GetGeometries().Where(i => i.STIntersects(geometry).IsTrue);

            foreach (var item in geometries)
            {
                result.Rows.Add(item);
            }


            return result;
        }

        public override DataTable GetEntireFeatures( )
        { 
            DataTable result = new DataTable();

            result.Columns.Add(new DataColumn("Geo"));
             
            var geometries = GetGeometries();

            foreach (var item in geometries)
            {
                result.Rows.Add(item);
            }
             
            return result;
        }

        //GetGeometryLabelPairs
        public override List<NamedSqlGeometry> GetGeometryLabelPairs()
        {
            throw new NotImplementedException();
        }

        public override List<NamedSqlGeometry> GetGeometryLabelPairs(BoundingBox boundingBox)
        {
            throw new NotImplementedException();
        }

        public override List<NamedSqlGeometry> GetGeometryLabelPairs(SqlGeometry geometry)
        {
            throw new NotImplementedException();
        }

        public override List<NamedSqlGeometry> GetGeometryLabelPairs(string whereClause)
        {
            throw new NotImplementedException();
        }

    }

    //public class MemoryDataSource<T> : IFeatureDataSource  
    //{
    //    public override BoundingBox Extent { get; protected set; }

    //    protected List<SqlGeometry> _geometries;

    //    protected List<T> _attributes;

    //    protected Func<T, string> _labelFunc;

    //    protected List<NamedSqlGeometry> geometryAttributePairs;



    //    public override int GetSrid()
    //    {
    //        return this._geometries?.SkipWhile(g => g.IsNotValidOrEmpty())?.FirstOrDefault().GetSrid() ?? 0;
    //    }

    //    private bool HasAttribute
    //    {
    //        get { return _attributes != null && _labelFunc != null && _attributes.Count == _geometries?.Count; }
    //    }

    //    public static MemoryDataSource  Create(SqlGeometry geometry)
    //    {
    //        return new MemoryDataSource (new List<SqlGeometry>() { geometry });
    //    }

    //    public MemoryDataSource()
    //    {

    //    }

    //    public MemoryDataSource(List<SqlGeometry> geometries)
    //        : this(geometries, null)
    //    {
    //    }

    //    public MemoryDataSource(List<SqlGeometry> geometries, List<T> attributes)
    //        : this(geometries, attributes, null)
    //    { }

    //    public MemoryDataSource(List<SqlGeometry> geometries, List<T> attributes, Func<T, string> labelFunc)
    //    {
    //        if (attributes == null || labelFunc == null)
    //        {
    //            this._geometries = geometries;
    //        }
    //        else
    //        {
    //            //this.geometryAttributePairs = geometries.Zip(attributes, (a, b) =>  Tuple.Create(a, labelFunc(b))).ToList();
    //            this.geometryAttributePairs = geometries.Zip(attributes, (a, b) => new NamedSqlGeometry(a, labelFunc(b))).ToList();
    //        }
    //        //this._geometries = geometries;

    //        this._attributes = attributes;

    //        this._labelFunc = labelFunc;

    //        this.Extent = geometries.GetBoundingBox();
    //    }

    //    //GetGeometries
    //    public override List<SqlGeometry> GetGeometries()
    //    {
    //        if (this._geometries == null)
    //        {
    //            return this.geometryAttributePairs.Select(i => i.Geometry).ToList();
    //        }
    //        else
    //        {
    //            return this._geometries;
    //        }
    //    }

    //    public override List<SqlGeometry> GetGeometries(string whereClause)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public override List<SqlGeometry> GetGeometries(BoundingBox boundingBox)
    //    {
    //        //SqlGeometry boundary =
    //        //     SqlGeometry.Parse(
    //        //         string.Format("POLYGON(({0} {1}, {0} {2}, {3} {2}, {3} {1}, {0} {1}))", boundingBox.XMin, boundingBox.YMin, boundingBox.YMax, boundingBox.XMax));

    //        //return GetGeometries().Where(i => i.STWithin(boundary).Value).ToList();
    //        SqlGeometry boundary = boundingBox.AsSqlGeometry(this.GetSrid()).MakeValid();

    //        return GetGeometries().Where(i => i.STIntersects(boundary).IsTrue).ToList();
    //    }

    //    public override List<SqlGeometry> GetGeometries(SqlGeometry geometry)
    //    {
    //        return GetGeometries().Where(i => i.STIntersects(geometry).IsTrue).ToList();
    //    }

    //    //GetAttributes
    //    public List<object> GetAttributes()
    //    {
    //        return this._attributes.Cast<object>().ToList();
    //    }

    //    public override List<object> GetAttributes(string attributeColumn, string whereClause)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    //GetEntireFeatures
    //    public override DataTable GetEntireFeatures(string whereClause)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public override DataTable GetEntireFeatures(BoundingBox boundary)
    //    {
    //        SqlGeometry geometry = boundary.AsSqlGeometry(GetSrid());

    //        return GetEntireFeatures(geometry);
    //    }

    //    public override DataTable GetEntireFeatures(SqlGeometry geometry)
    //    {
    //        if (geometry?.STSrid.Value != this.GetSrid())
    //        {
    //            throw new NotImplementedException();
    //        }

    //        DataTable result = new DataTable();

    //        result.Columns.Add(new DataColumn("Geo"));

    //        if (HasAttribute)
    //        {
    //            var geometries = GetGeometryLabelPairs(geometry);

    //            result.Columns.Add(new DataColumn("Attribute", typeof(string)));

    //            foreach (var item in geometries)
    //            {
    //                result.Rows.Add(item.Geometry, item.Label);
    //            }
    //        }
    //        else
    //        {
    //            var geometries = GetGeometries().Where(i => i.STIntersects(geometry).IsTrue);

    //            foreach (var item in geometries)
    //            {
    //                result.Rows.Add(item);
    //            }
    //        }

    //        return result;
    //    }

    //    //GetGeometryLabelPairs
    //    public override List<NamedSqlGeometry> GetGeometryLabelPairs()
    //    {
    //        return this.geometryAttributePairs;
    //    }

    //    public override List<NamedSqlGeometry> GetGeometryLabelPairs(BoundingBox boundingBox)
    //    {
    //        //SqlGeometry boundary =
    //        //    SqlGeometry.Parse(
    //        //        string.Format("POLYGON(({0} {1}, {0} {2}, {3} {2}, {3} {1}, {0} {1}))", boundingBox.XMin, boundingBox.YMin, boundingBox.YMax, boundingBox.XMax));

    //        //return _geometries.Zip(_attributes, (a, b) => Tuple.Create(a, _labelFunc(b))).Where(i => i.Item1.STWithin(boundary).Value).ToList();
    //        SqlGeometry boundary = boundingBox.AsSqlGeometry(GetSrid());

    //        //93.01.18
    //        //return _geometries.Zip(_attributes, (a, b) => Tuple.Create(a, _labelFunc(b))).Where(i => i.Item1.STIntersects(boundary).Value).ToList();

    //        //return this.geometryAttributePairs.Where(i => i.Geometry.STIntersects(boundary).IsTrue).ToList();
    //        return GetGeometryLabelPairs(boundary);
    //    }

    //    public override List<NamedSqlGeometry> GetGeometryLabelPairs(SqlGeometry geometry)
    //    {
    //        return this.geometryAttributePairs.Where(i => i.Geometry.STIntersects(geometry).IsTrue).ToList();
    //    }

    //    public override List<NamedSqlGeometry> GetGeometryLabelPairs(string whereClause)
    //    {
    //        throw new NotImplementedException();
    //    }

    //}
}
