using IRI.Maptor.Jab.Common.Model;
using IRI.Maptor.Sta.Spatial.Model;

namespace IRI.Extensions;

public static class TileInfoExtensions
{
    public static Tile Parse(this TileInfo tileInfo)
    {
        return new Tile(tileInfo.RowNumber, tileInfo.ColumnNumber, tileInfo.ZoomLevel);
    }
}
