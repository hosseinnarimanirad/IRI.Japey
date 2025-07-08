using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using IRI.Sta.Common.Primitives;
using IRI.Sta.Spatial.Primitives;
using IRI.Sta.SpatialReferenceSystem.MapProjections;
using IRI.Sta.ShapefileFormat.Dbf;

namespace IRI.Sta.Persistence.DataSources;

public static class ShapefileDataSourceFactory
{
    static Func<Geometry<Point>, Dictionary<string, object>, Feature<Point>> mapShapeToFeature = (geometry, attributes) => new Feature<Point>(geometry, attributes);

    static List<Func<Feature<Point>, object>> CreateInverseAttributeMap(Feature<Point> feature)
    {
        List<Func<Feature<Point>, object>> result = new List<Func<Feature<Point>, object>>();

        foreach (var item in feature.Attributes)
        {
            result.Add(d => d.Attributes[item.Key]);
        }

        return result;
    }


    public static ShapefileDataSource Create(string shapefileName, SrsBase targetCrs, Encoding encoding = null)
    {
        Func<Feature<Point>, List<object>> inverseAttributeMap = feature => feature.Attributes.Select(kvp => kvp.Value).ToList();

        var result = Create(shapefileName, mapShapeToFeature, inverseAttributeMap, targetCrs, encoding);

        //result.ToDataTableMappingFunc = ToDataTableDefaultMappings.SqlFeatureTypeMapping;

        return result;
    }


    public static async Task<ShapefileDataSource> CreateAsync(string shapefileName, SrsBase targetCrs, Encoding encoding = null)
    {
        Func<Feature<Point>, List<object>> inverseMap = feature => feature.Attributes.Select(kvp => kvp.Value).ToList();

        var result = await CreateAsync(shapefileName, mapShapeToFeature, inverseMap, targetCrs, encoding);

        //result.ToDataTableMappingFunc = ToDataTableDefaultMappings.SqlFeatureTypeMapping;

        return result;
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

}
