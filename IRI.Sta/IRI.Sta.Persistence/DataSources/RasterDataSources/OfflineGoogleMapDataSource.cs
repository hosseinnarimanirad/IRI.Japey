﻿using System.IO;
using System.Linq;
using System.Collections.Generic;

using IRI.Sta.Common.Model;
using IRI.Sta.Spatial.Helpers;
using IRI.Sta.Common.Primitives;
using IRI.Sta.SpatialReferenceSystem;
using IRI.Sta.Persistence.Abstractions;

namespace IRI.Sta.Persistence.RasterDataSources;

public class OfflineGoogleMapDataSource<T> : IRasterDataSource
{
    public BoundingBox WebMercatorExtent
    {
        get { return BoundingBox.NaN; }
    }

    private List<ImageSource> _imageSources;

    public List<ImageSource> ImageSources
    {
        get { return _imageSources; }

        private set { _imageSources = value; }
    }

    public int Srid => SridHelper.WebMercator;

    public OfflineGoogleMapDataSource(List<ImageSource> imageSources)
    {
        //if (imageSources.GroupBy(i => i.ZoomLevel).Any(i => i.Count() < 1))
        //{
        //    throw new NotImplementedException();
        //}

        _imageSources = imageSources;
    }

    public List<GeoReferencedImage> GetTiles(BoundingBox geographicBoundingBox, double mapScale)
    {
        //94.12.17
        //int zoomLevel = GetZoomLevel(mapScale);
        int zoomLevel = WebMercatorUtility.GetZoomLevel(mapScale);

        var result = new List<GeoReferencedImage>();

        //What if there were no imagesource for this zoom level
        if (!ImageSources.Any(i => i.ZoomLevel == zoomLevel))
        {
            return result;
        }

        var lowerLeft = WebMercatorUtility.LatLonToImageNumber(geographicBoundingBox.YMin, geographicBoundingBox.XMin, zoomLevel);

        var upperRight = WebMercatorUtility.LatLonToImageNumber(geographicBoundingBox.YMax, geographicBoundingBox.XMax, zoomLevel);

        var imageSource = ImageSources.Single(i => i.ZoomLevel == zoomLevel);

        for (int i = (int)lowerLeft.X; i <= upperRight.X; i++)
        {
            for (int j = (int)upperRight.Y; j <= lowerLeft.Y; j++)
            {
                string fileName = imageSource.GetFileName(j, i);

                if (File.Exists(fileName))
                {
                    result.Add(new GeoReferencedImage(
                        File.ReadAllBytes(fileName),
                        WebMercatorUtility.GetWgs84ImageBoundingBox(j, i, zoomLevel)));
                }
            }
        }

        System.Diagnostics.Trace.WriteLine(string.Format("{0} Images founded; zoom level = {1}", result.Count, zoomLevel));

        return result;
    }



    public List<GeoReferencedImage> GetTilesForGoogleEarth(BoundingBox geographicBoundingBox, double mapScale)
    {
        //94.12.17
        //int zoomLevel = GetZoomLevel(mapScale);
        int zoomLevel = WebMercatorUtility.GetZoomLevel(mapScale);

        var result = new List<GeoReferencedImage>();

        //What if there were no imagesource for this zoom level
        if (!ImageSources.Any(i => i.ZoomLevel == zoomLevel))
        {
            return result;
        }

        var lowerLeft = WebMercatorUtility.LatLonToImageNumber(geographicBoundingBox.YMin, geographicBoundingBox.XMin, zoomLevel);

        var upperRight = WebMercatorUtility.LatLonToImageNumber(geographicBoundingBox.YMax, geographicBoundingBox.XMax, zoomLevel);

        var imageSource = ImageSources.Single(i => i.ZoomLevel == zoomLevel);

        for (int i = (int)lowerLeft.X; i <= upperRight.X; i++)
        {
            for (int j = (int)upperRight.Y; j <= lowerLeft.Y; j++)
            {
                string fileName = imageSource.GetFileName(j, i);

                if (File.Exists(fileName))
                {
                    result.Add(new GeoReferencedImage(
                        File.ReadAllBytes(fileName),
                        WebMercatorUtility.GetWgs84ImageBoundingBox(j, i, zoomLevel)));
                }
            }
        }

        System.Diagnostics.Trace.WriteLine(string.Format("{0} Images founded; zoom level = {1}", result.Count, zoomLevel));

        return result;
    }

}
