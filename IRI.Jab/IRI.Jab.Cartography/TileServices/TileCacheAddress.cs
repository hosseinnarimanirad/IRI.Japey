
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRI.Sta.Common.Primitives;
using IRI.Sta.Common.Model;

namespace IRI.Jab.Cartography.TileServices
{
    public class TileCacheAddress
    {
        private MapProviderType provider;

        public MapProviderType Provider
        {
            get { return provider; }
            set
            {
                provider = value;

                url = MakeUrl();
            }
        }


        private TileType type;

        public TileType Type
        {
            get { return type; }
            set
            {
                type = value;

                url = MakeUrl();
            }
        }


        private string baseDirectory;

        public string BaseDirectory
        {
            get { return baseDirectory; }
            set
            {
                baseDirectory = value;

                url = MakeUrl();
            }
        }


        private string url;

        public string Url { get { return url; } }

        private Func<TileInfo, string> _getFilePath;

        public TileCacheAddress(MapProviderType provider, TileType type, Func<TileInfo, string> getFilePath = null)
        {
            if (getFilePath == null)
            {
                _getFilePath = t => $"{t.ZoomLevel}\\{t.RowNumber}_{t.ColumnNumber}.png";
            }
            else
            {
                _getFilePath = getFilePath;
            }

            Provider = provider;

            Type = type;
        }

        private string MakeUrl()
        {
            return $"{BaseDirectory}\\{Enum.GetName(typeof(MapProviderType), Provider)}\\{Enum.GetName(typeof(TileType), Type)}";
        }

        private string GetFilePath(TileInfo tile)
        {
            //return $"{Url}\\{tile.ZoomLevel}\\{tile.RowNumber}_{tile.ColumnNumber}.png";
            return $"{Url}\\{_getFilePath(tile)}";
        }

        public Task<GeoReferencedImage> GetTileAsync(TileInfo tile)
        {
            string filePath = GetFilePath(tile);

            System.IO.DirectoryInfo dinfo = new System.IO.DirectoryInfo(filePath);

            if (System.IO.File.Exists(filePath))
            {
                return Task.Run(() =>
                {
                    var bytes = System.IO.File.ReadAllBytes(filePath);

                    return new GeoReferencedImage(bytes, tile.GeodeticExtent);
                });
            }
            else
                return Task.Run(() => { return new GeoReferencedImage(null, BoundingBox.NaN, false); });
        }

        public GeoReferencedImage GetTile(TileInfo tile)
        {
            string filePath = GetFilePath(tile);

            if (System.IO.File.Exists(filePath))
            {
                var bytes = System.IO.File.ReadAllBytes(filePath);

                return new GeoReferencedImage(bytes, tile.GeodeticExtent);
            }
            else
                return new GeoReferencedImage(null, BoundingBox.NaN, false);
        }

        //POTENTIALLY ERROR PRONE: the process cannot access the file or path because another process is using...
        internal Task SaveAsync(GeoReferencedImage tileImage, TileInfo tile)
        {
            //return Task.Run(() =>
            //{
            //    var filePath = GetFilePath(tile);

            //    if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(filePath)))
            //    {
            //        System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(filePath));
            //    }

            //    System.IO.File.WriteAllBytes(GetFilePath(tile), tileImage.Image);
            //});

            return Task.Run(() =>
            {
                Save(tileImage, tile);
            });
        }

        internal void Save(GeoReferencedImage tileImage, TileInfo tile)
        {

            var filePath = GetFilePath(tile);

            if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(filePath)))
            {
                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(filePath));
            }

            try
            {
                System.IO.File.WriteAllBytes(GetFilePath(tile), tileImage.Image);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
    }
}
