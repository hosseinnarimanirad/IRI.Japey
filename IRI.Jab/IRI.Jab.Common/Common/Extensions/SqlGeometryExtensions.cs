using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Extensions
{
    public static class SqlGeometryExtensions
    {
        public static System.Windows.Point AsWpfPoint(this SqlGeometry pointGeometry)
        {
            return new System.Windows.Point(pointGeometry.STX.Value, pointGeometry.STY.Value);
        }

        public static System.Windows.Point AsWpfPoint(this IRI.Sta.Common.Primitives.Geometry<Sta.Common.Primitives.Point> pointGeometry)
        {
            return new System.Windows.Point(pointGeometry.Points[0].X, pointGeometry.Points[0].Y);
        }


    }
}
