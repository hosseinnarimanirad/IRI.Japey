using IRI.Ket.ShapefileFormat.Dbf;
using IRI.Ket.ShapefileFormat.EsriType;

using IRI.Ket.SqlServerSpatialExtension;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Types;
using IRI.Msh.Common.Primitives;
using System.Data;
using IRI.Ket.DataManagement.Model;
using IRI.Ket.SqlServerSpatialExtension.Model;

using IRI.Msh.CoordinateSystem.MapProjection;
using IRI.Ket.SpatialExtensions;
using IRI.Msh.Common.Extensions;
using IRI.Ket.SqlServerSpatialExtension.Helpers;

namespace IRI.Ket.DataManagement.DataSource
{
    public static class ShapefileDataSource
    {
        static Func<SqlGeometry, Dictionary<string, object>, SqlFeature> mapShapeToSqlFeature = (geometry, attributes) => new SqlFeature(geometry, attributes);

        public static ShapefileDataSource<SqlFeature> Create(string shapefileName, int srid, Encoding encoding, SrsBase targetCrs)
        {
            //var sourcePrj = IRI.Ket.ShapefileFormat.Shapefile.GetPrjFileName(shapefileName);

            //if (!System.IO.File.Exists(sourcePrj))
            //{
            //    throw new System.IO.FileNotFoundException($"prj file not found. {sourcePrj}");
            //}

            //var sourceCrs = new ShapefileFormat.Prj.EsriPrjFile(sourcePrj).AsMapProjection();

            //Func<Point, Point> func = null;

            //if (sourceCrs.Ellipsoid.AreTheSame(targetCrs.Ellipsoid))
            //{
            //    func = new Func<Point, Point>(p => (Point)targetCrs.FromGeodetic(sourceCrs.ToGeodetic(p)));
            //}
            //else
            //{
            //    func = new Func<Point, Point>(p => (Point)targetCrs.FromGeodetic(sourceCrs.ToGeodetic(p), sourceCrs.Ellipsoid));
            //}


            var sourceSrs = IRI.Ket.ShapefileFormat.Shapefile.TryGetSrs(shapefileName);

            Func<IPoint, IPoint> func = p => p.Project(sourceSrs, targetCrs);

            // IRI.Ket.ShapefileFormat.Shapefile.GetProjectFunc(shapefileName, targetCrs);

            return new ShapefileDataSource<SqlFeature>(shapefileName, srid, encoding, mapShapeToSqlFeature, func);
        }

        public static SqlFeatureDataSource Create(string shpFileName, Encoding dataEncoding, Encoding headerEncoding, bool correctFarsiCharacters, string label, SrsBase targetSrs = null)
        {
            var features = ShapefileHelper.ReadShapefile(shpFileName, dataEncoding, headerEncoding, correctFarsiCharacters, label, targetSrs);

            var result = new SqlFeatureDataSource(features, f => f.Label);

            //result.ToDataTableMappingFunc = sqlFeatureToDataTableMapping;

            return result;
        }

        public static async Task<SqlFeatureDataSource> CreateAsync(string shpFileName, Encoding dataEncoding, Encoding headerEncoding, bool correctFarsiCharacters, string label, SrsBase targetSrs = null)
        {
            var features = await ShapefileHelper.ReadShapefileAsync(shpFileName, dataEncoding, headerEncoding, correctFarsiCharacters, label, targetSrs);

            var result = new SqlFeatureDataSource(features, i => i.Label);

            //result.ToDataTableMappingFunc = sqlFeatureToDataTableMapping;

            return result;
        }
    }

    public class ShapefileDataSource<T> : MemoryDataSource<T> where T : class, ISqlGeometryAware
    {
        string _shapefileName;

        protected ShapefileDataSource()
        {
        }

        public ShapefileDataSource(string shapefileName, int srid, Encoding encoding, Func<SqlGeometry, Dictionary<string, object>, T> map, Func<IPoint, IPoint> transformFunc = null)
        {
            this._shapefileName = shapefileName;

            string title = System.IO.Path.GetFileNameWithoutExtension(shapefileName);

            var attributes = DbfFile.Read(ShapefileFormat.Shapefile.GetDbfFileName(shapefileName), encoding, encoding, true);

            var geometries = ShapefileFormat.Shapefile.ReadShapes(shapefileName);

            this.Extent = geometries.MainHeader.MinimumBoundingBox;

            if (transformFunc != null)
            {
                this.Extent = this.Extent.Transform(p => (Point)transformFunc(p));
            }

            if (geometries?.Count != attributes?.Count)
            {
                throw new NotImplementedException();
            }

            this._features = new List<T>();

            for (int i = 0; i < geometries.Count; i++)
            {
                SqlGeometry geometry = null;

                if (transformFunc == null)
                {
                    geometry = geometries[i].AsSqlGeometry().MakeValid();
                }
                else
                {
                    geometry = geometries[i].AsSqlGeometry().MakeValid().Transform(p => transformFunc(p), srid);
                }

                var feature = map(geometry, attributes[i]);

                feature.Id = GetNewId();

                this._features.Add(feature);
            }
        }
    }
}

//public class ShapefileDataSource<T> : FeatureDataSource<T> where T : ISqlGeometryAware
//{
//    public override BoundingBox Extent { get; protected set; }

//    string _shapefileName, _spatialColumnName, _labelColumnName;

//    int _srid;

//    //DataTable _table;

//    List<T> values;

//    bool _isGeometry;

//    public override int GetSrid()
//    {
//        return _srid;
//    }

//    protected ShapefileDataSource()
//    {
//    }

//    public ShapefileDataSource(string shapefileName, string spatialColumnName, int srid, bool isGeometry = true, string labelColumnName = null)
//        : this(shapefileName, spatialColumnName, srid, DbfFile._arabicEncoding, isGeometry, labelColumnName)
//    {

//    }


//    public ShapefileDataSource(string shapefileName, string spatialColumnName, int srid, Encoding encoding, bool isGeometry = true, string labelColumnName = null, Func<Point, Point> mapFunc = null)
//    {
//        this._isGeometry = isGeometry;

//        this._shapefileName = shapefileName;

//        this._spatialColumnName = spatialColumnName;

//        this._labelColumnName = labelColumnName;

//        this._srid = srid;

//        string title = System.IO.Path.GetFileNameWithoutExtension(shapefileName);

//        var attributes = DbfFile.Read(ShapefileFormat.Shapefile.GetDbfFileName(shapefileName), encoding, encoding, true);

//        //_table = DbfFile.Read(
//        //    ShapefileFormat.Shapefile.GetDbfFileName(shapefileName), title, encoding, encoding, true);

//        //DataColumn geoColumn = new DataColumn(spatialColumnName, isGeometry ? typeof(SqlGeometry) : typeof(SqlGeography));

//        //_table.Columns.Add(geoColumn);

//        var geometries = ShapefileFormat.Shapefile.ReadShapes(shapefileName);

//        this.Extent = geometries.MainHeader.MinimumBoundingBox;

//        if (mapFunc != null)
//        {
//            this.Extent = this.Extent.Transform(mapFunc);
//        }

//        if (geometries?.Count != attributes?.Count)
//        {
//            throw new NotImplementedException();
//        }

//        var invalidFeatures = new List<DataRow>();

//        //if (isGeometry)
//        //{
//        for (int i = 0; i < geometries.Count; i++)
//        {
//            if (mapFunc == null)
//            {
//                _table.Rows[i][geoColumn] = SqlGeometry.STGeomFromWKB(new System.Data.SqlTypes.SqlBytes(geometries[i].AsWkb()), srid).MakeValid();
//            }
//            else
//            {
//                _table.Rows[i][geoColumn] = SqlGeometry.STGeomFromWKB(new System.Data.SqlTypes.SqlBytes(geometries[i].AsWkb()), srid).MakeValid().Transform(p => mapFunc((Point)p), srid);
//            }
//        }
//        //}
//        //else
//        //{
//        //    if (mapFunc != null)
//        //    {
//        //        throw new NotImplementedException();
//        //    }

//        //    for (int i = 0; i < geometries.Count; i++)
//        //    {
//        //        try
//        //        {
//        //            _table.Rows[i][geoColumn] = SqlGeography.STGeomFromWKB(new System.Data.SqlTypes.SqlBytes(geometries[i].AsWkb()), srid);
//        //        }
//        //        catch (Exception)
//        //        {
//        //            invalidFeatures.Add(_table.Rows[i]);

//        //            continue;
//        //        }
//        //    }

//        //    foreach (var item in invalidFeatures)
//        //    {
//        //        _table.Rows.Remove(item);
//        //    }
//        //}
//    }

//    //public ShapefileDataSource(string shapefileName, string spatialColumnName, int srid, Encoding encoding, bool isGeometry = true, string labelColumnName = null, Func<Point, Point> mapFunc = null)
//    //{
//    //    this._isGeometry = isGeometry;

//    //    this._fileName = shapefileName;

//    //    this._spatialColumnName = spatialColumnName;

//    //    this._labelColumnName = labelColumnName;

//    //    this._srid = srid;

//    //    string tableName = System.IO.Path.GetFileNameWithoutExtension(shapefileName);

//    //    _table = DbfFile.Read(
//    //        ShapefileFormat.Shapefile.GetDbfFileName(shapefileName), tableName, encoding, encoding, true);

//    //    DataColumn geoColumn = new DataColumn(spatialColumnName, isGeometry ? typeof(SqlGeometry) : typeof(SqlGeography));

//    //    _table.Columns.Add(geoColumn);

//    //    var geometries = ShapefileFormat.Shapefile.ReadShapes(shapefileName);

//    //    this.Extent = geometries.MainHeader.MinimumBoundingBox;

//    //    if (mapFunc != null)
//    //    {
//    //        this.Extent = this.Extent.Transform(mapFunc);
//    //    }

//    //    if (geometries.Count != _table.Rows.Count)
//    //    {
//    //        throw new NotImplementedException();
//    //    }

//    //    var invalidFeatures = new List<DataRow>();

//    //    if (isGeometry)
//    //    {
//    //        for (int i = 0; i < geometries.Count; i++)
//    //        {
//    //            if (mapFunc == null)
//    //            {
//    //                _table.Rows[i][geoColumn] = SqlGeometry.STGeomFromWKB(new System.Data.SqlTypes.SqlBytes(geometries[i].AsWkb()), srid).MakeValid();
//    //            }
//    //            else
//    //            {
//    //                _table.Rows[i][geoColumn] = SqlGeometry.STGeomFromWKB(new System.Data.SqlTypes.SqlBytes(geometries[i].AsWkb()), srid).MakeValid().Transform(p => mapFunc((Point)p), srid);
//    //            }
//    //        }
//    //    }
//    //    else
//    //    {
//    //        if (mapFunc != null)
//    //        {
//    //            throw new NotImplementedException();
//    //        }

//    //        for (int i = 0; i < geometries.Count; i++)
//    //        {
//    //            try
//    //            {
//    //                _table.Rows[i][geoColumn] = SqlGeography.STGeomFromWKB(new System.Data.SqlTypes.SqlBytes(geometries[i].AsWkb()), srid);
//    //            }
//    //            catch (Exception)
//    //            {
//    //                invalidFeatures.Add(_table.Rows[i]);

//    //                continue;
//    //            }
//    //        }

//    //        foreach (var item in invalidFeatures)
//    //        {
//    //            _table.Rows.Remove(item);
//    //        }
//    //    }
//    //}

//    public static Task<ShapefileDataSource<T>> Create(string shpFileName, int srid, Encoding encoding, Func<Dictionary<string, object>, SqlGeometry, T> map)
//    { 
//        string title = System.IO.Path.GetFileNameWithoutExtension(shapefileName);

//        var attributes = DbfFile.Read(ShapefileFormat.Shapefile.GetDbfFileName(shapefileName), encoding, encoding, true);

//        //_table = DbfFile.Read(
//        //    ShapefileFormat.Shapefile.GetDbfFileName(shapefileName), title, encoding, encoding, true);

//        //DataColumn geoColumn = new DataColumn(spatialColumnName, isGeometry ? typeof(SqlGeometry) : typeof(SqlGeography));

//        //_table.Columns.Add(geoColumn);

//        var geometries = ShapefileFormat.Shapefile.ReadShapes(shapefileName);

//        this.Extent = geometries.MainHeader.MinimumBoundingBox;

//        if (mapFunc != null)
//        {
//            this.Extent = this.Extent.Transform(mapFunc);
//        }

//        if (geometries?.Count != attributes?.Count)
//        {
//            throw new NotImplementedException();
//        }

//        var invalidFeatures = new List<DataRow>();

//        //if (isGeometry)
//        //{
//        for (int i = 0; i < geometries.Count; i++)
//        {
//            if (mapFunc == null)
//            {
//                _table.Rows[i][geoColumn] = SqlGeometry.STGeomFromWKB(new System.Data.SqlTypes.SqlBytes(geometries[i].AsWkb()), srid).MakeValid();
//            }
//            else
//            {
//                _table.Rows[i][geoColumn] = SqlGeometry.STGeomFromWKB(new System.Data.SqlTypes.SqlBytes(geometries[i].AsWkb()), srid).MakeValid().Transform(p => mapFunc((Point)p), srid);
//            }
//        }
//    }

//    public static Task<ShapefileDataSource<SqlFeature>> Create(string shapefileName, string spatialColumnName, int srid, Encoding encoding, SrsBase targetCrs, string labelColumnName = null)
//    {
//        return Task.Run<ShapefileDataSource<SqlFeature>>(() =>
//        {
//            var sourcePrj = IRI.Ket.ShapefileFormat.Shapefile.GetPrjFileName(shapefileName);

//            if (!System.IO.File.Exists(sourcePrj))
//            {
//                throw new System.IO.FileNotFoundException($"prj file not found. {sourcePrj}");
//            }

//            var sourceCrs = new ShapefileFormat.Prj.EsriPrjFile(sourcePrj).AsMapProjection();

//            Func<Point, Point> func = null;

//            if (sourceCrs.Ellipsoid.AreTheSame(targetCrs.Ellipsoid))
//            {
//                func = new Func<Point, Point>(p => (Point)targetCrs.FromGeodetic(sourceCrs.ToGeodetic(p)));
//            }
//            else
//            {
//                func = new Func<Point, Point>(p => (Point)targetCrs.FromGeodetic(sourceCrs.ToGeodetic(p), sourceCrs.Ellipsoid));
//            }

//            return new ShapefileDataSource<SqlFeature>(shapefileName, spatialColumnName, srid, encoding, true, labelColumnName, func);
//        });
//    }

//    public override List<SqlGeometry> GetGeometries()
//    {
//        //return this._table.Select().Select(g => g[_spatialColumnName]).Cast<SqlGeometry>().ToList();
//        return this.values.Select(v => v.TheSqlGeometry).ToList();
//    }

//    public override List<SqlGeometry> GetGeometries(string filterExpression)
//    {
//        throw new NotImplementedException();
//        //return this._table.Select(filterExpression)
//        //                    .Select(i => i[_spatialColumnName])
//        //                    .Cast<SqlGeometry>()
//        //                    .ToList();
//    }

//    public override List<SqlGeometry> GetGeometries(SqlGeometry geometry)
//    {
//        if (geometry?.IsNullOrEmpty() == true)
//        {
//            return GetGeometries();
//        }
//        else
//        {
//            //return this._table.Select()
//            //                .Select(i => i[_spatialColumnName])
//            //                .Cast<SqlGeometry>()
//            //                .Where(i => !i.IsNotValidOrEmpty() && geometry.STIntersects(i.MakeValid()).IsTrue)
//            //                .ToList();
//            return this.values.Where(g => g.TheSqlGeometry != null && !g.TheSqlGeometry.IsNotValidOrEmpty() && geometry.STIntersects(g.TheSqlGeometry).IsTrue)
//                                .Select(g => g.TheSqlGeometry).ToList();
//        }
//    }

//    public override List<object> GetAttributes(string attributeColumn)
//    {
//        throw new NotImplementedException();

//        //int count = _table.Rows.Count;

//        //List<object> result = new List<object>(count);

//        //for (int i = 0; i < count; i++)
//        //{
//        //    result.Add(_table.Rows[i][attributeColumn]);
//        //}

//        //return result;
//    }

//    public override List<object> GetAttributes(string attributeColumn, string filterExpression)
//    {
//        throw new NotImplementedException();

//        //return this._table.Select(filterExpression)
//        //                   .Select(i => i[attributeColumn]).OfType<object>().ToList();
//    }

//    //public List<string> GetLabels()
//    //{
//    //    return this._table.Select()
//    //                        .Select(i => i[_labelColumnName].ToString())
//    //                        .ToList();
//    //}

//    public override List<NamedSqlGeometry> GetGeometryLabelPairs(SqlGeometry geometry)
//    {
//        if (string.IsNullOrEmpty(_labelColumnName) && _table.Columns.Contains(_labelColumnName))
//            return new List<NamedSqlGeometry>();

//        if (this._labelColumnName == null)
//        {
//            return this._table.Select()
//                               .Select(i => new NamedSqlGeometry((SqlGeometry)i[_spatialColumnName], string.Empty))
//                               .Where(i => !i.Geometry.IsNull && geometry.STIntersects(i.Geometry.MakeValid()).IsTrue)
//                               .ToList();
//        }
//        else
//        {
//            return this._table.Select()
//                                  .Select(i => new NamedSqlGeometry((SqlGeometry)i[_spatialColumnName], i[_labelColumnName]?.ToString()))
//                                  .Where(i => !i.Geometry.IsNull && geometry.STIntersects(i.Geometry.MakeValid()).IsTrue)
//                                  .ToList();
//        }
//    }

//    public override List<NamedSqlGeometry> GetGeometryLabelPairs()
//    {
//        if (string.IsNullOrEmpty(_labelColumnName) && _table.Columns.Contains(_labelColumnName))
//            return new List<NamedSqlGeometry>();

//        if (this._labelColumnName == null)
//        {
//            return this._table.Select()
//                               .Select(i => new NamedSqlGeometry((SqlGeometry)i[_spatialColumnName], string.Empty))
//                               .Where(i => !i.Geometry.IsNull)
//                               .ToList();
//        }
//        else
//        {
//            return this._table.Select()
//                                  .Select(i => new NamedSqlGeometry((SqlGeometry)i[_spatialColumnName], i[_labelColumnName]?.ToString()))
//                                  .Where(i => !i.Geometry.IsNull)
//                                  .ToList();
//        }
//    }

//    public override List<NamedSqlGeometry> GetGeometryLabelPairs(string whereClause)
//    {
//        throw new NotImplementedException();

//        //if (this._labelColumnName == null)
//        //{
//        //    return this._table.Select(whereClause)
//        //                       .Select(i => new NamedSqlGeometry((SqlGeometry)i[_spatialColumnName], string.Empty))
//        //                       .Where(i => !i.Geometry.IsNull)
//        //                       .ToList();
//        //}
//        //else
//        //{
//        //    return this._table.Select(whereClause)
//        //                          .Select(i => new NamedSqlGeometry((SqlGeometry)i[_spatialColumnName], i[_labelColumnName]?.ToString()))
//        //                          .Where(i => !i.Geometry.IsNull)
//        //                          .ToList();
//        //}

//    }

//    #region GetEntireFeatures

//    public override DataTable GetEntireFeatures()
//    {
//        return this._table;
//    }

//    public override DataTable GetEntireFeatures(string filterExpression)
//    {
//        return this._table.Select(filterExpression).CopyToDataTable();
//    }

//    public override DataTable GetEntireFeatures(SqlGeometry geometry)
//    {
//        var result = _table.Select().Where(i => (i[_spatialColumnName] as SqlGeometry).STIntersects(geometry).IsTrue);

//        if (result == null || result.Count() == 0)
//        {
//            return null;
//        }
//        else
//        {
//            return result.CopyToDataTable();
//        }
//    }

//    //public override DataTable GetEntireFeatures(BoundingBox boundingBox)
//    //{
//    //    var bound = boundingBox.AsSqlGeometry(_srid).MakeValid();

//    //    var result = _table.Select().Where(i => (i[_spatialColumnName] as SqlGeometry).STIntersects(bound).IsTrue);

//    //    if (result == null || result.Count() == 0)
//    //    {
//    //        return null;
//    //    }
//    //    else
//    //    {
//    //        return result.CopyToDataTable();
//    //    }
//    //}

//    #endregion


//    public override List<T> GetFeatures(SqlGeometry geometry)
//    {
//        throw new NotImplementedException();
//    }

//    public override void Add(T newGeometry)
//    {
//        throw new NotImplementedException();
//    }

//    public override void Remove(int geometryId)
//    {
//        throw new NotImplementedException();
//    }

//    public override void Update(T newGeometry, int geometryId)
//    {
//        throw new NotImplementedException();
//    }

//}
