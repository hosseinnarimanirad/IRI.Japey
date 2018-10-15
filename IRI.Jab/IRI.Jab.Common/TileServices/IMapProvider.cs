using IRI.Msh.Common.Helpers;
using IRI.Msh.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Jab.Common.TileServices
{
    public abstract class IMapProvider
    {
        //public string Name { get; set; }

        public TileType TileType { get; set; }

        public string ProviderName { get; protected set; }

        public Func<TileInfo, string> MakeRoadMapUrl { get; protected set; }

        public Func<TileInfo, string> MakeTerrainUrl { get; protected set; }

        public Func<TileInfo, string> MakeSatelliteUrl { get; protected set; }

        public Func<TileInfo, string> MakeHybridUrl { get; protected set; }

        public virtual string GetUrl(TileInfo tile)
        {
            switch (TileType)
            {
                case TileType.Satellite:
                    return MakeSatelliteUrl(tile);

                case TileType.RoadMap:
                    return MakeRoadMapUrl(tile);

                case TileType.Terrain:
                    return MakeTerrainUrl(tile);

                case TileType.Hybrid:
                    return MakeHybridUrl(tile);

                default:
                    return null;
            }
        }
    }
}
