using System.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using IRI.Ham.SpatialBase;
using Microsoft.SqlServer.Types;

using IRI.Ham.SpatialBase.Mapping;
using IRI.Ket.DataManagement.Model;
using IRI.Ket.Common.Model;
using IRI.Ket.SqlServerSpatialExtension.GeoStatistics;
using IRI.Ham.Common;

namespace IRI.Ket.DataManagement.DataSource
{
    public class ClusteredGeoTaggedImageSource : IDataSource
    {
        object _lock = new object();

        string _imageDirectory;

        public string ImageDirectory { get { return _imageDirectory; } }

        List<GeoTaggedImage> _images;

        private ClusteredGeoTaggedImageSource(string imageDirectory)
        {
            this._imageDirectory = imageDirectory;

            this._images = new List<GeoTaggedImage>();
        }

        public static ClusteredGeoTaggedImageSource Create(string imageDirectory)
        {
            ClusteredGeoTaggedImageSource result = new ClusteredGeoTaggedImageSource(imageDirectory);

            result.Load();

            return result;
        }

        private void Load()
        {
            //await Task.Factory.StartNew(() =>
            //{
            lock (_lock)
            {
                if (!System.IO.Directory.Exists(_imageDirectory))
                {
                    return;
                }

                var files = new System.IO.DirectoryInfo(_imageDirectory).GetFiles("*.jpg");

                foreach (var file in files)
                {
                    try
                    {
                        var geoTaggedImage = new GeoTaggedImage(file.FullName);

                        if (double.IsNaN(geoTaggedImage.MercatorLocation.X))
                            continue;

                        this._images.Add(geoTaggedImage);
                    }
                    catch (Exception)
                    {
                        continue;
                    }

                }
            }
            //});

        }

        public List<Group<GeoTaggedImage>> Get(double scale)
        {
            lock (_lock)
            {
                var cluster = new PointClusters<GeoTaggedImage>(_images);

                var logic = new Func<GeoTaggedImage, GeoTaggedImage, bool>((first, second) =>
                {
                    var distance = Point.EuclideanDistance(first.MercatorLocation, second.MercatorLocation);

                    var tolerance = 50 * ConversionHelper.InchToMeterFactor / 96.0;

                    //var zoomLevel = GoogleMapsUtility.GetGoogleZoomLevel(scale);

                    //var groundResolution = WebMercatorUtility.CalculateGroundResolution(zoomLevel, 35);

                    //var tolerance2 = groundResolution * 50;

                    return distance * scale < tolerance;
                });

                return cluster.GetClusters(logic);
            }
        }

        public BoundingBox Extent
        {
            get
            {
                if (_images == null)
                {
                    return BoundingBox.NaN;
                }

                return BoundingBox.CalculateBoundingBox(_images.Select(i => i.MercatorLocation));
            }
        }


        
    }
}
