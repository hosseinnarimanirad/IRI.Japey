using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using IRI.Jab.Common.Extensions;
using IRI.Jab.Common.Extensions;
using System.Threading.Tasks;
using IRI.Ket.SpatialExtensions;
using IRI.Jab.Common.Model.Symbology;

namespace IRI.Jab.Common.Convertor
{
    public class SqlSpatialToDrawingVisual
    {
        double pointSymbolWidth = 4, pointSymbolHeight = 4;

        double pointSymbolMinX, pointSymbolMinY;

        public DrawingVisual ParseSqlGeometry(List<SqlGeometry> geometries, Func<Point, Point> transform, Pen pen, Brush brush, SimplePointSymbol pointSymbol)
        {
            if (pen != null)
            {
                pen.Freeze();
            }

            if (brush != null)
            {
                brush.Freeze();
            }

            if (pointSymbol?.GeometryPointSymbol != null)
            {
                var rect = pointSymbol.GeometryPointSymbol.GetRenderBounds(new Pen(new SolidColorBrush(Colors.Black), 0));

                pointSymbolWidth = rect.Width;

                pointSymbolHeight = rect.Height;

                pointSymbolMinX = rect.BottomLeft.X;

                pointSymbolMinY = rect.BottomLeft.Y;
            }

            DrawingVisual result = new DrawingVisual();

            int p = 0;

            if (geometries != null)
            {
                using (DrawingContext context = result.RenderOpen())
                {
                    foreach (SqlGeometry item in geometries)
                    {
                        p += AddGeometry(context, item, transform, brush, pen, pointSymbol);
                    }
                }
            }

            return result;
        }

        private int AddGeometry(DrawingContext context, SqlGeometry geometry, Func<Point, Point> transform, Brush brush, Pen pen, SimplePointSymbol pointSymbol)
        {
            if (geometry.IsNotValidOrEmpty())
            {
                return 1;
            }

            var type = geometry.GetOpenGisType();

            switch (type)
            {
                case OpenGisGeometryType.Point:
                    AddPoint(context, geometry, transform, brush, pen, pointSymbol);
                    break;

                case OpenGisGeometryType.LineString:
                    AddLineString(context, geometry, transform, pen);
                    break;

                case OpenGisGeometryType.Polygon:
                    AddPolygon(context, geometry, transform, brush, pen);
                    break;

                case OpenGisGeometryType.MultiPoint:
                    AddMultiPoint(context, geometry, transform, brush, pen, pointSymbol);
                    break;

                case OpenGisGeometryType.MultiLineString:
                    AddMultiLineString(context, geometry, transform, pen);
                    break;

                case OpenGisGeometryType.MultiPolygon:
                    AddMultiPolygon(context, geometry, transform, brush, pen);
                    break;

                case OpenGisGeometryType.GeometryCollection:
                case OpenGisGeometryType.CircularString:
                case OpenGisGeometryType.CompoundCurve:
                case OpenGisGeometryType.CurvePolygon:
                default:
                    break;
            }

            return 0;
        }
          
        private void AddLineString(DrawingContext context, SqlGeometry lineString, Func<Point, Point> transform, Pen pen)
        {
            int numberOfPoints = lineString.STNumPoints().Value;

            //STPointN(index): index is between 1 and number of points
            for (int i = 1; i < numberOfPoints; i++)
            {
                var start = lineString.STPointN(i).AsWpfPoint();

                var end = lineString.STPointN(i + 1).AsWpfPoint();

                context.DrawLine(pen, transform(start), transform(end));
            }
        }

        private void AddMultiLineString(DrawingContext context, SqlGeometry multiLineString, Func<Point, Point> transform, Pen pen)
        {
            int numberOfLineStrings = multiLineString.STNumGeometries().Value;

            for (int i = 1; i <= numberOfLineStrings; i++)
            {
                SqlGeometry lineString = multiLineString.STGeometryN(i);

                AddLineString(context, lineString, transform, pen);
            }
        }

        //todo: performance can be enhanced, using DrawLine method for polygon drawing
        private void AddPolygon(DrawingContext context, SqlGeometry polygon, Func<Point, Point> transform, Brush brush, Pen pen)
        {
            //There is no DrawPolygon method for DrawingContext so we should get the Geometry and use the DrawGeometry method
            var geometry = SqlSpatialToStreamGeometry.ParseSqlGeometry(new List<SqlGeometry>() { polygon }, transform);
            
            context.DrawGeometry(brush, pen, geometry);
        }

        private void AddMultiPolygon(DrawingContext context, SqlGeometry multiPolygon, Func<Point, Point> transform, Brush brush, Pen pen)
        {
            //There is no DrawPolygon method for DrawingContext so we should get the Geometry and use the DrawGeometry method
            var geometry = SqlSpatialToStreamGeometry.ParseSqlGeometry(new List<SqlGeometry>() { multiPolygon }, transform);

            context.DrawGeometry(brush, pen, geometry);
        }

        private void AddPoint(DrawingContext context, SqlGeometry point, Func<Point, Point> transform, Brush brush, Pen pen, SimplePointSymbol pointSymbol)
        {
            if (pointSymbol?.GeometryPointSymbol != null)
            {
                var temp = transform(point.AsWpfPoint());

                var geometry = SqlSpatialToStreamGeometry.Transform(
                                    pointSymbol.GeometryPointSymbol,
                                    new System.Windows.Point(temp.X - pointSymbolMinX - pointSymbolWidth / 2.0, temp.Y - pointSymbolMinY + pointSymbolHeight / 2.0));

                context.DrawGeometry(brush, pen, geometry);
            }
            else if (pointSymbol?.ImagePointSymbol != null)
            {
                Point location = transform(point.AsWpfPoint());

                context.DrawImage(pointSymbol.ImagePointSymbol, new Rect(location.X, location.Y, pointSymbol.SymbolWidth, pointSymbol.SymbolHeight));
            }
            else
            {
                context.DrawEllipse(brush, pen, transform(point.AsWpfPoint()), pointSymbol.SymbolWidth, pointSymbol.SymbolHeight);
            }
        }

        private void AddMultiPoint(DrawingContext context, SqlGeometry multiPoint, Func<Point, Point> transform, Brush brush, Pen pen, SimplePointSymbol pointSymbol)
        {
            int numberOfPoints = multiPoint.STNumGeometries().Value;

            for (int i = 1; i <= numberOfPoints; i++)
            {
                SqlGeometry point = multiPoint.STGeometryN(i);

                AddPoint(context, point, transform, brush, pen, pointSymbol);
            }
        }

    }
}
