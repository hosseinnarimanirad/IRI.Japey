
using IRI.Sta.Common.Helpers;
using IRI.Sta.Common.Model;
using IRI.Jab.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Jab.Cartography.TileServices
{
    public static class CacheSourceFactory
    {
        private static int GetServer(int min = 1, int max = 4)
        {
            return RandomHelper.Get(min, max);
        }

        //Google
        private static string MakeGoogleRoadMapUrl(TileInfo tile, int server) => $@"https://mt{server}.google.com/vt?x={tile.ColumnNumber}&y={tile.RowNumber}&z={tile.ZoomLevel}";

        private static string MakeGoogleTerrainUrl(TileInfo tile, int server) => $@"http://mt{server}.google.com/vt/lyrs=t@131,r@176163100&hl=en&x={tile.ColumnNumber}&y={tile.RowNumber}&z={tile.ZoomLevel}";

        // http://mt1.google.com/vt/lyrs=s@901000000&hl=en&x=4&y=10&z=5&s=Ga
        // http://khm0.google.com/kh/v=748&s=&x=1354740&y=825228&z=21

        private static string MakeGoogleSatelliteUrl(TileInfo tile, int server) => $@"http://mt{server}.google.com/vt/lyrs=s@901000000&hl=en&x={tile.ColumnNumber}&y={tile.RowNumber}&z={tile.ZoomLevel}&s=Gal";

        private static string MakeGoogleHybridUrl(TileInfo tile, int server) => $@"http://mt{server}.google.com/vt/lyrs=y@901000000&hl=en&x={tile.ColumnNumber}&y={tile.RowNumber}&z={tile.ZoomLevel}&s=Gal";

        private static string GetGoogleUrl(TileType type, TileInfo tile)
        {
            switch (type)
            {
                case TileType.Satellite:
                    return MakeGoogleSatelliteUrl(tile, GetServer(0, 3));

                case TileType.RoadMap:
                    return MakeGoogleRoadMapUrl(tile, GetServer(0, 3));

                case TileType.Terrain:
                    return MakeGoogleTerrainUrl(tile, GetServer(0, 3));

                case TileType.Hybrid:
                    return MakeGoogleHybridUrl(tile, GetServer(0, 3));
                default:
                    return null;
            }
        }

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

        private static string GetNokiaUrl(TileType type, TileInfo tile)
        {
            switch (type)
            {
                case TileType.Satellite:
                    return MakeNokiaSatelliteUrl(tile, GetServer());

                case TileType.RoadMap:
                    return MakeNokiaRoadMapUrl(tile, GetServer());

                case TileType.Terrain:
                    return MakeNokiaTerrainUrl(tile, GetServer());

                case TileType.Hybrid:
                    return MakeNokiaHybridUrl(tile, GetServer());
                default:
                    return null;
            }
        }

        //Bing
        private static string MakeBingSatelliteUrl(TileInfo tile, int server) => $@"http://a{server}.ortho.tiles.virtualearth.net/tiles/a{TileXYToQuadKey(tile.ColumnNumber, tile.RowNumber, tile.ZoomLevel)}.jpeg?g=5925";

        private static string MakeBingHybridUrl(TileInfo tile, int server) => $@"http://h{server}.ortho.tiles.virtualearth.net/tiles/h{TileXYToQuadKey(tile.ColumnNumber, tile.RowNumber, tile.ZoomLevel)}.jpeg?g=5978";

        private static string MakeBingStreetUrl(TileInfo tile, int server) => $@"http://r{server}.ortho.tiles.virtualearth.net/tiles/r{TileXYToQuadKey(tile.ColumnNumber, tile.RowNumber, tile.ZoomLevel)}.jpeg?g=5978";
         

        //Bing
        private static string GetBingUrl(TileType type, TileInfo tile)
        {
            switch (type)
            {
                case TileType.Satellite:
                    return MakeBingSatelliteUrl(tile, GetServer(0, 3));
                    //return $"http://a0.ortho.tiles.virtualearth.net/tiles/a{TileXYToQuadKey(tile.ColumnNumber, tile.RowNumber, tile.ZoomLevel)}.jpeg?g=5925";

                case TileType.RoadMap:
                    return MakeBingStreetUrl(tile, GetServer(0, 3));

                case TileType.Terrain:
                case TileType.Hybrid:
                    return MakeBingHybridUrl(tile, GetServer(0, 3));

                case TileType.None:
                default:
                    return null;
            }
        }
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


        //
        public static string GetUrl(MapProviderType provider, TileType type, TileInfo tile)
        {
            switch (provider)
            {
                case MapProviderType.Google:
                    return GetGoogleUrl(type, tile);

                case MapProviderType.Nokia:
                    return GetNokiaUrl(type, tile);

                case MapProviderType.Bing:
                    return GetBingUrl(type, tile);

                case MapProviderType.Yahoo:
                case MapProviderType.OpenStreetMap:
                default:
                    return null;
            }
        }
    }
}
