﻿using IRI.Sta.Common.Model;
using IRI.Sta.Spatial.Model;
using IRI.Sta.Common.Helpers;
using IRI.Sta.Common.Primitives;
using IRI.Sta.SpatialReferenceSystem;


namespace IRI.Sta.Spatial.Helpers;

public static class WebMercatorUtility
{
    // 1401.03.16
    // There are 3 space
    // GROUND; WEB MECATOR MAP; SCREEN
    // 
    private const int ImageSize = 256;

    private const double EarthRadius = 6378137;
    private const double MinLatitude = -85.05112878;
    private const double MaxLatitude = 85.05112878;
    private const double MinLongitude = -180;
    private const double MaxLongitude = 180;
    private const double EarthCircumference = 2 * Math.PI * EarthRadius;

    //
    static double MaxIsometricLatitude, MinIsometricLatitude, MaxAllowableLatitude;

    //
    public static List<ZoomScale> ZoomLevels;

    static readonly int minZoomLevel, maxZoomLevel;

    //Spherical
    static double _firstEccentricity = 0;

    static WebMercatorUtility()
    {
        MaxAllowableLatitude = 85.05112877822864;

        MaxIsometricLatitude = MapProjects.GeodeticLatitudeToIsometricLatitude(MaxAllowableLatitude, _firstEccentricity);

        MinIsometricLatitude = MapProjects.GeodeticLatitudeToIsometricLatitude(-MaxAllowableLatitude, _firstEccentricity);

        ZoomLevels = Enumerable.Range(0, 24).Reverse().Select(i => new ZoomScale(i, 591657550.50 / Math.Pow(2, i))).ToList();

        minZoomLevel = 1;

        maxZoomLevel = 22;

        //ZoomLevels = new List<ZoomScale>();

        //24:  591657550.50 / 2 ^ (24 - 1) = 70.5311
        //23:  591657550.50 / 2 ^ (23 - 1) = 141.0622
        //22:  591657550.50 / 2 ^ (22 - 1) = 282.1243
        //21:  591657550.50 / 2 ^ (21 - 1) = 564.2486
        //20:  591657550.50 / 2 ^ (20 - 1) = 1128.4972

        //TO SUPPORT CORRECT ZOOM LEVEL
        //ZoomLevels.Add(new ZoomScale(20, double.Epsilon));

        //ZoomLevels.Add(new ZoomScale(19, 1128.497220));
        //ZoomLevels.Add(new ZoomScale(18, 2256.994440));
        //ZoomLevels.Add(new ZoomScale(17, 4513.988880));
        //ZoomLevels.Add(new ZoomScale(16, 9027.977761));
        //ZoomLevels.Add(new ZoomScale(15, 18055.955520));
        //ZoomLevels.Add(new ZoomScale(14, 36111.911040));
        //ZoomLevels.Add(new ZoomScale(13, 72223.822090));
        //ZoomLevels.Add(new ZoomScale(12, 144447.644200));
        //ZoomLevels.Add(new ZoomScale(11, 288895.288400));
        //ZoomLevels.Add(new ZoomScale(10, 577790.576700));
        //ZoomLevels.Add(new ZoomScale(9, 1155581.153000));
        //ZoomLevels.Add(new ZoomScale(8, 2311162.307000));
        //ZoomLevels.Add(new ZoomScale(7, 4622324.614000));
        //ZoomLevels.Add(new ZoomScale(6, 9244649.227000));
        //ZoomLevels.Add(new ZoomScale(5, 18489298.450000));
        //ZoomLevels.Add(new ZoomScale(4, 36978596.910000));
        //ZoomLevels.Add(new ZoomScale(3, 73957193.820000));
        //ZoomLevels.Add(new ZoomScale(2, 147914387.600000));
        //ZoomLevels.Add(new ZoomScale(1, 295828775.300000));
        //ZoomLevels.Add(new ZoomScale(0, 591657550.500000));
    }



    // ********************************************** SCREEN (PIXEL) *************************************************
    /// <summary>
    /// In Pixel
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public static long CalculateScreenSize(int level)
    {
        if (level < 0 || level > 24)
            throw new NotImplementedException();

        return ImageSize * (long)Math.Pow(2, level);
    }

    /// <summary>
    /// In Pixel
    /// </summary>
    /// <param name="level"></param>
    /// <param name="webMercatorLength"></param>
    /// <returns></returns>
    public static double ToScreenLength(int level, double webMercatorLength)
    {
        return webMercatorLength * CalculateScreenSize(level) / EarthCircumference;
    }

    ////1399.06.26
    //public static System.Drawing.Size CalculateWindowSize(BoundingBox groundBoundingBox, int level, bool exactFit = false)
    //{
    //    var scale = GetGoogleMapScale(level);

    //    var marginFactor = exactFit ? 1.0 : 1.2;

    //    var width = groundBoundingBox.Width * scale * ConversionHelper.MeterToPixelFactor * marginFactor;

    //    var height = groundBoundingBox.Height * scale * ConversionHelper.MeterToPixelFactor * marginFactor;

    //    return new System.Drawing.Size((int)width, (int)height);
    //}

    // 1401.03.16
    public static System.Drawing.Size ToScreenSize(int level, BoundingBox webMercatorBoundingBox, bool exactFit = false)
    {
        var marginFactor = exactFit ? 1.0 : 1.2;

        var width = ToScreenLength(level, webMercatorBoundingBox.Width) * marginFactor;

        var height = ToScreenLength(level, webMercatorBoundingBox.Height) * marginFactor;

        return new System.Drawing.Size((int)width, (int)height);
    }



    // ********************************************** GROUND *********************************************************
    /// <summary>
    /// The ground resolution indicates the distance on the ground that’s represented by a single pixel in the map.
    ///  For example, at a ground resolution of 10 meters/pixel, each pixel represents a ground distance of 10 meters. 
    /// </summary>
    /// <param name="level">google zoom level</param>
    /// <param name="latitude">in degree</param>
    /// <returns></returns>
    public static double CalculateGroundResolution(int level, double latitude)
    {
        // 1: 1 pixel
        return Math.Cos(latitude * Math.PI / 180.0) * ToWebMercatorLength(level, 1); // EarthCircumference / CalculateScreenSize(level);
    }

    /// <summary>
    /// The distance on the earth equivalent to 1 pixel at the specific scale
    /// </summary>
    /// <param name="mapScale"></param>
    /// <returns></returns>
    public static double CalculateGroundResolution(double mapScale)
    {
        // 1 pixel * PixelToMeterFactor (meter) / mapScale
        return ConversionHelper.PixelToMeterFactor / mapScale;
    }



    // ********************************************** WebMercator ****************************************************
    /// <summary>
    /// Returns the length in WebMercator
    /// </summary>
    /// <param name="level">Google Zoom Level</param>
    /// <param name="screenLengthInPixel">In Pixel</param>
    /// <returns></returns>
    public static double ToWebMercatorLength(int level, double screenLengthInPixel)
    {
        return screenLengthInPixel * EarthCircumference / CalculateScreenSize(level);
    }



    // ********************************************** Map SCALE (GROUND) *********************************************
    /// <summary>
    /// The map scale indicates the ratio between map distance and ground distance, when measured in the same units.
    /// </summary>
    /// <param name="level"></param>
    /// <param name="latitude"></param>
    /// <returns></returns>
    public static double CalculateMapScale(int level, double latitude)
    {
        return 1.0 / (CalculateGroundResolution(level, latitude) * ConversionHelper.MeterToPixelFactor);
    }


    // ********************************************** Google Zoom Level **********************************************
    /// <summary>
    /// In which scale this distance can be represented in one tile (256 pixel wide)
    /// </summary>
    /// <param name="webMercatorLength"></param>
    /// <param name="latitude">In Degree</param>
    /// <param name="screenWidth"></param>
    /// <returns></returns>
    public static int EstimateZoomLevel(double webMercatorLength/*, double latitude*/, double screenWidth)
    {
        //1 meter on ground ~ 3779.5 pixel (96 dpi)
        //1 pixel at 1/s scale ~ s/3779.5 meter on ground

        var scale = screenWidth * ConversionHelper.PixelToMeterFactor / webMercatorLength;

        return GetZoomLevel(scale);
    }

    public static int EstimateZoomLevel(BoundingBox webMercatorBoundingBox/*, double latitude*/, double screenWidth, double screenHeight)
    {
        var widthScale = screenWidth * ConversionHelper.PixelToMeterFactor / webMercatorBoundingBox.Width;

        var heightScale = screenHeight * ConversionHelper.PixelToMeterFactor / webMercatorBoundingBox.Height;

        var scale = Math.Min(widthScale, heightScale);

        return GetZoomLevel(scale);
    }

    //public static int GetZoomLevel(BoundingBox webMercatorBoundingBox, double screenWidth, double screenHeight)
    //{
    //    var scaleX = screenWidth / webMercatorBoundingBox.Width;

    //    var scaleY = screenHeight / webMercatorBoundingBox.Height;

    //    var screenSize = scaleX < scaleY ? screenWidth : screenHeight;

    //    return EstimateZoomLevel(Math.Max(webMercatorBoundingBox.Width, webMercatorBoundingBox.Height), /*35,*/ screenSize);

    //}

    public static int GetZoomLevel(double mapScale, double latitude = 0)
    {
        var level = (int)Math.Round(GetLevel(mapScale, latitude));

        return AdjustLevel(level);
    }

    private static double GetLevel(double mapScale, double latitude)
    {
        return Math.Log(Math.Cos(latitude * Math.PI / 180.0) * EarthCircumference * ConversionHelper.MeterToPixelFactor / ImageSize * mapScale, 2);
    }


    #region Application Level

    public static int GetNextZoomLevel(int currentZoomLevel)
    {
        if (currentZoomLevel >= maxZoomLevel)
        {
            return maxZoomLevel;
        }

        return currentZoomLevel + 1;
    }

    public static int GetPreviousZoomLevel(int currentZoomLevel)
    {
        if (currentZoomLevel <= minZoomLevel)
        {
            return minZoomLevel;
        }

        return currentZoomLevel - 1;
    }

    public static double GetGoogleMapScale(int zoomLevel)
    {
        if (zoomLevel < minZoomLevel)
        {
            return GetGoogleMapScale(minZoomLevel);
        }
        else if (zoomLevel > maxZoomLevel)
        {
            return GetGoogleMapScale(maxZoomLevel);
        }
        else
        {
            return ZoomLevels.Single(i => i.ZoomLevel == zoomLevel).Scale;
        }
    }

    public static double GetGoogleMapScale(int zoomLevel, double? latitude)
    {
        if (latitude == null)
        {
            return GetGoogleMapScale(zoomLevel);
        }

        if (zoomLevel < minZoomLevel)
        {
            return GetGoogleMapScale(minZoomLevel, latitude);
        }
        else if (zoomLevel > maxZoomLevel)
        {
            return GetGoogleMapScale(maxZoomLevel, latitude);
        }
        else
        {
            return ZoomLevels.Single(i => i.ZoomLevel == zoomLevel).GetScaleAt(latitude.Value);
        }
    }

    private static ZoomScale GetGoogleZoomScale(int zoomLevel)
    {
        if (zoomLevel < minZoomLevel)
        {
            return GetGoogleZoomScale(minZoomLevel);
        }
        else if (zoomLevel > maxZoomLevel)
        {
            return GetGoogleZoomScale(maxZoomLevel);
        }
        else
        {
            return ZoomLevels.Single(i => i.ZoomLevel == zoomLevel);
        }
    }


    private static int AdjustLevel(int level)
    {
        if (level > maxZoomLevel)
        {
            return maxZoomLevel;
        }
        else if (level < minZoomLevel)
        {
            return minZoomLevel;
        }
        else
        {
            return level;
        }
    }

    public static ZoomScale GetUpperLevel(double scale, double latitude)
    {
        var level = AdjustLevel((int)Math.Floor(GetLevel(scale, latitude)));

        return ZoomLevels.Single(i => i.ZoomLevel == level);
    }

    public static ZoomScale GetLowerLevel(double scale, double latitude)
    {
        var level = AdjustLevel((int)Math.Ceiling(GetLevel(scale, latitude)));

        return ZoomLevels.Single(i => i.ZoomLevel == level);
    }

    public static double GetUpperLevel(double scale, List<double> availableInverseScales)
    {
        double inverseScale = 1.0 / scale;

        var temp = availableInverseScales.Where(z => z > inverseScale).OrderBy(z => z).ToList();

        return temp.Count() > 0 ? temp.First() : availableInverseScales.Max();
    }

    public static double GetLowerLevel(double scale, List<double> availableInverseScales)
    {
        double inverseScale = 1.0 / scale;

        var temp = availableInverseScales.Where(z => z < inverseScale).OrderBy(z => z).ToList();

        return temp.Count() > 0 ? temp.Last() : availableInverseScales.Min();
    }

    #endregion


    public static Point LatLonToImageNumber(double geocentricLatitude, double geocentricLongitude, int zoom)
    {
        var tempLongitude = geocentricLongitude % 360;

        if (tempLongitude > 180)
        {
            tempLongitude -= 180;
        }
        else
        {
            tempLongitude += 180;
        }

        //This is not the total number of images. It's the number of images per row/column
        int numberOfImages = (int)Math.Pow(2, zoom);

        var xUnit = 360.0 / numberOfImages;

        var yUnit = (MaxIsometricLatitude - MinIsometricLatitude) / numberOfImages;

        var columnNumber = Math.Floor(tempLongitude / xUnit);

        var isoY = MapProjects.GeodeticLatitudeToIsometricLatitude(geocentricLatitude, _firstEccentricity);

        var rowNumber = isoY / yUnit;

        if (rowNumber < 0)
        {
            rowNumber = Math.Ceiling(-rowNumber) + Math.Ceiling(MaxIsometricLatitude / yUnit) - 1;
        }
        else
        {
            rowNumber = Math.Ceiling(MaxIsometricLatitude / yUnit) - Math.Ceiling(rowNumber);
        }

        return new Point(columnNumber, rowNumber);
    }

    public static List<TileInfo> GeodeticBoundingBoxToGoogleTileRegions(BoundingBox geodeticBoundingBox, int zoomLevel)
    {
        var lowerLeft = LatLonToImageNumber(geodeticBoundingBox.BottomRight.Y, geodeticBoundingBox.TopLeft.X, zoomLevel);

        var upperRight = LatLonToImageNumber(geodeticBoundingBox.TopLeft.Y, geodeticBoundingBox.BottomRight.X, zoomLevel);

        var result = new List<TileInfo>();

        for (int i = (int)lowerLeft.X; i <= upperRight.X; i++)
        {
            for (int j = (int)upperRight.Y; j <= lowerLeft.Y; j++)
            {
                result.Add(new TileInfo(j, i, zoomLevel));
            }
        }

        return result;
    }

    public static List<TileInfo> WebMercatorBoundingBoxToGoogleTileRegions(BoundingBox webMercatorBoundingBox, int zoomLevel)
    {
        var geographicBoundingBox = webMercatorBoundingBox.Transform(i => MapProjects.WebMercatorToGeodeticWgs84(i));

        return GeodeticBoundingBoxToGoogleTileRegions(geographicBoundingBox, zoomLevel);
    }

    public static void WriteGeodeticBoundingBoxToGoogleTileRegions(string fileName, BoundingBox geodeticBoundingBox, int zoomLevel)
    {
        var lowerLeft = LatLonToImageNumber(geodeticBoundingBox.BottomRight.Y, geodeticBoundingBox.TopLeft.X, zoomLevel);

        var upperRight = LatLonToImageNumber(geodeticBoundingBox.TopLeft.Y, geodeticBoundingBox.BottomRight.X, zoomLevel);

        var result = new List<string>();

        result.Add("ZoomLevel ; RowNumber ;  ColumnNumber ; XMin ; XMax ; YMin ; YMax");

        File.AppendAllLines(fileName, result);

        for (int i = (int)lowerLeft.X; i <= upperRight.X; i++)
        {
            result = new List<string>();

            for (int j = (int)upperRight.Y; j <= lowerLeft.Y; j++)
            {
                var tile = new TileInfo(j, i, zoomLevel);

                result.Add($"{tile.ZoomLevel} ; {tile.RowNumber} ; {tile.ColumnNumber} ; {tile.GeodeticExtent.XMin} ; {tile.GeodeticExtent.XMax} ; {tile.GeodeticExtent.YMin} ; {tile.GeodeticExtent.YMax}");
            }

            File.AppendAllLines(fileName, result);
        }

    }

    public static void WriteWebMercatorBoundingBoxToGoogleTileRegions(string fileName, BoundingBox webMercatorBoundingBox, int zoomLevel)
    {
        var geographicBoundingBox = webMercatorBoundingBox.Transform(MapProjects.WebMercatorToGeodeticWgs84);

        WriteGeodeticBoundingBoxToGoogleTileRegions(fileName, geographicBoundingBox, zoomLevel);
    }

    public static BoundingBox GetWgs84ImageBoundingBox(int row, int column, int zoom)
    {
        int numberOfImages = (int)Math.Pow(2, zoom);

        var unit = 360.0 / numberOfImages;

        double minLongitude = column * unit;

        double maxLongitude = (column + 1) * unit;

        minLongitude = minLongitude - 180;

        maxLongitude = maxLongitude - 180;

        var yUnit = (MaxIsometricLatitude - MinIsometricLatitude) / numberOfImages;

        var maxTempRow = row;

        double minTempRow = row;

        if (row > numberOfImages / 2.0)
        {
            maxTempRow = (int)(row - numberOfImages / 2.0);

            minTempRow = maxTempRow + 1;

            yUnit *= -1;
        }
        else
        {
            maxTempRow = (int)(numberOfImages / 2.0) - row;

            minTempRow = maxTempRow - 1;
        }

        var latitude01 = MapProjects.IsometricLatitudeToGeodeticLatitude(maxTempRow * yUnit, _firstEccentricity);

        var latitude02 = MapProjects.IsometricLatitudeToGeodeticLatitude(minTempRow * yUnit, _firstEccentricity);

        double minLatitude, maxLatitude;

        if (latitude01 < latitude02)
        {
            minLatitude = latitude01;

            maxLatitude = latitude02;
        }
        else
        {
            minLatitude = latitude02;

            maxLatitude = latitude01;
        }

        //var min = Transformation.ChangeDatum(new Point(minLongitude, minLatitude), Ellipsoids.Sphere, Ellipsoids.WGS84);

        //var max = Transformation.ChangeDatum(new Point(maxLongitude, maxLatitude), Ellipsoids.Sphere, Ellipsoids.WGS84);

        //return new BoundingBox(min.X, min.Y, max.X, max.Y);

        return new BoundingBox(minLongitude, minLatitude, maxLongitude, maxLatitude);
    }


}
