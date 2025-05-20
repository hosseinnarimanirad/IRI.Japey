using System.IO;
using IRI.Extensions;
using System.Diagnostics;
using IRI.Sta.ShapefileFormat;
using IRI.Sta.Spatial.Helpers;
using IRI.Sta.Spatial.Analysis;
using IRI.Sta.Spatial.Primitives;
using IRI.Sta.Common.Primitives;
using IRI.Sta.SpatialReferenceSystem;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using IRI.Sta.Spatial.Model.GeoJsonFormat;
using IRI.Jab.Common;
using System.Windows.Media.Media3D;

namespace IRI.Article04.FastSimplification;

public static class SimplificationHelper
{
    private static bool retain3Points = false;

    public async static Task GeneralTest()
    {
        // OSM dataset directory should be addressed here
        var dataFolder = @"E:\University.Ph.D\Sample Data\OSM-1401-03-03-WebMercator";
        //var dataFolder = @"E:\University.Ph.D\Sample Data\OSM-1404-02-29-WebMercator-Lake";

        var files = Directory.EnumerateFiles(dataFolder, "*.shp", SearchOption.AllDirectories);

        // Output directory should be addressed here
        var writeFolder = $@"E:\University.Ph.D\5. ISPRS Conf Paper\AlgorithmRun\{DateTime.Now.Date:yyyy-MM-dd}";

        if (!Directory.Exists(writeFolder))
            _ = Directory.CreateDirectory(writeFolder);

        StreamWriter writer = new StreamWriter($"{writeFolder}\\summary.txt", false);
        writer.WriteLine(SpeedLog.GetHeader());

        List<string> fileNames = ["AdminArea_WM", "Building_WM", "Landuse_WM", "Railway_WM"];

        var oldfiles = Directory.EnumerateFiles(writeFolder, "*.txt", SearchOption.AllDirectories)
                                .Select(s => System.IO.Path.GetFileNameWithoutExtension(s).Replace("-summary", string.Empty));

        string logFile = $"{writeFolder}\\Log-Visual-{DateTime.Now:yyyy-MM-dd HH-mm-ss}.txt";

        Stopwatch watch = Stopwatch.StartNew();

        foreach (var file in files)
        {
            var fileName = System.IO.Path.GetFileNameWithoutExtension(file);

            if (fileNames.Contains(fileName) || oldfiles.Contains(fileName))
                continue;

            fileNames.Add(fileName);

            await ProcessVisualQuality(file, writer, writeFolder);

            File.AppendAllLines(logFile, new List<string>() { $"Finished At: {DateTime.Now.ToLongTimeString()}; Ellapsed: {watch.ElapsedMilliseconds / 1000.0:N0000} (s) - ({fileName})" });
        }

        writer.Close();
        writer.Dispose();
    }

    private static void ProcessSpeed(string shpFile, StreamWriter writer, string outputDirectory)
    {
        var fileName = $"{Path.GetFileNameWithoutExtension(shpFile)}";

        //******************************************************
        //***************** read features **********************
        var features = Shapefile.ReadShapes(shpFile)
                                .Select(g => g.AsGeometry())
                                .SelectMany(g => g.Split(false))
                                .Where(g => !g.IsNullOrEmpty())
                                .Where(g => g.TotalNumberOfPoints > 10)
                                .ToList();

        foreach (var feature in features)
            feature.RemoveConsecutiveDuplicatePoints();

        features = features.Where(f => !f.HasDuplicatePoints()).ToList();

        List<SimplificationType> methods = new List<SimplificationType>()
        {
            SimplificationType.RamerDouglasPeucker,

            SimplificationType.VisvalingamWhyatt,

            SimplificationType.NormalOpeningWindow,
            SimplificationType.BeforeOpeningWindow,

            SimplificationType.CumulativeTriangleRoutine,
        };

        Dictionary<SimplificationType, string> methodNames = methods.ToDictionary(m => m, m => m.GetDescription());

        var groundBoundingBox = features.GetBoundingBox();

        // screen size: 4096x4096
        var estimatedZoomLevel = WebMercatorUtility.EstimateZoomLevel(groundBoundingBox, /*34,*/ 8000, 8000);

        // screen size      scale
        // 8k           =>  1:1,155,581
        // 4096         =>  1:2,300,000
        var scale = WebMercatorUtility.GetGoogleMapScale(estimatedZoomLevel);

        var currentScreenSize = WebMercatorUtility.ToScreenSize(estimatedZoomLevel, groundBoundingBox);

        // threshold values is set to 0.5 pixel
        var threshold = WebMercatorUtility.ToWebMercatorLength(estimatedZoomLevel, 0.5);

        var parameters = new SimplificationParamters() { AreaThreshold = threshold * threshold, DistanceThreshold = threshold, Retain3Points = retain3Points };

        //var originalVectorLayer = GeneralHelper.GetAsLayer("original", features);
        //var originalBitmap = await originalVectorLayer.ParseToBitmapImage(groundBoundingBox, currentScreenSize.Width, currentScreenSize.Height, scale);
        //originalBitmap.Save($"{outputDirectory}\\{fileName}-{estimatedZoomLevel}-original.png", System.Drawing.Imaging.ImageFormat.Tiff);


        Stopwatch stopwatch = Stopwatch.StartNew();

        List<int> featureCount = [1000, 10_000, 50_000, 100_000, 200_000, 300_000];

        foreach (var method in methods)
        {
            foreach (var count in featureCount)
            {
                if (count > features.Count)
                    break;

                var theFeatures = features.Take(count).ToList();

                stopwatch.Restart();

                var simplifiedFeatures = theFeatures.Simplify(method, parameters, reduceToPoint: false);

                stopwatch.Stop();

                if (simplifiedFeatures.IsNullOrEmpty())
                    continue;

                writer.WriteLine(
                  new SpeedLog(fileName,
                                methodNames[method],
                                theFeatures,
                                simplifiedFeatures,
                                stopwatch.ElapsedMilliseconds,
                                parameters).ToTsv());

                //var vectorLayer = GeneralHelper.GetAsLayer("original", [theFeatures]);
                //var simplifiedBitmap = await vectorLayer.ParseToBitmapImage(groundBoundingBox, currentScreenSize.Width, currentScreenSize.Height, scale);
                //simplifiedBitmap.Save($"{outputDirectory}\\{fileName}-{estimatedZoomLevel}-{method}-[{count}].png", System.Drawing.Imaging.ImageFormat.Tiff);
            }

            writer.Flush();
        }

    }

    private static async Task ProcessVisualQuality(string shpFile, StreamWriter writer, string outputDirectory)
    {
        var fileName = $"{Path.GetFileNameWithoutExtension(shpFile)}";

        //******************************************************
        //***************** read features **********************
        var features = Shapefile.ReadShapes(shpFile)
                                .Select(g => g.AsGeometry())
                                .SelectMany(g => g.Split(false))
                                .Where(g => !g.IsNullOrEmpty())
                                .Where(g => g.TotalNumberOfPoints > 30)
                                .ToList();

        foreach (var feature in features)
            feature.RemoveConsecutiveDuplicatePoints();

        features = features.Where(f => !f.HasDuplicatePoints()).ToList();

        List<SimplificationType> methods = new List<SimplificationType>()
        {
            SimplificationType.RamerDouglasPeucker,

            SimplificationType.VisvalingamWhyatt,

            SimplificationType.NormalOpeningWindow,
            SimplificationType.BeforeOpeningWindow,

            SimplificationType.CumulativeTriangleRoutine,
        };

        Dictionary<SimplificationType, string> methodNames = methods.ToDictionary(m => m, m => m.GetDescription());

        var groundBoundingBox = features.GetBoundingBox();

        var estimatedZoomLevel = WebMercatorUtility.EstimateZoomLevel(groundBoundingBox, /*34,*/ 4096, 4096);

        var scale = WebMercatorUtility.GetGoogleMapScale(estimatedZoomLevel);

        var currentScreenSize = WebMercatorUtility.ToScreenSize(estimatedZoomLevel, groundBoundingBox);

        // threshold values is set to 0.5 pixel
        var threshold = WebMercatorUtility.ToWebMercatorLength(estimatedZoomLevel, 0.5);

        var parameters = new SimplificationParamters() { AreaThreshold = threshold * threshold, DistanceThreshold = threshold, Retain3Points = retain3Points };

        var originalVectorLayer = GeneralHelper.GetAsLayer("original", features);

        var originalBitmap = await originalVectorLayer.ParseToBitmapImage(groundBoundingBox, currentScreenSize.Width, currentScreenSize.Height, scale);

        originalBitmap.Save($"{outputDirectory}\\{fileName}-{estimatedZoomLevel}-original.png", System.Drawing.Imaging.ImageFormat.Tiff);


        foreach (var feature in features)
        {
            foreach (var method in methods)
            {
                var simplifiedFeatures = feature.Simplify(method, parameters);

                // writer.WriteLine(
                //new SpeedLog(fileName,
                //              methodNames[method],
                //              theFeatures,
                //              simplifiedFeatures,
                //              stopwatch.ElapsedMilliseconds,
                //              parameters).ToTsv());

                //var vectorLayer = GeneralHelper.GetAsLayer("original", [theFeatures]);

                //var simplifiedBitmap = await vectorLayer.ParseToBitmapImage(groundBoundingBox, currentScreenSize.Width, currentScreenSize.Height, scale);

                //simplifiedBitmap.Save($"{outputDirectory}\\{fileName}-{estimatedZoomLevel}-{method}-[{count}].png", System.Drawing.Imaging.ImageFormat.Tiff);
            }

            writer.Flush();
        }


    }

    #region Test with visual output

    public static async Task TestWithOutput(string shpFile, StreamWriter writer, bool includeHeader, string outputDirectory)
    {
        var fileName = $"{Path.GetFileNameWithoutExtension(shpFile)}";

        var groundResolutionCoefs = new List<double>()
        {
            //1.0 / 6,
            1.0 / 5,
            1.0 / 4,
            //1.0 / 3,
            1.0 / 2,
            1,
            2,
            //3,
            4,
            5,
            //6
        };

        var groundResolutionCoefs_Range = groundResolutionCoefs.Max() - groundResolutionCoefs.Min();
        var groundResolutionCoefs_Min = groundResolutionCoefs.Min();

        //******************************************************
        //***************** read features **********************
        var shapes = Shapefile.ReadShapes(shpFile);

        var features = shapes
                                .Select(g => g.AsGeometry())
                                .SelectMany(g => g.NumberOfGeometries > 1 ? g.Split(false) : new List<Geometry<Point>>() { g })
                                .Where(g => !g.IsNullOrEmpty())
                                .Where(g => g.TotalNumberOfPoints > 15 && g.TotalNumberOfPoints < 500)
                                .ToList();

        //var isRingbase = shapes[0].IsRingBase();

        foreach (var feature in features)
        {
            //if (isRingbase)
            //{
            //    feature.CloseLineString();
            //}

            feature.Srid = SridHelper.WebMercator;
            feature.RemoveConsecutiveDuplicatePoints();
        }

        features = features.Where(f => !f.HasDuplicatePoints()).ToList();

        List<SimplificationType> methods = new List<SimplificationType>()
        {
            //SimplificationType.EuclideanDistance,

            SimplificationType.RamerDouglasPeucker,
            //SimplificationType.ReumannWitkam,

            //SimplificationType.PerpendicularDistance,

            //SimplificationType.VisvalingamWhyatt,
            SimplificationType.SleeveFitting,

            //SimplificationType.NormalOpeningWindow,
            //SimplificationType.BeforeOpeningWindow,

            //SimplificationType.TriangleRoutine,
        };

        var colros = new Dictionary<SimplificationType, System.Windows.Media.Color>()
        {
            {SimplificationType.EuclideanDistance, System.Windows.Media.Colors.DarkGray},
            {SimplificationType.RamerDouglasPeucker, Jab.Common.Helpers.ColorHelper.ToWpfColor("#E051AD")},
            {SimplificationType.ReumannWitkam, System.Windows.Media.Colors.Navy},
            {SimplificationType.PerpendicularDistance, System.Windows.Media.Colors.Blue},
            {SimplificationType.VisvalingamWhyatt, System.Windows.Media.Colors.Yellow},
            {SimplificationType.SleeveFitting, Jab.Common.Helpers.ColorHelper.ToWpfColor("#3B878C")},
            {SimplificationType.NormalOpeningWindow, System.Windows.Media.Colors.Brown},
            {SimplificationType.BeforeOpeningWindow, System.Windows.Media.Colors.Orange},
            {SimplificationType.TriangleRoutine, System.Windows.Media.Colors.Gray}
        };

        int featureIndex = 0;

        bool addHeaderLine = includeHeader;

        foreach (var feature in features)
        {
            featureIndex++;

            //if (!CheckZoom2(featureIndex, fileName)) continue;

            var outputDirectoryForFeature = $"{outputDirectory}\\{fileName}\\{featureIndex}";

            if (!Directory.Exists(outputDirectoryForFeature))
                Directory.CreateDirectory(outputDirectoryForFeature);

            var boundingBox = feature.GetBoundingBox().Expand(1.1);

            var estimatedZoomLevel = WebMercatorUtility.EstimateZoomLevel(boundingBox, /*34,*/ 256, 256);

            var webMercatorResolution = WebMercatorUtility.ToWebMercatorLength(estimatedZoomLevel, 10);

            var originalFrame = feature.AsDrawingVisual(VisualParameters.Get(Colors.Transparent, Jab.Common.Helpers.ColorHelper.ToWpfColor("#C0C0C0"), 4, 1), estimatedZoomLevel, boundingBox);

            //var originalFrame = await GetAsFrame(
            //    "original",
            //    estimatedZoomLevel,
            //    boundingBox,
            //    feature,
            //    Jab.Common.Helpers.ColorHelper.ToWpfColor("#C0C0C0"),
            //    4,
            //    1);

            GeoJsonFeatureSet originalFeatureSet = feature.AsGeoJsonFeatureSet();

            originalFeatureSet.Save($"{outputDirectoryForFeature}\\{fileName}-{featureIndex}-{estimatedZoomLevel}-original.json", false, true);


            foreach (var coef in groundResolutionCoefs)
            {
                var threshold = webMercatorResolution * coef;

                var parameters = new SimplificationParamters() { AreaThreshold = threshold * threshold, DistanceThreshold = threshold, Retain3Points = retain3Points };

                //var temp = new NineAlgoSixMeasureLog
                //            (fileName,
                //                        boundingBox,
                //                        featureIndex,
                //                        feature,
                //                        estimatedZoomLevel,
                //                        //methodNames[method],
                //                        //stopwatch.ElapsedMilliseconds,
                //                        parameters,
                //                        coef);

                //List<System.Drawing.Bitmap> images = new List<System.Drawing.Bitmap>();
                List<DrawingVisual> drawingVisuals = new List<DrawingVisual>
                {
                    originalFrame
                };

                foreach (var method in methods)
                {
                    //stopwatch.Restart();

                    var simplified = feature.Simplify(method, parameters);

                    //stopwatch.Stop();

                    if (simplified.IsNullOrEmpty())
                        continue;

                    if (simplified.Type == GeometryType.Point)
                        continue;

                    //temp.AddAlgoResult(method, feature, simplified);

                    //var simplifiedFrame = await GetAsFrame(method.ToString(), estimatedZoomLevel, boundingBox, simplified, colros[method], 2, 0.9);
                    var simplifiedFrame = simplified.AsDrawingVisual(VisualParameters.Get(Colors.Transparent, colros[method], 2, 0.9), estimatedZoomLevel, boundingBox);

                    drawingVisuals.Add(simplifiedFrame);

                    GeoJsonFeatureSet featureSet = simplified.AsGeoJsonFeatureSet();

                    featureSet.Save($"{outputDirectoryForFeature}\\{fileName}-{featureIndex}-{estimatedZoomLevel}-{coef}-{method}.json", false, true);
                }

                //Save($"{outputDirectoryForFeature}\\{fileName}-{featureIndex}-{estimatedZoomLevel}-{coef}.png", drawingVisuals, estimatedZoomLevel, boundingBox);
                var screenSize = WebMercatorUtility.ToScreenSize(estimatedZoomLevel, boundingBox);

                IRI.Jab.Common.Helpers.ImageUtility.MergeAndSave($"{outputDirectoryForFeature}\\{fileName}-{featureIndex}-{estimatedZoomLevel}-{coef}.png", drawingVisuals, screenSize.Width, screenSize.Height);

                //temp.UpdateAllRanks();

                //if (addHeaderLine == true)
                //{
                //    writer.WriteLine(temp.GetHeader());
                //    addHeaderLine = false;
                //}

                //writer.WriteLine(temp.ToTsv());
            }
        }
    }

     

    #endregion



    //// برای یک فایل مشخص به صورت خودکار
    //// زوم‌های مناسب فایل رو پیدا کرده و 
    //// برای هر زوم بر اساس حد آستانه‌های از پیش تعریف شده
    //// الگوریتم‌ها رو اجرا و اختلاف عکسی اون‌ها رو ذخیره می‌کنه
    //// نهایتا لاگ مقادیر محاسبه شده رو هم ذخیره می کنه
    //public static async Task TestThresholdChangesAlgo(string shpFile)
    //{
    //    var outputDirectoryBase = @"E:\University.Ph.D\Paper1_ReviewPaper\AlgorithmRun\1401-03-13";
    //    var fileName = $"{System.IO.Path.GetFileNameWithoutExtension(shpFile)}";
    //    var outputDirectory = $"{outputDirectoryBase}\\{fileName}";
    //    if (!Directory.Exists(outputDirectory))
    //        Directory.CreateDirectory(outputDirectory);

    //    var groundResolutionCoefs = new List<double>()
    //        {
    //            //1.0 / 6,
    //            //1.0 / 5,
    //            1.0 / 4,
    //            1.0 / 3,
    //            1.0 / 2,
    //            1,
    //            2,
    //            3,
    //            4,
    //            //5,
    //            //6
    //        };

    //    var groundResolutionCoefs_Range = groundResolutionCoefs.Max() - groundResolutionCoefs.Min();
    //    var groundResolutionCoefs_Min = groundResolutionCoefs.Min();

    //    foreach (var coef in groundResolutionCoefs)
    //    {
    //        if (!Directory.Exists($"{outputDirectory}\\{coef:N2}"))
    //            Directory.CreateDirectory($"{outputDirectory}\\{coef:N2}");
    //    }

    //    //******************************************************
    //    //***************** read features **********************
    //    var features = Shapefile.ReadShapes(shpFile)
    //                            .Select(g => g.AsGeometry())
    //                            .Where(g => !g.IsNullOrEmpty())
    //                            .ToList();

    //    foreach (var feature in features)
    //        feature.RemoveConsecutiveDuplicatePoints();

    //    var totalNumberOfPoints = features.Sum(f => f.TotalNumberOfPoints);

    //    var sqlFeatures = features.Select(f => f.AsSqlGeometry()).ToList();

    //    var groundBoundingBox = sqlFeatures.GetBoundingBox();

    //    var estimatedScale = WebMercatorUtility.EstimateZoomLevel(groundBoundingBox, /*34,*/ 900, 900);
    //    var screenSize = WebMercatorUtility.ToScreenSize(estimatedScale, groundBoundingBox);

    //    StringBuilder builder = new StringBuilder();
    //    builder.AppendLine(LogStructure.GetHeader());
    //    var originalVectorLayer = GeneralHelper.GetAsLayer("original", features);
    //    builder.AppendLine(new LogStructure(fileName, totalNumberOfPoints, features, 0, 0, "original", 0, new SimplificationParamters(), 1).ToTsv());

    //    var startIndex = Math.Max(3, estimatedScale - 5);
    //    var count = Math.Max(estimatedScale - startIndex + 2, 3);
    //    var groundLevels = Enumerable.Range(startIndex, count).ToList();

    //    List<SimplificationType> methods = new List<SimplificationType>()
    //        {
    //            SimplificationType.TriangleRoutine,
    //            SimplificationType.Angle,
    //            SimplificationType.CumulativeAngle,
    //            SimplificationType.EuclideanDistance,
    //            SimplificationType.CumulativeEuclideanDistance,
    //            SimplificationType.CumulativeAreaAngle,
    //            SimplificationType.NthPoint,
    //            SimplificationType.CumulativeTriangleRoutine,
    //            SimplificationType.VisvalingamWhyatt,
    //            SimplificationType.RamerDouglasPeucker,
    //            SimplificationType.ReumannWitkam,
    //            SimplificationType.Lang,
    //            SimplificationType.PerpendicularDistance,
    //            SimplificationType.NormalOpeningWindow,
    //            SimplificationType.BeforeOpeningWindow,
    //        };

    //    Stopwatch stopwatch = Stopwatch.StartNew();

    //    foreach (var level in groundLevels)
    //    {
    //        var currentScreenSize = WebMercatorUtility.ToScreenSize(level, groundBoundingBox);
    //        var scale = WebMercatorUtility.GetGoogleMapScale(level);

    //        var originalBitmap = await originalVectorLayer.ParseToBitmapImage(groundBoundingBox, currentScreenSize.Width, currentScreenSize.Height, scale);

    //        originalBitmap.Save($"{outputDirectory}\\{fileName}-{level}-original.png", System.Drawing.Imaging.ImageFormat.Tiff);

    //        var groundResolution = WebMercatorUtility.CalculateGroundResolution(level, 30); // mean latitude: 30

    //        var webMercatorResolution = Math.Max(groundBoundingBox.Width / currentScreenSize.Width, groundBoundingBox.Height / currentScreenSize.Height);

    //        foreach (var coef in groundResolutionCoefs)
    //        {
    //            foreach (var method in methods)
    //            {
    //                //if (method != SimplificationType.DouglasPeucker)
    //                //    continue;

    //                var threshold = webMercatorResolution * coef;

    //                var n = Math.Max(2, (uint)(features.Sum(f => f.TotalNumberOfPoints) / features.Count() * (2 + 8 * (coef - groundResolutionCoefs_Min) / groundResolutionCoefs_Range)));

    //                var parameters = new SimplificationParamters()
    //                {
    //                    N = n,
    //                    AreaThreshold = threshold * threshold,
    //                    DistanceThreshold = threshold,
    //                    Retain3Points = retain3Points,
    //                    LookAhead = (int)n,
    //                    AngleThreshold = 0.98
    //                };

    //                stopwatch.Restart();

    //                var simplified = features.Select(f => f.Simplify(method, parameters)).Where(f => f.Type != GeometryType.Point).ToList();

    //                stopwatch.Stop();

    //                var vectorLayer = GeneralHelper.GetAsLayer($"S-{level}-{method}", simplified);

    //                var tempDirectory = $"{outputDirectory}\\{coef:N2}";

    //                var diff = await GeneralHelper.CreateImages(groundBoundingBox, level, originalBitmap, vectorLayer, tempDirectory, fileName, $"{method}"/*, coef*/);

    //                builder.AppendLine(new LogStructure(fileName, totalNumberOfPoints, simplified, level, diff.percent, $"{method.GetDescription()}", stopwatch.ElapsedMilliseconds, parameters, coef).ToTsv());

    //                diff.image.Dispose();
    //            }
    //        }

    //        originalBitmap.Dispose();
    //    }

    //    File.WriteAllText($"{outputDirectory}\\{fileName}-summary.txt", builder.ToString());
    //}

}
