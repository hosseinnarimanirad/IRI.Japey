
using IRI.Msh.Common.Helpers;
using IRI.Msh.Common.Model;
using IRI.Jab.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Jab.Common.TileServices
{
    public static class MapProviderFactory
    {
        public static string GoogleProvider = "GOOGLE";
        public static string BingProvider = "BING";
        public static string NokiaProvider = "NOKIA";

        public static int GetServer(int min = 1, int max = 4)
        {
            return RandomHelper.Get(min, max);
        }

        public static IMapProvider CreateKnownProvider(MapProviderType provider, TileType type)
        {
            switch (provider)
            {
                case MapProviderType.Google:
                    return new GoogleMapProvider(type);

                case MapProviderType.Bing:
                    return new BingMapProvider(type);

                case MapProviderType.Nokia:
                    return new NokiaMapProvider(type);

                case MapProviderType.Yahoo:
                case MapProviderType.OpenStreetMap:
                case MapProviderType.Custom:
                default:
                    return null;
            }
        }


        //Google

        //private static string GetGoogleUrl(TileType type, TileInfo tile)
        //{
        //    switch (type)
        //    {
        //        case TileType.Satellite:
        //            return MakeGoogleSatelliteUrl(tile, GetServer(0, 3));

        //        case TileType.RoadMap:
        //            return MakeGoogleRoadMapUrl(tile, GetServer(0, 3));

        //        case TileType.Terrain:
        //            return MakeGoogleTerrainUrl(tile, GetServer(0, 3));

        //        case TileType.Hybrid:
        //            return MakeGoogleHybridUrl(tile, GetServer(0, 3));
        //        default:
        //            return null;
        //    }
        //}


        //
        //public static string GetUrl(MapProviderType provider, TileType type, TileInfo tile)
        //{
        //    switch (provider)
        //    {
        //        case MapProviderType.Google:
        //            return GetGoogleUrl(type, tile);

        //        case MapProviderType.Nokia:
        //            return GetNokiaUrl(type, tile);

        //        case MapProviderType.Bing:
        //            return GetBingUrl(type, tile);

        //        case MapProviderType.Yahoo:
        //        case MapProviderType.OpenStreetMap:
        //        default:
        //            return null;
        //    }
        //}
    }
}
