using IRI.Jab.Common.Convertor;
using IRI.Jab.Common;
using IRI.Sta.Common.Abstrations;
using IRI.Sta.Spatial.Primitives;
using System;
using System.Windows.Media;
using WpfPoint = System.Windows.Point;
//using System.Windows.Media;

namespace IRI.Extensions;

public static class GeometryExtensions
{
    public static void DrawGeometry(this StreamGeometryContext ctx, Geometry geo)
    {
        var pathGeometry = geo as PathGeometry ?? PathGeometry.CreateFromGeometry(geo);

        foreach (var figure in pathGeometry.Figures)
        {
            ctx.DrawFigure(figure);
        }
    }

    public static void DrawFigure(this StreamGeometryContext ctx, PathFigure figure)
    {
        ctx.BeginFigure(figure.StartPoint, figure.IsFilled, figure.IsClosed);

        foreach (var segment in figure.Segments)
        {
            var lineSegment = segment as LineSegment;

            if (lineSegment != null)
            {
                ctx.LineTo(lineSegment.Point, lineSegment.IsStroked, lineSegment.IsSmoothJoin);
                continue;
            }

            var bezierSegment = segment as BezierSegment;

            if (bezierSegment != null)
            {
                ctx.BezierTo(bezierSegment.Point1, bezierSegment.Point2, bezierSegment.Point3, bezierSegment.IsStroked, bezierSegment.IsSmoothJoin);
                continue;
            }

            var quadraticSegment = segment as QuadraticBezierSegment;

            if (quadraticSegment != null)
            {
                ctx.QuadraticBezierTo(quadraticSegment.Point1, quadraticSegment.Point2, quadraticSegment.IsStroked, quadraticSegment.IsSmoothJoin);
                continue;
            }

            var polylineSegment = segment as PolyLineSegment;

            if (polylineSegment != null)
            {
                ctx.PolyLineTo(polylineSegment.Points, polylineSegment.IsStroked, polylineSegment.IsSmoothJoin);
                continue;
            }

            var polyBezierSegment = segment as PolyBezierSegment;

            if (polyBezierSegment != null)
            {
                ctx.PolyBezierTo(polyBezierSegment.Points, polyBezierSegment.IsStroked, polyBezierSegment.IsSmoothJoin);
                continue;
            }

            var polyQuadraticSegment = segment as PolyQuadraticBezierSegment;

            if (polyQuadraticSegment != null)
            {
                ctx.PolyQuadraticBezierTo(polyQuadraticSegment.Points, polyQuadraticSegment.IsStroked, polyQuadraticSegment.IsSmoothJoin);
                continue;
            }

            var arcSegment = segment as ArcSegment;

            if (arcSegment != null)
            {
                ctx.ArcTo(arcSegment.Point, arcSegment.Size, arcSegment.RotationAngle, arcSegment.IsLargeArc, arcSegment.SweepDirection, arcSegment.IsStroked, arcSegment.IsSmoothJoin);
                continue;
            }


        }
    }

    public static DrawingVisual ParseToDrawingVisual<T>(this Geometry<T> geometry, int imageWidth, int imageHeight, VisualParameters visualParameters) where T : IPoint, new()
    {
        var mapExtent = geometry.GetBoundingBox();

        double xScale = imageWidth / mapExtent.Width;
        double yScale = imageHeight / mapExtent.Height;
        double scale = xScale > yScale ? yScale : xScale;

        Func<WpfPoint, WpfPoint> mapToScreen = new Func<WpfPoint, WpfPoint>(p => new WpfPoint((p.X - mapExtent.XMin) * scale, -(p.Y - mapExtent.YMax) * scale));

        var pen = visualParameters.GetWpfPen();
        pen.LineJoin = PenLineJoin.Round;
        pen.EndLineCap = PenLineCap.Round;
        pen.StartLineCap = PenLineCap.Round;

        Brush brush = visualParameters.Fill;

        DrawingVisual drawingVisual = new SqlSpatialToDrawingVisual().ParseSqlGeometry([geometry], mapToScreen, pen, brush, visualParameters.PointSymbol);

        drawingVisual.Opacity = visualParameters.Opacity;

        return drawingVisual;
    }
}
