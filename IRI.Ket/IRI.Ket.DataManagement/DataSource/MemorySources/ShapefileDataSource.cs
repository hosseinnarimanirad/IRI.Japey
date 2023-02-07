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
using IRI.Msh.Common.Helpers;
using IRI.Ket.ShapefileFormat.Model;

namespace IRI.Ket.DataManagement.DataSource
{
    public class ShapefileDataSource<T> : MemoryDataSource<T> where T : class, IGeometryAware<Point>
    {
        string _shapefileName;

        SrsBase _targetSrs;

        SrsBase _sourceSrs;

        //public List<ShapefileFormat.Model.ObjectToDbfTypeMap<T>> AttributeMap { get; set; }

        // 1400.02.03
        //ObjectToDfbFields<T> _fields;
        List<ObjectToDbfTypeMap<T>> _fields;

        protected ShapefileDataSource()
        {
        }

        private ShapefileDataSource(string shapefileName,
                                    IEsriShapeCollection geometries,
                                    EsriAttributeDictionary attributes,
                                    Func<Geometry<Point>, Dictionary<string, object>, T> map,
                                    Func<T, List<object>> inverseAttributeMap,
                                    SrsBase targetSrs)
        {
            if (attributes == null)
            {
                throw new NotImplementedException();
            }

            this._shapefileName = shapefileName;

            _sourceSrs = IRI.Ket.ShapefileFormat.Shapefile.TryGetSrs(shapefileName);

            _targetSrs = targetSrs;

            Func<Point, Point> transformFunc = null;

            if (targetSrs != null)
            {
                transformFunc = p => p.Project(_sourceSrs, targetSrs);
            }

            //string title = System.IO.Path.GetFileNameWithoutExtension(shapefileName);

            this.Extent = geometries.MainHeader.MinimumBoundingBox;

            if (transformFunc != null)
            {
                this.Extent = this.Extent.Transform(p => transformFunc(p));
            }

            // 1400.02.03-comment
            //this._fields = new ObjectToDfbFields<T>() { ExtractAttributesFunc = inverseAttributeMap, Fields = attributes.Fields };

            // 1400.02.19
            this._fields = new List<ObjectToDbfTypeMap<T>>();

            for (int i = 0; i < attributes.Fields.Count; i++)
            {
                this._fields.Add(new ObjectToDbfTypeMap<T>(attributes.Fields[i], t => inverseAttributeMap(t)[i]));
            }


            if (geometries?.Count != attributes.Attributes?.Count)
            {
                throw new NotImplementedException();
            }

            this._features = new List<T>();

            for (int i = 0; i < geometries.Count; i++)
            {
                Geometry<Point> geometry = null;

                if (transformFunc == null)
                {
                    geometry = geometries[i].AsGeometry();//.MakeValid();
                }
                else
                {
                    //targetSrs should not be null in this case
                    geometry = geometries[i].AsGeometry()/*.MakeValid()*/.Transform(p => transformFunc(p), targetSrs!.Srid);
                }

                var feature = map(geometry, attributes.Attributes[i]);

                feature.Id = GetNewId();

                this._idFunc = id => this._features.Single(f => f.Id == id);

                this._features.Add(feature);
            }
        }


        public static ShapefileDataSource<T> Create(string shapefileName, Func<Geometry<Point>, Dictionary<string, object>, T> map, Func<T, List<object>> inverseAttributeMap, SrsBase targetSrs = null, Encoding encoding = null)
        {
            var attributes = DbfFile.Read(ShapefileFormat.Shapefile.GetDbfFileName(shapefileName), true, encoding);

            var geometries = ShapefileFormat.Shapefile.ReadShapes(shapefileName);

            return new ShapefileDataSource<T>(shapefileName, geometries, attributes, map, inverseAttributeMap, targetSrs);
        }

        public static async Task<ShapefileDataSource<T>> CreateAsync(string shapefileName, Func<Geometry<Point>, Dictionary<string, object>, T> map, Func<T, List<Object>> inverseAttributeMap, SrsBase targetSrs = null, Encoding encoding = null)
        {
            var attributes = DbfFile.Read(ShapefileFormat.Shapefile.GetDbfFileName(shapefileName), true, encoding);

            var geometries = await ShapefileFormat.Shapefile.ReadShapesAsync(shapefileName);

            return new ShapefileDataSource<T>(shapefileName, geometries, attributes, map, inverseAttributeMap, targetSrs);
        }

        public override void SaveChanges()
        {
            Func<T, IEsriShape> geometryMap = null;

            //save shp, shx, dbf, prj, cpg

            if (_targetSrs != null)
            {
                Func<Point, Point> inverseTransformFunc = p => p.Project(_targetSrs, _sourceSrs);

                geometryMap = t => t.TheGeometry.AsEsriShape(_sourceSrs.Srid, inverseTransformFunc as Func<IPoint, IPoint>);
            }
            else
            {
                geometryMap = t => t.TheGeometry.AsEsriShape(_sourceSrs.Srid);
            }

            IRI.Ket.ShapefileFormat.Shapefile.Save(_shapefileName, _features, geometryMap, _fields, EncodingHelper.ArabicEncoding, _sourceSrs, true);
        }
    }

    public static class ShapefileDataSourceFactory
    {
        static Func<Geometry<Point>, Dictionary<string, object>, Feature<Point>> mapShapeToSqlFeature = (geometry, attributes) => new Feature<Point>(geometry, attributes);

        static List<Func<Feature<Point>, object>> CreateInverseAttributeMap(Feature<Point> feature)
        {
            List<Func<Feature<Point>, object>> result = new List<Func<Feature<Point>, object>>();

            foreach (var item in feature.Attributes)
            {
                result.Add(d => d.Attributes[item.Key]);
            }

            return result;
        }


        public static ShapefileDataSource<Feature<Point>> Create(string shapefileName, SrsBase targetCrs, Encoding encoding = null)
        {
            Func<Feature<Point>, List<object>> inverseAttributeMap = feature => feature.Attributes.Select(kvp => kvp.Value).ToList();

            var result = ShapefileDataSource<Feature<Point>>.Create(shapefileName, mapShapeToSqlFeature, inverseAttributeMap, targetCrs, encoding);

            result.ToDataTableMappingFunc = ToDataTableDefaultMappings.SqlFeatureTypeMapping;

            return result;
        }


        public static async Task<ShapefileDataSource<Feature<Point>>> CreateAsync(string shapefileName, SrsBase targetCrs, Encoding encoding = null)
        {
            Func<Feature<Point>, List<object>> inverseMap = feature => feature.Attributes.Select(kvp => kvp.Value).ToList();

            var result = await ShapefileDataSource<Feature<Point>>.CreateAsync(shapefileName, mapShapeToSqlFeature, inverseMap, targetCrs, encoding);

            result.ToDataTableMappingFunc = ToDataTableDefaultMappings.SqlFeatureTypeMapping;

            return result;
        }

    }
}
