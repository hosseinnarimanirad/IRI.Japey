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
        public static readonly char[] _serverChar;

        static TileMapProviderFactory()
        {
            _serverChar = new char[] { 'a', 'b', 'c', 'd' };

            BingSatellite = new TileMapProvider(BingProvider, "Satellite", tile => MakeBingSatelliteUrl(tile, GetServer(0, 3)));
            BingHybrid = new TileMapProvider(BingProvider, "Hybrid", tile => MakeBingHybridUrl(tile, GetServer(0, 3)));
            BingStreet = new TileMapProvider(BingProvider, "Street", tile => MakeBingStreetUrl(tile, GetServer(0, 3)));

            GoogleSatellite = new TileMapProvider(GoogleProvider, "Satellite", tile => MakeGoogleSatelliteUrl(tile, GetServer(0, 3)));
            GoogleHybrid = new TileMapProvider(GoogleProvider, "Hybrid", tile => MakeGoogleHybridUrl(tile, GetServer(0, 3)));
            GoogleRoadMap = new TileMapProvider(GoogleProvider, "RoadMap", tile => MakeGoogleRoadMapUrl(tile, GetServer(0, 3)));
            GoogleTerrain = new TileMapProvider(GoogleProvider, "Terrain", tile => MakeGoogleTerrainUrl(tile, GetServer(0, 3)));

            NokiaRoadMap = new TileMapProvider(NokiaProvider, "RoadMap", tile => MakeNokiaRoadMapUrl(tile, GetServer()));
            NokiaTerrain = new TileMapProvider(NokiaProvider, "Terrain", tile => MakeNokiaTerrainUrl(tile, GetServer()));
            NokiaSatellite = new TileMapProvider(NokiaProvider, "Satellite", tile => MakeNokiaSatelliteUrl(tile, GetServer()));
            NokiaHybrid = new TileMapProvider(NokiaProvider, "Hybrid", tile => MakeNokiaHybridUrl(tile, GetServer()));

            OpenStreetMap = new TileMapProvider(OsmProvider, "Street", tile => MakeOpenStreetMapUrl(tile, GetServerCharacter(0, 2)));
            OpenTopoMap = new TileMapProvider(OsmProvider, "Topo", tile => MakeOpenTopoMapUrl(tile, GetServerCharacter(0, 2)));


            WazeStreet = new TileMapProvider(WazeProvider, "Street", tile => MakeWazeRoadMapUrl(tile));
        }


        public static int GetServer(int min = 1, int max = 4)
        {
            return RandomHelper.Get(min, max);
        }

        public static char GetServerCharacter(int min = 1, int max = 4)
        {
            var random = RandomHelper.Get(min, max);

            return _serverChar[random];
        }

        public static string GoogleProvider = "GOOGLE";
        public static string BingProvider = "BING";
        public static string NokiaProvider = "NOKIA";
        public static string OsmProvider = "OPENSTREETMAP";
        public static string WazeProvider = "WAZE";


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

        private static string MakeBingSatelliteUrl(TileInfo tile, int server) => $@"http://a{server}.ortho.tiles.virtualearth.net/tiles/a{TileXYToQuadKey(tile.ColumnNumber, tile.RowNumber, tile.ZoomLevel)}.jpeg?g=5925";
        private static string MakeBingHybridUrl(TileInfo tile, int server) => $@"http://h{server}.ortho.tiles.virtualearth.net/tiles/h{TileXYToQuadKey(tile.ColumnNumber, tile.RowNumber, tile.ZoomLevel)}.jpeg?g=5978";
        private static string MakeBingStreetUrl(TileInfo tile, int server) => $@"http://r{server}.ortho.tiles.virtualearth.net/tiles/r{TileXYToQuadKey(tile.ColumnNumber, tile.RowNumber, tile.ZoomLevel)}.jpeg?g=5978";

        public static TileMapProvider BingSatellite { get; private set; }

        public static TileMapProvider BingStreet { get; private set; }

        public static TileMapProvider BingHybrid { get; private set; }

        #endregion


        #region Google

        // http://mt1.google.com/vt/lyrs=s@901000000&hl=en&x=4&y=10&z=5&s=Ga
        // http://khm0.google.com/kh/v=748&s=&x=1354740&y=825228&z=21

        private static string MakeGoogleRoadMapUrl(TileInfo tile, int server) => $@"https://mt{server}.google.com/vt?x={tile.ColumnNumber}&y={tile.RowNumber}&z={tile.ZoomLevel}";

        private static string MakeGoogleTerrainUrl(TileInfo tile, int server) => $@"http://mt{server}.google.com/vt/lyrs=t@131,r@176163100&hl=en&x={tile.ColumnNumber}&y={tile.RowNumber}&z={tile.ZoomLevel}";

        private static string MakeGoogleSatelliteUrl(TileInfo tile, int server) => $@"http://mt{server}.google.com/vt/lyrs=s@901000000&hl=en&x={tile.ColumnNumber}&y={tile.RowNumber}&z={tile.ZoomLevel}&s=Gal";

        private static string MakeGoogleHybridUrl(TileInfo tile, int server) => $@"http://mt{server}.google.com/vt/lyrs=y@901000000&hl=en&x={tile.ColumnNumber}&y={tile.RowNumber}&z={tile.ZoomLevel}&s=Gal";


        public static TileMapProvider GoogleSatellite { get; private set; }

        public static TileMapProvider GoogleHybrid { get; private set; }

        public static TileMapProvider GoogleRoadMap { get; private set; }

        public static TileMapProvider GoogleTerrain { get; private set; }




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


        public static TileMapProvider NokiaSatellite { get; private set; }

        public static TileMapProvider NokiaHybrid { get; private set; }

        public static TileMapProvider NokiaRoadMap { get; private set; }

        public static TileMapProvider NokiaTerrain { get; private set; }

        #endregion


        #region Osm


        private static string MakeOpenStreetMapUrl(TileInfo tile, char serverChar) => $@"http://{serverChar}.tile.openstreetmap.org/{tile.ZoomLevel}/{tile.ColumnNumber}/{tile.RowNumber}.png";

        //https://{a|b|c}.tile.opentopomap.org/{z}/{x}/{y}.png 
        private static string MakeOpenTopoMapUrl(TileInfo tile, char server) => $@"http://{server}.tile.opentopomap.org/{tile.ZoomLevel}/{tile.ColumnNumber}/{tile.RowNumber}.png";

        public static TileMapProvider OpenStreetMap { get; private set; }

        public static TileMapProvider OpenTopoMap { get; private set; }

        #endregion


        #region Waze

        //https://worldtiles3.waze.com/tiles/11/1313/805.png
        private static string MakeWazeRoadMapUrl(TileInfo tile) => $@"https://worldtiles3.waze.com/tiles/{tile.ZoomLevel}/{tile.ColumnNumber}/{tile.RowNumber}.png";

        public static TileMapProvider WazeStreet { get; private set; }

        public static List<TileMapProvider> GetAll()
        {
            return new List<TileMapProvider>()
            {
                BingHybrid, BingSatellite, BingStreet,
                GoogleHybrid, GoogleRoadMap, GoogleSatellite, GoogleTerrain,
                NokiaHybrid, NokiaRoadMap, NokiaSatellite, NokiaTerrain,
                OpenStreetMap, OpenTopoMap,
                WazeStreet
            };
        }


        #endregion
    }
}
