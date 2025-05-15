using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Collections.Generic;

using Microsoft.SqlServer.Types;

using IRI.Extensions;
using IRI.Sta.Common.Primitives;
using IRI.Sta.Spatial.Primitives;

using WpfPoint = System.Windows.Point;
using Point = IRI.Sta.Common.Primitives.Point;

namespace IRI.Jab.Common.Convertor;

public static class SqlSpatialToStreamGeometry
{
    const double pointSize = 4;

    //static Pen pen = new Pen(new SolidColorBrush(Colors.Black), 0);

    public static StreamGeometry ParseSqlGeometry(List<SqlGeometry> geometries, Func<WpfPoint, WpfPoint> transform, Geometry pointSymbol = null)
    {
        StreamGeometry result = new StreamGeometry();

        result.FillRule = FillRule.Nonzero;

        int p = 0;

        if (geometries != null)
        {
            using (StreamGeometryContext context = result.Open())
            {
                foreach (SqlGeometry item in geometries)
                {
                    p += AddGeometry(context, item, transform, pointSymbol);
                }
            }
        }

        //result.Freeze();

        return result;
    }

    private static int AddGeometry(StreamGeometryContext context, SqlGeometry geometry, Func<WpfPoint, WpfPoint> transform, Geometry pointSymbol)
    {
        if (geometry.IsNotValidOrEmpty())
            return 1;

        var type = geometry.GetOpenGisType();

        switch (type)
        {
            case OpenGisGeometryType.Point:
                if (pointSymbol != null)
                {
                    AddPoint(context, geometry, pointSymbol, transform);
                }
                else
                {
                    AddPoint(context, geometry, transform);
                }
                break;

            case OpenGisGeometryType.LineString:
                AddLineString(context, geometry, transform, false);
                break;

            case OpenGisGeometryType.Polygon:
                AddPolygon(context, geometry, transform);
                break;

            case OpenGisGeometryType.MultiPoint:
                AddMultiPoint(context, geometry, transform);
                break;

            case OpenGisGeometryType.MultiLineString:
                AddMultiLineString(context, geometry, transform);
                break;

            case OpenGisGeometryType.MultiPolygon:
                AddMultiPolygon(context, geometry, transform);
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

    private static void AddLineString(StreamGeometryContext context, SqlGeometry lineString, Func<WpfPoint, WpfPoint> transform, bool isClosed)
    {
        if (lineString.IsNull)
            return;

        int numberOfPoints = lineString.STNumPoints().Value;

        //context.BeginFigure(transform(
        //    new Point(lineString.STStartPoint().STX.Value,
        //                                lineString.STStartPoint().STY.Value)).AsWpfPoint(),

        context.BeginFigure(transform(lineString.STStartPoint().AsWpfPoint()), isFilled: true, isClosed: isClosed);

        //STPointN(index): index is between 1 and number of points
        for (int i = 2; i <= numberOfPoints; i++)
        {
            var point = transform(lineString.STPointN(i).AsWpfPoint());

            context.LineTo(point, isStroked: true, isSmoothJoin: false);
        }

    }

    private static void AddMultiLineString(StreamGeometryContext context, SqlGeometry multiLineString, Func<WpfPoint, WpfPoint> transform)
    {
        int numberOfLineStrings = multiLineString.STNumGeometries().Value;

        for (int i = 1; i <= numberOfLineStrings; i++)
        {
            SqlGeometry lineString = multiLineString.STGeometryN(i);

            AddLineString(context, lineString, transform, false);
        }
    }

    private static void AddPolygon(StreamGeometryContext context, SqlGeometry polygon, Func<WpfPoint, WpfPoint> transform)
    {
        int numberOfInteriorRings = polygon.STNumInteriorRing().Value;

        AddLineString(context, polygon.STExteriorRing(), transform, true);

        for (int i = 1; i <= numberOfInteriorRings; i++)
        {
            AddLineString(context, polygon.STInteriorRingN(i), transform, true);
        }

    }

    private static void AddMultiPolygon(StreamGeometryContext context, SqlGeometry multiPolygon, Func<WpfPoint, WpfPoint> transform)
    {
        int numberOfPolygons = multiPolygon.STNumGeometries().Value;

        for (int i = 1; i <= numberOfPolygons; i++)
        {
            SqlGeometry polygon = multiPolygon.STGeometryN(i);

            AddPolygon(context, polygon, transform);
        }
    }

    private static void AddPoint(StreamGeometryContext context, SqlGeometry point, Func<WpfPoint, WpfPoint> transform)
    {
        var center = transform(point.AsWpfPoint());

        context.DrawGeometry(new EllipseGeometry(new Rect(center.X - pointSize / 2.0, center.Y - pointSize / 2.0, pointSize, pointSize)));
        //context.DrawGeometry(new EllipseGeometry(transform(new Point(point.STX.Value, point.STY.Value)), pointSize, pointSize));
    }

    private static void AddPoint(StreamGeometryContext context, SqlGeometry point, Geometry pointSymbol, Func<WpfPoint, WpfPoint> transform)
    {
        var location = transform(point.AsWpfPoint());

        var geometry = pointSymbol.GetFlattenedPathGeometry();

        foreach (var figure in geometry.Figures)
        {
            var firstPoint = ((PolyLineSegment)figure.Segments[0]).Points[0];

            context.BeginFigure(new WpfPoint(firstPoint.X + location.X, firstPoint.Y + location.Y), figure.IsFilled, figure.IsClosed);

            foreach (var segment in figure.Segments)
            {
                context.PolyLineTo(((PolyLineSegment)segment).Points.Select(i => new WpfPoint(i.X + location.X, i.Y + location.Y)).ToList(), segment.IsStroked, segment.IsSmoothJoin);
            }
        }
    }

    private static void AddMultiPoint(StreamGeometryContext context, SqlGeometry multiPoint, Func<WpfPoint, WpfPoint> transform)
    {
        int numberOfPoints = multiPoint.STNumGeometries().Value;

        for (int i = 1; i <= numberOfPoints; i++)
        {
            SqlGeometry point = multiPoint.STGeometryN(i);

            AddPoint(context, point, transform);
        }
    }

    internal static StreamGeometry Transform(Geometry original, WpfPoint location)
    {
        StreamGeometry result = new StreamGeometry();

        using (StreamGeometryContext context = result.Open())
        {
            var geometry = original.GetFlattenedPathGeometry();

            foreach (var figure in geometry.Figures)
            {
                var firstPoint = ((PolyLineSegment)figure.Segments[0]).Points[0];

                context.BeginFigure(new WpfPoint(firstPoint.X + location.X, firstPoint.Y + location.Y), figure.IsFilled, figure.IsClosed);

                foreach (var segment in figure.Segments)
                {
                    if (segment is PolyLineSegment)
                    {
                        context.PolyLineTo(((PolyLineSegment)segment).Points.Select(i => new WpfPoint(i.X + location.X, i.Y + location.Y)).ToList(), segment.IsStroked, segment.IsSmoothJoin);
                    }
                    else if (segment is LineSegment)
                    {
                        context.LineTo(
                            new WpfPoint(
                                ((LineSegment)segment).Point.X + location.X,
                                ((LineSegment)segment).Point.Y + location.Y), segment.IsStroked, segment.IsSmoothJoin);
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }

                }
            }
        }

        return result;
    }


    // GEOMETRY<T>
    public static StreamGeometry ParseSqlGeometry(List<Geometry<Point>> geometries, Func<WpfPoint, WpfPoint> transform, Geometry? pointSymbol = null)
    {
        StreamGeometry result = new StreamGeometry();

        result.FillRule = FillRule.Nonzero;

        int p = 0;

        if (geometries != null)
        {
            using (StreamGeometryContext context = result.Open())
            {
                foreach (Geometry<Point> item in geometries)
                {
                    p += AddGeometry(context, item, transform, pointSymbol);
                }
            }
        }

        //result.Freeze();

        return result;
    }

    private static int AddGeometry(StreamGeometryContext context, Geometry<Point> geometry, Func<WpfPoint, WpfPoint> transform, Geometry pointSymbol)
    {
        if (geometry.IsNotValidOrEmpty())
            return 1;
         
        switch (geometry.Type)
        {
            case  GeometryType.Point:
                if (pointSymbol != null)
                {
                    AddPoint(context, geometry, pointSymbol, transform);
                }
                else
                {
                    AddPoint(context, geometry, transform);
                }
                break;

            case GeometryType.LineString:
                AddLineString(context, geometry, transform, false);
                break;

            case GeometryType.Polygon:
                AddPolygon(context, geometry, transform);
                break;

            case GeometryType.MultiPoint:
                AddMultiPoint(context, geometry, transform);
                break;

            case GeometryType.MultiLineString:
                AddMultiLineString(context, geometry, transform);
                break;

            case GeometryType.MultiPolygon:
                AddMultiPolygon(context, geometry, transform);
                break;

            case GeometryType.GeometryCollection:
            case GeometryType.CircularString:
            case GeometryType.CompoundCurve:
            case GeometryType.CurvePolygon:
            default:
                break;
        }


        return 0;
    }

    private static void AddLineString(StreamGeometryContext context, Geometry<Point> lineString, Func<WpfPoint, WpfPoint> transform, bool isClosed)
    {
        if (lineString.IsNullOrEmpty())
            return;

        int numberOfPoints = lineString.NumberOfPoints;
         
        context.BeginFigure(transform(lineString.Points[0].AsWpfPoint()), isFilled: true, isClosed: isClosed);
         
        for (int i = 1; i < numberOfPoints; i++)
        {
            var point = transform(lineString.Points[i].AsWpfPoint());

            context.LineTo(point, isStroked: true, isSmoothJoin: false);
        }

    }

    private static void AddMultiLineString(StreamGeometryContext context, Geometry<Point> multiLineString, Func<WpfPoint, WpfPoint> transform)
    {
        int numberOfLineStrings = multiLineString.NumberOfGeometries;

        for (int i = 0; i < numberOfLineStrings; i++)
        {
            var lineString = multiLineString.Geometries[i];

            AddLineString(context, lineString, transform, false);
        }
    }

    private static void AddPolygon(StreamGeometryContext context, Geometry<Point> polygon, Func<WpfPoint, WpfPoint> transform)
    {
        //int numberOfInteriorRings = polygon.STNumInteriorRing().Value;
        //AddLineString(context, polygon.STExteriorRing(), transform, true);
        //for (int i = 1; i <= numberOfInteriorRings; i++)
        //{
        //    AddLineString(context, polygon.STInteriorRingN(i), transform, true);
        //}
         
        for (int i = 0; i < polygon.NumberOfGeometries; i++)
        {
            AddLineString(context, polygon.Geometries[i], transform, true);
        }

    }

    private static void AddMultiPolygon(StreamGeometryContext context, Geometry<Point> multiPolygon, Func<WpfPoint, WpfPoint> transform)
    {
        int numberOfPolygons = multiPolygon.NumberOfGeometries;

        for (int i = 0; i < numberOfPolygons; i++)
        {
            Geometry<Point> polygon = multiPolygon.Geometries[i];

            AddPolygon(context, polygon, transform);
        }
    }

    private static void AddPoint(StreamGeometryContext context, Geometry<Point> point, Func<WpfPoint, WpfPoint> transform)
    {
        var center = transform(point.AsWpfPoint());

        context.DrawGeometry(new EllipseGeometry(new Rect(center.X - pointSize / 2.0, center.Y - pointSize / 2.0, pointSize, pointSize)));
        //context.DrawGeometry(new EllipseGeometry(transform(new Point(point.STX.Value, point.STY.Value)), pointSize, pointSize));
    }

    private static void AddPoint(StreamGeometryContext context, Geometry<Point> point, Geometry pointSymbol, Func<WpfPoint, WpfPoint> transform)
    {
        var location = transform(point.AsWpfPoint());

        var geometry = pointSymbol.GetFlattenedPathGeometry();

        foreach (var figure in geometry.Figures)
        {
            var firstPoint = ((PolyLineSegment)figure.Segments[0]).Points[0];

            context.BeginFigure(new WpfPoint(firstPoint.X + location.X, firstPoint.Y + location.Y), figure.IsFilled, figure.IsClosed);

            foreach (var segment in figure.Segments)
            {
                context.PolyLineTo(((PolyLineSegment)segment).Points.Select(i => new WpfPoint(i.X + location.X, i.Y + location.Y)).ToList(), segment.IsStroked, segment.IsSmoothJoin);
            }
        }
    }

    private static void AddMultiPoint(StreamGeometryContext context, Geometry<Point> multiPoint, Func<WpfPoint, WpfPoint> transform)
    {
        int numberOfPoints = multiPoint.NumberOfGeometries;

        for (int i = 0; i < numberOfPoints; i++)
        {
            Geometry<Point> point = multiPoint.Geometries[i];

            AddPoint(context, point, transform);
        }
    }
}
