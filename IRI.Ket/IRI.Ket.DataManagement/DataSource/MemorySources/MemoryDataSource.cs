﻿using System;
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

        //public override List<object> GetAttributes(string attributeColumn, string whereClause)
        //{
        //    throw new NotImplementedException();
        //}

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

        public override void Add(ISqlGeometryAware newValue)
        {
            _geometries.Add(newValue.TheSqlGeometry);
        }

        public override void Remove(ISqlGeometryAware value)
        {
            _geometries.Remove(value.TheSqlGeometry);
        }

        //ERROR PRONE: valueId is used as index
        public override void Update(ISqlGeometryAware newValue)
        {
            _geometries[newValue.Id] = newValue.TheSqlGeometry;
        }

        public override void UpdateFeature(ISqlGeometryAware newValue)
        {
            _geometries[newValue.Id] = newValue.TheSqlGeometry;
        }

        public override void SaveChanges()
        {
            throw new NotImplementedException();
        }

        public override SqlFeatureSet GetSqlFeatures()
        {
            var features = GetGeometries().Select(g => new SqlFeature(g, string.Empty) { Attributes = new Dictionary<string, object>() }).ToList();

            return new SqlFeatureSet(this.GetSrid()) { Features = features };
        }
    }

    public class MemoryDataSource<T> : FeatureDataSource<T> where T : class, ISqlGeometryAware
    {
        public override BoundingBox Extent { get; protected set; }

        protected List<T> _features;

        protected Func<T, string> _labelFunc;

        private int _uniqueId = 0;

        protected Func<int, T> _idFunc;

        protected Func<T, SqlFeature> _mapToFeatureFunc;

        public override int GetSrid()
        {
            return this._features?.SkipWhile(g => g == null || g.TheSqlGeometry == null || g.TheSqlGeometry.IsNotValidOrEmpty())?.FirstOrDefault()?.TheSqlGeometry.GetSrid() ?? 0;
        }

        public MemoryDataSource() : this(new List<SqlFeature>().Cast<T>().ToList(), null, null, null)
        {

        }

        public MemoryDataSource(List<SqlGeometry> geometries)
        {
            this._features = geometries.Select(g => new SqlFeature(g) { Id = GetNewId() }).Cast<T>().ToList();

            this._labelFunc = null;

            this._idFunc = id => this._features.Single(f => f.Id == id);

            this.Extent = GetGeometries().GetBoundingBox();
        }

        public MemoryDataSource(List<T> features, Func<T, string> labelFunc, Func<int, T> idFunc, Func<T, SqlFeature> mapToFeatureFunc)
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

        public override List<SqlGeometry> GetGeometries()
        {
            SqlGeometry geometry = null;

            return GetGeometries(geometry);
        }

        public override List<SqlGeometry> GetGeometries(SqlGeometry geometry)
        {
            //if (geometry == null)
            //{
            //    return this._features.Select(f => f.TheSqlGeometry).ToList();
            //}
            //else
            //{
            //    return this._features.Where(i => i?.TheSqlGeometry?.STIntersects(geometry).IsTrue == true).Select(f => f.TheSqlGeometry).ToList();
            //}
            return GetFeatures(geometry)?.Select(f => f.TheSqlGeometry).ToList();

        }

        //GetAttributes
        public override List<T> GetFeatures(SqlGeometry geometry)
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
                if (geometry?.STSrid.Value != this.GetSrid())
                {
                    throw new ArgumentException("srid mismatch");
                }

                return this._features.Where(i => i?.TheSqlGeometry?.STIntersects(geometry).IsTrue == true).ToList();
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
        //    SqlGeometry geometry = boundary.AsSqlGeometry(GetSrid());

        //    return GetEntireFeatures(geometry);
        //}



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
            //return this._features.Where(i => i?.TheSqlGeometry?.STIntersects(geometry).IsTrue == true)
            //                        .Select(f => new NamedSqlGeometry(f.TheSqlGeometry, _labelFunc(f))).ToList();

            return GetFeatures(geometry).Select(f => new NamedSqlGeometry(f.TheSqlGeometry, _labelFunc(f))).ToList();
        }

        public override void Add(ISqlGeometryAware newGeometry)
        {
            this._features.Add(newGeometry as T);
        }

        public override void Remove(ISqlGeometryAware geometry)
        {
            this._features.Remove(geometry as T);
        }

        public override void Update(ISqlGeometryAware newGeometry)
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

        public override void UpdateFeature(ISqlGeometryAware newGeometry)
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

        public override SqlFeatureSet GetSqlFeatures()
        {
            var features = GetFeatures().Select(f => _mapToFeatureFunc(f)).ToList();

            return new SqlFeatureSet(this.GetSrid()) { Features = features };
        }
    }
}