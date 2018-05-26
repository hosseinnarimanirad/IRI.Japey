using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using Microsoft.SqlServer.Types;
using System.Windows.Media;
using System.Windows;
using IRI.Jab.Cartography.Extensions;
using sb = IRI.Sta.Common;
using IRI.Ket.SpatialExtensions;
using IRI.Jab.Common.Extensions;


namespace IRI.Jab.Cartography.Convertor
{
    class SqlSpatialToWriteableBitmap
    {
        int pointSize = 4;

        //int intBorderColor;

        //int intFillColor;

        public WriteableBitmap ParseSqlGeometry(List<SqlGeometry> geometries, Func<Point, Point> transform, int width, int height, Color border, Color fill, ImageSource pointSymbol = null, sb.Primitives.Geometry symbol = null)
        {
            //int? intBorderColor = border.HasValue ? WriteableBitmapExtensions.ConvertColor(border.Value) : (int?)null;

            //int? intFillColor = fill.HasValue ? WriteableBitmapExtensions.ConvertColor(fill.Value) : (int?)null;
            int intBorderColor = WriteableBitmapExtensions.ConvertColor(border);

            int intFillColor = WriteableBitmapExtensions.ConvertColor(fill);

            WriteableBitmap result = new WriteableBitmap(width, height, 96, 96, PixelFormats.Pbgra32, null);
            if (geometries != null)
            {
                using (result.GetBitmapContext())
                {
                    foreach (SqlGeometry item in geometries)
                    {
                        AddGeometry(result, item, transform, intBorderColor, intFillColor, pointSymbol, symbol);
                    }
                }
            }

            //result.Freeze();

            return result;
        }

        private int AddGeometry(WriteableBitmap context, SqlGeometry geometry, Func<Point, Point> transform, int border, int fill, ImageSource imageSymbol, sb.Primitives.Geometry geometrySymbol)
        {
            if (geometry.IsNotValidOrEmpty())
            {
                return 1;
            }

            var type = geometry.GetOpenGisType();

            switch (type)
            {
                case OpenGisGeometryType.Point:
                    AddPoint(context, geometry, transform, border, fill, imageSymbol, geometrySymbol);
                    break;

                case OpenGisGeometryType.LineString:
                    AddLineString(context, geometry, transform, border, fill);
                    break;

                case OpenGisGeometryType.Polygon:
                    AddPolygon(context, geometry, transform, border, fill);
                    break;

                case OpenGisGeometryType.MultiPoint:
                    AddMultiPoint(context, geometry, transform, border, fill, imageSymbol, geometrySymbol);
                    break;

                case OpenGisGeometryType.MultiLineString:
                    AddMultiLineString(context, geometry, transform, border, fill);
                    break;

                case OpenGisGeometryType.MultiPolygon:
                    AddMultiPolygon(context, geometry, transform, border, fill);
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

        private void AddPoint(WriteableBitmap context, SqlGeometry point, Func<Point, Point> transform, int border, int fill, ImageSource imageSymbol, sb.Primitives.Geometry geometrySymbol)
        {
            var center = transform(point.AsWpfPoint()).AsPoint();

            if (geometrySymbol != null)
            {
                GeometryHelper.Transform(context, geometrySymbol, center, border, fill);
            }
            else if (imageSymbol != null)
            {
                throw new NotImplementedException();
            }
            else
            {
                context.DrawEllipseCentered((int)center.X, (int)center.Y, pointSize, pointSize, border);
            }
        }

        private void AddMultiPoint(WriteableBitmap context, SqlGeometry multiPoint, Func<Point, Point> transform, int border, int fill, ImageSource imageSymbol, sb.Primitives.Geometry geometrySymbol)
        {
            int numberOfPoints = multiPoint.STNumGeometries().Value;

            for (int i = 0; i < numberOfPoints; i++)
            {
                var point = multiPoint.STGeometryN(i + 1);

                if (point.IsNotValidOrEmpty())
                    continue;

                AddPoint(context, point, transform, border, fill, imageSymbol, geometrySymbol);
            }
        }

        private void AddLineString(WriteableBitmap context, SqlGeometry lineString, Func<Point, Point> transform, int border, int fill)
        {
            int numberOfPoints = lineString.STNumPoints().Value;

            int[] points = new int[2 * numberOfPoints];

            //STPointN(index): index is between 1 and number of points
            for (int i = 0; i < numberOfPoints; i++)
            {
                var point = transform(lineString.STPointN(i + 1).AsWpfPoint());

                points[2 * i] = (int)point.X;

                points[2 * i + 1] = (int)point.Y;
            }

            //context.DrawPolyline(points, intColor);
            //if (border.HasValue)
            //{
            WriteableBitmapExtensions.DrawPolylineAa(context, points, border);
            //}
        }

        private void AddMultiLineString(WriteableBitmap context, SqlGeometry multiLineString, Func<Point, Point> transform, int border, int fill)
        {
            int numberOfLineStrings = multiLineString.STNumGeometries().Value;

            for (int i = 1; i <= numberOfLineStrings; i++)
            {
                SqlGeometry lineString = multiLineString.STGeometryN(i);

                AddLineString(context, lineString, transform, border, fill);
            }
        }

        private void AddPolygon(WriteableBitmap context, SqlGeometry polygon, Func<Point, Point> transform, int border, int fill)
        {
            var exteriorRing = polygon.STExteriorRing();

            AddPolygonRing(context, exteriorRing, transform, border, fill);

            int numberOfInteriorRings = polygon.STNumInteriorRing().Value;

            for (int i = 0; i < numberOfInteriorRings; i++)
            {
                var ring = polygon.STInteriorRingN(i + 1);

                AddPolygonRing(context, ring, transform, border, fill);
            }
        }

        private void AddPolygonRing(WriteableBitmap context, SqlGeometry polygon, Func<Point, Point> transform, int border, int fill)
        {
            int numberOfPoints = polygon.STNumPoints().Value;

            int[] points = new int[2 * numberOfPoints];

            //STPointN(index): index is between 1 and number of points
            for (int i = 0; i < numberOfPoints; i++)
            {
                var point = transform(polygon.STPointN(i + 1).AsWpfPoint());

                points[2 * i] = (int)point.X;

                points[2 * i + 1] = (int)point.Y;
            }

            //if (border.HasValue)
            //{
            WriteableBitmapExtensions.DrawPolylineAa(context, points, border);
            //}

            //if (fill.HasValue)
            //{
            WriteableBitmapExtensions.FillPolygon(context, points, fill);
            //}

        }

        private void AddMultiPolygon(WriteableBitmap context, SqlGeometry multiPolygon, Func<Point, Point> transform, int border, int fill)
        {
            int numberOfLineStrings = multiPolygon.STNumGeometries().Value;

            for (int i = 1; i <= numberOfLineStrings; i++)
            {
                SqlGeometry polygon = multiPolygon.STGeometryN(i);

                AddPolygon(context, polygon, transform, border, fill);
            }
        }

    }
}
