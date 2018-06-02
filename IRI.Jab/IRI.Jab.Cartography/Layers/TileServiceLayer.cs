using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRI.Msh.Common.Primitives;
using IRI.Jab.Cartography.Model;
using IRI.Jab.Common;

using System.Net;
using IRI.Sta.CoordinateSystem;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Data;
using IRI.Ket.Common.Model;
using IRI.Jab.Common.Model;
using IRI.Sta.CoordinateSystem.MapProjection;
using IRI.Msh.Common.Model;

namespace IRI.Jab.Cartography
{
    public class TileServiceLayer : BaseLayer
    {
        public int GroupId { get; set; } = 1;

        public static readonly byte[] notFoundImage;

        static TileServiceLayer()
        {
            notFoundImage = IRI.Jab.Common.Helpers.ImageUtility.AsByteArray(Properties.Resources.imageNotFound);
        }

        private TileServices.MapProviderType _provider;

        public TileServices.MapProviderType Provider
        {
            get { return _provider; }
            set { _provider = value; }
        }

        private TileServices.TileType _tileType;

        public TileServices.TileType TileType
        {
            get { return _tileType; }
            set { _tileType = value; }
        }

        public bool IsOffline { get; set; }

        public TileServiceLayer(TileServices.MapProviderType provider, TileServices.TileType type, Func<TileInfo, string> getFilePath = null)
        {
            this.Provider = provider;

            this.TileType = type;

            this.Cache = new TileServices.TileCacheAddress(provider, type, getFilePath);

            this.VisualParameters = new VisualParameters(System.Windows.Media.Colors.Transparent);
        }

        public override BoundingBox Extent
        {
            get
            {
                throw new NotImplementedException();
            }
            protected set { }
        }

        //public Guid Id { get; private set; }

        //public bool IsValid { get; set; }

        //private string _layerName;

        //public string LayerName
        //{
        //    get { return _layerName; }
        //    set
        //    {
        //        _layerName = value;
        //        RaisePropertyChanged();
        //    }
        //}

        public override RenderingApproach Rendering
        {
            get { return RenderingApproach.Tiled; }
            protected set { }
        }

        //public RasterizationApproach ToRasterTechnique { get { return RasterizationApproach.None; } }

        public override LayerType Type
        {
            get { return LayerType.BaseMap; }
            protected set { }
        }

        //private VisualParameters _visualParameters;

        //public VisualParameters VisualParameters
        //{
        //    get { return _visualParameters; }
        //    set
        //    {
        //        _visualParameters = value;
        //        RaisePropertyChanged();
        //    }
        //}

        //private ScaleInterval _visibleRange;

        //public ScaleInterval VisibleRange
        //{
        //    get { return _visibleRange; }
        //    set
        //    {
        //        _visibleRange = value;
        //        RaisePropertyChanged();
        //    }
        //}

        //public int ZIndex { get; set; }

        //public void Invalidate() => IsValid = false;

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

            this.Cache.BaseDirectory = baseDirectory;
        }

        private bool _isCacheEnabled;

        private bool IsCacheEnabled
        {
            get { return _isCacheEnabled; }
            set
            {
                this._isCacheEnabled = value;
                RaisePropertyChanged();
            }
        }

        private TileServices.TileCacheAddress Cache;

        public async Task<GeoReferencedImage> DownloadTileAsync(TileInfo tile, WebProxy proxy)
        {
            try
            {
                if (IsOffline)
                {
                    return new GeoReferencedImage(Common.Helpers.ImageUtility.AsByteArray(Properties.Resources.imageNotFound), tile.GeodeticExtent, false);
                }

                WiseWebClient client = new WiseWebClient(40);

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

                var url = TileServices.CacheSourceFactory.GetUrl(Cache.Provider, Cache.Type, tile);

                if (url == null)
                {
                    return GetNotFoundImage(tile);
                }

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
                if (IsOffline)
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

                var url = TileServices.CacheSourceFactory.GetUrl(Cache.Provider, Cache.Type, tile);

                if (url == null)
                {
                    return GetNotFoundImage(tile);
                }

                var byteImage = client.DownloadData(url);

                return new GeoReferencedImage(byteImage, tile.GeodeticExtent);
            }
            catch (Exception ex)
            {
                //var byteImage = IRI.Jab.Common.Helpers.ImageUtility.AsByteArray(IRI.Jab.Cartography.Properties.Resources.imageNotFound);

                return GetNotFoundImage(tile);
            }
        }

        public async Task<GeoReferencedImage> GetTileAsync(TileInfo tile, WebProxy proxy)
        {
            GeoReferencedImage result;

            if (IsCacheEnabled)
            {
                result = await Cache.GetTileAsync(tile);

                if (!result.IsValid)
                {
                    result = await DownloadTileAsync(tile, proxy);

                    //Do not save imageNotFounds
                    if (result.IsValid)
                    {
                        await Cache.SaveAsync(result, tile);
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
                result = Cache.GetTile(tile);

                if (!result.IsValid)
                {
                    result = DownloadTile(tile, proxy);

                    //Do not save imageNotFounds
                    if (result.IsValid)
                    {
                        Cache.Save(result, tile);
                    }
                }
            }
            else
            {
                result = DownloadTile(tile, proxy);
            }

            return result;
        }
    }
}
