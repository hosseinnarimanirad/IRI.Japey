using IRI.Jab.Common.Model;
using IRI.Sta.Common.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Extensions;

public static class GeometryTypeExtensions
{
    public static LayerType? AsLayerType(this GeometryType? geometryType)
    {
        if (geometryType is null)
            return null;

        switch (geometryType)
        {
            case GeometryType.Point:
            case GeometryType.MultiPoint:
                return LayerType.Point;

            case GeometryType.LineString:
            case GeometryType.MultiLineString:
                return LayerType.Polyline;

            case GeometryType.Polygon:
            case GeometryType.MultiPolygon:
                return LayerType.Polygon;

            case GeometryType.GeometryCollection:
            case GeometryType.CircularString:
            case GeometryType.CompoundCurve:
            case GeometryType.CurvePolygon:
                return null;

            default:
                throw new NotImplementedException("GeometryTypeExtensions > AsLayerType");
        }
    }
}
