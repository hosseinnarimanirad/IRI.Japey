using IRI.Ket.DigitalImageProcessing;
using IRI.Ket.SpatialExtensions;
using IRI.Ket.SqlServerSpatialExtension.Model;
using IRI.Msh.Algebra;
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
        public static void Create(List<ISqlGeometryAware> points, Func<ISqlGeometryAware, double> valueFunc, int width, int height, Color minColor, Color maxColor, Color midColor)
        {
            var boundingBox = SqlSpatialExtensions.GetBoundingBox(points.Select(p => p.TheSqlGeometry).ToList());

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
                    var value = IRI.Msh.Common.Analysis.Interpolation.Idw.Calculate(pointSet, new Msh.Common.Primitives.Point(x, y));

                    //map value to color
                    //var r = (int)(minR + rangeR / rangeValue * (value - minValue));
                    //var g = (int)(minG + rangeG / rangeValue * (value - minValue));
                    //var b = (int)(minB + rangeB / rangeValue * (value - minValue));

                    Color color;

                    if (value < midValue)
                    {
                        color = step1.Interpolate(value, minValue, maxValue);
                    }
                    else
                    {
                        color = step2.Interpolate(value, minValue, maxValue);
                    }

                    //var color = Color.FromArgb(r, g, b);


                    result.SetPixel(j, i + 0, color); //result.SetPixel(j + 1, i + 0, color);
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

        public static void Create(List<ISqlGeometryAware> points, Func<ISqlGeometryAware, double> valueFunc, int width, int height, List<double> values, List<Color> colors)
        {
            var boundingBox = SqlSpatialExtensions.GetBoundingBox(points.Select(p => p.TheSqlGeometry).ToList());

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
            DiscreteRangeColor ranges = new DiscreteRangeColor(values, colors);

            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    var x = boundingBox.XMin + j / scale;
                    var y = boundingBox.YMax - i / scale;
                    var value = IRI.Msh.Common.Analysis.Interpolation.Idw.Calculate(pointSet, new Msh.Common.Primitives.Point(x, y));

                    result.SetPixel(j, i + 0, ranges.Interpolate(value));
                }
            }

            stopwatch.Stop();
            var ellapsedtime = stopwatch.ElapsedMilliseconds;
            stopwatch.Restart();

            result.Save("result.bmp");

        }

        public static void CreateFast(List<ISqlGeometryAware> points, Func<ISqlGeometryAware, double> valueFunc, int width, int height, List<double> values, List<Color> colors)
        {
            var boundingBox = SqlSpatialExtensions.GetBoundingBox(points.Select(p => p.TheSqlGeometry).ToList());

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
                    var value = IRI.Msh.Common.Analysis.Interpolation.Idw.Calculate(pointSet, new Msh.Common.Primitives.Point(x, y));

                    var color = ranges.Interpolate(value);

                    red[i, j] = color.R;
                    green[i, j] = color.G;
                    blue[i, j] = color.B;

                    //result.SetPixel(j, i + 0, ranges.Interpolate(value));
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

        public static void CreateForPolygon(List<ISqlGeometryAware> points, Func<ISqlGeometryAware, double> valueFunc, int width, int height, List<double> values, List<Color> colors)
        {
            var boundingBox = SqlSpatialExtensions.GetBoundingBox(points.Select(p => p.TheSqlGeometry).ToList());

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

    public class DiscreteRangeColor
    {
        public double MinValue { get; }

        public double MaxValue { get; }

        public List<double> MidValues { get; }

        public List<Color> Colors { get; }

        public DiscreteRangeColor(List<double> values, List<Color> colors)
        {
            if (values.Count - 1 != colors.Count)
            {
                throw new NotImplementedException();
            }

            MidValues = values;

            this.Colors = colors;
        }

        public Color Interpolate(double value)
        {
            var index = MidValues.IndexOf(MidValues.First(v => v >= value));

            return Colors[index == 0 ? 0 : index - 1];
        }
    }

    public class ContinousRangeColor
    {
        public double MinValue { get; }

        public double MaxValue { get; }

        public List<double> MidValues { get; }

        public List<Color> Colors { get; }

        public List<ColorInterpolation> Interpolations { get; private set; }

        public ContinousRangeColor(List<double> values, List<Color> colors)
        {
            if (values.Count - 1 != colors.Count)
            {
                throw new NotImplementedException();
            }

            MinValue = values.First();

            MaxValue = values.Last();

            MidValues = values;

            Interpolations = new List<ColorInterpolation>();

            for (int i = 0; i < colors.Count - 1; i++)
            {
                Interpolations.Add(new ColorInterpolation(colors[i], colors[i + 1]));
            }
        }

        public Color Interpolate(double value)
        {
            var index = MidValues.IndexOf(MidValues.First(v => v >= value));

            index = index == Interpolations.Count ? Interpolations.Count - 1 : index;

            return Interpolations[index].Interpolate(value, MidValues[index - 1], MidValues[index]);
        }
    }

    public class ColorInterpolation
    {
        Color _minColor, _maxColor;

        int rangeR, rangeB, rangeG;

        public ColorInterpolation(Color minColor, Color maxColor)
        {
            _minColor = minColor;

            _maxColor = maxColor;

            rangeR = maxColor.R - minColor.R;
            rangeG = maxColor.G - minColor.G;
            rangeB = maxColor.B - minColor.B;

        }

        public Color Interpolate(double value, double minValue, double maxValue)
        {
            var r = (int)(_minColor.R + rangeR / (maxValue - minValue) * (value - minValue));
            var g = (int)(_minColor.G + rangeG / (maxValue - minValue) * (value - minValue));
            var b = (int)(_minColor.B + rangeB / (maxValue - minValue) * (value - minValue));

            return Color.FromArgb(r, g, b);

        }

    }
}
