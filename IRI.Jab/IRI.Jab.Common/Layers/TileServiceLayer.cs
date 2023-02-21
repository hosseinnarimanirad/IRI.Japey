using System;
using System.Net;
using System.Net.Http;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Data;
using System.Threading.Tasks;

using IRI.Msh.Common.Primitives;
using IRI.Msh.Common.Model;
using IRI.Sta.Common.Model;
using IRI.Jab.Common.Model;
using IRI.Jab.Common.TileServices;


namespace IRI.Jab.Common
{
    public class TileServiceLayer : BaseLayer
    {
        public int GroupId { get; set; } = 1;

        public static readonly byte[] notFoundImage;

        static TileServiceLayer()
        {
            notFoundImage = IRI.Jab.Common.Helpers.ImageUtility.AsByteArray(Properties.Resources.imageNotFound);
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

        public async Task<GeoReferencedImage> DownloadTileAsync(TileInfo tile, WebProxy proxy)
        {
            try
            {
                if (IsOffline && _mapProvider.RequireInternetConnection)
                    return GetNotFoundImage(tile);
                 
                WiseWebClient client = new WiseWebClient(3000);

                // 1401.10.27
                // by default webClient try to get proxy from IE
                // and it makes it very slow
                // https://stackoverflow.com/a/4420429/1468295
                //if (proxy != null)
                //{
                client.Proxy = proxy;
                //}

                client.Headers.Add(HttpRequestHeader.UserAgent, "App!");
                 
                var url = this._mapProvider.GetUrl(tile);

                if (url == null)
                    return GetNotFoundImage(tile);

                System.Diagnostics.Debug.WriteLine("Getting Tile at " + url);

                var byteImage = await client.DownloadDataTaskAsync(url);

                if (IRI.Jab.Common.Helpers.ImageUtility.ToImage(byteImage) == null)
                    return GetNotFoundImage(tile);

                return new GeoReferencedImage(byteImage, tile.GeodeticExtent);
            }
            catch (Exception ex)
            {
                return GetNotFoundImage(tile);
            }
        }

        public async Task<GeoReferencedImage> GetTileAsync(TileInfo tile, HttpClient client)
        {
            GeoReferencedImage result;

            if (IsCacheEnabled && _mapProvider.AllowCache)
            {
                result = _cache.GetTile(tile);

                if (!result.IsValid)
                {
                    result = await DownloadTileAsync(tile, client);

                    //Do not save imageNotFounds
                    if (result.IsValid)
                    {
                        await _cache.SaveAsync(result, tile);
                    }
                }
            }
            else
            {
                result = await DownloadTileAsync(tile, client);
            }

            return result;
        }

        public async Task<GeoReferencedImage> DownloadTileAsync(TileInfo tile, HttpClient client)
        {
            try
            {
                if (IsOffline && _mapProvider.RequireInternetConnection)
                    return GetNotFoundImage(tile);
                   
                var url = this._mapProvider.GetUrl(tile);

                if (url == null)
                    return GetNotFoundImage(tile);

                System.Diagnostics.Debug.WriteLine("Getting Tile at " + url);

                var response = await client.GetAsync(url);

                var byteImage = await response.Content.ReadAsByteArrayAsync();

                if (IRI.Jab.Common.Helpers.ImageUtility.ToImage(byteImage) == null)
                    return GetNotFoundImage(tile);

                return new GeoReferencedImage(byteImage, tile.GeodeticExtent);
            }
            catch (Exception ex)
            {
                return GetNotFoundImage(tile);
            }
        }

        public void EnableCaching(string baseDirectory)
        {
            IsCacheEnabled = true;

            this._cache.BaseDirectory = baseDirectory;
        }

        private GeoReferencedImage GetNotFoundImage(TileInfo tile)
        {
            return new GeoReferencedImage(notFoundImage, tile.GeodeticExtent, false);
        }

        public bool HasTheSameMapProvider(TileMapProvider provider)
        {
            return _mapProvider == provider;
        }
    }
}
