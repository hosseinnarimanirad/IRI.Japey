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

namespace IRI.Ket.DataManagement.DataSource
{
    public class ShapefileDataSource<T> : IFeatureDataSource
    {
        public override BoundingBox Extent { get; protected set; }

        string _fileName, _spatialColumnName, _labelColumnName;

        int _srid;

        DataTable _table;

        bool isGeometry;

        public override int GetSrid()
        {
            return _srid;
        }

        public ShapefileDataSource(string shapeFileName, string spatialColumnName, int srid, bool isGeometry = true, string labelColumnName = null)
            : this(shapeFileName, spatialColumnName, srid, DbfFile.ArabicEncoding, isGeometry, labelColumnName)
        {

        }

        public ShapefileDataSource(string shapeFileName, string spatialColumnName, int srid, Encoding encoding, bool isGeometry = true, string labelColumnName = null, Func<Point, Point> mapFunc = null)
        {
            this.isGeometry = isGeometry;

            this._fileName = shapeFileName;

            this._spatialColumnName = spatialColumnName;

            this._labelColumnName = labelColumnName;

            this._srid = srid;

            string tableName = System.IO.Path.GetFileNameWithoutExtension(shapeFileName);

            _table = DbfFile.Read(
                ShapefileFormat.Shapefile.GetDbfFileName(shapeFileName), tableName, encoding, encoding, true);

            DataColumn geoColumn = new DataColumn(spatialColumnName, isGeometry ? typeof(SqlGeometry) : typeof(SqlGeography));

            _table.Columns.Add(geoColumn);

            var geometries = ShapefileFormat.Shapefile.ReadShapes(shapeFileName);

            this.Extent = geometries.MainHeader.MinimumBoundingBox;

            if (mapFunc != null)
            {
                this.Extent = this.Extent.Transform(mapFunc);
            }

            if (geometries.Count != _table.Rows.Count)
            {
                throw new NotImplementedException();
            }

            var invalidFeatures = new List<DataRow>();

            if (isGeometry)
            {
                for (int i = 0; i < geometries.Count; i++)
                {
                    if (mapFunc == null)
                    {
                        _table.Rows[i][geoColumn] = SqlGeometry.STGeomFromWKB(new System.Data.SqlTypes.SqlBytes(geometries[i].AsWkb()), srid).MakeValid();
                    }
                    else
                    {
                        _table.Rows[i][geoColumn] = SqlGeometry.STGeomFromWKB(new System.Data.SqlTypes.SqlBytes(geometries[i].AsWkb()), srid).MakeValid().Transform(p => mapFunc((Point)p), srid);
                    }
                }
            }
            else
            {
                if (mapFunc != null)
                {
                    throw new NotImplementedException();
                }

                for (int i = 0; i < geometries.Count; i++)
                {
                    try
                    {
                        _table.Rows[i][geoColumn] = SqlGeography.STGeomFromWKB(new System.Data.SqlTypes.SqlBytes(geometries[i].AsWkb()), srid);
                    }
                    catch (Exception)
                    {
                        invalidFeatures.Add(_table.Rows[i]);

                        continue;
                    }
                }

                foreach (var item in invalidFeatures)
                {
                    _table.Rows.Remove(item);
                }
            }
        }

        public static Task<ShapefileDataSource<object>> Create(string shapeFileName, string spatialColumnName, int srid, Encoding encoding, CrsBase targetCrs, string labelColumnName = null)
        {
            return Task.Run<ShapefileDataSource<object>>(() =>
            {
                var sourcePrj = IRI.Ket.ShapefileFormat.Shapefile.GetPrjFileName(shapeFileName);

                if (!System.IO.File.Exists(sourcePrj))
                {
                    throw new System.IO.FileNotFoundException($"prj file not found. {sourcePrj}");
                }

                var sourceCrs = new ShapefileFormat.Prj.EsriPrjFile(sourcePrj).AsMapProjection();

                Func<Point, Point> func = null;

                if (sourceCrs.Ellipsoid.AreTheSame(targetCrs.Ellipsoid))
                {
                    func = new Func<Point, Point>(p => (Point)targetCrs.FromGeodetic(sourceCrs.ToGeodetic(p)));
                }
                else
                {
                    func = new Func<Point, Point>(p => (Point)targetCrs.FromGeodetic(sourceCrs.ToGeodetic(p), sourceCrs.Ellipsoid));
                }

                return new ShapefileDataSource<object>(shapeFileName, spatialColumnName, srid, encoding, true, labelColumnName, func);
            });
        }

        public override List<SqlGeometry> GetGeometries()
        {
            //return
            //    IRI.Ket.ShapefileFormat.Shapefile.Read(this._fileName)
            //        .Select(i => i.ToSqlGeometry(_srid)).ToList();
            return this._table.Select()
                                 .Select(i => i[_spatialColumnName])
                                 .Cast<SqlGeometry>()
                                 .ToList();
        }

        public override List<SqlGeometry> GetGeometries(string filterExpression)
        {
            return this._table.Select(filterExpression)
                                .Select(i => i[_spatialColumnName])
                                .Cast<SqlGeometry>()
                                .ToList();
        }

        //POTENTIALLY ERROR PRONOUN: Check SqlGeometry special cases: IsNull, ...
        public override List<SqlGeometry> GetGeometries(BoundingBox boundingBox)
        {
            var bound = boundingBox.AsSqlGeometry(_srid).MakeValid();

            return this._table.Select()
                                  .Select(i => i[_spatialColumnName])
                                  .Cast<SqlGeometry>()
                                  .Where(i => !i.IsNotValidOrEmpty() && bound.STIntersects(i.MakeValid()).IsTrue)
                                  .ToList();
        }

        //public override Task<List<SqlGeometry>> GetGeometriesAsync(BoundingBox boundingBox)
        //{
        //    return Task.Run<List<SqlGeometry>>(() => { return GetGeometries(boundingBox); });
        //}

        public override List<object> GetAttributes(string attributeColumn)
        {
            int count = _table.Rows.Count;

            List<object> result = new List<object>(count);

            for (int i = 0; i < count; i++)
            {
                result.Add(_table.Rows[i][attributeColumn]);
            }

            return result;
        }

        public override List<object> GetAttributes(string attributeColumn, string filterExpression)
        {
            return this._table.Select(filterExpression)
                               .Select(i => i[attributeColumn]).OfType<object>().ToList();
        }

        public override DataTable GetEntireFeatures(string filterExpression)
        {
            return this._table.Select(filterExpression).CopyToDataTable();
        }

        public List<string> GetLabels()
        {
            return this._table.Select()
                                .Select(i => i[_labelColumnName].ToString())
                                .ToList();
        }

        public override List<NamedSqlGeometry> GetGeometryLabelPairs(BoundingBox boundingBox)
        {
            var bound = boundingBox.AsSqlGeometry(_srid).MakeValid();

            return GetGeometryLabelPairs(bound);
        }

        public override List<NamedSqlGeometry> GetGeometryLabelPairs(SqlGeometry geometry)
        {
            if (string.IsNullOrEmpty(_labelColumnName) && _table.Columns.Contains(_labelColumnName))
                return new List<NamedSqlGeometry>();

            if (this._labelColumnName == null)
            {
                return this._table.Select()
                                   .Select(i => new NamedSqlGeometry((SqlGeometry)i[_spatialColumnName], string.Empty))
                                   .Where(i => !i.Geometry.IsNull && geometry.STIntersects(i.Geometry.MakeValid()).IsTrue)
                                   .ToList();
            }
            else
            {
                return this._table.Select()
                                      .Select(i => new NamedSqlGeometry((SqlGeometry)i[_spatialColumnName], i[_labelColumnName]?.ToString()))
                                      .Where(i => !i.Geometry.IsNull && geometry.STIntersects(i.Geometry.MakeValid()).IsTrue)
                                      .ToList();
            }
        }

        public override DataTable GetEntireFeatures(SqlGeometry geometry)
        {
            var result = _table.Select().Where(i => (i[_spatialColumnName] as SqlGeometry).STIntersects(geometry).IsTrue);

            if (result == null || result.Count() == 0)
            {
                return null;
            }
            else
            {
                return result.CopyToDataTable();
            }
        }

        public override List<SqlGeometry> GetGeometries(SqlGeometry geometry)
        {
            return this._table.Select()
                                .Select(i => i[_spatialColumnName])
                                .Cast<SqlGeometry>()
                                .Where(i => !i.IsNotValidOrEmpty() && geometry.STIntersects(i.MakeValid()).IsTrue)
                                .ToList();
        }

        public override List<NamedSqlGeometry> GetGeometryLabelPairs()
        {
            if (string.IsNullOrEmpty(_labelColumnName) && _table.Columns.Contains(_labelColumnName))
                return new List<NamedSqlGeometry>();

            if (this._labelColumnName == null)
            {
                return this._table.Select()
                                   .Select(i => new NamedSqlGeometry((SqlGeometry)i[_spatialColumnName], string.Empty))
                                   .Where(i => !i.Geometry.IsNull)
                                   .ToList();
            }
            else
            {
                return this._table.Select()
                                      .Select(i => new NamedSqlGeometry((SqlGeometry)i[_spatialColumnName], i[_labelColumnName]?.ToString()))
                                      .Where(i => !i.Geometry.IsNull)
                                      .ToList();
            }
        }

        public override List<NamedSqlGeometry> GetGeometryLabelPairs(string whereClause)
        {
            if (this._labelColumnName == null)
            {
                return this._table.Select(whereClause)
                                   .Select(i => new NamedSqlGeometry((SqlGeometry)i[_spatialColumnName], string.Empty))
                                   .Where(i => !i.Geometry.IsNull)
                                   .ToList();
            }
            else
            {
                return this._table.Select(whereClause)
                                      .Select(i => new NamedSqlGeometry((SqlGeometry)i[_spatialColumnName], i[_labelColumnName]?.ToString()))
                                      .Where(i => !i.Geometry.IsNull)
                                      .ToList();
            }

        }

        public override DataTable GetEntireFeatures(BoundingBox boundingBox)
        {
            var bound = boundingBox.AsSqlGeometry(_srid).MakeValid();

            var result = _table.Select().Where(i => (i[_spatialColumnName] as SqlGeometry).STIntersects(bound).IsTrue);

            if (result == null || result.Count() == 0)
            {
                return null;
            }
            else
            {
                return result.CopyToDataTable();
            }
        }
    }
}
