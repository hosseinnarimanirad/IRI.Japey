using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRI.Msh.Common.Primitives;
using IRI.Jab.Common.Model;
using IRI.Jab.Common;

using System.Net;
using IRI.Msh.CoordinateSystem;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Data;
using IRI.Ket.Common.Model;
using IRI.Jab.Common.Model;
using IRI.Msh.CoordinateSystem.MapProjection;
using IRI.Msh.Common.Model;
using IRI.Jab.Common.TileServices;

namespace IRI.Jab.Common
{
    public class TileServiceLayer : BaseLayer
    {
        public int GroupId { get; set; } = 1;

        public static readonly byte[] notFoundImage;

        static TileServiceLayer()
        {
            notFoundImage = IRI.Jab.Common.Helpers.ImageUtility.AsByteArray(IRI.Jab.Common.Properties.Resources.imageNotFound);
        }

        private TileServices.TileCacheAddress _cache;

        private TileServices.TileMapProvider _mapProvider;


        private bool _isCacheEnabled;

        public bool IsCacheEnabled
        {
            get { return _isCacheEnabled; }
            set
            {
                this._isCacheEnabled = value;
                RaisePropertyChanged();
            }
        }

        public string ProviderFullName
        {
            get { return _mapProvider.FullName; }
        }

        //public TileServices.TileType TileType
        //{
        //    get { return _mapProvider.TileType; }
        //}

        public bool IsOffline { get; set; }


        public TileServiceLayer(TileServices.TileMapProvider mapProvider, Func<TileInfo, string> getFileName = null)
        {
            //this.Provider = TileServices.MapProviderType.Custom;

            this._cache = new TileServices.TileCacheAddress(mapProvider.Provider.EnglishTitle, mapProvider.MapType.EnglishTitle, getFileName);

            this.VisualParameters = new VisualParameters(System.Windows.Media.Colors.Transparent);

            this._mapProvider = mapProvider;
        }

        public override BoundingBox Extent
        {
            get
            {
                throw new NotImplementedException();
            }
            protected set { }
        }


        public override RenderingApproach Rendering
        {
            get { return RenderingApproach.Tiled; }
            protected set { }
        }

        public override LayerType Type
        {
            get { return LayerType.BaseMap; }
            protected set { }
        }

        private FrameworkElement frameworkElement;

        public FrameworkElement Element
        {
            get { return this.frameworkElement; }

            set
            {
                this.frameworkElement = value;

                this.BindWithFrameworkElement(value);

                RaisePropertyChanged();
            }
        }

        public void BindWithFrameworkElement(FrameworkElement element)
        {
            if (element is Path || element is Rectangle)
            {
                Binding binding1 = new Binding() { Source = this, Path = new PropertyPath("VisualParameters.Stroke"), Mode = BindingMode.TwoWay };
                element.SetBinding(Path.StrokeProperty, binding1);

                //Binding binding2 = new Binding() { Source = this._parent, Path = new PropertyPath("VisualParameters.Fill"), Mode = BindingMode.TwoWay };
                //element.SetBinding(Path.FillProperty, binding2);

                Binding binding3 = new Binding() { Source = this, Path = new PropertyPath("VisualParameters.StrokeThickness"), Mode = BindingMode.TwoWay };
                element.SetBinding(Path.StrokeThicknessProperty, binding3);

                Binding binding4 = new Binding() { Source = this, Path = new PropertyPath("VisualParameters.Visibility"), Mode = BindingMode.TwoWay };
                element.SetBinding(Path.VisibilityProperty, binding4);

                Binding binding5 = new Binding() { Source = this, Path = new PropertyPath("VisualParameters.Opacity"), Mode = BindingMode.TwoWay };
                element.SetBinding(Path.OpacityProperty, binding5);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public void EnableCaching(string baseDirectory)
        {
            IsCacheEnabled = true;

            this._cache.BaseDirectory = baseDirectory;
        }

        public async Task<GeoReferencedImage> DownloadTileAsync(TileInfo tile, WebProxy proxy)
        {
            try
            {
                if (IsOffline && _mapProvider.RequireInternetConnection)
                {
                    return GetNotFoundImage(tile);
                }

                WiseWebClient client = new WiseWebClient(30);

                if (proxy != null)
                {
                    client.Proxy = proxy;
                }

                client.Headers.Add("user-agent", "App!");

                //var zoom = IRI.Msh.Common.Mapping.GoogleMapsUtility.GetGoogleZoomLevel(mapScale);

                //google map
                //var url = $@"https://mt0.google.com/vt?x={tile.ColumnNumber}&y={tile.RowNumber}&z={tile.ZoomLevel}";

                ////google terrain
                //var url = $@"http://mt1.google.com/vt/lyrs=t@131,r@176163100&hl=en&x={tile.ColumnNumber}&y={tile.RowNumber}&z={tile.ZoomLevel}";

                //var url = TileServices.MapProviderFactory.GetUrl(_cache.Provider, _cache.Type, tile);
                var url = this._mapProvider.GetUrl(tile);

                if (url == null)
                {
                    return GetNotFoundImage(tile);
                }

                System.Diagnostics.Debug.WriteLine("Getting Tile at " + url);

                var byteImage = await client.DownloadDataTaskAsync(url);

                if (IRI.Jab.Common.Helpers.ImageUtility.ToImage(byteImage) == null)
                    return GetNotFoundImage(tile);

                //return new GeoReferencedImage(byteImage, tile.MercatorExtent.Transform(i => MapProjects.MercatorToGeodetic(i)));
                return new GeoReferencedImage(byteImage, tile.GeodeticExtent);
            }
            catch (Exception ex)
            {
                //var byteImage = IRI.Jab.Common.Helpers.ImageUtility.AsByteArray(Properties.Resources.imageNotFound);

                //return new GeoReferencedImage(byteImage, tile.MercatorExtent.Transform(i => Projection.MercatorToGeodetic(i)), false);
                return GetNotFoundImage(tile);
            }
        }

        private GeoReferencedImage GetNotFoundImage(TileInfo tile)
        {
            return new GeoReferencedImage(notFoundImage, tile.GeodeticExtent, false);
        }

        public GeoReferencedImage DownloadTile(TileInfo tile, WebProxy proxy)
        {
            try
            {
                if (IsOffline && _mapProvider.RequireInternetConnection)
                {
                    return GetNotFoundImage(tile);
                }

                WiseWebClient client = new WiseWebClient(3000);

                if (proxy != null)
                {
                    client.Proxy = proxy;
                }

                client.Headers.Add("user-agent", "App!");

                //var zoom = IRI.Msh.Common.Mapping.GoogleMapsUtility.GetGoogleZoomLevel(mapScale);

                //google map
                //var url = $@"https://mt0.google.com/vt?x={tile.ColumnNumber}&y={tile.RowNumber}&z={tile.ZoomLevel}";

                ////google terrain
                //var url = $@"http://mt1.google.com/vt/lyrs=t@131,r@176163100&hl=en&x={tile.ColumnNumber}&y={tile.RowNumber}&z={tile.ZoomLevel}";

                //var url = TileServices.MapProviderFactory.GetUrl(_cache.Provider, _cache.Type, tile);
                var url = _mapProvider.GetUrl(tile);

                if (url == null)
                {
                    return GetNotFoundImage(tile);
                }

                var byteImage = client.DownloadData(url);

                return new GeoReferencedImage(byteImage, tile.GeodeticExtent);
            }
            catch (Exception ex)
            {
                //var byteImage = IRI.Jab.Common.Helpers.ImageUtility.AsByteArray(IRI.Jab.Common.Properties.Resources.imageNotFound);

                return GetNotFoundImage(tile);
            }
        }

        public async Task<GeoReferencedImage> GetTileAsync(TileInfo tile, WebProxy proxy)
        {
            GeoReferencedImage result;

            if (IsCacheEnabled && _mapProvider.AllowCache)
            {
                result = _cache.GetTile(tile);

                if (!result.IsValid)
                {
                    result = await DownloadTileAsync(tile, proxy);

                    //Do not save imageNotFounds
                    if (result.IsValid)
                    {
                        await _cache.SaveAsync(result, tile);
                    }
                }
            }
            else
            {
                result = await DownloadTileAsync(tile, proxy);
            }

            return result;
        }

        public GeoReferencedImage GetTile(TileInfo tile, WebProxy proxy)
        {
            GeoReferencedImage result;

            if (IsCacheEnabled)
            {
                result = _cache.GetTile(tile);

                if (!result.IsValid)
                {
                    result = DownloadTile(tile, proxy);

                    //Do not save imageNotFounds
                    if (result.IsValid)
                    {
                        _cache.Save(result, tile);
                    }
                }
            }
            else
            {
                result = DownloadTile(tile, proxy);
            }

            return result;
        }


        public bool HasTheSameMapProvider(TileMapProvider provider)
        { 
            return _mapProvider == provider;
        }
    }
}
