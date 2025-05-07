using IRI.Jab.Common.Model;
using IRI.Sta.Spatial.Model;

namespace IRI.Extensions;

public static class TileInfoExtensions
{
    public static Tile Parse(this TileInfo tileInfo)
    {
        return new Tile(tileInfo.RowNumber, tileInfo.ColumnNumber, tileInfo.ZoomLevel);
    }
}
