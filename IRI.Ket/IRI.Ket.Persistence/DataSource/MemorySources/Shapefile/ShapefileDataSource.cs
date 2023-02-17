using IRI.Sta.ShapefileFormat.Dbf;
using IRI.Sta.ShapefileFormat.EsriType;
using System.Text;
using IRI.Msh.Common.Primitives;
using System.Data;

using IRI.Msh.CoordinateSystem.MapProjection;
using IRI.Extensions;
using IRI.Msh.Common.Helpers;
using IRI.Sta.ShapefileFormat.Model;

namespace IRI.Ket.DataManagement.DataSource
{
    public class ShapefileDataSource<TGeometryAware> : MemoryDataSource<TGeometryAware, Point> where TGeometryAware : class, IGeometryAware<Point>
    {
        string _shapefileName;

        SrsBase _targetSrs;

        SrsBase _sourceSrs;

        //public List<ShapefileFormat.Model.ObjectToDbfTypeMap<T>> AttributeMap { get; set; }

        public override GeometryType? GeometryType { get; protected set; }

        // 1400.02.03
        //ObjectToDfbFields<T> _fields;
        List<ObjectToDbfTypeMap<TGeometryAware>> _fields;

        protected ShapefileDataSource()
        {
        }

        private ShapefileDataSource(string shapefileName,
                                    IEsriShapeCollection geometries,
                                    EsriAttributeDictionary attributes,
                                    Func<Geometry<Point>, Dictionary<string, object>, TGeometryAware> map,
                                    Func<TGeometryAware, List<object>> inverseAttributeMap,
                                    SrsBase targetSrs)
        {
            if (attributes == null)
            {
                throw new NotImplementedException();
            }

            this._shapefileName = shapefileName;

            _sourceSrs = IRI.Sta.ShapefileFormat.Shapefile.TryGetSrs(shapefileName);

            _targetSrs = targetSrs;

            Func<Point, Point> transformFunc = null;

            if (targetSrs != null)
            {
                transformFunc = p => p.Project(_sourceSrs, targetSrs);
            }

            //string title = System.IO.Path.GetFileNameWithoutExtension(shapefileName);

            this.Extent = geometries.MainHeader.MinimumBoundingBox;

            // 1401.11.28
            this.GeometryType = geometries.MainHeader.ShapeType.AsGeometryType();

            if (transformFunc != null)
            {
                this.Extent = this.Extent.Transform(p => transformFunc(p));
            }

            // 1400.02.03-comment
            //this._fields = new ObjectToDfbFields<T>() { ExtractAttributesFunc = inverseAttributeMap, Fields = attributes.Fields };

            // 1400.02.19
            this._fields = new List<ObjectToDbfTypeMap<TGeometryAware>>();

            for (int i = 0; i < attributes.Fields.Count; i++)
            {
                this._fields.Add(new ObjectToDbfTypeMap<TGeometryAware>(attributes.Fields[i], t => inverseAttributeMap(t)[i]));
            }


            if (geometries?.Count != attributes.Attributes?.Count)
            {
                throw new NotImplementedException();
            }

            this._features = new List<TGeometryAware>();

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


        public static ShapefileDataSource<TGeometryAware> Create(string shapefileName, Func<Geometry<Point>, Dictionary<string, object>, TGeometryAware> map, Func<TGeometryAware, List<object>> inverseAttributeMap, SrsBase targetSrs = null, Encoding encoding = null)
        {
            var attributes = DbfFile.Read(IRI.Sta.ShapefileFormat.Shapefile.GetDbfFileName(shapefileName), true, encoding);

            var geometries = IRI.Sta.ShapefileFormat.Shapefile.ReadShapes(shapefileName);

            return new ShapefileDataSource<TGeometryAware>(shapefileName, geometries, attributes, map, inverseAttributeMap, targetSrs);
        }

        public static async Task<ShapefileDataSource<TGeometryAware>> CreateAsync(string shapefileName, Func<Geometry<Point>, Dictionary<string, object>, TGeometryAware> map, Func<TGeometryAware, List<Object>> inverseAttributeMap, SrsBase targetSrs = null, Encoding encoding = null)
        {
            var attributes = DbfFile.Read(IRI.Sta.ShapefileFormat.Shapefile.GetDbfFileName(shapefileName), true, encoding);

            var geometries = await IRI.Sta.ShapefileFormat.Shapefile.ReadShapesAsync(shapefileName);

            return new ShapefileDataSource<TGeometryAware>(shapefileName, geometries, attributes, map, inverseAttributeMap, targetSrs);
        }

        public override void SaveChanges()
        {
            Func<TGeometryAware, IEsriShape> geometryMap = null;

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

            IRI.Sta.ShapefileFormat.Shapefile.Save(_shapefileName, _features, geometryMap, _fields, EncodingHelper.ArabicEncoding, _sourceSrs, true);
        }
    }


}
