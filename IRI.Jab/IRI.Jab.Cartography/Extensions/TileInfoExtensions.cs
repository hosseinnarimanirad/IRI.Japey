
using IRI.Ham.SpatialBase.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Jab.Cartography.Extensions
{
    public static class TileInfoExtensions
    {
        public static Model.Tile Parse(this TileInfo tileInfo)
        {
            return new Model.Tile(tileInfo.RowNumber, tileInfo.ColumnNumber, tileInfo.ZoomLevel);
        }
    }
}
