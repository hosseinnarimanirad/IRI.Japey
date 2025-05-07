using IRI.Extensions;
using IRI.Sta.Spatial.Analysis;
using IRI.Sta.Spatial.Mapping;
using IRI.Sta.Spatial.Primitives;
using IRI.Sta.ShapefileFormat;
using System.Diagnostics;
using IRI.Sta.Spatial.Model.GeoJson;
using IRI.Sta.CoordinateSystems.MapProjection;
using System.Text;
using System.Windows.Threading;
using System.IO;

namespace IRI.Article04.FastSimplification;

public static class SimplificationHelper
{
    private static bool retain3Points = false;

    public async static Task GeneralTest()
    {
        // OSM dataset directory should be addressed here
        var dataFolder = @"E:\University.Ph.D\Sample Data\OSM-1401-03-03-WebMercator";

        var files = Directory.EnumerateFiles(dataFolder, "*.shp", SearchOption.AllDirectories);

        // Output directory should be addressed here
        var writeFolder = $@"E:\University.Ph.D\5. ISPRS Conf Paper\AlgorithmRun\{DateTime.Now.Date:yyyy-MM-dd}";

        if (!Directory.Exists(writeFolder))
            _ = Directory.CreateDirectory(writeFolder);

        StreamWriter writer = new StreamWriter($"{writeFolder}\\summary.txt", false);
        writer.WriteLine(SpeedLog.GetHeader());

        List<string> fileNames = new List<string>();

        var oldfiles = Directory.EnumerateFiles(writeFolder, "*.txt", SearchOption.AllDirectories)
                                .Select(s => System.IO.Path.GetFileNameWithoutExtension(s).Replace("-summary", string.Empty));

        string logFile = $"{writeFolder}\\Log-{DateTime.Now:yyyy-MM-dd HH-mm-ss}.txt";

        Stopwatch watch = Stopwatch.StartNew();

        foreach (var file in files)
        {
            var fileName = System.IO.Path.GetFileNameWithoutExtension(file);

            if (fileNames.Contains(fileName) || oldfiles.Contains(fileName))
                continue;

            fileNames.Add(fileName);

            watch.Restart();

            await ProcessShapefile(file, writer, writeFolder);

            File.AppendAllLines(logFile, new List<string>() { $"Finished At: {DateTime.Now.ToLongTimeString()}; Ellapsed: {watch.ElapsedMilliseconds / 1000.0:N0000} (s) - ({fileName})" });
        }

        writer.Close();
        writer.Dispose();
    }

    private static async Task ProcessShapefile(string shpFile, StreamWriter writer, string outputDirectory)
    {
        var fileName = $"{Path.GetFileNameWithoutExtension(shpFile)}";

        //******************************************************
        //***************** read features **********************
        var features = Shapefile.ReadShapes(shpFile)
                                .Select(g => g.AsGeometry())
                                .SelectMany(g => g.Split(false))
                                .Where(g => !g.IsNullOrEmpty())
                                .Where(g => g.TotalNumberOfPoints > 4)
                                .ToList();

        foreach (var feature in features)
            feature.RemoveConsecutiveDuplicatePoints();

        features = features.Where(f => !f.HasDuplicatePoints()).ToList();

        List<SimplificationType> methods = new List<SimplificationType>()
            {
                SimplificationType.RamerDouglasPeucker,
                SimplificationType.ReumannWitkam,

                SimplificationType.VisvalingamWhyatt,

                SimplificationType.NormalOpeningWindow,
                SimplificationType.BeforeOpeningWindow,

                SimplificationType.CumulativeTriangleRoutine,
            };

        Dictionary<SimplificationType, string> methodNames = methods.ToDictionary(m => m, m => m.GetDescription());

        var groundBoundingBox = features.GetBoundingBox();

        var estimatedZoomLevel = WebMercatorUtility.EstimateZoomLevel(groundBoundingBox, /*34,*/ 512, 512);

        var scale = WebMercatorUtility.GetGoogleMapScale(estimatedZoomLevel);

        var currentScreenSize = WebMercatorUtility.ToScreenSize(estimatedZoomLevel, groundBoundingBox);

        // threshold values is set to 0.5 pixel
        var threshold = WebMercatorUtility.ToWebMercatorLength(estimatedZoomLevel, 0.5);

        var parameters = new SimplificationParamters() { AreaThreshold = threshold * threshold, DistanceThreshold = threshold, Retain3Points = retain3Points };
         
        var originalVectorLayer = GeneralHelper.GetAsLayer("original", features);

        var originalBitmap = await originalVectorLayer.ParseToBitmapImage(groundBoundingBox, currentScreenSize.Width, currentScreenSize.Height, scale);

        originalBitmap.Save($"{outputDirectory}\\{fileName}-{estimatedZoomLevel}-original.png", System.Drawing.Imaging.ImageFormat.Tiff);


        Stopwatch stopwatch = Stopwatch.StartNew();

        foreach (var method in methods)
        {
            stopwatch.Restart();

            var simplifiedFeatures = features.Simplify(method, parameters);

            stopwatch.Stop();

            if (simplifiedFeatures.IsNullOrEmpty())
                continue;

            writer.WriteLine(
              new SpeedLog(fileName,
                            methodNames[method],
                            features,
                            simplifiedFeatures,
                            stopwatch.ElapsedMilliseconds,
                            parameters).ToTsv());

            var vectorLayer = GeneralHelper.GetAsLayer("original", features);

            var simplifiedBitmap = await vectorLayer.ParseToBitmapImage(groundBoundingBox, currentScreenSize.Width, currentScreenSize.Height, scale);

            simplifiedBitmap.Save($"{outputDirectory}\\{fileName}-{estimatedZoomLevel}-{method}.png", System.Drawing.Imaging.ImageFormat.Tiff);

            //var diff = await GeneralHelper.CreateImages(groundBoundingBox, estimatedZoomLevel, originalBitmap, vectorLayer, tempDirectory, fileName, $"{method}");
            //builder.AppendLine(new LogStructure(fileName, totalNumberOfPoints, simplified, level, diff.percent, $"{method.GetDescription()}", stopwatch.ElapsedMilliseconds, parameters, coef).ToTsv());
            //diff.image.Dispose();
        }

    }


    #region Test with visual output

    //public static async Task TestWithOutput(string shpFile, StreamWriter writer, bool includeHeader, string outputDirectory)
    //{
    //    var fileName = $"{Path.GetFileNameWithoutExtension(shpFile)}";

    //    var groundResolutionCoefs = new List<double>()
    //    {
    //        //1.0 / 6,
    //        1.0 / 5,
    //        1.0 / 4,
    //        //1.0 / 3,
    //        1.0 / 2,
    //        1,
    //        2,
    //        //3,
    //        4,
    //        5,
    //        //6
    //    };

    //    var groundResolutionCoefs_Range = groundResolutionCoefs.Max() - groundResolutionCoefs.Min();
    //    var groundResolutionCoefs_Min = groundResolutionCoefs.Min();

    //    //******************************************************
    //    //***************** read features **********************
    //    var shapes = Shapefile.ReadShapes(shpFile);

    //    var features = shapes
    //                            .Select(g => g.AsGeometry())
    //                            .SelectMany(g => g.NumberOfGeometries > 1 ? g.Split(false) : new List<Geometry<Point>>() { g })
    //                            .Where(g => !g.IsNullOrEmpty())
    //                            .Where(g => g.TotalNumberOfPoints > 15 && g.TotalNumberOfPoints < 500)
    //                            .ToList();

    //    //var isRingbase = shapes[0].IsRingBase();

    //    foreach (var feature in features)
    //    {
    //        //if (isRingbase)
    //        //{
    //        //    feature.CloseLineString();
    //        //}

    //        feature.Srid = SridHelper.WebMercator;
    //        feature.RemoveConsecutiveDuplicatePoints();
    //    }

    //    features = features.Where(f => !f.HasDuplicatePoints()).ToList();

    //    List<SimplificationType> methods = new List<SimplificationType>()
    //    {
    //        //SimplificationType.EuclideanDistance,

    //        SimplificationType.RamerDouglasPeucker,
    //        //SimplificationType.ReumannWitkam,

    //        //SimplificationType.PerpendicularDistance,

    //        //SimplificationType.VisvalingamWhyatt,
    //        SimplificationType.SleeveFitting,

    //        //SimplificationType.NormalOpeningWindow,
    //        //SimplificationType.BeforeOpeningWindow,

    //        //SimplificationType.TriangleRoutine,
    //    };

    //    var colros = new Dictionary<SimplificationType, System.Windows.Media.Color>()
    //    {
    //        {SimplificationType.EuclideanDistance, System.Windows.Media.Colors.DarkGray},
    //        {SimplificationType.RamerDouglasPeucker, Jab.Common.Helpers.ColorHelper.ToWpfColor("#E051AD")},
    //        {SimplificationType.ReumannWitkam, System.Windows.Media.Colors.Navy},
    //        {SimplificationType.PerpendicularDistance, System.Windows.Media.Colors.Blue},
    //        {SimplificationType.VisvalingamWhyatt, System.Windows.Media.Colors.Yellow},
    //        {SimplificationType.SleeveFitting, Jab.Common.Helpers.ColorHelper.ToWpfColor("#3B878C")},
    //        {SimplificationType.NormalOpeningWindow, System.Windows.Media.Colors.Brown},
    //        {SimplificationType.BeforeOpeningWindow, System.Windows.Media.Colors.Orange},
    //        {SimplificationType.TriangleRoutine, System.Windows.Media.Colors.Gray}
    //    };

    //    int featureIndex = 0;

    //    bool addHeaderLine = includeHeader;

    //    foreach (var feature in features)
    //    {
    //        featureIndex++;

    //        if (!CheckZoom2(featureIndex, fileName)) continue;

    //        var outputDirectoryForFeature = $"{outputDirectory}\\{fileName}\\{featureIndex}";

    //        if (!Directory.Exists(outputDirectoryForFeature))
    //            Directory.CreateDirectory(outputDirectoryForFeature);

    //        var boundingBox = feature.GetBoundingBox().Expand(1.1);

    //        var estimatedZoomLevel = WebMercatorUtility.EstimateZoomLevel(boundingBox, /*34,*/ 256, 256);

    //        var webMercatorResolution = WebMercatorUtility.ToWebMercatorLength(estimatedZoomLevel, 10);

    //        var originalFrame = await GetAsFrame(
    //            "original",
    //            estimatedZoomLevel,
    //            boundingBox,
    //            feature,
    //            Jab.Common.Helpers.ColorHelper.ToWpfColor("#C0C0C0"),
    //            4,
    //            1);

    //        GeoJsonFeatureSet originalFeatureSet = feature.AsGeoJsonFeatureSet();

    //        originalFeatureSet.Save($"{outputDirectoryForFeature}\\{fileName}-{featureIndex}-{estimatedZoomLevel}-original.json", false, true);


    //        foreach (var coef in groundResolutionCoefs)
    //        {
    //            var threshold = webMercatorResolution * coef;

    //            var parameters = new SimplificationParamters() { AreaThreshold = threshold * threshold, DistanceThreshold = threshold, Retain3Points = retain3Points };

    //            var temp = new NineAlgoSixMeasureLog
    //                        (fileName,
    //                                    boundingBox,
    //                                    featureIndex,
    //                                    feature,
    //                                    estimatedZoomLevel,
    //                                    //methodNames[method],
    //                                    //stopwatch.ElapsedMilliseconds,
    //                                    parameters,
    //                                    coef);

    //            //List<System.Drawing.Bitmap> images = new List<System.Drawing.Bitmap>();
    //            List<DrawingVisual> drawingVisuals = new List<DrawingVisual>
    //            {
    //                originalFrame
    //            };

    //            foreach (var method in methods)
    //            {
    //                //stopwatch.Restart();

    //                var simplified = feature.Simplify(method, parameters);

    //                //stopwatch.Stop();

    //                if (simplified.IsNullOrEmpty())
    //                    continue;

    //                if (simplified.Type == GeometryType.Point)
    //                    continue;

    //                temp.AddAlgoResult(method, feature, simplified);

    //                var simplifiedFrame = await GetAsFrame(method.ToString(), estimatedZoomLevel, boundingBox, simplified, colros[method], 2, 0.9);

    //                drawingVisuals.Add(simplifiedFrame);

    //                GeoJsonFeatureSet featureSet = simplified.AsGeoJsonFeatureSet();

    //                featureSet.Save($"{outputDirectoryForFeature}\\{fileName}-{featureIndex}-{estimatedZoomLevel}-{coef}-{method}.json", false, true);
    //            }

    //            Save($"{outputDirectoryForFeature}\\{fileName}-{featureIndex}-{estimatedZoomLevel}-{coef}.png", drawingVisuals, estimatedZoomLevel, boundingBox);

    //            temp.UpdateAllRanks();

    //            if (addHeaderLine == true)
    //            {
    //                writer.WriteLine(temp.GetHeader());
    //                addHeaderLine = false;
    //            }

    //            writer.WriteLine(temp.ToTsv());
    //        }
    //    }
    //}

    //private static bool CheckZoom2(int featureIndex, string fileName)
    //{
    //    var type = fileName.Split('_').First();

    //    switch (type)
    //    {
    //        case "AdminArea":
    //            return featureIndex == 1507 || featureIndex == 995 || featureIndex == 1117 || featureIndex == 1219
    //                    || featureIndex == 1184 || featureIndex == 1219 || featureIndex == 1529;

    //        case "Building":
    //            // SF is better (less TLVD per length) -Z2
    //            return featureIndex == 1322 || featureIndex == 2181 ||
    //                // RDP is better (less TLVD per length)
    //                featureIndex == 902 || featureIndex == 8760 || featureIndex == 3815 || featureIndex == 8760 ||

    //                // SF is better (less TLVD per length) - Z1
    //                featureIndex == 22523;

    //        case "LandUse":
    //        case "Water":
    //            return featureIndex == 36;

    //        case "Railway":
    //            // SF is better (less TLVD per length) -Z2
    //            return featureIndex == 149 || featureIndex == 359 ||
    //                // RDP is better (less TLVD per length)
    //                featureIndex == 747 || featureIndex == 192;

    //        case "Road":
    //            return new List<int>()
    //            {
    //                // SF is better (less TLVD per length)
    //                 136284, 7727, 108622, 134791, 8593,51092, 115606,

    //                 // RDP is better (less TLVD per length)
    //                 80721, 119726, 33390, 91799, 103402, 112277, 43487, 7823, 127214
    //            }.Contains(featureIndex);

    //        case "Waterways":
    //            // SF is better (less TLVD per length)
    //            return featureIndex == 17658 ||

    //                // RDP is better (less TLVD per length)
    //                featureIndex == 5054;

    //        default:
    //            return false;
    //    }
    //}

    //private static async Task<DrawingVisual> GetAsFrame(
    //    string layerName,
    //    int level,
    //    BoundingBox groundBoundingBox,
    //    Geometry<Point> geometry,
    //    System.Windows.Media.Color color,
    //    int strokeThickness,
    //    double opacity)
    //{
    //    var vectorLayer = new VectorLayer(layerName,
    //                                        new List<Geometry<Point>>() { geometry },
    //                                        VisualParameters.Get(Colors.Transparent, color, strokeThickness, opacity),
    //                                        LayerType.VectorLayer,
    //                                        RenderingApproach.Default,
    //                                        RasterizationApproach.DrawingVisual);

    //    var currentScreenSize = WebMercatorUtility.ToScreenSize(level, groundBoundingBox);

    //    var scale = WebMercatorUtility.GetGoogleMapScale(level);

    //    return await vectorLayer.AsDrawingVisual(groundBoundingBox, currentScreenSize.Width, currentScreenSize.Height, scale);
    //}

    //private static void Save(string fileName, List<DrawingVisual> drawingVisuals, int level, BoundingBox groundBoundingBox)
    //{
    //    var currentScreenSize = WebMercatorUtility.ToScreenSize(level, groundBoundingBox);

    //    RenderTargetBitmap image = new RenderTargetBitmap(currentScreenSize.Width, currentScreenSize.Height, 96, 96, PixelFormats.Pbgra32);

    //    foreach (var drawingVisual in drawingVisuals)
    //    {
    //        image.Render(drawingVisual);
    //    }

    //    var frame = BitmapFrame.Create(image);

    //    PngBitmapEncoder pngImage = new PngBitmapEncoder();

    //    pngImage.Frames.Add(frame);

    //    using (System.IO.Stream stream = System.IO.File.Create(fileName))
    //    {
    //        pngImage.Save(stream);
    //    }
    //}

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
