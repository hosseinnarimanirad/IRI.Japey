
//using IRI.Msh.Common.Helpers;
//using IRI.Msh.Common.Model;
//using IRI.Jab.Common;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace IRI.Jab.Common.TileServices
//{
//    public static class MapProviderFactory
//    {
//        public static string GoogleProvider = "GOOGLE";
//        public static string BingProvider = "BING";
//        public static string NokiaProvider = "NOKIA";
//        public static string OsmProvider = "OPENSTREETMAP";
//        public static string WazeProvider = "WAZE";

//        public static readonly char[] _serverChar;

//        static MapProviderFactory()
//        {
//            _serverChar = new char[] { 'a', 'b', 'c', 'd' };
//        }

//        public static int GetServer(int min = 1, int max = 4)
//        {
//            return RandomHelper.Get(min, max);
//        }

//        public static char GetServerCharacter(int min = 1, int max = 4)
//        {
//            var random = RandomHelper.Get(min, max);

//            return _serverChar[random];
//        }

//        public static IMapProvider CreateKnownProvider(MapProviderType provider, TileType type)
//        {
//            switch (provider)
//            {
//                case MapProviderType.Google:
//                    return new GoogleMapProvider(type);

//                case MapProviderType.Bing:
//                    return new BingMapProvider(type);

//                case MapProviderType.Nokia:
//                    return new NokiaMapProvider(type);

//                case MapProviderType.OpenStreetMap:
//                    return new OsmMapProvider(type);

//                case MapProviderType.Waze:
//                    return new WazeMapProvider(type);

//                case MapProviderType.Yahoo:
//                case MapProviderType.Custom:
//                default:
//                    return null;
//            }
//        }

         
//    }
//}
