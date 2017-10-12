using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRI.Ham.SpatialBase;
using Microsoft.SqlServer.Types;

using IRI.Ket.DataManagement.Model;
using IRI.Ham.SpatialBase.Mapping;

namespace IRI.Ket.DataManagement.DataSource
{
    public class ZippedImagePyramidDataSource : IDataSource
    {
        public BoundingBox Extent { get { return BoundingBox.NaN; } }

        System.IO.Compression.ZipArchive _archive;

        Func<int, int, int, string> _fileNameRule;

        public ZippedImagePyramidDataSource(string worldFilePyramidFileName, Func<int, int, int, string> fileNameRule = null)
        {
            if (!System.IO.File.Exists(worldFilePyramidFileName))
                throw new NotImplementedException();

            _archive = System.IO.Compression.ZipFile.OpenRead(worldFilePyramidFileName);

            if (fileNameRule == null)
                _fileNameRule = (z, r, c) => System.IO.Path.Combine(z.ToString(), $"{z}, {r}, {c}.jpg");
            //_fileNameRule = (z, r, c) => $"{z}/{z}, {r}, {c}.jpg";

            else
                _fileNameRule = fileNameRule;
        }

        public List<GeoReferencedImage> GetTiles(BoundingBox geographicBoundingBox, double mapScale)
        {
            //94.12.17
            //int zoomLevel = GetZoomLevel(mapScale);
            int zoomLevel = IRI.Ham.SpatialBase.Mapping.WebMercatorUtility.GetZoomLevel(mapScale);

            var result = new List<IRI.Ham.SpatialBase.GeoReferencedImage>();

            //What if there were no imagesource for this zoom level
            if (!_archive.Entries.Any(i => i.FullName.StartsWith(zoomLevel.ToString(), StringComparison.OrdinalIgnoreCase)))
            {
                return result;
            }

            var lowerLeft = WebMercatorUtility.LatLonToImageNumber(geographicBoundingBox.YMin, geographicBoundingBox.XMin, zoomLevel);

            var upperRight = WebMercatorUtility.LatLonToImageNumber(geographicBoundingBox.YMax, geographicBoundingBox.XMax, zoomLevel);

            for (int i = (int)lowerLeft.X; i <= upperRight.X; i++)
            {
                for (int j = (int)upperRight.Y; j <= lowerLeft.Y; j++)
                {
                     
                    //var zipArchive = new System.IO.Compression.ZipArchive(archive.Open());

                    if (_archive.Entries.Any(e => e.FullName.Equals(_fileNameRule(zoomLevel, j, i), StringComparison.OrdinalIgnoreCase)))
                    {
                        var stream = _archive.Entries.Single(e => e.FullName.Equals(_fileNameRule(zoomLevel, j, i), StringComparison.OrdinalIgnoreCase)).Open();

                        byte[] bytes = Common.Helpers.StreamHelper.ToByteArray(stream);

                        //using (var memoryStream = new System.IO.MemoryStream())
                        //{
                        //    stream.CopyTo(memoryStream);

                        //    bytes = memoryStream.ToArray();
                        //}

                        result.Add(new GeoReferencedImage(bytes, WebMercatorUtility.GetWgs84ImageBoundingBox(j, i, zoomLevel)));
                    }
                }
            }

            System.Diagnostics.Trace.WriteLine(string.Format("{0} Images founded; zoom level = {1}", result.Count, zoomLevel));

            return result;
        }

    }
}
