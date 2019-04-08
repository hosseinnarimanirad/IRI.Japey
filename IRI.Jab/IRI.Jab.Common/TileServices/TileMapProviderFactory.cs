using IRI.Jab.Common.Helpers;
using IRI.Msh.Common.Helpers;
using IRI.Msh.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Jab.Common.TileServices
{
    public static class TileMapProviderFactory
    {
        public static readonly char[] _serverChar = new char[] { 'a', 'b', 'c', 'd' };

        public static string GoogleProvider = "GOOGLE";
        public static string BingProvider = "BING";
        public static string NokiaProvider = "NOKIA";
        public static string OsmProvider = "OPENSTREETMAP";
        public static string WazeProvider = "WAZE";
        public static string CartoProvider = "CARTO";
        public static string Yandex = "YANDEX";

        //this can be used in the case of interanet network without internet connection. samples:
        //this.AddProvider(TileMapProviderFactory.CreateInteranetProvider("localGoogle", "roadMap", t => $@"http://v-gisserver2/Google/Road/{t.ZoomLevel}/gm_{t.ColumnNumber}_{t.RowNumber}_{t.ZoomLevel}.png"));
        // this.AddProvider(TileMapProviderFactory.CreateInteranetProvider("localGoogle", "terrainMap", t => $@"http://v-gisserver2/Google/TerrainWithRoad/{t.ZoomLevel}/gtr_{t.ColumnNumber}_{t.RowNumber}_{t.ZoomLevel}.jpg"));
        // this.AddProvider(TileMapProviderFactory.CreateInteranetProvider("localGoogle", "satelliteMap", t => $@"http://v-gisserver2/Google/Satellite/{t.ZoomLevel}/gs_{t.ColumnNumber}_{t.RowNumber}_{t.ZoomLevel}.jpg"));
        // this.AddProvider(TileMapProviderFactory.CreateInteranetProvider("localGoogle", "hybridMap", t => $@"http://v-gisserver2/Google/Satellite/{t.ZoomLevel}/gs_{t.ColumnNumber}_{t.RowNumber}_{t.ZoomLevel}.jpg"));
        public static TileMapProvider CreateInteranetProvider(string providerName, string subTitle, Func<TileInfo, string> interanetUrlFunc)
        {
            TileMapProvider result = new TileMapProvider(providerName, subTitle, interanetUrlFunc)
            {
                RequireInternetConnection = false
            };

            return result;
        }

        #region Bing

        //this is used for bing maps
        public static string TileXYToQuadKey(int tileX, int tileY, int levelOfDetail)
        {
            StringBuilder quadKey = new StringBuilder();
            for (int i = levelOfDetail; i > 0; i--)
            {
                char digit = '0';
                int mask = 1 << (i - 1);
                if ((tileX & mask) != 0)
                {
                    digit++;
                }
                if ((tileY & mask) != 0)
                {
                    digit++;
                    digit++;
                }
                quadKey.Append(digit);
            }
            return quadKey.ToString();
        }

        private static string MakeBingSatelliteUrl(TileInfo tile, string server) => $@"http://a{server}.ortho.tiles.virtualearth.net/tiles/a{TileXYToQuadKey(tile.ColumnNumber, tile.RowNumber, tile.ZoomLevel)}.jpeg?g=5925";
        private static string MakeBingHybridUrl(TileInfo tile, string server) => $@"http://h{server}.ortho.tiles.virtualearth.net/tiles/h{TileXYToQuadKey(tile.ColumnNumber, tile.RowNumber, tile.ZoomLevel)}.jpeg?g=5978";
        private static string MakeBingStreetUrl(TileInfo tile, string server) => $@"http://r{server}.ortho.tiles.virtualearth.net/tiles/r{TileXYToQuadKey(tile.ColumnNumber, tile.RowNumber, tile.ZoomLevel)}.jpeg?g=5978";

        private static TileMapProvider _bingSatellite;
        public static TileMapProvider BingSatellite
        {
            get
            {
                if (_bingSatellite == null)
                {
                    _bingSatellite = new TileMapProvider(
                        new Model.Globalization.PersianEnglishItem("بینگ", BingProvider),
                        new Model.Globalization.PersianEnglishItem("ماهواره", "Satellite"),
                        tile => MakeBingSatelliteUrl(tile, GetServer(0, 3)))
                    {
                        Thumbnail = ResourceHelper.ReadBinaryStreamFromResource(@"IRI.Jab.Common;component/Assets/Images/BaseMaps/bingSatellite.jpg")
                    };
                }

                return _bingSatellite;
            }
        }
         
        private static TileMapProvider _bingStreet;
        public static TileMapProvider BingStreet
        {
            get
            {
                if (_bingStreet == null)
                {
                    _bingStreet = new TileMapProvider(
                        new Model.Globalization.PersianEnglishItem("بینگ", BingProvider),
                        new Model.Globalization.PersianEnglishItem("راه", "Street"),
                        tile => MakeBingStreetUrl(tile, GetServer(0, 3)))
                    {
                        Thumbnail = ResourceHelper.ReadBinaryStreamFromResource(@"IRI.Jab.Common;component/Assets/Images/BaseMaps/bingStreet.jpg")
                    };
                }

                return _bingStreet;
            }
        }


        private static TileMapProvider _bingHybrid;
        public static TileMapProvider BingHybrid
        {
            get
            {
                if (_bingHybrid == null)
                {
                    _bingHybrid = new TileMapProvider(
                        new Model.Globalization.PersianEnglishItem("بینگ", BingProvider),
                        new Model.Globalization.PersianEnglishItem("ترکیبی", "Hybrid"),
                        tile => MakeBingHybridUrl(tile, GetServer(0, 3)))
                    {
                        Thumbnail = ResourceHelper.ReadBinaryStreamFromResource(@"IRI.Jab.Common;component/Assets/Images/BaseMaps/bingHybrid.jpg")
                    };
                }

                return _bingHybrid;
            }
        }

        #endregion


        #region Google

        // http://mt1.google.com/vt/lyrs=s@901000000&hl=en&x=4&y=10&z=5&s=Ga
        // http://khm0.google.com/kh/v=748&s=&x=1354740&y=825228&z=21

        private static string MakeGoogleRoadMapUrl(TileInfo tile, string server) => $@"https://mt{server}.google.com/vt?x={tile.ColumnNumber}&y={tile.RowNumber}&z={tile.ZoomLevel}";

        private static string MakeGoogleTerrainUrl(TileInfo tile, string server) => $@"http://mt{server}.google.com/vt/lyrs=t@131,r@176163100&hl=en&x={tile.ColumnNumber}&y={tile.RowNumber}&z={tile.ZoomLevel}";

        private static string MakeGoogleSatelliteUrl(TileInfo tile, string server) => $@"http://mt{server}.google.com/vt/lyrs=s@901000000&hl=en&x={tile.ColumnNumber}&y={tile.RowNumber}&z={tile.ZoomLevel}&s=Gal";

        private static string MakeGoogleHybridUrl(TileInfo tile, string server) => $@"http://mt{server}.google.com/vt/lyrs=y@901000000&hl=en&x={tile.ColumnNumber}&y={tile.RowNumber}&z={tile.ZoomLevel}&s=Gal";


        private static TileMapProvider _googleSatellite;
        public static TileMapProvider GoogleSatellite
        {
            get
            {
                if (_googleSatellite == null)
                {
                    _googleSatellite = new TileMapProvider(
                        new Model.Globalization.PersianEnglishItem("گوگل", GoogleProvider),
                        new Model.Globalization.PersianEnglishItem("ماهواره", "Satellite"),
                        tile => MakeGoogleSatelliteUrl(tile, GetServer(0, 3)))
                    {
                        Thumbnail = ResourceHelper.ReadBinaryStreamFromResource(@"IRI.Jab.Common;component/Assets/Images/BaseMaps/googleSatellite.jpg")
                    };
                }

                return _googleSatellite;
            }
        }


        private static TileMapProvider _googleHybrid;
        public static TileMapProvider GoogleHybrid
        {
            get
            {
                if (_googleHybrid == null)
                {
                    _googleHybrid = new TileMapProvider(
                        new Model.Globalization.PersianEnglishItem("گوگل", GoogleProvider),
                        new Model.Globalization.PersianEnglishItem("ترکیبی", "Hybrid"),
                        tile => MakeGoogleHybridUrl(tile, GetServer(0, 3)))
                    {
                        Thumbnail = ResourceHelper.ReadBinaryStreamFromResource(@"IRI.Jab.Common;component/Assets/Images/BaseMaps/googleHybrid.jpg")
                    };
                }

                return _googleHybrid;
            }
        }


        private static TileMapProvider _googleRoadMap;
        public static TileMapProvider GoogleRoadMap
        {
            get
            {
                if (_googleRoadMap == null)
                {
                    _googleRoadMap = new TileMapProvider(
                        new Model.Globalization.PersianEnglishItem("گوگل", GoogleProvider),
                        new Model.Globalization.PersianEnglishItem("راه", "RoadMap"),
                        tile => MakeGoogleRoadMapUrl(tile, GetServer(0, 3)))
                    {
                        Thumbnail = ResourceHelper.ReadBinaryStreamFromResource(@"IRI.Jab.Common;component/Assets/Images/BaseMaps/googleRoadmap.jpg")
                    };
                }

                return _googleRoadMap;
            }
        }


        private static TileMapProvider _googleTerrain;
        public static TileMapProvider GoogleTerrain
        {
            get
            {
                if (_googleTerrain == null)
                {
                    _googleTerrain = new TileMapProvider(
                        new Model.Globalization.PersianEnglishItem("گوگل", GoogleProvider),
                        new Model.Globalization.PersianEnglishItem("توپو", "Terrain"),
                        tile => MakeGoogleTerrainUrl(tile, GetServer(0, 3)))
                    {
                        Thumbnail = ResourceHelper.ReadBinaryStreamFromResource(@"IRI.Jab.Common;component/Assets/Images/BaseMaps/googleTerrain.png")
                    };
                }

                return _googleTerrain;
            }
        }



        #endregion


        #region Nokia

        //Nokia
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tile"></param>
        /// <param name="server">1, 2, 3 or 5</param>
        /// <returns></returns>
        private static string MakeNokiaRoadMapUrl(TileInfo tile, int server) => $@"http://{server}.maps.nlp.nokia.com/maptile/2.1/maptile/newest/normal.day/{tile.ZoomLevel}/{tile.ColumnNumber}/{tile.RowNumber}/256/png8?app_id=SqE1xcSngCd3m4a1zEGb&token=r0sR1DzqDkS6sDnh902FWQ&lg=ENG";

        private static string MakeNokiaTerrainUrl(TileInfo tile, int server) => $@"http://{server}.maps.nlp.nokia.com/maptile/2.1/maptile/newest/terrain.day/{tile.ZoomLevel}/{tile.ColumnNumber}/{tile.RowNumber}/256/png8?app_id=SqE1xcSngCd3m4a1zEGb&token=r0sR1DzqDkS6sDnh902FWQ&lg=ENG";

        private static string MakeNokiaSatelliteUrl(TileInfo tile, int server) => $@"http://{server}.maps.nlp.nokia.com/maptile/2.1/maptile/newest/satellite.day/{tile.ZoomLevel}/{tile.ColumnNumber}/{tile.RowNumber}/256/png8?app_id=SqE1xcSngCd3m4a1zEGb&token=r0sR1DzqDkS6sDnh902FWQ&lg=ENG";

        private static string MakeNokiaHybridUrl(TileInfo tile, int server) => $@"http://{server}.maps.nlp.nokia.com/maptile/2.1/maptile/newest/hybrid.day/{tile.ZoomLevel}/{tile.ColumnNumber}/{tile.RowNumber}/256/png8?app_id=SqE1xcSngCd3m4a1zEGb&token=r0sR1DzqDkS6sDnh902FWQ&lg=ENG";


        //public static TileMapProvider NokiaSatellite { get; private set; }

        //public static TileMapProvider NokiaHybrid { get; private set; }

        //public static TileMapProvider NokiaRoadMap { get; private set; }

        //public static TileMapProvider NokiaTerrain { get; private set; }

        #endregion


        #region Osm


        private static string MakeOpenStreetMapUrl(TileInfo tile, char serverChar) => $@"http://{serverChar}.tile.openstreetmap.org/{tile.ZoomLevel}/{tile.ColumnNumber}/{tile.RowNumber}.png";

        //https://{a|b|c}.tile.opentopomap.org/{z}/{x}/{y}.png 
        private static string MakeOpenTopoMapUrl(TileInfo tile, char serverChar) => $@"http://{serverChar}.tile.opentopomap.org/{tile.ZoomLevel}/{tile.ColumnNumber}/{tile.RowNumber}.png";
         
        //https://tiles.wmflabs.org/hikebike/11/1103/669.png
        private static string MakeOsmHikeBikeUrl(TileInfo tile) => $@"https://tiles.wmflabs.org/hikebike/{tile.ZoomLevel}/{tile.ColumnNumber}/{tile.RowNumber}.png";

        //https://m1.mapserver.mapy.cz/winter-m/13-5264-3223
        //server: 1, 2, 3, 4
        private static string MakeMapyWinterUrl(TileInfo tile) => $@"https://m{GetServer(1, 4)}.mapserver.mapy.cz/winter-m/{tile.ZoomLevel}-{tile.ColumnNumber}-{tile.RowNumber}";

        private static string MakeMapyTouristUrl(TileInfo tile) => $@"https://m{GetServer(1, 4)}.mapserver.mapy.cz/turist-m/{tile.ZoomLevel}-{tile.ColumnNumber}-{tile.RowNumber}";

        //http://c.tile.stamen.com/watercolor/${z}/${x}/${y}.jpg 
        private static string MakeStamenWatercolorUrl(TileInfo tile) => $@"http://{GetServerCharacter()}.tile.stamen.com/watercolor/{tile.ZoomLevel}/{tile.ColumnNumber}/{tile.RowNumber}.jpg";


        private static TileMapProvider _openStreetMap;
        public static TileMapProvider OpenStreetMap
        {
            get
            {
                if (_openStreetMap == null)
                {
                    _openStreetMap = new TileMapProvider(
                        new Model.Globalization.PersianEnglishItem("OSM", OsmProvider),
                        new Model.Globalization.PersianEnglishItem("راه", "Street"),
                        tile => MakeOpenStreetMapUrl(tile, GetServerCharacter()))
                    {
                        Thumbnail = ResourceHelper.ReadBinaryStreamFromResource(@"IRI.Jab.Common;component/Assets/Images/BaseMaps/openStreetMap.png")
                    };
                }

                return _openStreetMap;
            }
        }


        private static TileMapProvider _openTopoMap;
        public static TileMapProvider OpenTopoMap
        {
            get
            {
                if (_openTopoMap == null)
                {
                    _openTopoMap = new TileMapProvider(
                        new Model.Globalization.PersianEnglishItem("OSM", OsmProvider),
                        new Model.Globalization.PersianEnglishItem("توپو", "Topo"),
                        tile => MakeOpenTopoMapUrl(tile, GetServerCharacter()))
                    {
                        Thumbnail = ResourceHelper.ReadBinaryStreamFromResource(@"IRI.Jab.Common;component/Assets/Images/BaseMaps/openTopoMap.png")
                    };
                }

                return _openTopoMap;
            }
        }


        private static TileMapProvider _mapyWinter;
        public static TileMapProvider MapyWinter
        {
            get
            {
                if (_mapyWinter == null)
                {
                    _mapyWinter = new TileMapProvider(
                        new Model.Globalization.PersianEnglishItem("OSM", OsmProvider),
                        new Model.Globalization.PersianEnglishItem("MapyWinter", "MapyWinter"),
                        tile => MakeMapyWinterUrl(tile))
                    {
                        Thumbnail = ResourceHelper.ReadBinaryStreamFromResource(@"IRI.Jab.Common;component/Assets/Images/BaseMaps/mapyWinter.jpg")
                    };
                }

                return _mapyWinter;
            }
        }


        private static TileMapProvider _mapyTourist;
        public static TileMapProvider MapyTourist
        {
            get
            {
                if (_mapyTourist == null)
                {
                    _mapyTourist = new TileMapProvider(
                        new Model.Globalization.PersianEnglishItem("OSM", OsmProvider),
                        new Model.Globalization.PersianEnglishItem("MapyTourist", "MapyTourist"),
                        tile => MakeMapyTouristUrl(tile))
                    {
                        Thumbnail = ResourceHelper.ReadBinaryStreamFromResource(@"IRI.Jab.Common;component/Assets/Images/BaseMaps/mapyTourism.jpg")
                    };
                }

                return _mapyTourist;
            }
        }


        private static TileMapProvider _osmHikeBike;
        public static TileMapProvider OsmHikeBike
        {
            get
            {
                if (_osmHikeBike == null)
                {
                    _osmHikeBike = new TileMapProvider(
                        new Model.Globalization.PersianEnglishItem("OSM", OsmProvider),
                        new Model.Globalization.PersianEnglishItem("طبیعت‌گردی", "HikeBike"),
                        tile => MakeOsmHikeBikeUrl(tile))
                    {
                        Thumbnail = ResourceHelper.ReadBinaryStreamFromResource(@"IRI.Jab.Common;component/Assets/Images/BaseMaps/osmHikeBike.jpg")
                    };
                }

                return _osmHikeBike;
            }
        }


        private static TileMapProvider _stamentWatercolor;
        public static TileMapProvider StamenWatercolor
        {
            get
            {
                if (_stamentWatercolor == null)
                {
                    _stamentWatercolor = new TileMapProvider(
                        new Model.Globalization.PersianEnglishItem("OSM", OsmProvider),
                        new Model.Globalization.PersianEnglishItem("آبرنگ", "Watercolor"),
                        tile => MakeStamenWatercolorUrl(tile))
                    {
                        Thumbnail = ResourceHelper.ReadBinaryStreamFromResource(@"IRI.Jab.Common;component/Assets/Images/BaseMaps/stamenWatercolor.jpg")
                    };
                }

                return _stamentWatercolor;
            }
        }



        #endregion


        #region Waze

        //https://worldtiles3.waze.com/tiles/11/1313/805.png
        //server: 1, 2, 3, 4
        private static string MakeWazeRoadMapUrl(TileInfo tile) => $@"https://worldtiles{GetServer()}.waze.com/tiles/{tile.ZoomLevel}/{tile.ColumnNumber}/{tile.RowNumber}.png";

        private static TileMapProvider _wazeStreet;
        public static TileMapProvider WazeStreet
        {
            get
            {
                if (_wazeStreet == null)
                {
                    _wazeStreet = new TileMapProvider(
                        new Model.Globalization.PersianEnglishItem(WazeProvider, WazeProvider),
                        new Model.Globalization.PersianEnglishItem("راه", "Street"),
                        tile => MakeWazeRoadMapUrl(tile))
                    {
                        Thumbnail = ResourceHelper.ReadBinaryStreamFromResource(@"IRI.Jab.Common;component/Assets/Images/BaseMaps/waze.png")
                    };
                }

                return _wazeStreet;
            }
        }

        #endregion


        #region Carto

        //@2x parameter say image should be twise in size
        //servers: a, b, c, d
        //https://cartodb-basemaps-c.global.ssl.fastly.net/light_all/14/10525/6444@2x.png

        private static string MakeCartoLightUrl(TileInfo tile) => $@"https://cartodb-basemaps-{GetServerCharacter()}.global.ssl.fastly.net/light_all/{tile.ZoomLevel}/{tile.ColumnNumber}/{tile.RowNumber}.png";

        private static string MakeCartoDarkUrl(TileInfo tile) => $@"https://cartodb-basemaps-{GetServerCharacter()}.global.ssl.fastly.net/dark_all/{tile.ZoomLevel}/{tile.ColumnNumber}/{tile.RowNumber}.png";

        private static TileMapProvider _cartoDark;
        public static TileMapProvider CartoDark
        {
            get
            {
                if (_cartoDark == null)
                {
                    _cartoDark = new TileMapProvider(
                        new Model.Globalization.PersianEnglishItem("کارتو", CartoProvider),
                        new Model.Globalization.PersianEnglishItem("تیره", "Dark"),
                        tile => MakeCartoDarkUrl(tile))
                    {
                        Thumbnail = ResourceHelper.ReadBinaryStreamFromResource(@"IRI.Jab.Common;component/Assets/Images/BaseMaps/cartoDark.jpg")
                    };
                }

                return _cartoDark;
            }
        }

        private static TileMapProvider _cartoLight;
        public static TileMapProvider CartoLight
        {
            get
            {
                if (_cartoLight == null)
                {
                    _cartoLight = new TileMapProvider(
                        new Model.Globalization.PersianEnglishItem("کارتو", CartoProvider),
                        new Model.Globalization.PersianEnglishItem("روشن", "Light"),
                        tile => MakeCartoLightUrl(tile))
                    {
                        Thumbnail = ResourceHelper.ReadBinaryStreamFromResource(@"IRI.Jab.Common;component/Assets/Images/BaseMaps/cartoLight.jpg")
                    };
                }

                return _cartoLight;
            }
        }

        #endregion




        #region Yandex

        ////در راستای y شیفت داره
        ////https://vec01.maps.yandex.net/tiles?l=map&x=10529&y=6455&z=14
        ////servers: 01,02,03,04
        //private static string MakeYandexMapUrl(TileInfo tile) => $@"https://vec0{GetServer(1, 4)}.maps.yandex.net/tiles?l=map&x={tile.ColumnNumber}&y={tile.RowNumber}&z={tile.ZoomLevel}";


        //private static TileMapProvider _yandexMap;
        //public static TileMapProvider YandexMap
        //{
        //    get
        //    {
        //        if (_yandexMap == null)
        //        {
        //            _yandexMap = new TileMapProvider(
        //                new Model.Globalization.PersianEnglishItem("یاندکس", Yandex),
        //                new Model.Globalization.PersianEnglishItem("راه", "Road"),
        //                tile => MakeYandexMapUrl(tile))
        //            {
        //                Thumbnail = ResourceHelper.ReadBinaryStreamFromResource(@"IRI.Jab.Common;component/Assets/Images/BaseMaps/cartoDark.jpg")
        //            };
        //        }

        //        return _yandexMap;
        //    }
        //}

        #endregion

        public static List<TileMapProvider> GetAll()
        {
            return new List<TileMapProvider>()
            {
                BingHybrid, BingSatellite, BingStreet,
                GoogleHybrid, GoogleRoadMap, GoogleSatellite, GoogleTerrain,
                OpenStreetMap, OpenTopoMap,
                WazeStreet
            };
        }

        public static List<TileMapProvider> GetDefault()
        {
            return new List<TileMapProvider>()
            {
                BingHybrid, BingSatellite,
                GoogleHybrid, GoogleRoadMap, GoogleSatellite, GoogleTerrain,
                OpenStreetMap, OpenTopoMap
            };
        }


        public static string GetServer(int min = 1, int max = 4)
        {
            //first bound is inclusive second bound is exclusive
            return RandomHelper.Get(min, max + 1).ToString(System.Globalization.CultureInfo.InvariantCulture);
        }

        public static char GetServerCharacter(int min = 0, int max = 2)
        {
            //first bound is inclusive second bound is exclusive
            var random = RandomHelper.Get(min, max + 1);

            return _serverChar[random];
        }

    }
}
