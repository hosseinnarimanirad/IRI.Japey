using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using IRI.Msh.Common.Mapping;
using IRI.Msh.Common.Model;
using IRI.Msh.Common.Primitives;

namespace IRI.Ket.DataManagement.DataSource
{
    public class OfflineGoogleMapDataSource<T> : IDataSource
    {
        public BoundingBox Extent
        {
            get { return BoundingBox.NaN; }
        }

        private List<ImageSource> _imageSources;

        public List<ImageSource> ImageSources
        {
            get { return _imageSources; }

            private set { _imageSources = value; }
        }

        public OfflineGoogleMapDataSource(List<ImageSource> imageSources)
        {
            if (imageSources.GroupBy(i => i.ZoomLevel).Any(i => i.Count() > 1))
            {
                throw new NotImplementedException();
            }

            this.ImageSources = imageSources;
        }

        public List<GeoReferencedImage> GetTiles(BoundingBox geographicBoundingBox, double mapScale)
        {
            //94.12.17
            //int zoomLevel = GetZoomLevel(mapScale);
            int zoomLevel = IRI.Msh.Common.Mapping.WebMercatorUtility.GetZoomLevel(mapScale);

            var result = new List<GeoReferencedImage>();

            //What if there were no imagesource for this zoom level
            if (!this.ImageSources.Any(i => i.ZoomLevel == zoomLevel))
            {
                return result;
            }

            var lowerLeft = WebMercatorUtility.LatLonToImageNumber(geographicBoundingBox.YMin, geographicBoundingBox.XMin, zoomLevel);

            var upperRight = WebMercatorUtility.LatLonToImageNumber(geographicBoundingBox.YMax, geographicBoundingBox.XMax, zoomLevel);

            var imageSource = this.ImageSources.Single(i => i.ZoomLevel == zoomLevel);

            for (int i = (int)lowerLeft.X; i <= upperRight.X; i++)
            {
                for (int j = (int)upperRight.Y; j <= lowerLeft.Y; j++)
                {
                    //94.12.17
                    //string imageName;

                    //if (imageSource.IsZoomLevelIncluded)
                    //{
                    //    imageName = string.Format("{0}{1}_{2}_{3}.{4}", imageSource.ImagePrefix, i, j, zoomLevel, imageSource.FileExtension);
                    //}
                    //else
                    //{
                    //    imageName = string.Format("{0}{1}_{2}.{3}", imageSource.ImagePrefix, i, j, imageSource.FileExtension);
                    //}

                    //string fileName = System.IO.Path.Combine(imageSource.ImageDirectory, imageName);
                    string fileName = imageSource.GetFileName(j, i);

                    if (System.IO.File.Exists(fileName))
                    {
                        result.Add(new GeoReferencedImage(
                            System.IO.File.ReadAllBytes(fileName),
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
            int zoomLevel = IRI.Msh.Common.Mapping.WebMercatorUtility.GetZoomLevel(mapScale);

            var result = new List<GeoReferencedImage>();

            //What if there were no imagesource for this zoom level
            if (!this.ImageSources.Any(i => i.ZoomLevel == zoomLevel))
            {
                return result;
            }

            var lowerLeft = WebMercatorUtility.LatLonToImageNumber(geographicBoundingBox.YMin, geographicBoundingBox.XMin, zoomLevel);

            var upperRight = WebMercatorUtility.LatLonToImageNumber(geographicBoundingBox.YMax, geographicBoundingBox.XMax, zoomLevel);

            var imageSource = this.ImageSources.Single(i => i.ZoomLevel == zoomLevel);

            for (int i = (int)lowerLeft.X; i <= upperRight.X; i++)
            {
                for (int j = (int)upperRight.Y; j <= lowerLeft.Y; j++)
                {  
                    string fileName = imageSource.GetFileName(j, i);

                    if (System.IO.File.Exists(fileName))
                    {
                        result.Add(new GeoReferencedImage(
                            System.IO.File.ReadAllBytes(fileName),
                            WebMercatorUtility.GetWgs84ImageBoundingBox(j, i, zoomLevel)));
                    }
                }
            }

            System.Diagnostics.Trace.WriteLine(string.Format("{0} Images founded; zoom level = {1}", result.Count, zoomLevel));

            return result;
        }

    }
}
