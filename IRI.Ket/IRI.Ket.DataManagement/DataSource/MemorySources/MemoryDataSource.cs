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
using IRI.Msh.Common.Extensions;

namespace IRI.Ket.DataManagement.DataSource
{
    public class MemoryDataSource : FeatureDataSource
    {
        const string _geoColumnName = "Geo";

        public override BoundingBox Extent { get; protected set; }

        protected List<Geometry<Point>> _geometries;

        public MemoryDataSource()
        {

        }

        public MemoryDataSource(List<Geometry<Point>> geometries)
        {
            this._geometries = geometries;

            this.Extent = geometries.GetBoundingBox();
        }

        //GetGeometries
        public override List<Geometry<Point>> GetGeometries()
        {
            return this._geometries;
        }

        //public override List<object> GetAttributes(string attributeColumn, string whereClause)
        //{
        //    throw new NotImplementedException();
        //}

        public override DataTable GetEntireFeatures(Geometry<Point> geometry)
        {
            DataTable result = new DataTable();

            result.Columns.Add(new DataColumn(_geoColumnName));

            IEnumerable<Geometry<Point>> geometries;

            if (geometry == null)
            {
                geometries = GetGeometries();
            }
            else
            {
                if (geometry?.Srid != this.GetSrid())
                {
                    throw new ArgumentException("srid mismatch");
                }

                geometries = GetGeometries()?.Where(i => i.Intersects(geometry));
            }

            foreach (var item in geometries)
            {
                result.Rows.Add(item);
            }

            return result;
        }

        public override List<NamedGeometry<Point>> GetGeometryLabelPairs(Geometry<Point> geometry)
        {
            return GetGeometries().Select(g => new NamedGeometry<Point>(g, string.Empty)).ToList();
        }

        public override void Add(IGeometryAware<Point> newValue)
        {
            _geometries.Add(newValue.TheGeometry);
        }

        public override void Remove(IGeometryAware<Point> value)
        {
            _geometries.Remove(value.TheGeometry);
        }

        //ERROR PRONE: valueId is used as index
        public override void Update(IGeometryAware<Point> newValue)
        {
            _geometries[newValue.Id] = newValue.TheGeometry;
        }

        public override void UpdateFeature(IGeometryAware<Point> newValue)
        {
            _geometries[newValue.Id] = newValue.TheGeometry;
        }

        public override void SaveChanges()
        {
            throw new NotImplementedException();
        }

        public override FeatureSet GetSqlFeatures()
        {
            var features = GetGeometries().Select(g => new Feature<Point>(g, string.Empty) { Attributes = new Dictionary<string, object>() }).ToList();

            return new FeatureSet(this.GetSrid()) { Features = features };
        }
    }

    public class MemoryDataSource<T> : FeatureDataSource<T> where T : class, IGeometryAware<Point>
    {
        public override BoundingBox Extent { get; protected set; }

        protected List<T> _features;

        protected Func<T, string> _labelFunc;

        private int _uniqueId = 0;

        protected Func<int, T> _idFunc;

        protected Func<T, Feature<Point>> _mapToFeatureFunc;

        public override int GetSrid()
        {
            return this._features?.SkipWhile(g => g == null || g.TheGeometry == null || g.TheGeometry.IsNotValidOrEmpty())?.FirstOrDefault()?.TheGeometry.Srid ?? 0;
        }

        public MemoryDataSource() : this(new List<SqlFeature>().Cast<T>().ToList(), null, null, null)
        {

        }

        public MemoryDataSource(List<Geometry<Point>> geometries)
        {
            this._features = geometries.Select(g => new Feature<Point>(g) { Id = GetNewId() }).Cast<T>().ToList();

            this._labelFunc = null;

            this._idFunc = id => this._features.Single(f => f.Id == id);

            this.Extent = GetGeometries().GetBoundingBox();
        }

        public MemoryDataSource(List<T> features, Func<T, string> labelFunc, Func<int, T> idFunc, Func<T, Feature<Point>> mapToFeatureFunc)
        {
            this._features = features;

            foreach (var item in _features)
            {
                item.Id = GetNewId();
            }

            this._labelFunc = labelFunc;

            this._idFunc = idFunc;

            this.Extent = GetGeometries().GetBoundingBox();

            _mapToFeatureFunc = mapToFeatureFunc;
        }

        protected int GetNewId()
        {
            return _uniqueId++;
        }

        public override List<Geometry<Point>> GetGeometries()
        {
            Geometry<Point> geometry = null;

            return GetGeometries(geometry);
        }

        public override List<Geometry<Point>> GetGeometries(Geometry<Point> geometry)
        {
            //if (geometry == null)
            //{
            //    return this._features.Select(f => f.TheGeometry).ToList();
            //}
            //else
            //{
            //    return this._features.Where(i => i?.TheGeometry?.Intersects(geometry) == true).Select(f => f.TheGeometry).ToList();
            //}
            return GetFeatures(geometry)?.Select(f => f.TheGeometry).ToList();

        }

        //GetAttributes
        public override List<T> GetFeatures(Geometry<Point> geometry)
        {
            if (this._features == null || this._features.Count == 0)
            {
                return new List<T>();
            }

            if (geometry == null)
            {
                return this._features.ToList();
            }
            else
            {
                if (geometry?.Srid != this.GetSrid())
                {
                    throw new ArgumentException("srid mismatch");
                }

                return this._features.Where(i => i?.TheGeometry?.Intersects(geometry) == true).ToList();
            }
        }

        //public override List<object> GetAttributes(string attributeColumn, string whereClause)
        //{
        //    throw new NotImplementedException();
        //}

        //GetEntireFeatures
        //public override DataTable GetEntireFeatures(string whereClause)
        //{
        //    throw new NotImplementedException();
        //}

        //public override DataTable GetEntireFeatures(BoundingBox boundary)
        //{
        //    Geometry<Point> geometry = boundary.AsSqlGeometry(GetSrid());

        //    return GetEntireFeatures(geometry);
        //}



        //GetGeometryLabelPairs
        public override List<NamedGeometry<Point>> GetGeometryLabelPairs()
        {
            return this._features.Select(f => new NamedGeometry<Point>(f.TheGeometry, _labelFunc(f))).ToList();
        }

        //public override List<NamedGeometry<Point>> GetGeometryLabelPairs(BoundingBox boundingBox)
        //{
        //    Geometry<Point> boundary = boundingBox.AsSqlGeometry(GetSrid());

        //    return GetGeometryLabelPairs(boundary);
        //}

        public override List<NamedGeometry<Point>> GetGeometryLabelPairs(Geometry<Point> geometry)
        {
            //return this._features.Where(i => i?.TheGeometry?.Intersects(geometry) == true)
            //                        .Select(f => new NamedGeometry<Point>(f.TheGeometry, _labelFunc(f))).ToList();

            return GetFeatures(geometry).Select(f => new NamedGeometry<Point>(f.TheGeometry, _labelFunc(f))).ToList();
        }

        public override void Add(IGeometryAware<Point> newGeometry)
        {
            this._features.Add(newGeometry as T);
        }

        public override void Remove(IGeometryAware<Point> geometry)
        {
            this._features.Remove(geometry as T);
        }

        public override void Update(IGeometryAware<Point> newGeometry)
        {
            if (_idFunc == null)
            {
                return;
            }

            var geometry = _idFunc(newGeometry.Id);

            var index = this._features.IndexOf(geometry);

            //var index = newGeometry.Id;

            //if (index < 0)
            //{
            //    return;
            //}

            this._features[index] = newGeometry as T;
        }

        public override void UpdateFeature(IGeometryAware<Point> newGeometry)
        {
            if (_idFunc == null)
            {
                return;
            }

            var geometry = _idFunc(newGeometry.Id);

            var index = this._features.IndexOf(geometry);

            //var index = newGeometry.Id;

            //if (index < 0)
            //{
            //    return;
            //}

            _features[index] = newGeometry as T;
        }

        public override void SaveChanges()
        {

        }

        public override FeatureSet GetSqlFeatures()
        {
            var features = GetFeatures().Select(f => _mapToFeatureFunc(f)).ToList();

            return new FeatureSet(this.GetSrid()) { Features = features };
        }
    }
}