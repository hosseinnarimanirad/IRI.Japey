using System;
using System.Collections.Generic;
using System.Windows;
using drawing = System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Types;
using IRI.Jab.Cartography.Extensions;
//using sb = IRI.Ham.SpatialBase;
using System.Windows.Media;
using IRI.Ket.SpatialExtensions;
using IRI.Jab.Common.Extensions;
using IRI.Ket.SqlServerSpatialExtension.Model;
using IRI.Jab.Cartography.Model.Symbology;

namespace IRI.Jab.Cartography.Convertor
{
    public static class SqlSpatialToGdiBitmap
    {
        //const int _pointSize = 2;

        //const int _offset = (int)(_pointSize / 2.0);

        ////const int _symbolSize = 16;
        //static readonly int _symbolOffset;//= (int)(_symbolSize / 2.0);

        public static void WriteToImage(drawing.Bitmap image, List<SqlGeometry> geometries, Func<Point, Point> transform, drawing.Pen pen, drawing.Brush brush, double pointSize, SimplePointSymbol pointSymbol)
        {
            drawing.Graphics graphics = drawing.Graphics.FromImage(image);

            int p = 0;

            if (geometries != null)
            {
                foreach (SqlGeometry item in geometries)
                {
                    p += AddGeometry(graphics, item, transform, pen, brush, pointSize, pointSymbol);
                }
            }

            //return image;
        }

        public static drawing.Bitmap ParseSqlGeometry(List<SqlGeometry> geometries, double width, double height, Func<Point, Point> transform, drawing.Pen pen, drawing.Brush brush, double pointSize, SimplePointSymbol pointSymbol)
        {
            var result = new drawing.Bitmap((int)width, (int)height);

            drawing.Graphics graphics = drawing.Graphics.FromImage(result);

            int p = 0;

            if (geometries != null)
            {
                foreach (SqlGeometry item in geometries)
                {
                    p += AddGeometry(graphics, item, transform, pen, brush, pointSize, pointSymbol);
                }
            }

            return result;
        }


        private static int AddGeometry(drawing.Graphics graphics, SqlGeometry geometry, Func<Point, Point> transform, drawing.Pen pen, drawing.Brush brush, double pointSize, SimplePointSymbol pointSymbol)
        {
            if (geometry.IsNotValidOrEmpty())
                return 1;

            //CheckGeometry(geometry);
            var type = geometry.GetOpenGisType();

            switch (type)
            {
                case OpenGisGeometryType.Point:
                    AddPoint(graphics, geometry, transform, pen, brush, pointSize, pointSymbol);//, pointSymbol, symbol);
                    break;

                case OpenGisGeometryType.LineString:
                    AddLineString(graphics, geometry, transform, pen, brush);
                    break;

                case OpenGisGeometryType.Polygon:
                    AddPolygon(graphics, geometry, transform, pen, brush);
                    break;

                case OpenGisGeometryType.MultiPoint:
                    AddMultiPoint(graphics, geometry, transform, pen, brush, pointSize, pointSymbol);//, pointSymbol, symbol);
                    break;

                case OpenGisGeometryType.MultiLineString:
                    AddMultiLineString(graphics, geometry, transform, pen, brush);
                    break;

                case OpenGisGeometryType.MultiPolygon:
                    AddMultiPolygon(graphics, geometry, transform, pen, brush);
                    break;

                case OpenGisGeometryType.GeometryCollection:
                    System.Diagnostics.Trace.WriteLine($"******************WARNNING------at SqlSpatialToGdiBitmap.cs GeometryCollection escaped {new System.Diagnostics.StackTrace().GetFrame(0).GetFileLineNumber()}");
                    break;
                case OpenGisGeometryType.CircularString:
                case OpenGisGeometryType.CompoundCurve:
                case OpenGisGeometryType.CurvePolygon:
                default:
                    throw new NotImplementedException();
            }
            return 0;
        }

        private static void AddPoint(drawing.Graphics graphics, SqlGeometry point, Func<Point, Point> transform, drawing.Pen pen, drawing.Brush brush, double pointSize, SimplePointSymbol pointSymbol)
        {
            var parsedPoint = transform(point.AsWpfPoint()).AsPoint();

            if (pointSymbol?.GeometryPointSymbol != null)
            {
                GeometryHelper.Transform(graphics, pointSymbol.GeometryPointSymbol, parsedPoint, pen, brush);
            }
            else if (pointSymbol?.ImagePointSymbolGdiPlus != null)
            {
                //96.09.21
                //graphics.DrawImage(pointSymbol.ImagePointSymbol, new drawing.RectangleF((float)parsedPoint.X - _symbolOffset, (float)parsedPoint.Y - _symbolOffset, _symbolSize, _symbolSize));
                graphics.DrawImage(pointSymbol.ImagePointSymbolGdiPlus, new drawing.RectangleF((float)(parsedPoint.X - pointSymbol.SymbolWidth / 2.0), (float)(parsedPoint.Y - pointSymbol.SymbolHeight), (float)pointSymbol.SymbolWidth, (float)pointSymbol.SymbolHeight));
            }
            else
            {
                graphics.DrawEllipse(pen, (float)(parsedPoint.X - pointSymbol.SymbolWidth / 2.0), (float)(parsedPoint.Y - pointSymbol.SymbolHeight / 2.0), (float)pointSymbol.SymbolWidth, (float)pointSymbol.SymbolHeight);
            }

        }

        private static void AddMultiPoint(drawing.Graphics graphics, SqlGeometry multiPoint, Func<Point, Point> transform, drawing.Pen pen, drawing.Brush brush, double pointSize, SimplePointSymbol pointSymbol)//, ImageSource pointSymbol, Geometry symbol)
        {
            int numberOfPoints = multiPoint.STNumGeometries().Value;

            for (int i = 0; i < numberOfPoints; i++)
            {
                var point = multiPoint.STGeometryN(i + 1);

                if (point.IsNotValidOrEmpty())
                    continue;

                AddPoint(graphics, point, transform, pen, brush, pointSize, pointSymbol);
            }
        }

        private static void AddLineString(drawing.Graphics graphics, SqlGeometry lineString, Func<Point, Point> transform, drawing.Pen pen, drawing.Brush brush)
        {
            int numberOfPoints = lineString.STNumPoints().Value;

            drawing.PointF[] points = new drawing.PointF[numberOfPoints];

            //STPointN(index): index is between 1 and number of points
            for (int i = 0; i < numberOfPoints; i++)
            {
                var parsedPoint = transform(lineString.STPointN(i + 1).AsWpfPoint());

                points[i] = new drawing.PointF((float)parsedPoint.X, (float)parsedPoint.Y);
            }

            graphics.DrawLines(pen, points);
        }

        private static void AddMultiLineString(drawing.Graphics graphics, SqlGeometry multiLineString, Func<Point, Point> transform, drawing.Pen pen, drawing.Brush brush)
        {
            int numberOfLineStrings = multiLineString.STNumGeometries().Value;

            for (int i = 1; i <= numberOfLineStrings; i++)
            {
                SqlGeometry lineString = multiLineString.STGeometryN(i);

                if (lineString.IsNotValidOrEmpty())
                    continue;

                AddLineString(graphics, lineString, transform, pen, brush);
            }
        }

        private static void AddPolygonRing(drawing.Graphics graphics, SqlGeometry ring, Func<Point, Point> transform, drawing.Pen pen, drawing.Brush brush)
        {
            int numberOfPoints = ring.STNumPoints().Value;

            drawing.PointF[] points = new drawing.PointF[numberOfPoints];

            //STPointN(index): index is between 1 and number of points
            for (int i = 0; i < numberOfPoints; i++)
            {
                var parsedPoint = transform(ring.STPointN(i + 1).AsWpfPoint());

                points[i] = new drawing.PointF((float)parsedPoint.X, (float)parsedPoint.Y);
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

        private static void AddPolygon(drawing.Graphics graphics, SqlGeometry polygon, Func<Point, Point> transform, drawing.Pen pen, drawing.Brush brush)
        {
            var exteriorRing = polygon.STExteriorRing();

            AddPolygonRing(graphics, exteriorRing, transform, pen, brush);

            int numberOfInteriorRings = polygon.STNumInteriorRing().Value;

            for (int i = 0; i < numberOfInteriorRings; i++)
            {
                var ring = polygon.STInteriorRingN(i + 1);

                AddPolygonRing(graphics, ring, transform, pen, brush);
            }
        }

        private static void AddMultiPolygon(drawing.Graphics graphics, SqlGeometry multiPolygon, Func<Point, Point> transform, drawing.Pen pen, drawing.Brush brush)
        {
            int numberOfPolygons = multiPolygon.STNumGeometries().Value;

            for (int i = 0; i < numberOfPolygons; i++)
            {
                var polygon = multiPolygon.STGeometryN(i + 1);

                if (polygon.IsNotValidOrEmpty())
                    continue;

                AddPolygon(graphics, polygon, transform, pen, brush);
            }
        }


        //Labeling
        public static void DrawLabels(List<string> labels, List<SqlGeometry> geometries, drawing.Bitmap image, Func<Point, Point> mapToScreen, LabelParameters labelParameters)
        {
            if (labels.Count != geometries.Count)
                return;

            var mapCoordinates = geometries.ConvertAll(
                      (g) =>
                      {
                          return labelParameters.PositionFunc(g).AsWpfPoint();
                      }).ToList();

            var font = new drawing.Font(labelParameters.FontFamily.FamilyNames.First().Value, labelParameters.FontSize);

            var graphic = drawing.Graphics.FromImage(image);

            graphic.SmoothingMode = drawing.Drawing2D.SmoothingMode.AntiAlias;

            graphic.InterpolationMode = drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            graphic.PixelOffsetMode = drawing.Drawing2D.PixelOffsetMode.HighQuality;

            graphic.TextRenderingHint = drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;

            for (int i = 0; i < labels.Count; i++)
            {
                var location = mapToScreen(mapCoordinates[i]);

                graphic.DrawString(labels[i], font, labelParameters.Foreground.AsGdiBrush(), (float)location.X, (float)location.Y);
            }

            graphic.Flush();
        }

        public static void DrawLabels(List<NamedSqlGeometry> namedGeometries, drawing.Bitmap image, Func<Point, Point> mapToScreen, LabelParameters labelParameters)
        {
            //var mapCoordinates = geometries..ConvertAll(
            //          (g) =>
            //          {
            //              return labelParameters.PositionFunc(g).AsWpfPoint();
            //          }).ToList();

            var font = new drawing.Font(labelParameters.FontFamily.FamilyNames.First().Value, labelParameters.FontSize);

            var graphic = drawing.Graphics.FromImage(image);

            graphic.SmoothingMode = drawing.Drawing2D.SmoothingMode.AntiAlias;

            graphic.InterpolationMode = drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            graphic.PixelOffsetMode = drawing.Drawing2D.PixelOffsetMode.HighQuality;

            graphic.TextRenderingHint = drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;

            for (int i = 0; i < namedGeometries.Count; i++)
            {
                //var location = mapToScreen(mapCoordinates[i]);
                //graphic.DrawString(labels[i], font, labelParameters.Foreground.AsGdiBrush(), (float)location.X, (float)location.Y);

                var location = mapToScreen(labelParameters.PositionFunc(namedGeometries[i].Geometry).AsWpfPoint());

                graphic.DrawString(namedGeometries[i].Label ?? string.Empty, font, labelParameters.Foreground.AsGdiBrush(), (float)location.X, (float)location.Y);
            }

            graphic.Flush();
        }
    }
}
