using IRI.Msh.Common.Primitives;
using IRI.Msh.CoordinateSystem.MapProjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.DataManagement.DataSource;

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

        var result = ShapefileDataSource.Create(shapefileName, mapShapeToFeature, inverseAttributeMap, targetCrs, encoding);

        //result.ToDataTableMappingFunc = ToDataTableDefaultMappings.SqlFeatureTypeMapping;

        return result;
    }


    public static async Task<ShapefileDataSource> CreateAsync(string shapefileName, SrsBase targetCrs, Encoding encoding = null)
    {
        Func<Feature<Point>, List<object>> inverseMap = feature => feature.Attributes.Select(kvp => kvp.Value).ToList();

        var result = await ShapefileDataSource.CreateAsync(shapefileName, mapShapeToFeature, inverseMap, targetCrs, encoding);

        //result.ToDataTableMappingFunc = ToDataTableDefaultMappings.SqlFeatureTypeMapping;

        return result;
    }

}
