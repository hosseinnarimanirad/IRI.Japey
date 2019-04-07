//using IRI.Msh.Common.Model;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace IRI.Jab.Common.TileServices
//{
//    public class WazeMapProvider : IMapProvider
//    {
//        //https://worldtiles3.waze.com/tiles/11/1313/805.png
//        private static string MakeWazeRoadMapUrl(TileInfo tile, int server) => $@"https://mt{server}.google.com/vt?x={tile.ColumnNumber}&y={tile.RowNumber}&z={tile.ZoomLevel}";

//        public WazeMapProvider(TileType type)
//        {
//            this.TileType = type;

//            this.ProviderName = MapProviderFactory.WazeProvider;

//            MakeRoadMapUrl = tile => $@"https://worldtiles3.waze.com/tiles/{tile.ZoomLevel}/{tile.ColumnNumber}/{tile.RowNumber}.png";
//            //MakeTerrainUrl = tile => MakeGoogleTerrainUrl(tile, MapProviderFactory.GetServer(0, 3));
//            //MakeSatelliteUrl = tile => MakeGoogleSatelliteUrl(tile, MapProviderFactory.GetServer(0, 3));
//            //MakeHybridUrl = tile => MakeGoogleHybridUrl(tile, MapProviderFactory.GetServer(0, 3));
//        }

//    }
//}
