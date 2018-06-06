﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Types;
using System.Diagnostics;
using IRI.Msh.Common.Primitives;
using IRI.Msh.CoordinateSystem.MapProjection;

namespace IRI.Ket.SqlServerSpatialExtension
{
    public static class Utility
    {
        public static SqlGeometry MakeGeometry(List<Point> points, bool isClosed, int srid = 32639)
        {
            SqlGeometryBuilder builder = new SqlGeometryBuilder();

            builder.SetSrid(srid);

            builder.BeginGeometry(isClosed ? OpenGisGeometryType.Polygon : OpenGisGeometryType.LineString);

            builder.BeginFigure(points[0].X, points[0].Y);

            for (int i = 1; i < points.Count; i++)
            {
                builder.AddLine(points[i].X, points[i].Y);
            }

            if (isClosed)
            {
                builder.AddLine(points[0].X, points[0].Y);
            }

            builder.EndFigure();

            builder.EndGeometry();

            return builder.ConstructedGeometry.MakeValid();
        }

        public static SqlGeometry MakePointCollection(IEnumerable<IPoint> points)
        {
            var wkt = $"MULTIPOINT({string.Join(",", points.Select(i => $"({i.X} {i.Y})"))})";

            return SqlGeometry.Parse(new System.Data.SqlTypes.SqlString(wkt));
        }

        /// <summary>
        /// Makes Geography with default ellipsoid WGS84: 4326
        /// </summary>
        /// <param name="points"></param>
        /// <param name="isClosed"></param>
        /// <returns></returns>
        public static SqlGeography MakeGeography(List<Point> points, bool isClosed, int srid = SridHelper.GeodeticWGS84)
        {
            if (points == null || points.Count < 1)
            {
                return null;
            }

            SqlGeographyBuilder builder = new SqlGeographyBuilder();

            builder.SetSrid(srid);

            builder.BeginGeography(isClosed ? OpenGisGeographyType.Polygon : OpenGisGeographyType.LineString);

            builder.BeginFigure(points[0].Y, points[0].X);

            for (int i = 1; i < points.Count; i++)
            {
                builder.AddLine(points[i].Y, points[i].X);
            }

            if (isClosed)
            {
                builder.AddLine(points[0].Y, points[0].X);
            }

            builder.EndFigure();

            builder.EndGeography();

            var resultGeography = builder.ConstructedGeography.STIsValid().Value ? builder.ConstructedGeography : builder.ConstructedGeography.MakeValid();

            return resultGeography.EnvelopeAngle().Value == 180 ? resultGeography.ReorientObject() : resultGeography;
        }

        public static SqlGeometry Union(List<SqlGeometry> geometries)
        {
            return Aggregate(geometries, (g1, g2) => g1.STUnion(g2));
        }

        private static SqlGeometry Aggregate(List<SqlGeometry> geometries, Func<SqlGeometry, SqlGeometry, SqlGeometry> map)
        {
            if (geometries == null || geometries.Count < 1)
            {
                return null;
            }

            var result = geometries.First();

            for (int i = 1; i < geometries.Count; i++)
            {
                result = map(result, geometries[i]);
            }

            return result;
        }
    }
}
