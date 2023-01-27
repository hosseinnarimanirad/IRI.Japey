//using IRI.Msh.Common.Model;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace IRI.Jab.Common.TileServices
//{
//    public class OsmMapProvider : IMapProvider
//    {
//        //https://c.tile.openstreetmap.org/12/2047/1878.png
//        //https://{a|b|c}.tile.opentopomap.org/{z}/{x}/{y}.png 
//        private static string MakeOpenStreetMapUrl(TileInfo tile, char serverChar) => $@"http://{serverChar}.tile.openstreetmap.org/{tile.ZoomLevel}/{tile.ColumnNumber}/{tile.RowNumber}.png";

//        private static string MakeOpenTopoMapUrl(TileInfo tile, char server) => $@"http://{server}.tile.opentopomap.org/{tile.ZoomLevel}/{tile.ColumnNumber}/{tile.RowNumber}.png";

//        public OsmMapProvider(TileType type)
//        {
//            this.TileType = type;

//            this.ProviderName = MapProviderFactory.OsmProvider;

//            MakeRoadMapUrl = tile => MakeOpenStreetMapUrl(tile, MapProviderFactory.GetServerCharacter(0,2));
//            MakeTerrainUrl = tile => MakeOpenTopoMapUrl(tile, MapProviderFactory.GetServerCharacter(0, 2));
//            MakeSatelliteUrl = tile => null;
//            MakeHybridUrl = tile => null;
//        }

//    }
//}
