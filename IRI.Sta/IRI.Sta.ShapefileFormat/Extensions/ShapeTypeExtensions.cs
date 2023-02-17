﻿using IRI.Msh.Common.Primitives;
using IRI.Sta.ShapefileFormat.EsriType;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Extensions
{
    public static class ShapeTypeExtensions
    {
        public static GeometryType? AsGeometryType(this EsriShapeType type)
        {
            switch (type)
            {
                case EsriShapeType.EsriPoint:
                case EsriShapeType.EsriPointZM:
                case EsriShapeType.EsriPointM:
                case EsriShapeType.EsriPointZ:
                    return GeometryType.Point;

                case EsriShapeType.EsriPolyLine:
                case EsriShapeType.EsriPolyLineZM:
                case EsriShapeType.EsriPolyLineM:
                case EsriShapeType.EsriPolyLineZ:
                    return GeometryType.LineString;

                case EsriShapeType.EsriPolygon:
                case EsriShapeType.EsriPolygonZM:
                case EsriShapeType.EsriPolygonM:
                case EsriShapeType.EsriPolygonZ:
                    return GeometryType.Polygon;

                case EsriShapeType.EsriMultiPoint:
                case EsriShapeType.EsriMultiPointZM:
                case EsriShapeType.EsriMultiPointM:
                case EsriShapeType.EsriMultiPointZ:
                    return GeometryType.MultiPoint;

                case EsriShapeType.NullShape:
                case EsriShapeType.EsriMultiPatch:
                    return null;

                default:
                    throw new NotImplementedException("ShapeTypeExtensions > AsGeometryType");
            }
        }

        public static bool HasZ(this EsriShapeType type)
        {
            switch (type)
            {
                case EsriShapeType.NullShape:
                case EsriShapeType.EsriPoint:
                case EsriShapeType.EsriPolyLine:
                case EsriShapeType.EsriPolygon:
                case EsriShapeType.EsriMultiPoint:

                case EsriShapeType.EsriPointM:
                case EsriShapeType.EsriPolyLineM:
                case EsriShapeType.EsriPolygonM:
                case EsriShapeType.EsriMultiPointM:

                case EsriShapeType.EsriMultiPatch:
                    return false;

                case EsriShapeType.EsriPointZM:
                case EsriShapeType.EsriPolyLineZM:
                case EsriShapeType.EsriPolygonZM:
                case EsriShapeType.EsriMultiPointZM:

                case EsriShapeType.EsriPointZ:
                case EsriShapeType.EsriPolyLineZ:
                case EsriShapeType.EsriPolygonZ:
                case EsriShapeType.EsriMultiPointZ:
                    return true;

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
