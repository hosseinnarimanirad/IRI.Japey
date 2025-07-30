using System;
using System.Windows.Media;
using System.Collections.Generic;

using IRI.Extensions;
using IRI.Sta.Common.Primitives;
using IRI.Sta.Spatial.Primitives;

using WpfPoint = System.Windows.Point;
using IRI.Jab.Common.Cartography.Symbologies;
using IRI.Jab.Common.Cartography.Rendering.Helpers;

namespace IRI.Jab.Common.Cartography.Rendering;

public class DrawingVisualRenderStrategy : RenderStrategy
{
    double pointSymbolWidth = 4, pointSymbolHeight = 4;

    double pointSymbolMinX, pointSymbolMinY;

    private readonly Pen _defaultPen = new Pen(Brushes.Black, 1);
    private readonly Brush _defaultBrush = Brushes.Gray;

    public DrawingVisual ParseGeometry(
        List<Feature<Point>> features,
        //Func<WpfPoint, WpfPoint> transform,
        Pen? pen,
        Brush? brush,
        SimplePointSymbolizer? pointSymbol)
    {
        DrawingVisual result = new DrawingVisual();

        if (features.IsNullOrEmpty())
            return result;

        if (pen != null)
            pen.Freeze();

        if (brush != null)
            brush.Freeze();

        if (pointSymbol?.GeometryPointSymbol != null)
        {
            var rect = pointSymbol.GeometryPointSymbol.GetRenderBounds(new Pen(new SolidColorBrush(Colors.Black), 0));

            pointSymbolWidth = rect.Width;

            pointSymbolHeight = rect.Height;

            pointSymbolMinX = rect.BottomLeft.X;

            pointSymbolMinY = rect.BottomLeft.Y;
        }

        using (DrawingContext context = result.RenderOpen())
        {
            foreach (var item in features)
            {
                AddGeometry(context, item.TheGeometry, /*transform,*/ brush, pen, pointSymbol);
            }
        }

        return result;
    }

    private void AddGeometry(
        DrawingContext context,
        Geometry<Point> geometry,
        //Func<WpfPoint, WpfPoint> transform,
        Brush? brush,
        Pen? pen,
        SimplePointSymbolizer? pointSymbol)
    {
        if (geometry.IsNotValidOrEmpty())
            return;

        switch (geometry.Type)
        {
            case GeometryType.Point:
                AddPoint(context, geometry, /*transform,*/ brush, pen, pointSymbol);
                break;

            case GeometryType.LineString:
                AddLineString(context, geometry, /*transform, */pen ?? _defaultPen);
                break;

            case GeometryType.Polygon:
                AddPolygon(context, geometry, /*transform, */brush, pen);
                break;

            case GeometryType.MultiPoint:
                AddMultiPoint(context, geometry, /*transform, */brush, pen, pointSymbol);
                break;

            case GeometryType.MultiLineString:
                AddMultiLineString(context, geometry, /*transform, */pen ?? _defaultPen);
                break;

            case GeometryType.MultiPolygon:
                AddMultiPolygon(context, geometry, /*transform, */brush, pen);
                break;

            case GeometryType.GeometryCollection:
                AddGeometryCollection(context, geometry, /*transform,*/ brush, pen, pointSymbol);
                break;

            case GeometryType.CircularString:
            case GeometryType.CompoundCurve:
            case GeometryType.CurvePolygon:
            default:
                break;
        }

        return;
    }

    private void AddLineString(DrawingContext context, Geometry<Point> lineString, /*Func<WpfPoint, WpfPoint> transform,*/ Pen pen)
    {
        int numberOfPoints = lineString.NumberOfPoints;

        for (int i = 0; i < numberOfPoints - 1; i++)
        {
            var start = lineString.Points[i].AsWpfPoint();

            var end = lineString.Points[i + 1].AsWpfPoint();

            //context.DrawLine(pen, transform(start), transform(end));
            context.DrawLine(pen, start, end);
        }
    }

    private void AddMultiLineString(DrawingContext context, Geometry<Point> multiLineString, /*Func<WpfPoint, WpfPoint> transform,*/ Pen pen)
    {
        int numberOfLineStrings = multiLineString.NumberOfGeometries;

        for (int i = 0; i < numberOfLineStrings; i++)
        {
            var lineString = multiLineString.Geometries[i];

            AddLineString(context, lineString, /*transform, */pen);
        }
    }

    //todo: performance can be enhanced, using DrawLine method for polygon drawing
    private void AddPolygon(DrawingContext context, Geometry<Point> polygon, /*Func<WpfPoint, WpfPoint> transform, */Brush? brush, Pen? pen)
    {
        //There is no DrawPolygon method for DrawingContext so we should get the Geometry and use the DrawGeometry method
        var geometry = StreamGeometryRenderer.ParseSqlGeometry(new List<Feature<Point>>() { polygon.AsFeature() }/*, transform*/);

        context.DrawGeometry(brush, pen, geometry);
    }

    private void AddMultiPolygon(DrawingContext context, Geometry<Point> multiPolygon, /*Func<WpfPoint, WpfPoint> transform,*/ Brush? brush, Pen? pen)
    {
        //There is no DrawPolygon method for DrawingContext so we should get the Geometry and use the DrawGeometry method
        var geometry = StreamGeometryRenderer.ParseSqlGeometry(new List<Feature<Point>>() { multiPolygon.AsFeature() }/*, transform*/);

        context.DrawGeometry(brush, pen, geometry);
    }

    private void AddGeometryCollection(DrawingContext context, Geometry<Point> multiLineString, /*Func<WpfPoint, WpfPoint> transform,*/ Brush? brush, Pen? pen, SimplePointSymbolizer? pointSymbol)
    {
        int numberOfLineStrings = multiLineString.NumberOfGeometries;

        for (int i = 0; i < numberOfLineStrings; i++)
        {
            var lineString = multiLineString.Geometries[i];

            AddGeometry(context, lineString, /*transform, */brush, pen, pointSymbol);
        }
    }

    private void AddPoint(DrawingContext context, Geometry<Point> point, /*Func<WpfPoint, WpfPoint> transform,*/ Brush? brush, Pen? pen, SimplePointSymbolizer? pointSymbol)
    {
        var symbolWidth = pointSymbol?.SymbolWidth ?? pointSymbolWidth;
        var symbolHeight = pointSymbol?.SymbolHeight ?? pointSymbolHeight;

        var location = /*transform*/point.AsWpfPoint();

        if (pointSymbol?.GeometryPointSymbol != null)
        {
            var geometry = StreamGeometryRenderer.Transform(
                                pointSymbol.GeometryPointSymbol,
                                new WpfPoint(location.X - pointSymbolMinX - symbolWidth / 2.0, location.Y - pointSymbolMinY + symbolHeight / 2.0));

            context.DrawGeometry(brush, pen, geometry);
        }
        else if (pointSymbol?.ImagePointSymbol != null)
        {
            context.DrawImage(pointSymbol.ImagePointSymbol, new System.Windows.Rect(location.X, location.Y, symbolWidth, symbolHeight));
        }
        else
        {
            context.DrawEllipse(brush, pen, location, symbolWidth, symbolHeight);
        }
    }

    private void AddMultiPoint(DrawingContext context, Geometry<Point> multiPoint, /*Func<WpfPoint, WpfPoint> transform,*/ Brush? brush, Pen? pen, SimplePointSymbolizer? pointSymbol)
    {
        int numberOfPoints = multiPoint.NumberOfGeometries;

        for (int i = 0; i < numberOfPoints; i++)
        {
            var point = multiPoint.Geometries[i];

            AddPoint(context, point, /*transform,*/ brush, pen, pointSymbol);
        }
    }


    public override ImageBrush? Render(List<Feature<Point>> features, double mapScale, double screenWidth, double screenHeight)
    {
        throw new NotImplementedException();
    }

    //// ******************************************************** SqlGeometry ********************************************************************

    //#region SqlGeometry to Drawing Visual

    //public DrawingVisual ParseSqlGeometry(List<SqlGeometry> geometries, Func<WpfPoint, WpfPoint> transform, Pen? pen, Brush? brush, SimplePointSymbolizer? pointSymbol)
    //{
    //    if (pen != null)
    //        pen.Freeze();

    //    if (brush != null)
    //        brush.Freeze();

    //    if (pointSymbol?.GeometryPointSymbol != null)
    //    {
    //        var rect = pointSymbol.GeometryPointSymbol.GetRenderBounds(new Pen(new SolidColorBrush(Colors.Black), 0));

    //        pointSymbolWidth = rect.Width;

    //        pointSymbolHeight = rect.Height;

    //        pointSymbolMinX = rect.BottomLeft.X;

    //        pointSymbolMinY = rect.BottomLeft.Y;
    //    }

    //    DrawingVisual result = new DrawingVisual();

    //    if (geometries != null)
    //    {
    //        using (DrawingContext context = result.RenderOpen())
    //        {
    //            foreach (SqlGeometry item in geometries)
    //            {
    //                AddGeometry(context, item, transform, brush, pen, pointSymbol);
    //            }
    //        }
    //    }

    //    return result;
    //}

    //private void AddGeometry(DrawingContext context, SqlGeometry geometry, Func<WpfPoint, WpfPoint> transform, Brush? brush, Pen? pen, SimplePointSymbolizer? pointSymbol)
    //{
    //    if (geometry.IsNotValidOrEmpty())
    //        return;

    //    var type = geometry.GetOpenGisType();

    //    switch (type)
    //    {
    //        case OpenGisGeometryType.Point:
    //            AddPoint(context, geometry, transform, brush, pen, pointSymbol);
    //            break;

    //        case OpenGisGeometryType.LineString:
    //            AddLineString(context, geometry, transform, pen ?? _defaultPen);
    //            break;

    //        case OpenGisGeometryType.Polygon:
    //            AddPolygon(context, geometry, transform, brush, pen);
    //            break;

    //        case OpenGisGeometryType.MultiPoint:
    //            AddMultiPoint(context, geometry, transform, brush, pen, pointSymbol);
    //            break;

    //        case OpenGisGeometryType.MultiLineString:
    //            AddMultiLineString(context, geometry, transform, pen ?? _defaultPen);
    //            break;

    //        case OpenGisGeometryType.MultiPolygon:
    //            AddMultiPolygon(context, geometry, transform, brush, pen);
    //            break;

    //        case OpenGisGeometryType.GeometryCollection:
    //        case OpenGisGeometryType.CircularString:
    //        case OpenGisGeometryType.CompoundCurve:
    //        case OpenGisGeometryType.CurvePolygon:
    //        default:
    //            break;
    //    }

    //    return;
    //}

    //private void AddLineString(DrawingContext context, SqlGeometry lineString, Func<WpfPoint, WpfPoint> transform, Pen pen)
    //{
    //    int numberOfPoints = lineString.STNumPoints().Value;

    //    //STPointN(index): index is between 1 and number of points
    //    for (int i = 1; i < numberOfPoints; i++)
    //    {
    //        var start = lineString.STPointN(i).AsWpfPoint();

    //        var end = lineString.STPointN(i + 1).AsWpfPoint();

    //        context.DrawLine(pen, transform(start), transform(end));
    //    }
    //}

    //private void AddMultiLineString(DrawingContext context, SqlGeometry multiLineString, Func<WpfPoint, WpfPoint> transform, Pen pen)
    //{
    //    int numberOfLineStrings = multiLineString.STNumGeometries().Value;

    //    for (int i = 1; i <= numberOfLineStrings; i++)
    //    {
    //        SqlGeometry lineString = multiLineString.STGeometryN(i);

    //        AddLineString(context, lineString, transform, pen);
    //    }
    //}

    ////todo: performance can be enhanced, using DrawLine method for polygon drawing
    //private void AddPolygon(DrawingContext context, SqlGeometry polygon, Func<WpfPoint, WpfPoint> transform, Brush? brush, Pen? pen)
    //{
    //    //There is no DrawPolygon method for DrawingContext so we should get the Geometry and use the DrawGeometry method
    //    var geometry = StreamGeometryRenderer.ParseSqlGeometry(new List<SqlGeometry>() { polygon }, transform);

    //    context.DrawGeometry(brush, pen, geometry);
    //}

    //private void AddMultiPolygon(DrawingContext context, SqlGeometry multiPolygon, Func<WpfPoint, WpfPoint> transform, Brush? brush, Pen? pen)
    //{
    //    //There is no DrawPolygon method for DrawingContext so we should get the Geometry and use the DrawGeometry method
    //    var geometry = StreamGeometryRenderer.ParseSqlGeometry(new List<SqlGeometry>() { multiPolygon }, transform);

    //    context.DrawGeometry(brush, pen, geometry);
    //}

    //private void AddPoint(DrawingContext context, SqlGeometry point, Func<WpfPoint, WpfPoint> transform, Brush? brush, Pen? pen, SimplePointSymbolizer? pointSymbol)
    //{
    //    var symbolWidth = pointSymbol?.SymbolWidth ?? pointSymbolWidth;
    //    var symbolHeight = pointSymbol?.SymbolHeight ?? pointSymbolHeight;

    //    var location = transform(point.AsWpfPoint());

    //    if (pointSymbol?.GeometryPointSymbol != null)
    //    {
    //        var geometry = StreamGeometryRenderer.Transform(
    //                            pointSymbol.GeometryPointSymbol,
    //                            new WpfPoint(location.X - pointSymbolMinX - symbolWidth / 2.0, location.Y - pointSymbolMinY + symbolHeight / 2.0));

    //        context.DrawGeometry(brush, pen, geometry);
    //    }
    //    else if (pointSymbol?.ImagePointSymbol != null)
    //    {
    //        context.DrawImage(pointSymbol.ImagePointSymbol, new Rect(location.X, location.Y, symbolWidth, symbolHeight));
    //    }
    //    else
    //    {
    //        context.DrawEllipse(brush, pen, location, symbolWidth, symbolHeight);
    //    }
    //}

    //private void AddMultiPoint(DrawingContext context, SqlGeometry multiPoint, Func<WpfPoint, WpfPoint> transform, Brush? brush, Pen? pen, SimplePointSymbolizer? pointSymbol)
    //{
    //    int numberOfPoints = multiPoint.STNumGeometries().Value;

    //    for (int i = 1; i <= numberOfPoints; i++)
    //    {
    //        SqlGeometry point = multiPoint.STGeometryN(i);

    //        AddPoint(context, point, transform, brush, pen, pointSymbol);
    //    }
    //}

    //#endregion
}
