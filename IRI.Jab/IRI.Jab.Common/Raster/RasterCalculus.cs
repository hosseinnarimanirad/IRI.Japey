﻿using IRI.Jab.Common.Helpers;
using IRI.Jab.Common.Model.Symbology;
using IRI.Jab.Common.Raster.Model;
using IRI.Ket.DigitalImageProcessing;
using IRI.Ket.SpatialExtensions;
using IRI.Ket.SqlServerSpatialExtension.Model;
using IRI.Msh.Algebra;
using IRI.Msh.Common.Model;
using IRI.Msh.Common.Primitives;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Jab.Common.Raster
{
    public static class RasterCalculus
    {
        public static void Create(List<ISqlGeometryAware> points, Func<ISqlGeometryAware, double> valueFunc, int width, int height, Color minColor, Color maxColor, Color midColor, double? maxDistance)
        {
            var boundingBox = SqlGeometryExtensions.GetBoundingBox(points.Select(p => p.TheSqlGeometry).ToList());

            //scale
            var scaleX = width / boundingBox.Width;
            var scaleY = height / boundingBox.Height;
            var scale = Math.Min(scaleX, scaleY);

            width = (int)(scale * boundingBox.Width);
            height = (int)(scale * boundingBox.Height);

            //create empty raster
            Bitmap result = new Bitmap(width, height);

            List<Point3D> pointSet = points.Select(p => new Point3D(p.TheSqlGeometry.STX.Value, p.TheSqlGeometry.STY.Value, valueFunc(p))).ToList();

            var maxValue = pointSet.Max(p => p.Z);
            var minValue = pointSet.Min(p => p.Z);
            var rangeValue = maxValue - minValue;
            var midValue = rangeValue / 2.0 + minValue;
            //var minR = minColor.R;
            //var maxR = maxColor.R;
            //var rangeR = maxR - minR;

            //var minG = minColor.G;
            //var maxG = maxColor.G;
            //var rangeG = maxG - minG;

            //var minB = minColor.B;
            //var maxB = maxColor.B;
            //var rangeB = maxB - minB;
            ColorInterpolation step1 = new ColorInterpolation(minColor, midColor);

            ColorInterpolation step2 = new ColorInterpolation(midColor, maxColor);

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    var x = boundingBox.XMin + j / scale;
                    var y = boundingBox.YMax - i / scale;
                    var value = IRI.Msh.Common.Analysis.Interpolation.Idw.Calculate(pointSet, new Msh.Common.Primitives.Point(x, y), maxDistance);

                    //map value to color
                    //var r = (int)(minR + rangeR / rangeValue * (value - minValue));
                    //var g = (int)(minG + rangeG / rangeValue * (value - minValue));
                    //var b = (int)(minB + rangeB / rangeValue * (value - minValue));

                    Color color;

                    if (value.HasValue)
                    {

                        if (value < midValue)
                        {
                            color = step1.Interpolate(value.Value, minValue, maxValue);
                        }
                        else
                        {
                            color = step2.Interpolate(value.Value, minValue, maxValue);
                        }

                        //var color = Color.FromArgb(r, g, b);

                        result.SetPixel(j, i + 0, color); //result.SetPixel(j + 1, i + 0, color);
                    }

                    //result.SetPixel(j, i + 1, color); result.SetPixel(j + 1, i + 1, color);
                    //result.SetPixel(j, i + 2, color); result.SetPixel(j + 1, i + 2, color);
                    //result.SetPixel(j, i + 3, color); result.SetPixel(j + 1, i + 3, color);
                    //result.SetPixel(j, i + 4, color); result.SetPixel(j + 1, i + 4, color);

                    //result.SetPixel(j + 2, i + 0, color); result.SetPixel(j + 3, i + 0, color);
                    //result.SetPixel(j + 2, i + 1, color); result.SetPixel(j + 3, i + 1, color);
                    //result.SetPixel(j + 2, i + 2, color); result.SetPixel(j + 3, i + 2, color);
                    //result.SetPixel(j + 2, i + 3, color); result.SetPixel(j + 3, i + 3, color);
                    //result.SetPixel(j + 2, i + 4, color); result.SetPixel(j + 3, i + 4, color);

                    //result.SetPixel(j + 4, i + 0, color);
                    //result.SetPixel(j + 4, i + 1, color);
                    //result.SetPixel(j + 4, i + 2, color);
                    //result.SetPixel(j + 4, i + 3, color);
                    //result.SetPixel(j + 4, i + 4, color);

                }
            }

            result.Save("result.bmp");

            //return result;
        }

        public static async Task<GeoReferencedImage> Create(List<ISqlGeometryAware> points, Func<ISqlGeometryAware, double> valueFunc, int width, int height, DiscreteRangeColor ranges, double? maxDistance)
        {
            return await Task.Run<GeoReferencedImage>(() =>
            {
                var boundingBox = SqlGeometryExtensions.GetBoundingBox(points.Select(p => p.TheSqlGeometry).ToList());

                //scale
                var scaleX = width / boundingBox.Width;
                var scaleY = height / boundingBox.Height;
                var scale = Math.Min(scaleX, scaleY);

                width = (int)(scale * boundingBox.Width);
                height = (int)(scale * boundingBox.Height);

                //create empty raster

                Bitmap result = new Bitmap(width, height);


                List<Point3D> pointSet = points.Select(p => new Point3D(p.TheSqlGeometry.STX.Value, p.TheSqlGeometry.STY.Value, valueFunc(p))).ToList();

                var maxValue = pointSet.Max(p => p.Z);
                var minValue = pointSet.Min(p => p.Z);
                var rangeValue = maxValue - minValue;
                var midValue = rangeValue / 2.0 + minValue;

                //ContinousRangeColor ranges = new ContinousRangeColor(values, colors);
                //DiscreteRangeColor ranges = new DiscreteRangeColor(values, colors);

                var stopwatch = System.Diagnostics.Stopwatch.StartNew();

                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        var x = boundingBox.XMin + j / scale;
                        var y = boundingBox.YMax - i / scale;
                        var value = IRI.Msh.Common.Analysis.Interpolation.Idw.Calculate(pointSet, new Msh.Common.Primitives.Point(x, y), maxDistance);

                        if (value.HasValue)
                        {
                            try
                            {
                                result.SetPixel(j, i + 0, ranges.Interpolate(value.Value));
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        else
                        {
                            //result.SetPixel(j, i, Color.Transparent);
                        }

                    }
                }

                stopwatch.Stop();
                var ellapsedtime = stopwatch.ElapsedMilliseconds;
                stopwatch.Restart();

                return new GeoReferencedImage(ImageUtility.AsByteArray(result), boundingBox.Transform(IRI.Msh.CoordinateSystem.MapProjection.MapProjects.WebMercatorToGeodeticWgs84));
            });
        }

        public static void CreateFast(List<ISqlGeometryAware> points, Func<ISqlGeometryAware, double> valueFunc, int width, int height, List<double> values, List<Color> colors, double? maxDistance)
        {
            var boundingBox = SqlGeometryExtensions.GetBoundingBox(points.Select(p => p.TheSqlGeometry).ToList());

            //scale
            var scaleX = width / boundingBox.Width;
            var scaleY = height / boundingBox.Height;
            var scale = Math.Min(scaleX, scaleY);

            width = (int)(scale * boundingBox.Width);
            height = (int)(scale * boundingBox.Height);

            Matrix red = new Matrix(height, width);
            Matrix green = new Matrix(height, width);
            Matrix blue = new Matrix(height, width);

            //create empty raster

            Bitmap result;


            List<Point3D> pointSet = points.Select(p => new Point3D(p.TheSqlGeometry.STX.Value, p.TheSqlGeometry.STY.Value, valueFunc(p))).ToList();

            var maxValue = pointSet.Max(p => p.Z);
            var minValue = pointSet.Min(p => p.Z);
            var rangeValue = maxValue - minValue;
            var midValue = rangeValue / 2.0 + minValue;

            //ContinousRangeColor ranges = new ContinousRangeColor(values, colors);
            DiscreteRangeColor ranges = new DiscreteRangeColor(values, colors);

            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    var x = boundingBox.XMin + j / scale;
                    var y = boundingBox.YMax - i / scale;
                    var value = IRI.Msh.Common.Analysis.Interpolation.Idw.Calculate(pointSet, new Msh.Common.Primitives.Point(x, y), maxDistance);

                    //حالت transparent چی؟
                    if (value.HasValue)
                    {
                        var color = ranges.Interpolate(value.Value);

                        red[i, j] = color.R;
                        green[i, j] = color.G;
                        blue[i, j] = color.B;
                        //result.SetPixel(j, i + 0, ranges.Interpolate(value));
                    }
                }
            }
            stopwatch.Stop();
            var ellapsedtime = stopwatch.ElapsedMilliseconds;
            stopwatch.Restart();


            result = Conversion.RgbToColorImage(new RgbValues(red, green, blue));
            stopwatch.Stop();
            var ellapsedtime2 = stopwatch.ElapsedMilliseconds;
            result.Save("result.bmp");

        }

        public static void CreateForPolygon(List<ISqlGeometryAware> points, Func<ISqlGeometryAware, double> valueFunc, int width, int height, List<double> values, List<Color> colors, double? maxDistance)
        {
            var boundingBox = SqlGeometryExtensions.GetBoundingBox(points.Select(p => p.TheSqlGeometry).ToList());

            //scale
            var scaleX = width / boundingBox.Width;
            var scaleY = height / boundingBox.Height;
            var scale = Math.Min(scaleX, scaleY);

            width = (int)(scale * boundingBox.Width);
            height = (int)(scale * boundingBox.Height);

            var bitmap = new Bitmap(width, height);


            var maxValue = points.Max(p => (double.Parse(((SqlFeature)p).Attributes["Value"].ToString())));
            var minValue = points.Min(p => (double.Parse(((SqlFeature)p).Attributes["Value"].ToString())));
            var rangeValue = maxValue - minValue;
            var midValue = rangeValue / 2.0 + minValue;

            //ContinousRangeColor ranges = new ContinousRangeColor(values, colors);
            DiscreteRangeColor ranges = new DiscreteRangeColor(values, colors);

            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            var graphics = Graphics.FromImage(bitmap);

            foreach (var item in points)
            {
                var value = double.Parse((((SqlFeature)item).Attributes["Value"].ToString()));

                var color = ranges.Interpolate(value);

                var mapBound = item.TheSqlGeometry.GetBoundingBox();

                var minBitmapX = (mapBound.XMin - boundingBox.XMin) * scale;
                var maxBitmapX = (mapBound.XMax - boundingBox.XMin) * scale;

                var maxBitmapY = (boundingBox.Height - (mapBound.YMin - boundingBox.YMin)) * scale;
                var minBitmapY = (boundingBox.Height - (mapBound.YMax - boundingBox.YMin)) * scale;

                graphics.FillRectangle(new System.Drawing.SolidBrush(color), new RectangleF((float)minBitmapX, (float)minBitmapY, (float)(maxBitmapX - minBitmapX), (float)(maxBitmapY - minBitmapY)));
            }

            stopwatch.Stop();
            var ellapsedtime = stopwatch.ElapsedMilliseconds;
            stopwatch.Restart();

            bitmap.Save("result2.bmp");

        }

    }


}
