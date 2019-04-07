using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRI.Msh.Common.Model;

namespace IRI.Jab.Common.TileServices
{
    //public class GoogleMapProvider : IMapProvider
    //{
    //    // http://mt1.google.com/vt/lyrs=s@901000000&hl=en&x=4&y=10&z=5&s=Ga
    //    // http://khm0.google.com/kh/v=748&s=&x=1354740&y=825228&z=21

    //    private static string MakeGoogleRoadMapUrl(TileInfo tile, int server) => $@"https://mt{server}.google.com/vt?x={tile.ColumnNumber}&y={tile.RowNumber}&z={tile.ZoomLevel}";

    //    private static string MakeGoogleTerrainUrl(TileInfo tile, int server) => $@"http://mt{server}.google.com/vt/lyrs=t@131,r@176163100&hl=en&x={tile.ColumnNumber}&y={tile.RowNumber}&z={tile.ZoomLevel}";

    //    private static string MakeGoogleSatelliteUrl(TileInfo tile, int server) => $@"http://mt{server}.google.com/vt/lyrs=s@901000000&hl=en&x={tile.ColumnNumber}&y={tile.RowNumber}&z={tile.ZoomLevel}&s=Gal";

    //    private static string MakeGoogleHybridUrl(TileInfo tile, int server) => $@"http://mt{server}.google.com/vt/lyrs=y@901000000&hl=en&x={tile.ColumnNumber}&y={tile.RowNumber}&z={tile.ZoomLevel}&s=Gal";


    //    internal GoogleMapProvider(TileType type)
    //    {
    //        this.TileType = type;

    //        this.ProviderName = MapProviderFactory.GoogleProvider;

    //        MakeRoadMapUrl = tile => MakeGoogleRoadMapUrl(tile, MapProviderFactory.GetServer(0, 3));
    //        MakeTerrainUrl = tile => MakeGoogleTerrainUrl(tile, MapProviderFactory.GetServer(0, 3));
    //        MakeSatelliteUrl = tile => MakeGoogleSatelliteUrl(tile, MapProviderFactory.GetServer(0, 3));
    //        MakeHybridUrl = tile => MakeGoogleHybridUrl(tile, MapProviderFactory.GetServer(0, 3));
    //    }

    //    //private override string GetUrl(TileType type, TileInfo tile)
    //    //{
    //    //    switch (type)
    //    //    {
    //    //        case TileType.Satellite:
    //    //            return MakeGoogleSatelliteUrl(tile, CacheSourceFactory.GetServer(0, 3));

    //    //        case TileType.RoadMap:
    //    //            return MakeGoogleRoadMapUrl(tile, CacheSourceFactory.GetServer(0, 3));

    //    //        case TileType.Terrain:
    //    //            return MakeGoogleTerrainUrl(tile, CacheSourceFactory.GetServer(0, 3));

    //    //        case TileType.Hybrid:
    //    //            return MakeGoogleHybridUrl(tile, CacheSourceFactory.GetServer(0, 3));
    //    //        default:
    //    //            return null;
    //    //    }
    //    //}
    //    //public override string GetUrl(TileType type, TileInfo tile)
    //    //{
    //    //    switch (type)
    //    //    {
    //    //        case TileType.Satellite:
    //    //            return MakeSatelliteUrl(tile, CacheSourceFactory.GetServer(0, 3));

    //    //        case TileType.RoadMap:
    //    //            return MakeRoadMapUrl(tile, GetServer(0, 3));

    //    //        case TileType.Terrain:
    //    //            return MakeTerrainUrl(tile, GetServer(0, 3));

    //    //        case TileType.Hybrid:
    //    //            return MakeHybridUrl(tile, GetServer(0, 3));
    //    //        default:
    //    //            return null;
    //    //    }
    //    //}
    //}
}
