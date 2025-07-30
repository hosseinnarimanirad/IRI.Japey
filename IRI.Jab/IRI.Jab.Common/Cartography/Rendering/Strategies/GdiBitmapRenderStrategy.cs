using System;
using System.Linq;
using System.Collections.Generic;

using IRI.Extensions;
using IRI.Sta.Common.Primitives;
using IRI.Sta.Spatial.Primitives;

using Drawing = System.Drawing;
using Point = IRI.Sta.Common.Primitives.Point;
using IRI.Jab.Common.Cartography.Symbologies;
using System.Windows.Media;
using IRI.Jab.Common.Cartography.Rendering.Helpers;
using IRI.Jab.Common.Helpers;
using System.Windows.Media.Imaging;
using DocumentFormat.OpenXml.Drawing.Charts;


namespace IRI.Jab.Common.Cartography.Rendering;

public class GdiBitmapRenderStrategy : RenderStrategy
{
    static readonly Drawing.SolidBrush _labelBackground = new Drawing.SolidBrush(Drawing.Color.FromArgb(150, 255, 255, 255));

    public GdiBitmapRenderStrategy(List<ISymbolizer> symbolizer) : base(symbolizer)
    {
    }

    public override ImageBrush? Render(List<Feature<Point>> features, double mapScale, double screenWidth, double screenHeight)
    {
        var bitmap = AsGdiBitmap(features, screenWidth, screenHeight, mapScale);

        if (bitmap is null)
            return null;

        BitmapImage image = ImageUtility.AsBitmapImage(bitmap, System.Drawing.Imaging.ImageFormat.Png);

        bitmap.Dispose();

        image.Freeze();

        return new ImageBrush(image);

    }

    public System.Drawing.Bitmap? AsGdiBitmap(List<Feature<Point>> features, double imageWidth, double imageHeight, double mapScale)
    {
        if (features.IsNullOrEmpty())
            return null;

        Drawing.Bitmap image = new Drawing.Bitmap((int)imageWidth, (int)imageHeight);

        using (Drawing.Graphics graphics = Drawing.Graphics.FromImage(image))
        {
            foreach (var symbolizer in _symbolizers)
            { 
                // check scale
                if (!symbolizer.IsInScaleRange(mapScale))
                    continue;

                // filter features
                var filteredFeatures = features.Where(symbolizer.IsFilterPassed).ToList();

                switch (symbolizer)
                {
                    case SimplePointSymbolizer simplePointSymbolizer:
                        break;

                    case SimpleSymbolizer simpleSymbolizer:

                        ParseSqlGeometry(
                            graphics,
                            filteredFeatures,
                            imageWidth,
                            imageHeight,
                            //mapToScreen,
                            simpleSymbolizer.Param.GetGdiPlusPen(),
                            simpleSymbolizer.Param.Fill.AsGdiBrush(),
                            simpleSymbolizer.Param.PointSymbol);

                        if (image is null)
                            return null;

                        break;

                    case LabelSymbolizer labelSymbolizer:
                        if (labelSymbolizer.Labels?.IsLabeled(1.0 / mapScale) == true)
                        {
                             DrawLabels(filteredFeatures, graphics, /*mapToScreen, */labelSymbolizer.Labels);
                        }
                        break;

                    default:
                        break;
                }

            }
        }
         
        return image;
    }


    public void WriteToImage(Drawing.Bitmap image, List<Feature<Point>> features, /*Func<WpfPoint, WpfPoint> transform,*/ Drawing.Pen pen, Drawing.Brush brush, SimplePointSymbolizer pointSymbol)
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

    public void ParseSqlGeometry(Drawing.Graphics graphics, List<Feature<Point>> features, double width, double height, /*Func<WpfPoint, WpfPoint> transform,*/ Drawing.Pen pen, Drawing.Brush brush, SimplePointSymbolizer pointSymbol)
    {
        //var result = new Drawing.Bitmap((int)width, (int)height);

        if (features.IsNullOrEmpty())
            return /*result*/;

        //Drawing.Graphics graphics = Drawing.Graphics.FromImage(result);

        int p = 0;

        foreach (var item in features)
        {
            p += AddGeometry(graphics, item.TheGeometry, /*transform,*/ pen, brush, pointSymbol);
        }

        return /*result*/;
    }

    //public Drawing.Bitmap ParseSqlGeometry(List<Feature<Point>> features, double width, double height, /*Func<WpfPoint, WpfPoint> transform,*/ Drawing.Pen pen, Drawing.Brush brush, SimplePointSymbolizer pointSymbol)
    //{
    //    var result = new Drawing.Bitmap((int)width, (int)height);

    //    if (features.IsNullOrEmpty())
    //        return result;

    //    Drawing.Graphics graphics = Drawing.Graphics.FromImage(result);

    //    int p = 0;

    //    foreach (var item in features)
    //    {
    //        p += AddGeometry(graphics, item.TheGeometry, /*transform,*/ pen, brush, pointSymbol);
    //    }

    //    return result;
    //}

    internal Drawing.Bitmap ParseSqlGeometry(
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

                WriteToImage(graphics, item.TheGeometry, /*mapToScreen,*/ pen, brush, symbology.PointSymbol);
            }
        }

        return result;
    }

    internal void WriteToImage(Drawing.Graphics graphics, Geometry<Point> geometry, /*Func<WpfPoint, WpfPoint> transform,*/ Drawing.Pen pen, Drawing.Brush brush, SimplePointSymbolizer pointSymbol)
    {
        if (geometry != null)
        {
            AddGeometry(graphics, geometry, /*transform,*/ pen, brush, pointSymbol);
        }
    }


    private int AddGeometry(Drawing.Graphics graphics, Geometry<Point> geometry, /*Func<WpfPoint, WpfPoint> transform,*/ Drawing.Pen pen, Drawing.Brush brush, SimplePointSymbolizer pointSymbol)
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

    private void AddPoint(Drawing.Graphics graphics, Geometry<Point> point, /*Func<WpfPoint, WpfPoint> transform,*/ Drawing.Pen pen, Drawing.Brush brush, SimplePointSymbolizer pointSymbol)
    {
        var parsedPoint = /*transform*/point.AsWpfPoint().AsPoint();

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

    private void AddMultiPoint(Drawing.Graphics graphics, Geometry<Point> multiPoint, /*Func<WpfPoint, WpfPoint> transform,*/ Drawing.Pen pen, Drawing.Brush brush, SimplePointSymbolizer pointSymbol)//, ImageSource pointSymbol, Geometry symbol)
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

    private void AddLineString(Drawing.Graphics graphics, Geometry<Point> lineString, /*Func<WpfPoint, WpfPoint> transform,*/ Drawing.Pen pen, Drawing.Brush brush)
    {
        int numberOfPoints = lineString.NumberOfPoints;

        Drawing.PointF[] points = new Drawing.PointF[numberOfPoints];

        //STPointN(index): index is between 1 and number of points
        for (int i = 0; i < numberOfPoints; i++)
        {
            var parsedPoint = /*transform*/lineString.Points[i].AsWpfPoint();

            points[i] = new Drawing.PointF((float)parsedPoint.X, (float)parsedPoint.Y);
        }

        graphics.DrawLines(pen, points);
    }

    private void AddMultiLineString(Drawing.Graphics graphics, Geometry<Point> multiLineString, /*Func<WpfPoint, WpfPoint> transform,*/ Drawing.Pen pen, Drawing.Brush brush)
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

    private void AddPolygonRing(Drawing.Graphics graphics, Geometry<Point> ring, /*Func<WpfPoint, WpfPoint> transform,*/ Drawing.Pen pen, Drawing.Brush brush)
    {
        int numberOfPoints = ring.NumberOfPoints;

        Drawing.PointF[] points = new Drawing.PointF[numberOfPoints];

        //STPointN(index): index is between 1 and number of points
        for (int i = 0; i < numberOfPoints; i++)
        {
            var parsedPoint = /*transform*/ring.Points[i].AsWpfPoint();

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

    private void AddPolygon(Drawing.Graphics graphics, Geometry<Point> polygon, /*Func<WpfPoint, WpfPoint> transform,*/ Drawing.Pen pen, Drawing.Brush brush)
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

    private void AddMultiPolygon(Drawing.Graphics graphics, Geometry<Point> multiPolygon, /*Func<WpfPoint, WpfPoint> transform,*/ Drawing.Pen pen, Drawing.Brush brush)
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

    private void AddGeometryCollection(Drawing.Graphics graphics, Geometry<Point> multiPolygon, /*Func<WpfPoint, WpfPoint> transform,*/ Drawing.Pen pen, Drawing.Brush brush, SimplePointSymbolizer pointSymbol)
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
    public void DrawLabels(List<Feature<Point>> features, Drawing.Graphics graphic, /*Func<WpfPoint, WpfPoint> mapToScreen,*/ LabelParameters labelParameters)
    {
        if (features.IsNullOrEmpty())
            return;

        var mapCoordinates = features.ConvertAll(
                  (g) =>
                  {
                      return labelParameters.PositionFunc(g.TheGeometry).AsWpfPoint();
                  }).ToList();

        var font = new Drawing.Font(labelParameters.FontFamily.FamilyNames.First().Value, labelParameters.FontSize, Drawing.FontStyle.Bold);

        //var graphic = Drawing.Graphics.FromImage(image);

        graphic.SmoothingMode = Drawing.Drawing2D.SmoothingMode.AntiAlias;

        graphic.InterpolationMode = Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

        graphic.PixelOffsetMode = Drawing.Drawing2D.PixelOffsetMode.HighQuality;

        graphic.TextRenderingHint = Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;

        var brush = labelParameters.Foreground.AsGdiBrush();

        Drawing.StringFormat format = new Drawing.StringFormat();

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

}
