using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Standards.OGC.SFA
{
    public static class WkbGeometryTypeFactory
    {
        public static Dictionary<Type, WkbGeometryType> WkbTypeMap;

        static WkbGeometryTypeFactory()
        {
            WkbTypeMap = new Dictionary<Type, WkbGeometryType>();

            //WkbTypeMap.Add(typeof(WkbGeometry<Point>), WkbGeometryType.Geometry);
            WkbTypeMap.Add(typeof(OgcPoint<Point>), WkbGeometryType.Point);
            WkbTypeMap.Add(typeof(OgcLineString<Point>), WkbGeometryType.LineString);
            WkbTypeMap.Add(typeof(OgcPolygon<Point>), WkbGeometryType.Polygon);
            WkbTypeMap.Add(typeof(OgcMultiPoint<Point>), WkbGeometryType.MultiPoint);
            WkbTypeMap.Add(typeof(OgcMultiLineString<Point>), WkbGeometryType.MultiLineString);
            WkbTypeMap.Add(typeof(OgcMultiPolygon<Point>), WkbGeometryType.MultiPolygon);
            //WkbTypeMap.Add(typeof(WkbGeometryCollection<Point>), WkbGeometryType.GeometryCollection);
            //WkbTypeMap.Add(typeof(WkbCircularString<Point>), WkbGeometryType.CircularString);
            //WkbTypeMap.Add(typeof(WkbCompoundCurve<Point>), WkbGeometryType.CompoundCurve);
            //WkbTypeMap.Add(typeof(WkbCurvePolygon<Point>), WkbGeometryType.CurvePolygon);
            //WkbTypeMap.Add(typeof(WkbMultiCurve<Point>), WkbGeometryType.MultiCurve);
            //WkbTypeMap.Add(typeof(WkbMultiSurface<Point>), WkbGeometryType.MultiSurface);
            //WkbTypeMap.Add(typeof(WkbCurve<Point>), WkbGeometryType.Curve);
            //WkbTypeMap.Add(typeof(WkbSurface<Point>), WkbGeometryType.Surface);
            //WkbTypeMap.Add(typeof(WkbPolyhedralSurface<Point>), WkbGeometryType.PolyhedralSurface);
            //WkbTypeMap.Add(typeof(WkbTIN<Point>), WkbGeometryType.TIN);

            //WkbTypeMap.Add(typeof(WkbGeometry<PointZ>), WkbGeometryType.GeometryZ);
            WkbTypeMap.Add(typeof(OgcPoint<PointZ>), WkbGeometryType.PointZ);
            WkbTypeMap.Add(typeof(OgcLineString<PointZ>), WkbGeometryType.LineStringZ);
            WkbTypeMap.Add(typeof(OgcPolygon<PointZ>), WkbGeometryType.PolygonZ);
            WkbTypeMap.Add(typeof(OgcMultiPoint<PointZ>), WkbGeometryType.MultiPointZ);
            WkbTypeMap.Add(typeof(OgcMultiLineString<PointZ>), WkbGeometryType.MultiLineStringZ);
            WkbTypeMap.Add(typeof(OgcMultiPolygon<PointZ>), WkbGeometryType.MultiPolygonZ);
            //WkbTypeMap.Add(typeof(WkbGeometryCollection<PointZ>), WkbGeometryType.GeometryCollectionZ);
            //WkbTypeMap.Add(typeof(WkbCircularString<PointZ>), WkbGeometryType.CircularStringZ);
            //WkbTypeMap.Add(typeof(WkbCompoundCurve<PointZ>), WkbGeometryType.CompoundCurveZ);
            //WkbTypeMap.Add(typeof(WkbCurvePolygon<PointZ>), WkbGeometryType.CurvePolygonZ);
            //WkbTypeMap.Add(typeof(WkbMultiCurve<PointZ>), WkbGeometryType.MultiCurveZ);
            //WkbTypeMap.Add(typeof(WkbMultiSurface<PointZ>), WkbGeometryType.MultiSurfaceZ);
            //WkbTypeMap.Add(typeof(WkbCurve<PointZ>), WkbGeometryType.CurveZ);
            //WkbTypeMap.Add(typeof(WkbSurface<PointZ>), WkbGeometryType.SurfaceZ);
            //WkbTypeMap.Add(typeof(WkbPolyhedralSurface<PointZ>), WkbGeometryType.PolyhedralSurfaceZ);
            //WkbTypeMap.Add(typeof(WkbTIN<PointZ>), WkbGeometryType.TINZ);

            //WkbTypeMap.Add(typeof(WkbGeometry<PointM>), WkbGeometryType.GeometryM);
            WkbTypeMap.Add(typeof(OgcPoint<PointM>), WkbGeometryType.PointM);
            WkbTypeMap.Add(typeof(OgcLineString<PointM>), WkbGeometryType.LineStringM);
            WkbTypeMap.Add(typeof(OgcPolygon<PointM>), WkbGeometryType.PolygonM);
            WkbTypeMap.Add(typeof(OgcMultiPoint<PointM>), WkbGeometryType.MultiPointM);
            WkbTypeMap.Add(typeof(OgcMultiLineString<PointM>), WkbGeometryType.MultiLineStringM);
            WkbTypeMap.Add(typeof(OgcMultiPolygon<PointM>), WkbGeometryType.MultiPolygonM);
            //WkbTypeMap.Add(typeof(WkbGeometryCollection<PointM>), WkbGeometryType.GeometryCollectionM);
            //WkbTypeMap.Add(typeof(WkbCircularString<PointM>), WkbGeometryType.CircularStringM);
            //WkbTypeMap.Add(typeof(WkbCompoundCurve<PointM>), WkbGeometryType.CompoundCurveM);
            //WkbTypeMap.Add(typeof(WkbCurvePolygon<PointM>), WkbGeometryType.CurvePolygonM);
            //WkbTypeMap.Add(typeof(WkbMultiCurve<PointM>), WkbGeometryType.MultiCurveM);
            //WkbTypeMap.Add(typeof(WkbMultiSurface<PointM>), WkbGeometryType.MultiSurfaceM);
            //WkbTypeMap.Add(typeof(WkbCurve<PointM>), WkbGeometryType.CurveM);
            //WkbTypeMap.Add(typeof(WkbSurface<PointM>), WkbGeometryType.SurfaceM);
            //WkbTypeMap.Add(typeof(WkbPolyhedralSurface<PointM>), WkbGeometryType.PolyhedralSurfaceM);
            //WkbTypeMap.Add(typeof(WkbTIN<PointM>), WkbGeometryType.TIN);

            //WkbTypeMap.Add(typeof(WkbGeometry<PointZM>), WkbGeometryType.GeometryZM);
            WkbTypeMap.Add(typeof(OgcPoint<PointZM>), WkbGeometryType.PointZM);
            WkbTypeMap.Add(typeof(OgcLineString<PointZM>), WkbGeometryType.LineStringZM);
            WkbTypeMap.Add(typeof(OgcPolygon<PointZM>), WkbGeometryType.PolygonZM);
            WkbTypeMap.Add(typeof(OgcMultiPoint<PointZM>), WkbGeometryType.MultiPointZM);
            WkbTypeMap.Add(typeof(OgcMultiLineString<PointZM>), WkbGeometryType.MultiLineStringZM);
            WkbTypeMap.Add(typeof(OgcMultiPolygon<PointZM>), WkbGeometryType.MultiPolygonZM);
            //WkbTypeMap.Add(typeof(WkbGeometryCollection<PointZM>), WkbGeometryType.GeometryCollectionZM);
            //WkbTypeMap.Add(typeof(WkbCircularString<PointZM>), WkbGeometryType.CircularStringZM);
            //WkbTypeMap.Add(typeof(WkbCompoundCurve<PointZM>), WkbGeometryType.CompoundCurveZM);
            //WkbTypeMap.Add(typeof(WkbCurvePolygon<PointZM>), WkbGeometryType.CurvePolygonZM);
            //WkbTypeMap.Add(typeof(WkbMultiCurve<PointZM>), WkbGeometryType.MultiCurveZM);
            //WkbTypeMap.Add(typeof(WkbMultiSurface<PointZM>), WkbGeometryType.MultiSurfaceZM);
            //WkbTypeMap.Add(typeof(WkbCurve<PointZM>), WkbGeometryType.CurveZM);
            //WkbTypeMap.Add(typeof(WkbSurface<PointZM>), WkbGeometryType.SurfaceZM);
            //WkbTypeMap.Add(typeof(WkbPolyhedralSurface<PointZM>), WkbGeometryType.PolyhedralSurfaceZM);
            //WkbTypeMap.Add(typeof(WkbTIN<PointZM>), WkbGeometryType.TINZM);

        }
    }
}