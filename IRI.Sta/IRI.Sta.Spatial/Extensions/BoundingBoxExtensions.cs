using IRI.Sta.Spatial.Primitives;
using IRI.Sta.CoordinateSystems;
using IRI.Sta.CoordinateSystems.MapProjection;
using IRI.Sta.Common.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Extensions;

public static class BoundingBoxExtensions
{
    public static Geometry<Point> GeodeticWgs84MbbToUtmGeometry(this BoundingBox boundingBox, int? zone = null)
    {
        zone = zone ?? MapProjects.FindUtmZone(boundingBox.Center.X);
        //var zone = CoordinateSystem.MapProjection.MapProjects.FindZone(boundingBox.Center.X);

        var isNorthHemisphere = boundingBox.Center.Y > 0;

        return boundingBox.TransofrmBy8Point(p => MapProjects.GeodeticToUTM(p, Ellipsoids.WGS84, zone.Value, isNorthHemisphere));
    }

    public static BoundingBox GeodeticWgs84MbbToUtmMbb(this BoundingBox boundingBox, int? zone = null)
    {
        return GeodeticWgs84MbbToUtmGeometry(boundingBox, zone).GetBoundingBox();
    }

    public static Geometry<Point> UtmMbbToGeodeticWgs84Geometry(this BoundingBox boundingBox, int zone)
    {
        return boundingBox.TransofrmBy8Point(p => MapProjects.UTMToGeodetic(p, zone));
    }

    public static BoundingBox UtmMbbToGeodeticWgs84Mbb(BoundingBox boundingBox, int zone)
    {
        return UtmMbbToGeodeticWgs84Geometry(boundingBox, zone).GetBoundingBox();
    }


    public static Geometry<Point> TransofrmBy4Point(this BoundingBox boundingBox, Func<Point, Point> func)
    {
        var p1 = func(boundingBox.BottomLeft);

        var p3 = func(boundingBox.TopLeft);

        var p5 = func(boundingBox.TopRight);

        var p7 = func(boundingBox.BottomRight);

        return Geometry<Point>.Create(new List<Point>() { p1, p3, p5, p7 }, GeometryType.Polygon, 0);
    }

    public static Geometry<Point> TransofrmBy8Point(this BoundingBox boundingBox, Func<Point, Point> func)
    {
        var p1 = func(boundingBox.BottomLeft);

        var p2 = func(boundingBox.MiddleLeft);

        var p3 = func(boundingBox.TopLeft);

        var p4 = func(boundingBox.MiddleTop);

        var p5 = func(boundingBox.TopRight);

        var p6 = func(boundingBox.MiddleRight);

        var p7 = func(boundingBox.BottomRight);

        var p8 = func(boundingBox.MiddleBottom);

        return Geometry<Point>.Create(new List<Point>() { p1, p2, p3, p4, p5, p6, p7, p8 }, GeometryType.Polygon, 0);
    }


    public static Geometry<T> AsGeometry<T>(this BoundingBox boundingBox, int srid) where T : IPoint, new()
    {
        return Geometry<T>.Create(boundingBox.GetClockWiseOrderOfEsriPoints<T>()/*.ToArray()*/, GeometryType.Polygon, srid);
    }
}
