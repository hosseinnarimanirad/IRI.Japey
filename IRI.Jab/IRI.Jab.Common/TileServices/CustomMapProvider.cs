using IRI.Msh.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Jab.Common.TileServices
{
    public class CustomMapProvider : IMapProvider
    {
        //public Func<TileInfo, string> MakeRoadMapUrl { get; set; }

        //public Func<TileInfo, string> MakeTerrainUrl { get; set; }

        //public Func<TileInfo, string> MakeSatelliteUrl { get; set; }

        //public Func<TileInfo, string> MakeHybridUrl { get; set; }

        ////public string GetUrl(TileType type, TileInfo tile)
        //{
        //    switch (type)
        //    {
        //        case TileType.Satellite:
        //            return MakeSatelliteUrl(tile);

        //        case TileType.RoadMap:
        //            return MakeRoadMapUrl(tile);

        //        case TileType.Terrain:
        //            return MakeTerrainUrl(tile);

        //        case TileType.Hybrid:
        //            return MakeHybridUrl(tile);

        //        default:
        //            return null;
        //    }
        //}

        public CustomMapProvider(string providerName, Func<TileInfo, string> roadMapFunc, Func<TileInfo, string> satelliteFunc, Func<TileInfo, string> terrainFunc, Func<TileInfo, string> hybridFunc)
        {
            RequireInternetConnection = false;

            this.MakeRoadMapUrl = roadMapFunc;
            this.MakeTerrainUrl = terrainFunc;
            this.MakeSatelliteUrl = satelliteFunc;
            this.MakeHybridUrl = hybridFunc;

            this.ProviderName = providerName.ToUpper();

            //this.Name = name;
        }
    }
}
