using IRI.Sta.Spatial.Primitives;
using IRI.Sta.Common.Primitives;
using Microsoft.SqlServer.Types; 

namespace IRI.Extensions;

public static class SqlGeometryExtensions
{
    public static System.Windows.Point AsWpfPoint(this SqlGeometry pointGeometry)
    {
        return new System.Windows.Point(pointGeometry.STX.Value, pointGeometry.STY.Value);
    }

    public static System.Windows.Point AsWpfPoint(this Geometry<Point> pointGeometry)
    {
        return new System.Windows.Point(pointGeometry.Points[0].X, pointGeometry.Points[0].Y);
    }


}
