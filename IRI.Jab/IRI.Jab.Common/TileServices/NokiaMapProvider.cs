using IRI.Msh.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Jab.Common.TileServices
{
    public class NokiaMapProvider : IMapProvider
    {
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

        public NokiaMapProvider(TileType type)
        {
            this.TileType = type;

            this.ProviderName = MapProviderFactory.NokiaProvider;

            MakeRoadMapUrl = tile => MakeNokiaRoadMapUrl(tile, MapProviderFactory.GetServer());
            MakeTerrainUrl = tile => MakeNokiaTerrainUrl(tile, MapProviderFactory.GetServer());
            MakeSatelliteUrl = tile => MakeNokiaSatelliteUrl(tile, MapProviderFactory.GetServer());
            MakeHybridUrl = tile => MakeNokiaHybridUrl(tile, MapProviderFactory.GetServer());
        }



    }
}
