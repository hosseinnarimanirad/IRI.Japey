﻿using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Collections.Generic;

using IRI.Maptor.Extensions;
using IRI.Maptor.Sta.Common.Primitives;
using IRI.Maptor.Sta.Spatial.Primitives;
using IRI.Maptor.Jab.Common.Models;
using IRI.Maptor.Jab.Common.Helpers;
using IRI.Maptor.Jab.Common.Cartography.Helpers;
using IRI.Maptor.Jab.Common.Cartography.Symbologies;

using WpfPoint = System.Windows.Point;
using Point = IRI.Maptor.Sta.Common.Primitives.Point;

namespace IRI.Maptor.Jab.Common.Cartography.RenderingStrategies;

public class DrawingVisualRenderStrategy : RenderStrategy
{
    double pointSymbolWidth = 4, pointSymbolHeight = 4;

    double pointSymbolMinX, pointSymbolMinY;

    private readonly Pen _defaultPen = new Pen(Brushes.Black, 1);
    private readonly Brush _defaultBrush = Brushes.Gray;

    public DrawingVisualRenderStrategy(List<ISymbolizer> symbolizer) : base(symbolizer)
    {
    }

    public override ImageBrush? Render(List<Feature<Point>> features, double mapScale, double screenWidth, double screenHeight)
    {
        if (features.IsNullOrEmpty())
            return null;

        var drawingVisuals = AsDrawingVisual(features, mapScale);

        var image = ImageUtility.Render(drawingVisuals, (int)screenWidth, (int)screenHeight);

        return new ImageBrush(image);
    }

    public List<DrawingVisual> AsDrawingVisual(List<Feature<Point>> features, double mapScale)
    {
        var result = new List<DrawingVisual>();

        foreach (var symbolizer in _symbolizers)
        {
            // check scale
            if (!symbolizer.IsInScaleRange(mapScale))
                continue;

            // filter features
            var filteredFeatures = features.Where(symbolizer.IsFilterPassed).ToList();

            //var visualParameters = symbolizer.Get();

            // check symbology type
            switch (symbolizer)
            {
                case SimplePointSymbolizer simplePointSymbolizer:
                    break;

                case SimpleSymbolizer simpleSymbolizer:

                    var pen = simpleSymbolizer.Param.GetWpfPen();

                    if (pen is not null)
                    {
                        pen.LineJoin = PenLineJoin.Round;
                        pen.EndLineCap = PenLineCap.Round;
                        pen.StartLineCap = PenLineCap.Round;
                    }

                    Brush brush = simpleSymbolizer.Param.Fill;

                    DrawingVisual drawingVisual = ParseGeometry(
                                                    filteredFeatures,
                                                    pen,
                                                    brush,
                                                    simpleSymbolizer.Param.PointSymbol);

                    drawingVisual.Opacity = simpleSymbolizer.Param.Opacity;

                    result.Add(drawingVisual);

                    break;

                case LabelSymbolizer labelSymbolizer:

                    if (labelSymbolizer.Labels?.IsLabeled(1.0 / mapScale) == true)
                    {
                        var renderedLabels = DrawLabels(filteredFeatures, labelSymbolizer.Labels);

                        if (renderedLabels is not null)
                        {
                            result.Add(renderedLabels);
                        }
                    }

                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        return result;
    }

    private DrawingVisual ParseGeometry(List<Feature<Point>> features, Pen? pen, Brush? brush, SimplePointSymbolizer? pointSymbol)
    {
        DrawingVisual result = new DrawingVisual();

        if (features.IsNullOrEmpty())
            return result;

        if (pen != null)
            pen.Freeze();

        if (brush != null)
            brush.Freeze();

        if (pointSymbol?.GeometrySymbol != null)
        {
            var rect = pointSymbol.GeometrySymbol.GetRenderBounds(new Pen(new SolidColorBrush(Colors.Black), 0));

            pointSymbolWidth = rect.Width;

            pointSymbolHeight = rect.Height;

            pointSymbolMinX = rect.BottomLeft.X;

            pointSymbolMinY = rect.BottomLeft.Y;
        }

        using (DrawingContext context = result.RenderOpen())
        {
            foreach (var item in features)
            {
                AddGeometry(context, item.TheGeometry, brush, pen, pointSymbol);
            }
        }

        return result;
    }

    #region Private Methods

    private void AddGeometry(DrawingContext context, Geometry<Point> geometry, Brush? brush, Pen? pen, SimplePointSymbolizer? pointSymbol)
    {
        if (geometry.IsNotValidOrEmpty())
            return;

        switch (geometry.Type)
        {
            case GeometryType.Point:
                AddPoint(context, geometry, brush, pen, pointSymbol);
                break;

            case GeometryType.LineString:
                AddLineString(context, geometry, pen ?? _defaultPen);
                break;

            case GeometryType.Polygon:
                AddPolygon(context, geometry, brush, pen);
                break;

            case GeometryType.MultiPoint:
                AddMultiPoint(context, geometry, brush, pen, pointSymbol);
                break;

            case GeometryType.MultiLineString:
                AddMultiLineString(context, geometry, pen ?? _defaultPen);
                break;

            case GeometryType.MultiPolygon:
                AddMultiPolygon(context, geometry, brush, pen);
                break;

            case GeometryType.GeometryCollection:
                AddGeometryCollection(context, geometry, brush, pen, pointSymbol);
                break;

            case GeometryType.CircularString:
            case GeometryType.CompoundCurve:
            case GeometryType.CurvePolygon:
            default:
                break;
        }

        return;
    }

    private void AddLineString(DrawingContext context, Geometry<Point> lineString, Pen pen)
    {
        int numberOfPoints = lineString.NumberOfPoints;

        for (int i = 0; i < numberOfPoints - 1; i++)
        {
            var start = lineString.Points[i].AsWpfPoint();

            var end = lineString.Points[i + 1].AsWpfPoint();

            context.DrawLine(pen, start, end);
        }
    }

    private void AddMultiLineString(DrawingContext context, Geometry<Point> multiLineString, Pen pen)
    {
        int numberOfLineStrings = multiLineString.NumberOfGeometries;

        for (int i = 0; i < numberOfLineStrings; i++)
        {
            var lineString = multiLineString.Geometries[i];

            AddLineString(context, lineString, pen);
        }
    }

    //todo: performance can be enhanced, using DrawLine method for polygon drawing
    private void AddPolygon(DrawingContext context, Geometry<Point> polygon, Brush? brush, Pen? pen)
    {
        //There is no DrawPolygon method for DrawingContext so we should get the Geometry and use the DrawGeometry method
        var geometry = StreamGeometryRenderer.ParseSqlGeometry(new List<Feature<Point>>() { polygon.AsFeature() });

        context.DrawGeometry(brush, pen, geometry);
    }

    private void AddMultiPolygon(DrawingContext context, Geometry<Point> multiPolygon, Brush? brush, Pen? pen)
    {
        //There is no DrawPolygon method for DrawingContext so we should get the Geometry and use the DrawGeometry method
        var geometry = StreamGeometryRenderer.ParseSqlGeometry(new List<Feature<Point>>() { multiPolygon.AsFeature() });

        context.DrawGeometry(brush, pen, geometry);
    }

    private void AddGeometryCollection(DrawingContext context, Geometry<Point> multiLineString, Brush? brush, Pen? pen, SimplePointSymbolizer? pointSymbol)
    {
        int numberOfLineStrings = multiLineString.NumberOfGeometries;

        for (int i = 0; i < numberOfLineStrings; i++)
        {
            var lineString = multiLineString.Geometries[i];

            AddGeometry(context, lineString, brush, pen, pointSymbol);
        }
    }

    private void AddPoint(DrawingContext context, Geometry<Point> point, Brush? brush, Pen? pen, SimplePointSymbolizer? pointSymbol)
    {
        var symbolWidth = pointSymbol?.SymbolWidth ?? pointSymbolWidth;
        var symbolHeight = pointSymbol?.SymbolHeight ?? pointSymbolHeight;

        var location = point.AsWpfPoint();

        if (pointSymbol?.GeometrySymbol != null)
        {
            var geometry = StreamGeometryRenderer.Transform(
                                pointSymbol.GeometrySymbol,
                                new WpfPoint(location.X - pointSymbolMinX - symbolWidth / 2.0, location.Y - pointSymbolMinY + symbolHeight / 2.0));

            context.DrawGeometry(brush, pen, geometry);
        }
        else if (pointSymbol?.ImageSymbol != null)
        {
            context.DrawImage(pointSymbol.ImageSymbol, new Rect(location.X, location.Y, symbolWidth, symbolHeight));
        }
        else
        {
            context.DrawEllipse(brush, pen, location, symbolWidth, symbolHeight);
        }
    }

    private void AddMultiPoint(DrawingContext context, Geometry<Point> multiPoint, Brush? brush, Pen? pen, SimplePointSymbolizer? pointSymbol)
    {
        int numberOfPoints = multiPoint.NumberOfGeometries;

        for (int i = 0; i < numberOfPoints; i++)
        {
            var point = multiPoint.Geometries[i];

            AddPoint(context, point, brush, pen, pointSymbol);
        }
    }

    private DrawingVisual? DrawLabels(List<Feature<Point>> features, LabelParameters labels)
    {
        if (features.IsNullOrEmpty())
            return null;

        var mapCoordinates = features.ConvertAll(
                  (g) =>
                  {
                      var point = labels.PositionFunc(g.TheGeometry);
                      return new WpfPoint(point.Points[0].X, point.Points[0].Y);
                  }).ToList();

        DrawingVisual drawingVisual = new DrawingVisual();

        using (DrawingContext drawingContext = drawingVisual.RenderOpen())
        {
            var typeface = new Typeface(labels.FontFamily, FontStyles.Normal, FontWeights.Normal, FontStretches.Normal);

            var flowDirection = labels.IsRtl ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;

            var culture = System.Globalization.CultureInfo.CurrentCulture;

            var backgroundBrush = new SolidColorBrush(Color.FromArgb(200, 255, 255, 255));

            for (int i = 0; i < features.Count; i++)
            {
                var label = features[i].Label;

                if (string.IsNullOrEmpty(label))
                    continue;

                FormattedText formattedText =
                    new FormattedText(label,
                                        culture,
                                        flowDirection,
                                        typeface,
                                        labels.FontSize,
                                        labels.Foreground,
                                        pixelsPerDip: 96);

                WpfPoint location = mapCoordinates[i];

                var temp = new WpfPoint(location.X - formattedText.Width * 1.5, location.Y - formattedText.Height / 2.0);

                if (flowDirection == FlowDirection.LeftToRight)
                {
                    drawingContext.DrawRectangle(backgroundBrush, null, new Rect(location, new Size(formattedText.Width, formattedText.Height)));
                }
                else
                {
                    drawingContext.DrawRectangle(backgroundBrush, null, new Rect(temp, new Size(formattedText.Width, formattedText.Height)));
                }

                drawingContext.DrawText(formattedText, new WpfPoint(location.X - formattedText.Width / 2.0, location.Y - formattedText.Height / 2.0));
            }
        }

        return drawingVisual;
    }

    #endregion
}
