using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using IRI.Extensions;
using IRI.Sta.Common.Helpers;
using IRI.Sta.Common.Primitives;
using IRI.Sta.Spatial.Primitives;
using IRI.Sta.Common.Abstrations;
using IRI.Sta.ShapefileFormat.Dbf;
using IRI.Sta.ShapefileFormat.Model;
using IRI.Sta.ShapefileFormat.EsriType;
using IRI.Sta.SpatialReferenceSystem.MapProjections;
using IRI.Sta.ShapefileFormat.ShapeTypes.Abstractions;

namespace IRI.Sta.Persistence.DataSources;

public class ShapefileDataSource : MemoryDataSource//<Feature<Point>>
{
    string _shapefileName;

    SrsBase _targetSrs;

    SrsBase _sourceSrs;

    //public List<ShapefileFormat.Model.ObjectToDbfTypeMap<T>> AttributeMap { get; set; }


    // 1400.02.03
    //ObjectToDfbFields<T> _fields;
    List<ObjectToDbfTypeMap<Feature<Point>>> _fields;

    protected ShapefileDataSource()
    {
    }

    private ShapefileDataSource(string shapefileName,
                                IEsriShapeCollection geometries,
                                EsriAttributeDictionary attributes,
                                Func<Geometry<Point>, Dictionary<string, object>, Feature<Point>> map,
                                Func<Feature<Point>, List<object>> inverseAttributeMap,
                                SrsBase targetSrs)
    {
        if (attributes == null)
        {
            throw new NotImplementedException();
        }

        _shapefileName = shapefileName;

        _sourceSrs = ShapefileFormat.Shapefile.TryGetSrs(shapefileName);

        _targetSrs = targetSrs;

        Func<Point, Point> transformFunc = null;

        if (targetSrs != null)
        {
            transformFunc = p => p.Project(_sourceSrs, targetSrs);
        }

        //string title = System.IO.Path.GetFileNameWithoutExtension(shapefileName);

        WebMercatorExtent = geometries.MainHeader.MinimumBoundingBox;

        // 1401.11.28
        GeometryType = geometries.MainHeader.ShapeType.AsGeometryType();

        if (transformFunc != null)
        {
            WebMercatorExtent = WebMercatorExtent.Transform(p => transformFunc(p));
        }

        // 1400.02.03-comment
        //this._fields = new ObjectToDfbFields<T>() { ExtractAttributesFunc = inverseAttributeMap, Fields = attributes.Fields };

        // 1400.02.19
        _fields = new List<ObjectToDbfTypeMap<Feature<Point>>>();

        for (int i = 0; i < attributes.Fields.Count; i++)
        {
            _fields.Add(new ObjectToDbfTypeMap<Feature<Point>>(attributes.Fields[i], t => inverseAttributeMap(t)[i]));
        }


        if (geometries?.Count != attributes.Attributes?.Count)
        {
            throw new NotImplementedException();
        }

        var features = new List<Feature<Point>>();

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

            features.Add(feature);
        }

        _features = FeatureSet<Point>.Create(string.Empty, features);

        //_idFunc = id => _features.Single(f => f.Id == id);

        //_mapToFeatureFunc = f => f;
    }


    public static ShapefileDataSource Create(string shapefileName, Func<Geometry<Point>, Dictionary<string, object>, Feature<Point>> map, Func<Feature<Point>, List<object>> inverseAttributeMap, SrsBase targetSrs = null, Encoding encoding = null)
    {
        var attributes = DbfFile.Read(ShapefileFormat.Shapefile.GetDbfFileName(shapefileName), true, encoding);

        var geometries = ShapefileFormat.Shapefile.ReadShapes(shapefileName);

        return new ShapefileDataSource(shapefileName, geometries, attributes, map, inverseAttributeMap, targetSrs);
    }

    public static async Task<ShapefileDataSource> CreateAsync(string shapefileName, Func<Geometry<Point>, Dictionary<string, object>, Feature<Point>> map, Func<Feature<Point>, List<object>> inverseAttributeMap, SrsBase targetSrs = null, Encoding encoding = null)
    {
        var attributes = DbfFile.Read(ShapefileFormat.Shapefile.GetDbfFileName(shapefileName), true, encoding);

        var geometries = await ShapefileFormat.Shapefile.ReadShapesAsync(shapefileName);

        return new ShapefileDataSource(shapefileName, geometries, attributes, map, inverseAttributeMap, targetSrs);
    }

    public override void SaveChanges()
    {
        Func<Feature<Point>, IEsriShape> geometryMap = null;

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

        ShapefileFormat.Shapefile.Save(_shapefileName, _features.Features, geometryMap, _fields, EncodingHelper.ArabicEncoding, _sourceSrs, true);
    }
}
