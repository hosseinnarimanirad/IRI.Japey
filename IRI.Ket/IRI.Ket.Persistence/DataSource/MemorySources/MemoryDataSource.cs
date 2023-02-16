using IRI.Msh.Common.Primitives;
using System.Data;
using IRI.Extensions;
using IRI.Msh.CoordinateSystem.MapProjection;
using System.Text;
using IRI.Ket.ShapefileFormat;
using IRI.Msh.Common.Mapping;

namespace IRI.Ket.DataManagement.DataSource
{
    public class MemoryDataSource : MemoryDataSource<Feature<Point>, Point>
    {
        //const string _geoColumnName = "Geo";


        public MemoryDataSource() : this(new List<Feature<Point>>(), null, null)
        {

        }

        public MemoryDataSource(List<Geometry<Point>> geometries)
        {
            this._features = geometries.Select(g => new Feature<Point>(g) { Id = GetNewId() }).ToList();

            this._labelFunc = null;

            this._idFunc = id => this._features.Single(f => f.Id == id);

            this.Extent = geometries.GetBoundingBox();
        }

        public MemoryDataSource(List<Feature<Point>> features, Func<Feature<Point>, string>? labelFunc, Func<int, Feature<Point>>? idFunc)
        {
            this._features = features;

            foreach (var item in _features)
            {
                item.Id = GetNewId();
            }

            this._labelFunc = labelFunc;

            this._idFunc = idFunc;

            this.Extent = _features.Select(f => f.TheGeometry).GetBoundingBox();

            _mapToFeatureFunc = f => f;// mapToFeatureFunc;
        }

        public static MemoryDataSource CreateFromShapefile(string shpFileName, string label, SrsBase targetSrs = null, bool correctFarsiCharacters = true, Encoding dataEncoding = null, Encoding headerEncoding = null)
        {
            //var features = ShapefileHelper.ReadAsSqlFeature(shpFileName, dataEncoding, targetSrs, headerEncoding, correctFarsiCharacters, label);
            var features = Shapefile.ReadAsFeature(shpFileName, dataEncoding, targetSrs, headerEncoding, correctFarsiCharacters, label);

            var result = new MemoryDataSource(features, f => f.Label, i => (Feature)features.Single(tt => tt.Id == i));

            //result.ToDataTableMappingFunc = sqlFeatureToDataTableMapping;

            return result;
        }

        public static async Task<MemoryDataSource> CreateFromShapefileAsync(string shpFileName, string label, Encoding dataEncoding = null, SrsBase targetSrs = null, Encoding headerEncoding = null, bool correctFarsiCharacters = true)
        {
            var features = await Shapefile.ReadAsFeatureAsync(shpFileName, dataEncoding, targetSrs, headerEncoding, correctFarsiCharacters, label);

            var result = new MemoryDataSource(features, i => i.Label, i => (Feature)features.Single(tt => tt.Id == i));

            //result.ToDataTableMappingFunc = sqlFeatureToDataTableMapping;

            return result;
        }

        public override FeatureSet<Point> GetAsFeatureSet(Geometry<Point>? geometry)
        {
            return new FeatureSet<Point>(GetGeometryAwares(geometry)
                    .Select(ToFeatureMappingFunc)
                    .ToList());
        }
        //public override List<Geometry<Point>> GetGeometries()
        //{
        //    return this._geometries;
        //}

        //public override DataTable GetEntireFeatures(Geometry<Point> geometry)
        //{
        //    DataTable result = new DataTable();

        //    result.Columns.Add(new DataColumn(_geoColumnName));

        //    IEnumerable<Geometry<Point>> geometries;

        //    if (geometry == null)
        //    {
        //        geometries = GetGeometries();
        //    }
        //    else
        //    {
        //        if (geometry?.Srid != this.GetSrid())
        //        {
        //            throw new ArgumentException("srid mismatch");
        //        }

        //        geometries = GetGeometries()?.Where(i => i.Intersects(geometry));
        //    }

        //    foreach (var item in geometries)
        //    {
        //        result.Rows.Add(item);
        //    }

        //    return result;
        //}

        //public override List<NamedGeometry> GetGeometryLabelPairs(Geometry<Point> geometry)
        //{
        //    return GetGeometries().Select(g => new NamedGeometry(g, string.Empty)).ToList();
        //}


        //public override FeatureSet GetSqlFeatures()
        //{
        //    var features = GetGeometries().Select(g => new Feature<Point>(g, string.Empty) { Attributes = new Dictionary<string, object>() }).ToList();

        //    return new FeatureSet(this.GetSrid()) { Features = features };
        //}
    }

    public class MemoryDataSource<TGeometryAware, TPoint> : VectorDataSource<TGeometryAware, TPoint>
        where TGeometryAware : class, IGeometryAware<TPoint>
        where TPoint : IPoint, new()
    {
        public override BoundingBox Extent { get; protected set; }

        protected List<TGeometryAware> _features;

        protected Func<TGeometryAware, string>? _labelFunc;

        private int _uniqueId = 0;

        protected Func<int, TGeometryAware>? _idFunc;

        protected Func<TGeometryAware, Feature<TPoint>> _mapToFeatureFunc;

        public MemoryDataSource()
        {

        }

        public MemoryDataSource(
            List<TGeometryAware> features,
            Func<TGeometryAware, string> labelingFunc,
            Func<int, TGeometryAware> idFunc,
            Func<TGeometryAware, Feature<TPoint>> mapToFeatureFunc)
        {
            this._features = features;
            this._labelFunc = labelingFunc;
            this._idFunc = idFunc;
            _mapToFeatureFunc = mapToFeatureFunc;
        }

        // todo: remove this method
        public int GetSrid()
        {
            return this._features?.SkipWhile(g => g == null || g.TheGeometry == null || g.TheGeometry.IsNotValidOrEmpty())?.FirstOrDefault()?.TheGeometry.Srid ?? 0;
        }

        public override int Srid { get => GetSrid(); protected set => _ = value; }

        protected int GetNewId()
        {
            return _uniqueId++;
        }

        //public override List<Geometry<Point>> GetGeometries()
        //{
        //    Geometry<Point> geometry = null;

        //    return GetGeometries(geometry);
        //}

        //public override List<Geometry<Point>> GetGeometries(Geometry<Point> geometry)
        //{
        //    return GetFeatures(geometry)?.Select(f => f.TheGeometry).ToList();

        //}

        //public override List<T> GetFeatures(Geometry<Point> geometry)
        //{
        //    if (this._features == null || this._features.Count == 0)
        //    {
        //        return new List<T>();
        //    }

        //    if (geometry == null)
        //    {
        //        return this._features.ToList();
        //    }
        //    else
        //    {
        //        if (geometry?.Srid != this.GetSrid())
        //        {
        //            throw new ArgumentException("srid mismatch");
        //        }

        //        return this._features.Where(i => i?.TheGeometry?.Intersects(geometry) == true).ToList();
        //    }
        //}

        //public override List<NamedGeometry> GetGeometryLabelPairs()
        //{
        //    return this._features.Select(f => new NamedGeometry(f.TheGeometry, _labelFunc(f))).ToList();
        //}

        //public override List<NamedGeometry> GetGeometryLabelPairs(Geometry<Point> geometry)
        //{
        //    //return this._features.Where(i => i?.TheGeometry?.Intersects(geometry) == true)
        //    //                        .Select(f => new NamedGeometry(f.TheGeometry, _labelFunc(f))).ToList();

        //    return GetFeatures(geometry).Select(f => new NamedGeometry(f.TheGeometry, _labelFunc(f))).ToList();
        //}


        #region CRUD

        //public override void Add(IGeometryAware<Point> newValue)
        //{
        //    Add(newValue as TGeometryAware);
        //}

        //public override void Remove(IGeometryAware<Point> newValue)
        //{
        //    Remove(newValue as TGeometryAware);
        //}

        //public override void Update(IGeometryAware<Point> newValue)
        //{
        //    Update(newValue as TGeometryAware);
        //}

        public override void Add(TGeometryAware newGeometry)
        {
            this._features.Add(newGeometry);
        }

        public override void Remove(TGeometryAware geometry)
        {
            this._features.Remove(geometry);
        }

        public override void Update(TGeometryAware newGeometry)
        {
            if (_idFunc == null)
                return;

            var geometry = _idFunc(newGeometry.Id);

            var index = this._features.IndexOf(geometry);

            //var index = newGeometry.Id;

            //if (index < 0)
            //{
            //    return;
            //}

            this._features[index] = newGeometry;
        }

        public override void SaveChanges()
        {
            return;
        }

        #endregion


        //public override void UpdateFeature(TGeometryAware newGeometry)
        //{
        //    if (_idFunc == null)
        //        return;

        //    var geometry = _idFunc(newGeometry.Id);

        //    var index = this._features.IndexOf(geometry);

        //    //var index = newGeometry.Id;

        //    //if (index < 0)
        //    //{
        //    //    return;
        //    //}

        //    _features[index] = newGeometry as T;
        //}

        public override List<TGeometryAware> GetGeometryAwares(Geometry<TPoint>? geometry)
        {
            return this._features.Where(f => f.TheGeometry.Intersects(geometry)).ToList();
        }

        public override FeatureSet<TPoint> GetFeatures()
        {
            var features = _features.Select(f => _mapToFeatureFunc(f)).ToList();

            return new FeatureSet<TPoint>(features);
        }

        protected override Feature<TPoint> ToFeatureMappingFunc(TGeometryAware geometryAware)
        {
            return _mapToFeatureFunc(geometryAware);
        }

        public override FeatureSet<Point> GetAsFeatureSet(Geometry<Point>? geometry)
        {
            return new FeatureSet<Point>(GetGeometryAwares(geometry.NeutralizeGenericPoint<TPoint>())
                    .Select(ToFeatureMappingFunc)
                    .Select(g => new Feature<Point>(g.TheGeometry.NeutralizeGenericPoint<Point>())
                    {
                        LabelAttribute = g.LabelAttribute,
                        Id = g.Id,
                        Attributes = g.Attributes
                    })
                    .ToList());
        }
    }
}