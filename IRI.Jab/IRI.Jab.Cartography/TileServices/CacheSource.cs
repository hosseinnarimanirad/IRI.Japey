
using IRI.Ham.Common.Helpers;
using IRI.Ham.SpatialBase.Model;
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
        public static string MakeGoogleRoadMapUrl(TileInfo tile, int server) => $@"https://mt{server}.google.com/vt?x={tile.ColumnNumber}&y={tile.RowNumber}&z={tile.ZoomLevel}";

        public static string MakeGoogleTerrainUrl(TileInfo tile, int server) => $@"http://mt{server}.google.com/vt/lyrs=t@131,r@176163100&hl=en&x={tile.ColumnNumber}&y={tile.RowNumber}&z={tile.ZoomLevel}";

        ///* http://mt1.google.com/vt/lyrs=s@901000000&hl=en&x=4&y=10&z=5&s=Ga

        public static string MakeGoogleSatelliteUrl(TileInfo tile, int server) => $@"http://mt{server}.google.com/vt/lyrs=s@901000000&hl=en&x={tile.ColumnNumber}&y={tile.RowNumber}&z={tile.ZoomLevel}&s=Gal";

        public static string MakeGoogleHybridUrl(TileInfo tile, int server) => $@"http://mt{server}.google.com/vt/lyrs=y@901000000&hl=en&x={tile.ColumnNumber}&y={tile.RowNumber}&z={tile.ZoomLevel}&s=Gal";

        public static string GetGoogleUrl(TileType type, TileInfo tile)
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
                    throw new NotImplementedException();
            }
        }

        //Nokia
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tile"></param>
        /// <param name="server">1, 2, 3 or 5</param>
        /// <returns></returns>
        public static string MakeNokiaRoadMapUrl(TileInfo tile, int server) => $@"http://{server}.maps.nlp.nokia.com/maptile/2.1/maptile/newest/normal.day/{tile.ZoomLevel}/{tile.ColumnNumber}/{tile.RowNumber}/256/png8?app_id=SqE1xcSngCd3m4a1zEGb&token=r0sR1DzqDkS6sDnh902FWQ&lg=ENG";

        public static string MakeNokiaTerrainUrl(TileInfo tile, int server) => $@"http://{server}.maps.nlp.nokia.com/maptile/2.1/maptile/newest/terrain.day/{tile.ZoomLevel}/{tile.ColumnNumber}/{tile.RowNumber}/256/png8?app_id=SqE1xcSngCd3m4a1zEGb&token=r0sR1DzqDkS6sDnh902FWQ&lg=ENG";

        public static string MakeNokiaSatelliteUrl(TileInfo tile, int server) => $@"http://{server}.maps.nlp.nokia.com/maptile/2.1/maptile/newest/satellite.day/{tile.ZoomLevel}/{tile.ColumnNumber}/{tile.RowNumber}/256/png8?app_id=SqE1xcSngCd3m4a1zEGb&token=r0sR1DzqDkS6sDnh902FWQ&lg=ENG";

        public static string MakeNokiaHybridUrl(TileInfo tile, int server) => $@"http://{server}.maps.nlp.nokia.com/maptile/2.1/maptile/newest/hybrid.day/{tile.ZoomLevel}/{tile.ColumnNumber}/{tile.RowNumber}/256/png8?app_id=SqE1xcSngCd3m4a1zEGb&token=r0sR1DzqDkS6sDnh902FWQ&lg=ENG";

        public static string GetNokiaUrl(TileType type, TileInfo tile)
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
                    throw new NotImplementedException();
            }
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
                case MapProviderType.Yahoo:
                case MapProviderType.OpenStreetMap:
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
