using System;
using System.Linq;
using System.Collections.Generic;

using IRI.Extensions;
using IRI.Sta.Common.Primitives;
using IRI.Sta.Spatial.Primitives;

using Drawing = System.Drawing;
using WpfPoint = System.Windows.Point;
using Point = IRI.Sta.Common.Primitives.Point;
using IRI.Jab.Common.Cartography.Symbologies;
using IRI.Sta.Common.Services;

namespace IRI.Jab.Common.Convertor;

public static class GdiBitmapRenderer
{
    static readonly Drawing.SolidBrush _labelBackground = new Drawing.SolidBrush(Drawing.Color.FromArgb(150, 255, 255, 255));

    #region Geometry to GdiBitmap

    public static void WriteToImage(Drawing.Bitmap image, List<Feature<Point>> features, /*Func<WpfPoint, WpfPoint> transform,*/ Drawing.Pen pen, Drawing.Brush brush, SimplePointSymbolizer pointSymbol)
    {
        if (features.IsNullOrEmpty())
            return;

        Drawing.Graphics graphics = Drawing.Graphics.FromImage(image);

        int p = 0;

        foreach (var item in features)
        {
            p += AddGeometry(graphics, item.TheGeometry, /*transform,*/ pen, brush, pointSymbol);
        } 
    }

    public static Drawing.Bitmap ParseSqlGeometry(List<Feature<Point>> features, double width, double height, /*Func<WpfPoint, WpfPoint> transform,*/ Drawing.Pen pen, Drawing.Brush brush, SimplePointSymbolizer pointSymbol)
    {
        var result = new Drawing.Bitmap((int)width, (int)height);

        if (features.IsNullOrEmpty())
            return result;

        Drawing.Graphics graphics = Drawing.Graphics.FromImage(result);

        int p = 0;

        foreach (var item in features)
        {
            p += AddGeometry(graphics, item.TheGeometry, /*transform,*/ pen, brush, pointSymbol);
        }
         
        return result;
    }

    internal static Drawing.Bitmap ParseSqlGeometry(
      List<Feature<Point>> features,
      double width,
      double height,
      //Func<WpfPoint, WpfPoint> mapToScreen,
      Func<Feature<Point>, VisualParameters> symbologyRule)
    {
        var result = new Drawing.Bitmap((int)width, (int)height);

        Drawing.Graphics graphics = Drawing.Graphics.FromImage(result);

        int p = 0;

        if (features != null)
        {
            foreach (var item in features)
            {
                var symbology = symbologyRule(item);

                var pen = symbology.GetGdiPlusPen(symbology.Opacity);

                var brush = symbology.GetGdiPlusFillBrush(symbology.Opacity);

                GdiBitmapRenderer.WriteToImage(graphics, item.TheGeometry, /*mapToScreen,*/ pen, brush, symbology.PointSymbol);
            }
        }

        return result;
    }

    internal static void WriteToImage(Drawing.Graphics graphics, Geometry<Point> geometry, /*Func<WpfPoint, WpfPoint> transform,*/ Drawing.Pen pen, Drawing.Brush brush, SimplePointSymbolizer pointSymbol)
    {
        if (geometry != null)
        {
            AddGeometry(graphics, geometry, /*transform,*/ pen, brush, pointSymbol);
        }
    }


    private static int AddGeometry(Drawing.Graphics graphics, Geometry<Point> geometry, /*Func<WpfPoint, WpfPoint> transform,*/ Drawing.Pen pen, Drawing.Brush brush, SimplePointSymbolizer pointSymbol)
    {
        if (geometry.IsNotValidOrEmpty())
            return 1;

        switch (geometry.Type)
        {
            case GeometryType.Point:
                AddPoint(graphics, geometry, /*transform,*/ pen, brush, pointSymbol);//, pointSymbol, symbol);
                break;

            case GeometryType.LineString:
                AddLineString(graphics, geometry, /*transform,*/ pen, brush);
                break;

            case GeometryType.Polygon:
                AddPolygon(graphics, geometry, /*transform,*/ pen, brush);
                break;

            case GeometryType.MultiPoint:
                AddMultiPoint(graphics, geometry, /*transform,*/ pen, brush, pointSymbol);//, pointSymbol, symbol);
                break;

            case GeometryType.MultiLineString:
                AddMultiLineString(graphics, geometry, /*transform,*/ pen, brush);
                break;

            case GeometryType.MultiPolygon:
                AddMultiPolygon(graphics, geometry, /*transform,*/ pen, brush);
                break;

            case GeometryType.GeometryCollection:
                AddGeometryCollection(graphics, geometry, /*transform,*/ pen, brush, pointSymbol);
                break;

            case GeometryType.CircularString:
            case GeometryType.CompoundCurve:
            case GeometryType.CurvePolygon:
            default:
                throw new NotImplementedException();
        }
        return 0;
    }

    private static void AddPoint(Drawing.Graphics graphics, Geometry<Point> point, /*Func<WpfPoint, WpfPoint> transform,*/ Drawing.Pen pen, Drawing.Brush brush, SimplePointSymbolizer pointSymbol)
    {
        var parsedPoint = /*transform*/(point.AsWpfPoint()).AsPoint();

        if (pointSymbol?.GeometryPointSymbol != null)
        {
            GeometryHelper.Transform(graphics, pointSymbol.GeometryPointSymbol, parsedPoint, pen, brush);
        }
        else if (pointSymbol?.ImagePointSymbolGdiPlus != null)
        {
            //96.09.21
            //graphics.DrawImage(pointSymbol.ImagePointSymbol, new drawing.RectangleF((float)parsedPoint.X - _symbolOffset, (float)parsedPoint.Y - _symbolOffset, _symbolSize, _symbolSize));
            graphics.DrawImage(pointSymbol.ImagePointSymbolGdiPlus, new Drawing.RectangleF((float)(parsedPoint.X - pointSymbol.SymbolWidth / 2.0), (float)(parsedPoint.Y - pointSymbol.SymbolHeight), (float)pointSymbol.SymbolWidth, (float)pointSymbol.SymbolHeight));
        }
        else
        {
            if (pen != null)
            {
                graphics.DrawEllipse(pen, (float)(parsedPoint.X - pointSymbol.SymbolWidth / 2.0), (float)(parsedPoint.Y - pointSymbol.SymbolHeight / 2.0), (float)pointSymbol.SymbolWidth, (float)pointSymbol.SymbolHeight);
            }
            if (brush != null)
            {
                graphics.FillEllipse(brush, (float)(parsedPoint.X - pointSymbol.SymbolWidth / 2.0), (float)(parsedPoint.Y - pointSymbol.SymbolHeight / 2.0), (float)pointSymbol.SymbolWidth, (float)pointSymbol.SymbolHeight);
            }
        }
    }

    private static void AddMultiPoint(Drawing.Graphics graphics, Geometry<Point> multiPoint, /*Func<WpfPoint, WpfPoint> transform,*/ Drawing.Pen pen, Drawing.Brush brush, SimplePointSymbolizer pointSymbol)//, ImageSource pointSymbol, Geometry symbol)
    {
        int numberOfPoints = multiPoint.NumberOfGeometries;

        for (int i = 0; i < numberOfPoints; i++)
        {
            var point = multiPoint.Geometries[i];

            if (point.IsNotValidOrEmpty())
                continue;

            AddPoint(graphics, point, /*transform,*/ pen, brush, pointSymbol);
        }
    }

    private static void AddLineString(Drawing.Graphics graphics, Geometry<Point> lineString, /*Func<WpfPoint, WpfPoint> transform,*/ Drawing.Pen pen, Drawing.Brush brush)
    {
        int numberOfPoints = lineString.NumberOfPoints;

        Drawing.PointF[] points = new Drawing.PointF[numberOfPoints];

        //STPointN(index): index is between 1 and number of points
        for (int i = 0; i < numberOfPoints; i++)
        {
            var parsedPoint = /*transform*/(lineString.Points[i].AsWpfPoint());

            points[i] = new Drawing.PointF((float)parsedPoint.X, (float)parsedPoint.Y);
        }

        graphics.DrawLines(pen, points);
    }

    private static void AddMultiLineString(Drawing.Graphics graphics, Geometry<Point> multiLineString, /*Func<WpfPoint, WpfPoint> transform,*/ Drawing.Pen pen, Drawing.Brush brush)
    {
        int numberOfLineStrings = multiLineString.NumberOfGeometries;

        for (int i = 0; i < numberOfLineStrings; i++)
        {
            var lineString = multiLineString.Geometries[i];

            if (lineString.IsNotValidOrEmpty())
                continue;

            AddLineString(graphics, lineString, /*transform,*/ pen, brush);
        }
    }

    private static void AddPolygonRing(Drawing.Graphics graphics, Geometry<Point> ring, /*Func<WpfPoint, WpfPoint> transform,*/ Drawing.Pen pen, Drawing.Brush brush)
    {
        int numberOfPoints = ring.NumberOfPoints;

        Drawing.PointF[] points = new Drawing.PointF[numberOfPoints];

        //STPointN(index): index is between 1 and number of points
        for (int i = 0; i < numberOfPoints; i++)
        {
            var parsedPoint = /*transform*/(ring.Points[i].AsWpfPoint());

            points[i] = new Drawing.PointF((float)parsedPoint.X, (float)parsedPoint.Y);
        }

        if (pen != null)
        {
            graphics.DrawPolygon(pen, points);
        }

        if (brush != null)
        {
            graphics.FillPolygon(brush, points);
        }
    }

    private static void AddPolygon(Drawing.Graphics graphics, Geometry<Point> polygon, /*Func<WpfPoint, WpfPoint> transform,*/ Drawing.Pen pen, Drawing.Brush brush)
    {
        //var exteriorRing = polygon.STExteriorRing();

        //AddPolygonRing(graphics, exteriorRing, /*transform,*/ pen, brush);

        //int numberOfInteriorRings = polygon.STNumInteriorRing().Value;

        //for (int i = 0; i < numberOfInteriorRings; i++)
        //{
        //    var ring = polygon.STInteriorRingN(i + 1);

        //    AddPolygonRing(graphics, ring, /*transform,*/ pen, brush);
        //}

        int numberOfInteriorRings = polygon.NumberOfGeometries;

        for (int i = 0; i < numberOfInteriorRings; i++)
        {
            var ring = polygon.Geometries[i];

            AddPolygonRing(graphics, ring, /*transform,*/ pen, brush);
        }
    }

    private static void AddMultiPolygon(Drawing.Graphics graphics, Geometry<Point> multiPolygon, /*Func<WpfPoint, WpfPoint> transform,*/ Drawing.Pen pen, Drawing.Brush brush)
    {
        int numberOfPolygons = multiPolygon.NumberOfGeometries;

        for (int i = 0; i < numberOfPolygons; i++)
        {
            var polygon = multiPolygon.Geometries[i];

            if (polygon.IsNotValidOrEmpty())
                continue;

            AddPolygon(graphics, polygon, /*transform,*/ pen, brush);
        }
    }

    private static void AddGeometryCollection(Drawing.Graphics graphics, Geometry<Point> multiPolygon, /*Func<WpfPoint, WpfPoint> transform,*/ Drawing.Pen pen, Drawing.Brush brush, SimplePointSymbolizer pointSymbol)
    {
        int numberOfPolygons = multiPolygon.NumberOfGeometries;

        for (int i = 0; i < numberOfPolygons; i++)
        {
            var polygon = multiPolygon.Geometries[i];

            if (polygon.IsNotValidOrEmpty())
                continue;

            AddGeometry(graphics, polygon, /*transform,*/ pen, brush, pointSymbol);
        }
    }

    //Labeling
    public static void DrawLabels(List<Feature<Point>> features, Drawing.Bitmap image, /*Func<WpfPoint, WpfPoint> mapToScreen,*/ LabelParameters labelParameters)
    {
        if (features.IsNullOrEmpty())
            return;

        var mapCoordinates = features.ConvertAll(
                  (g) =>
                  {
                      return labelParameters.PositionFunc(g.TheGeometry).AsWpfPoint();
                  }).ToList();

        var font = new Drawing.Font(labelParameters.FontFamily.FamilyNames.First().Value, labelParameters.FontSize, Drawing.FontStyle.Bold);

        var graphic = Drawing.Graphics.FromImage(image);

        graphic.SmoothingMode = Drawing.Drawing2D.SmoothingMode.AntiAlias;

        graphic.InterpolationMode = Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

        graphic.PixelOffsetMode = Drawing.Drawing2D.PixelOffsetMode.HighQuality;

        graphic.TextRenderingHint = Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;

        var brush = labelParameters.Foreground.AsGdiBrush();

        System.Drawing.StringFormat format = new Drawing.StringFormat();

        if (labelParameters.IsRtl)
        {
            format.FormatFlags = Drawing.StringFormatFlags.DirectionRightToLeft;
        }

        for (int i = 0; i < features.Count; i++)
        {
            var location = /*mapToScreen*/mapCoordinates[i];

            var stringSize = graphic.MeasureString(features[i].Label, font);

            Drawing.PointF locationF = new Drawing.PointF((float)(location.X - stringSize.Width / 2.0), (float)(location.Y - stringSize.Height / 2.0));

            var rectangleF = labelParameters.IsRtl ?
                 new Drawing.RectangleF(locationF.X - stringSize.Width, locationF.Y, stringSize.Width, stringSize.Height) :
                 new Drawing.RectangleF(locationF.X, locationF.Y, stringSize.Width, stringSize.Height);

            graphic.FillRectangle(_labelBackground, rectangleF);

            graphic.DrawString(features[i].Label, font, brush, locationF, format);

            //graphic.DrawString(labels[i], font, brush, (float)(location.X - stringSize.Width / 2.0), (float)(location.Y - stringSize.Height / 2.0), format);
        }

        graphic.Flush();

        graphic.Dispose();
        brush.Dispose();
    }

    #endregion

    #region Old Codes

    //public static void DrawLabels(List<NamedGeometry> namedGeometries, drawing.Bitmap image, Func<WpfPoint, WpfPoint> mapToScreen, LabelParameters labelParameters)
    //{
    //    var font = new drawing.Font(labelParameters.FontFamily.FamilyNames.First().Value, labelParameters.FontSize);

    //    var graphic = drawing.Graphics.FromImage(image);

    //    graphic.SmoothingMode = drawing.Drawing2D.SmoothingMode.AntiAlias;

    //    graphic.InterpolationMode = drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

    //    graphic.PixelOffsetMode = drawing.Drawing2D.PixelOffsetMode.HighQuality;

    //    graphic.TextRenderingHint = drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;

    //    var brush = labelParameters.Foreground.AsGdiBrush();

    //    System.Drawing.StringFormat format = new drawing.StringFormat();

    //    if (labelParameters.IsRtl)
    //    {
    //        format.FormatFlags = drawing.StringFormatFlags.DirectionRightToLeft;
    //    }

    //    for (int i = 0; i < namedGeometries.Count; i++)
    //    {
    //        var location = mapToScreen(labelParameters.PositionFunc(namedGeometries[i].TheGeometry).AsWpfPoint());

    //        var stringSize = graphic.MeasureString(namedGeometries[i].Label, font);

    //        var locationF = new drawing.PointF((float)(location.X - stringSize.Width / 2.0), (float)(location.Y - stringSize.Height / 2.0));

    //        graphic.FillRectangle(_labelBackground, new drawing.RectangleF((float)(location.X - 3.0 / 2.0 * stringSize.Width), (float)(location.Y - stringSize.Height / 2.0), stringSize.Width, stringSize.Height));

    //        graphic.DrawString(namedGeometries[i].Label ?? string.Empty, font, brush, locationF, format);
    //    }

    //    graphic.Flush();

    //    graphic.Dispose();
    //    brush.Dispose();
    //}

    //public static void DrawLabels(List<NamedSqlGeometry> namedGeometries, drawing.Bitmap image, Func<WpfPoint, WpfPoint> mapToScreen, LabelParameters labelParameters)
    //{
    //    //var mapCoordinates = geometries..ConvertAll(
    //    //          (g) =>
    //    //          {
    //    //              return labelParameters.PositionFunc(g).AsWpfPoint();
    //    //          }).ToList();

    //    var font = new drawing.Font(labelParameters.FontFamily.FamilyNames.First().Value, labelParameters.FontSize);

    //    var graphic = drawing.Graphics.FromImage(image);

    //    graphic.SmoothingMode = drawing.Drawing2D.SmoothingMode.AntiAlias;

    //    graphic.InterpolationMode = drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

    //    graphic.PixelOffsetMode = drawing.Drawing2D.PixelOffsetMode.HighQuality;

    //    graphic.TextRenderingHint = drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;

    //    for (int i = 0; i < namedGeometries.Count; i++)
    //    {
    //        //var location = mapToScreen(mapCoordinates[i]);
    //        //graphic.DrawString(labels[i], font, labelParameters.Foreground.AsGdiBrush(), (float)location.X, (float)location.Y);

    //        var location = mapToScreen(labelParameters.PositionFunc(namedGeometries[i].TheSqlGeometry.AsGeometry()).AsWpfPoint());

    //        System.Drawing.StringFormat format = new drawing.StringFormat();

    //        if (labelParameters.IsRtl)
    //        {
    //            format.FormatFlags = drawing.StringFormatFlags.DirectionRightToLeft;
    //        }

    //        var stringSize = graphic.MeasureString(namedGeometries[i].Label, font);

    //        graphic.DrawString(namedGeometries[i].Label ?? string.Empty, font, labelParameters.Foreground.AsGdiBrush(), (float)(location.X - stringSize.Width / 2.0), (float)(location.Y - stringSize.Height / 2.0), format);
    //    }

    //    graphic.Flush();
    //}

    #endregion

    #region SqlGeometry to GdiBitmap

    //public static void WriteToImage(Drawing.Bitmap image, List<SqlGeometry> geometries, /*Func<WpfPoint, WpfPoint> transform,*/ Drawing.Pen pen, Drawing.Brush brush, SimplePointSymbolizer pointSymbol)
    //{
    //    Drawing.Graphics graphics = Drawing.Graphics.FromImage(image);

    //    int p = 0;

    //    if (geometries != null)
    //    {
    //        foreach (SqlGeometry item in geometries)
    //        {
    //            p += AddGeometry(graphics, item, /*transform,*/ pen, brush, pointSymbol);
    //        }
    //    }

    //    //return image;
    //}

    //internal static void WriteToImage(Drawing.Graphics graphics, SqlGeometry geometry, /*Func<WpfPoint, WpfPoint> transform,*/ Drawing.Pen pen, Drawing.Brush brush, SimplePointSymbolizer pointSymbol)
    //{
    //    if (geometry != null)
    //    {
    //        AddGeometry(graphics, geometry, /*transform,*/ pen, brush, pointSymbol);
    //    }
    //}

    //public static Drawing.Bitmap ParseSqlGeometry(List<SqlGeometry> geometries, double width, double height, /*Func<WpfPoint, WpfPoint> transform,*/ Drawing.Pen pen, Drawing.Brush brush, SimplePointSymbolizer pointSymbol)
    //{
    //    var result = new Drawing.Bitmap((int)width, (int)height);

    //    Drawing.Graphics graphics = Drawing.Graphics.FromImage(result);

    //    int p = 0;

    //    if (geometries != null)
    //    {
    //        foreach (SqlGeometry item in geometries)
    //        {
    //            p += AddGeometry(graphics, item, /*transform,*/ pen, brush, pointSymbol);
    //        }
    //    }

    //    return result;
    //}


    //private static int AddGeometry(Drawing.Graphics graphics, SqlGeometry geometry, /*Func<WpfPoint, WpfPoint> transform,*/ Drawing.Pen pen, Drawing.Brush brush, SimplePointSymbolizer pointSymbol)
    //{
    //    if (geometry.IsNotValidOrEmpty())
    //        return 1;

    //    //CheckGeometry(geometry);
    //    var type = geometry.GetOpenGisType();

    //    switch (type)
    //    {
    //        case OpenGisGeometryType.Point:
    //            AddPoint(graphics, geometry, /*transform,*/ pen, brush, pointSymbol);//, pointSymbol, symbol);
    //            break;

    //        case OpenGisGeometryType.LineString:
    //            AddLineString(graphics, geometry, /*transform,*/ pen, brush);
    //            break;

    //        case OpenGisGeometryType.Polygon:
    //            AddPolygon(graphics, geometry, /*transform,*/ pen, brush);
    //            break;

    //        case OpenGisGeometryType.MultiPoint:
    //            AddMultiPoint(graphics, geometry, /*transform,*/ pen, brush, pointSymbol);//, pointSymbol, symbol);
    //            break;

    //        case OpenGisGeometryType.MultiLineString:
    //            AddMultiLineString(graphics, geometry, /*transform,*/ pen, brush);
    //            break;

    //        case OpenGisGeometryType.MultiPolygon:
    //            AddMultiPolygon(graphics, geometry, /*transform,*/ pen, brush);
    //            break;

    //        case OpenGisGeometryType.GeometryCollection:
    //            System.Diagnostics.Trace.WriteLine($"******************WARNNING------at SqlSpatialToGdiBitmap.cs GeometryCollection escaped {new System.Diagnostics.StackTrace().GetFrame(0).GetFileLineNumber()}");
    //            break;
    //        case OpenGisGeometryType.CircularString:
    //        case OpenGisGeometryType.CompoundCurve:
    //        case OpenGisGeometryType.CurvePolygon:
    //        default:
    //            throw new NotImplementedException();
    //    }
    //    return 0;
    //}

    //private static void AddPoint(Drawing.Graphics graphics, SqlGeometry point, /*Func<WpfPoint, WpfPoint> transform,*/ Drawing.Pen pen, Drawing.Brush brush, SimplePointSymbolizer pointSymbol)
    //{
    //    var parsedPoint = transform(point.AsWpfPoint()).AsPoint();

    //    if (pointSymbol?.GeometryPointSymbol != null)
    //    {
    //        GeometryHelper.Transform(graphics, pointSymbol.GeometryPointSymbol, parsedPoint, pen, brush);
    //    }
    //    else if (pointSymbol?.ImagePointSymbolGdiPlus != null)
    //    {
    //        //96.09.21
    //        //graphics.DrawImage(pointSymbol.ImagePointSymbol, new drawing.RectangleF((float)parsedPoint.X - _symbolOffset, (float)parsedPoint.Y - _symbolOffset, _symbolSize, _symbolSize));
    //        graphics.DrawImage(pointSymbol.ImagePointSymbolGdiPlus, new Drawing.RectangleF((float)(parsedPoint.X - pointSymbol.SymbolWidth / 2.0), (float)(parsedPoint.Y - pointSymbol.SymbolHeight), (float)pointSymbol.SymbolWidth, (float)pointSymbol.SymbolHeight));
    //    }
    //    else
    //    {
    //        if (pen != null)
    //        {
    //            graphics.DrawEllipse(pen, (float)(parsedPoint.X - pointSymbol.SymbolWidth / 2.0), (float)(parsedPoint.Y - pointSymbol.SymbolHeight / 2.0), (float)pointSymbol.SymbolWidth, (float)pointSymbol.SymbolHeight);
    //        }
    //        if (brush != null)
    //        {
    //            graphics.FillEllipse(brush, (float)(parsedPoint.X - pointSymbol.SymbolWidth / 2.0), (float)(parsedPoint.Y - pointSymbol.SymbolHeight / 2.0), (float)pointSymbol.SymbolWidth, (float)pointSymbol.SymbolHeight);
    //        }
    //    }
    //}

    //private static void AddMultiPoint(Drawing.Graphics graphics, SqlGeometry multiPoint, /*Func<WpfPoint, WpfPoint> transform,*/ Drawing.Pen pen, Drawing.Brush brush, SimplePointSymbolizer pointSymbol)//, ImageSource pointSymbol, Geometry symbol)
    //{
    //    int numberOfPoints = multiPoint.STNumGeometries().Value;

    //    for (int i = 0; i < numberOfPoints; i++)
    //    {
    //        var point = multiPoint.STGeometryN(i + 1);

    //        if (point.IsNotValidOrEmpty())
    //            continue;

    //        AddPoint(graphics, point, /*transform,*/ pen, brush, pointSymbol);
    //    }
    //}

    //private static void AddLineString(Drawing.Graphics graphics, SqlGeometry lineString, /*Func<WpfPoint, WpfPoint> transform,*/ Drawing.Pen pen, Drawing.Brush brush)
    //{
    //    int numberOfPoints = lineString.STNumPoints().Value;

    //    Drawing.PointF[] points = new Drawing.PointF[numberOfPoints];

    //    //STPointN(index): index is between 1 and number of points
    //    for (int i = 0; i < numberOfPoints; i++)
    //    {
    //        var parsedPoint = transform(lineString.STPointN(i + 1).AsWpfPoint());

    //        points[i] = new Drawing.PointF((float)parsedPoint.X, (float)parsedPoint.Y);
    //    }

    //    graphics.DrawLines(pen, points);
    //}

    //private static void AddMultiLineString(Drawing.Graphics graphics, SqlGeometry multiLineString, /*Func<WpfPoint, WpfPoint> transform,*/ Drawing.Pen pen, Drawing.Brush brush)
    //{
    //    int numberOfLineStrings = multiLineString.STNumGeometries().Value;

    //    for (int i = 1; i <= numberOfLineStrings; i++)
    //    {
    //        SqlGeometry lineString = multiLineString.STGeometryN(i);

    //        if (lineString.IsNotValidOrEmpty())
    //            continue;

    //        AddLineString(graphics, lineString, /*transform,*/ pen, brush);
    //    }
    //}

    //private static void AddPolygonRing(Drawing.Graphics graphics, SqlGeometry ring, /*Func<WpfPoint, WpfPoint> transform,*/ Drawing.Pen pen, Drawing.Brush brush)
    //{
    //    int numberOfPoints = ring.STNumPoints().Value;

    //    Drawing.PointF[] points = new Drawing.PointF[numberOfPoints];

    //    //STPointN(index): index is between 1 and number of points
    //    for (int i = 0; i < numberOfPoints; i++)
    //    {
    //        var parsedPoint = transform(ring.STPointN(i + 1).AsWpfPoint());

    //        points[i] = new Drawing.PointF((float)parsedPoint.X, (float)parsedPoint.Y);
    //    }

    //    if (pen != null)
    //    {
    //        graphics.DrawPolygon(pen, points);
    //    }

    //    if (brush != null)
    //    {
    //        graphics.FillPolygon(brush, points);
    //    }
    //}

    //private static void AddPolygon(Drawing.Graphics graphics, SqlGeometry polygon, /*Func<WpfPoint, WpfPoint> transform,*/ Drawing.Pen pen, Drawing.Brush brush)
    //{
    //    var exteriorRing = polygon.STExteriorRing();

    //    AddPolygonRing(graphics, exteriorRing, /*transform,*/ pen, brush);

    //    int numberOfInteriorRings = polygon.STNumInteriorRing().Value;

    //    for (int i = 0; i < numberOfInteriorRings; i++)
    //    {
    //        var ring = polygon.STInteriorRingN(i + 1);

    //        AddPolygonRing(graphics, ring, /*transform,*/ pen, brush);
    //    }
    //}

    //private static void AddMultiPolygon(Drawing.Graphics graphics, SqlGeometry multiPolygon, /*Func<WpfPoint, WpfPoint> transform,*/ Drawing.Pen pen, Drawing.Brush brush)
    //{
    //    int numberOfPolygons = multiPolygon.STNumGeometries().Value;

    //    for (int i = 0; i < numberOfPolygons; i++)
    //    {
    //        var polygon = multiPolygon.STGeometryN(i + 1);

    //        if (polygon.IsNotValidOrEmpty())
    //            continue;

    //        AddPolygon(graphics, polygon, /*transform,*/ pen, brush);
    //    }
    //}


    ////Labeling
    //public static void DrawLabels(List<string> labels, List<SqlGeometry> geometries, Drawing.Bitmap image, Func<WpfPoint, WpfPoint> mapToScreen, LabelParameters labelParameters)
    //{
    //    if (labels.Count != geometries.Count)
    //        return;

    //    var mapCoordinates = geometries.ConvertAll(
    //              (g) =>
    //              {
    //                  return labelParameters.PositionFunc(g.AsGeometry()).AsWpfPoint();
    //              }).ToList();

    //    var font = new Drawing.Font(labelParameters.FontFamily.FamilyNames.First().Value, labelParameters.FontSize, Drawing.FontStyle.Bold);

    //    var graphic = Drawing.Graphics.FromImage(image);

    //    graphic.SmoothingMode = Drawing.Drawing2D.SmoothingMode.AntiAlias;

    //    graphic.InterpolationMode = Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

    //    graphic.PixelOffsetMode = Drawing.Drawing2D.PixelOffsetMode.HighQuality;

    //    graphic.TextRenderingHint = Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;

    //    for (int i = 0; i < labels.Count; i++)
    //    {
    //        var location = mapToScreen(mapCoordinates[i]);

    //        System.Drawing.StringFormat format = new Drawing.StringFormat();

    //        if (labelParameters.IsRtl)
    //        {
    //            format.FormatFlags = Drawing.StringFormatFlags.DirectionRightToLeft;
    //        }

    //        var stringSize = graphic.MeasureString(labels[i], font);

    //        graphic.DrawString(labels[i], font, labelParameters.Foreground.AsGdiBrush(), (float)(location.X - stringSize.Width / 2.0), (float)(location.Y - stringSize.Height / 2.0), format);
    //    }

    //    graphic.Flush();
    //}

    #endregion

}
