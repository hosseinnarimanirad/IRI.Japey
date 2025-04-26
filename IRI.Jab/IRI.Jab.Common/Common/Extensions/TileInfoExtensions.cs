
using IRI.Jab.Common.Model;
using IRI.Sta.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Extensions
{
    public static class TileInfoExtensions
    {
        public static Tile Parse(this TileInfo tileInfo)
        {
            return new Tile(tileInfo.RowNumber, tileInfo.ColumnNumber, tileInfo.ZoomLevel);
        }
    }
}
