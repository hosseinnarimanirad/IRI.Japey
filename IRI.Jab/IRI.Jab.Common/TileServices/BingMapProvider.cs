using IRI.Msh.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Jab.Common.TileServices
{
    public class BingMapProvider : IMapProvider
    {
        //return $"http://a0.ortho.tiles.virtualearth.net/tiles/a{TileXYToQuadKey(tile.ColumnNumber, tile.RowNumber, tile.ZoomLevel)}.jpeg?g=5925";
        private static string MakeBingSatelliteUrl(TileInfo tile, int server) => $@"http://a{server}.ortho.tiles.virtualearth.net/tiles/a{TileXYToQuadKey(tile.ColumnNumber, tile.RowNumber, tile.ZoomLevel)}.jpeg?g=5925";

        private static string MakeBingHybridUrl(TileInfo tile, int server) => $@"http://h{server}.ortho.tiles.virtualearth.net/tiles/h{TileXYToQuadKey(tile.ColumnNumber, tile.RowNumber, tile.ZoomLevel)}.jpeg?g=5978";

        private static string MakeBingStreetUrl(TileInfo tile, int server) => $@"http://r{server}.ortho.tiles.virtualearth.net/tiles/r{TileXYToQuadKey(tile.ColumnNumber, tile.RowNumber, tile.ZoomLevel)}.jpeg?g=5978";

        public BingMapProvider(TileType type)
        {
            this.TileType = type;

            this.ProviderName = MapProviderFactory.BingProvider;

            MakeRoadMapUrl = tile => MakeBingStreetUrl(tile, MapProviderFactory.GetServer(0, 3));
            MakeTerrainUrl = tile => null;
            MakeSatelliteUrl = tile => MakeBingSatelliteUrl(tile, MapProviderFactory.GetServer(0, 3));
            MakeHybridUrl = tile => MakeBingHybridUrl(tile, MapProviderFactory.GetServer(0, 3));
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
    }
}
