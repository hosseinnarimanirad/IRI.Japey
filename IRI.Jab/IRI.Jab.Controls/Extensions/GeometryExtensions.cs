using IRI.Ham.SpatialBase.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Jab.Controls.Extensions
{
    public static class GeometryExtensions
    {
        public static Model.CoordinateEditor.CoordinateEditor AsCoordinateEditor(this Geometry geometry)
        {
            if (geometry == null)
                return null;

            switch (geometry.Type)
            {
                case GeometryType.LineString:
                    return new Model.CoordinateEditor.LineStringEditorModel(geometry);

                case GeometryType.Polygon:
                    return new Model.CoordinateEditor.PolygonEditorModel(geometry);

                case GeometryType.Point:
                case GeometryType.MultiPoint:
                case GeometryType.MultiLineString:
                case GeometryType.MultiPolygon:
                case GeometryType.GeometryCollection:
                case GeometryType.CircularString:
                case GeometryType.CompoundCurve:
                case GeometryType.CurvePolygon:
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
