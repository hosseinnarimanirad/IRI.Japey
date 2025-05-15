using System;
using System.Windows;
using System.Windows.Media;
using System.Collections.Generic;
using Microsoft.SqlServer.Types;

using IRI.Extensions;
using IRI.Sta.Common.Primitives;
using IRI.Sta.Spatial.Primitives;
using IRI.Jab.Common.Model.Symbology; 
 
using WpfPoint = System.Windows.Point;
using Point = IRI.Sta.Common.Primitives.Point;

namespace IRI.Jab.Common.Convertor;

public class SqlSpatialToDrawingVisual
{
    double pointSymbolWidth = 4, pointSymbolHeight = 4;

    double pointSymbolMinX, pointSymbolMinY;

    public DrawingVisual ParseSqlGeometry(List<SqlGeometry> geometries, Func<WpfPoint, WpfPoint> transform, Pen pen, Brush brush, SimplePointSymbol pointSymbol)
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

    private int AddGeometry(DrawingContext context, SqlGeometry geometry, Func<WpfPoint, WpfPoint> transform, Brush brush, Pen pen, SimplePointSymbol pointSymbol)
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

    private void AddLineString(DrawingContext context, SqlGeometry lineString, Func<WpfPoint, WpfPoint> transform, Pen pen)
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

    private void AddMultiLineString(DrawingContext context, SqlGeometry multiLineString, Func<WpfPoint, WpfPoint> transform, Pen pen)
    {
        int numberOfLineStrings = multiLineString.STNumGeometries().Value;

        for (int i = 1; i <= numberOfLineStrings; i++)
        {
            SqlGeometry lineString = multiLineString.STGeometryN(i);

            AddLineString(context, lineString, transform, pen);
        }
    }

    //todo: performance can be enhanced, using DrawLine method for polygon drawing
    private void AddPolygon(DrawingContext context, SqlGeometry polygon, Func<WpfPoint, WpfPoint> transform, Brush brush, Pen pen)
    {
        //There is no DrawPolygon method for DrawingContext so we should get the Geometry and use the DrawGeometry method
        var geometry = SqlSpatialToStreamGeometry.ParseSqlGeometry(new List<SqlGeometry>() { polygon }, transform);

        context.DrawGeometry(brush, pen, geometry);
    }

    private void AddMultiPolygon(DrawingContext context, SqlGeometry multiPolygon, Func<WpfPoint, WpfPoint> transform, Brush brush, Pen pen)
    {
        //There is no DrawPolygon method for DrawingContext so we should get the Geometry and use the DrawGeometry method
        var geometry = SqlSpatialToStreamGeometry.ParseSqlGeometry(new List<SqlGeometry>() { multiPolygon }, transform);

        context.DrawGeometry(brush, pen, geometry);
    }

    private void AddPoint(DrawingContext context, SqlGeometry point, Func<WpfPoint, WpfPoint> transform, Brush brush, Pen pen, SimplePointSymbol pointSymbol)
    {
        if (pointSymbol?.GeometryPointSymbol != null)
        {
            var temp = transform(point.AsWpfPoint());

            var geometry = SqlSpatialToStreamGeometry.Transform(
                                pointSymbol.GeometryPointSymbol,
                                new WpfPoint(temp.X - pointSymbolMinX - pointSymbolWidth / 2.0, temp.Y - pointSymbolMinY + pointSymbolHeight / 2.0));

            context.DrawGeometry(brush, pen, geometry);
        }
        else if (pointSymbol?.ImagePointSymbol != null)
        {
            WpfPoint location = transform(point.AsWpfPoint());

            context.DrawImage(pointSymbol.ImagePointSymbol, new Rect(location.X, location.Y, pointSymbol.SymbolWidth, pointSymbol.SymbolHeight));
        }
        else
        {
            context.DrawEllipse(brush, pen, transform(point.AsWpfPoint()), pointSymbol.SymbolWidth, pointSymbol.SymbolHeight);
        }
    }

    private void AddMultiPoint(DrawingContext context, SqlGeometry multiPoint, Func<WpfPoint, WpfPoint> transform, Brush brush, Pen pen, SimplePointSymbol pointSymbol)
    {
        int numberOfPoints = multiPoint.STNumGeometries().Value;

        for (int i = 1; i <= numberOfPoints; i++)
        {
            SqlGeometry point = multiPoint.STGeometryN(i);

            AddPoint(context, point, transform, brush, pen, pointSymbol);
        }
    }


    // GEOMETRY<T>

    public DrawingVisual ParseSqlGeometry(List<Geometry<Point>> geometries, Func<WpfPoint, WpfPoint> transform, Pen pen, Brush brush, SimplePointSymbol pointSymbol)
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
                foreach (Geometry<Point> item in geometries)
                {
                    p += AddGeometry(context, item, transform, brush, pen, pointSymbol);
                }
            }
        }

        return result;
    }

    private int AddGeometry(DrawingContext context, Geometry<Point> geometry, Func<WpfPoint, WpfPoint> transform, Brush brush, Pen pen, SimplePointSymbol pointSymbol)
    {
        if (geometry.IsNotValidOrEmpty())
        {
            return 1;
        }

        switch (geometry.Type)
        {
            case GeometryType.Point:
                AddPoint(context, geometry, transform, brush, pen, pointSymbol);
                break;

            case GeometryType.LineString:
                AddLineString(context, geometry, transform, pen);
                break;

            case GeometryType.Polygon:
                AddPolygon(context, geometry, transform, brush, pen);
                break;

            case GeometryType.MultiPoint:
                AddMultiPoint(context, geometry, transform, brush, pen, pointSymbol);
                break;

            case GeometryType.MultiLineString:
                AddMultiLineString(context, geometry, transform, pen);
                break;

            case GeometryType.MultiPolygon:
                AddMultiPolygon(context, geometry, transform, brush, pen);
                break;

            case GeometryType.GeometryCollection:
                AddGeometryCollection(context, geometry, transform, brush, pen, pointSymbol);
                break;

            case GeometryType.CircularString:
            case GeometryType.CompoundCurve:
            case GeometryType.CurvePolygon:
            default:
                break;
        }

        return 0;
    }

    private void AddLineString(DrawingContext context, Geometry<Point> lineString, Func<WpfPoint, WpfPoint> transform, Pen pen)
    {
        int numberOfPoints = lineString.NumberOfPoints;

        for (int i = 0; i < numberOfPoints - 1; i++)
        {
            var start = lineString.Points[i].AsWpfPoint();

            var end = lineString.Points[i + 1].AsWpfPoint();

            context.DrawLine(pen, transform(start), transform(end));
        }
    }

    private void AddMultiLineString(DrawingContext context, Geometry<Point> multiLineString, Func<WpfPoint, WpfPoint> transform, Pen pen)
    {
        int numberOfLineStrings = multiLineString.NumberOfGeometries;

        for (int i = 0; i < numberOfLineStrings; i++)
        {
            var lineString = multiLineString.Geometries[i];

            AddLineString(context, lineString, transform, pen);
        }
    }

    //todo: performance can be enhanced, using DrawLine method for polygon drawing
    private void AddPolygon(DrawingContext context, Geometry<Point> polygon, Func<WpfPoint, WpfPoint> transform, Brush brush, Pen pen)
    {
        //There is no DrawPolygon method for DrawingContext so we should get the Geometry and use the DrawGeometry method
        var geometry = SqlSpatialToStreamGeometry.ParseSqlGeometry(new List<Geometry<Point>>() { polygon }, transform);

        context.DrawGeometry(brush, pen, geometry);
    }

    private void AddMultiPolygon(DrawingContext context, Geometry<Point> multiPolygon, Func<WpfPoint, WpfPoint> transform, Brush brush, Pen pen)
    {
        //There is no DrawPolygon method for DrawingContext so we should get the Geometry and use the DrawGeometry method
        var geometry = SqlSpatialToStreamGeometry.ParseSqlGeometry(new List<Geometry<Point>>() { multiPolygon }, transform);

        context.DrawGeometry(brush, pen, geometry);
    }

    private void AddGeometryCollection(DrawingContext context, Geometry<Point> multiLineString, Func<WpfPoint, WpfPoint> transform, Brush brush, Pen pen, SimplePointSymbol pointSymbol)
    {
        int numberOfLineStrings = multiLineString.NumberOfGeometries;

        for (int i = 0; i < numberOfLineStrings; i++)
        {
            var lineString = multiLineString.Geometries[i];

            AddGeometry(context, lineString, transform, brush, pen, pointSymbol);
        }
    }

    private void AddPoint(DrawingContext context, Geometry<Point> point, Func<WpfPoint, WpfPoint> transform, Brush brush, Pen pen, SimplePointSymbol pointSymbol)
    {
        if (pointSymbol?.GeometryPointSymbol != null)
        {
            var temp = transform(point.AsWpfPoint());

            var geometry = SqlSpatialToStreamGeometry.Transform(
                                pointSymbol.GeometryPointSymbol,
                                new WpfPoint(temp.X - pointSymbolMinX - pointSymbolWidth / 2.0, temp.Y - pointSymbolMinY + pointSymbolHeight / 2.0));

            context.DrawGeometry(brush, pen, geometry);
        }
        else if (pointSymbol?.ImagePointSymbol != null)
        {
            WpfPoint location = transform(point.AsWpfPoint());

            context.DrawImage(pointSymbol.ImagePointSymbol, new Rect(location.X, location.Y, pointSymbol.SymbolWidth, pointSymbol.SymbolHeight));
        }
        else
        {
            context.DrawEllipse(brush, pen, transform(point.AsWpfPoint()), pointSymbol.SymbolWidth, pointSymbol.SymbolHeight);
        }
    }

    private void AddMultiPoint(DrawingContext context, Geometry<Point> multiPoint, Func<WpfPoint, WpfPoint> transform, Brush brush, Pen pen, SimplePointSymbol pointSymbol)
    {
        int numberOfPoints = multiPoint.NumberOfGeometries;

        for (int i = 0; i < numberOfPoints; i++)
        {
            var point = multiPoint.Geometries[i];

            AddPoint(context, point, transform, brush, pen, pointSymbol);
        }
    }

}
