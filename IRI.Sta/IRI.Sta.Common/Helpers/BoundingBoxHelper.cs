using IRI.Msh.CoordinateSystem;
using IRI.Msh.CoordinateSystem.MapProjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Sta.Common.Primitives;

public static class BoundingBoxHelper
{
    public static Geometry<Point> GeodeticWgs84MbbToUtmGeometry(BoundingBox boundingBox, int? zone = null)
    {
        zone = zone ?? MapProjects.FindZone(boundingBox.Center.X);
        //var zone = CoordinateSystem.MapProjection.MapProjects.FindZone(boundingBox.Center.X);

        var isNorthHemisphere = boundingBox.Center.Y > 0;

        return boundingBox.TransofrmBy8Point(p => MapProjects.GeodeticToUTM(p, Ellipsoids.WGS84, zone.Value, isNorthHemisphere));
    }

    public static BoundingBox GeodeticWgs84MbbToUtmMbb(BoundingBox boundingBox, int? zone = null)
    {
        return GeodeticWgs84MbbToUtmGeometry(boundingBox, zone).GetBoundingBox();
    }

    public static Geometry<Point> UtmMbbToGeodeticWgs84Geometry(BoundingBox boundingBox, int zone)
    {
        return boundingBox.TransofrmBy8Point(p => MapProjects.UTMToGeodetic(p, zone));
    }

    public static BoundingBox UtmMbbToGeodeticWgs84Mbb(BoundingBox boundingBox, int zone)
    {
        return UtmMbbToGeodeticWgs84Geometry(boundingBox, zone).GetBoundingBox();
    }



}
