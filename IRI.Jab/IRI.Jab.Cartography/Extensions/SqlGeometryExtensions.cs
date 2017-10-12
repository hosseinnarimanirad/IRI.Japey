using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Jab.Cartography.Extensions
{
    public static class SqlGeometryExtensions
    {
        public static System.Windows.Point AsWpfPoint(this SqlGeometry pointGeometry)
        {
            return new System.Windows.Point(pointGeometry.STX.Value, pointGeometry.STY.Value);
        }
    }
}
