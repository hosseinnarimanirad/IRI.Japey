﻿using IRI.Extensions;
using IRI.Maptor.Sta.Common.Contracts.Google;
using IRI.Maptor.Sta.Spatial.Primitives;
using IRI.Maptor.Sta.SpatialReferenceSystem.MapProjections;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geometry = IRI.Maptor.Sta.Spatial.Primitives.Geometry<IRI.Maptor.Sta.Common.Primitives.Point>;
using IRI.Maptor.Sta.Common.Primitives;

namespace IRI.Maptor.Tst.NetFrameworkTest.Assets
{
    public static class GeometrySamples
    {
        public static Geometry EmptyPoint = Geometry.CreateEmpty(GeometryType.Point, 0);
        public static Geometry EmptyMultipoint = Geometry.CreateEmpty(GeometryType.MultiPoint, 0);
        public static Geometry EmptyLinestring = Geometry.CreateEmpty(GeometryType.LineString, 0);
        public static Geometry EmptyMultiLinestring = Geometry.CreateEmpty(GeometryType.MultiLineString, 0);
        public static Geometry EmptyPolygon = Geometry.CreateEmpty(GeometryType.Polygon, 0);
        public static Geometry EmptyMultiPolygon = Geometry.CreateEmpty(GeometryType.MultiPolygon, 0);

        public static Geometry Point = SqlGeometrySamples.Point.AsGeometry();

        public static Geometry PointZ = SqlGeometrySamples.PointZ.AsGeometry();

        public static Geometry PointZM = SqlGeometrySamples.PointZM.AsGeometry();

        public static Geometry MultipointComplex = SqlGeometrySamples.MultipointComplex.AsGeometry();

        public static Geometry Multipoint = SqlGeometrySamples.Multipoint.AsGeometry();


        public static Geometry Linestring = SqlGeometrySamples.Linestring.AsGeometry();

        public static Geometry LinestringZM = SqlGeometrySamples.LinestringZM.AsGeometry();

        public static Geometry MultiLineString = SqlGeometrySamples.MultiLineString.AsGeometry();

        public static Geometry Polygon = SqlGeometrySamples.Polygon.AsGeometry();

        public static Geometry PolygonWithHole = SqlGeometrySamples.PolygonWithHole.AsGeometry();

        public static Geometry PolygonWithTwoHole = SqlGeometrySamples.PolygonWithTwoHole.AsGeometry();

        public static Geometry MultiPolygon01 = SqlGeometrySamples.MultiPolygon01.AsGeometry();

        public static Geometry MultiPolygon02 = SqlGeometrySamples.MultiPolygon02.AsGeometry();

        public static List<Geometry> AllGeometries = new List<Geometry>()
        {
            EmptyLinestring, EmptyMultiLinestring, EmptyMultipoint, EmptyMultiPolygon, EmptyPoint, EmptyPolygon,
            Point, PointZ, PointZM,
            MultipointComplex, Multipoint,
            Linestring, LinestringZM,
            MultiLineString,
            Polygon, PolygonWithHole, PolygonWithTwoHole,
            MultiPolygon01, MultiPolygon02
        };
    }
}
