﻿using IRI.Msh.Common.Primitives;
using IRI.Msh.CoordinateSystem.MapProjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Msh.Common.Mapping
{
    public static class UtmIndexes
    {
        #region Constants

        internal const double _2kUtmXmin = 164000;
        internal const double _2kUtmXmax = 836000;
        internal const double _2kUtmYmin = 2752800;
        internal const double _2kUtmYmax = 4432800;

        internal static readonly BoundingBox _2kUtmBoudingBox;

        internal const double _2kUtmBlockWidth = 32000;
        internal const double _2kUtmBlockHeight = 24000;
        internal const double _2kUtmSheetWidth = 1600;
        internal const double _2kUtmSheetHeight = 1200;
        internal const double _1kUtmSheetWidth = 800;
        internal const double _1kUtmSheetHeight = 600;
        internal const double _500UtmSheetWidth = 400;
        internal const double _500UtmSheetHeight = 300;

        #endregion


        static UtmIndexes()
        {
            _2kUtmBoudingBox = new BoundingBox(_2kUtmXmin, _2kUtmYmin, _2kUtmXmax, _2kUtmYmax);
        }

        //private static string Get2kBlockSheetName(double minUtmX, double maxUtmY)
        //{
        //    //  A   B   C   ...     T   U
        //    //1
        //    //2
        //    //.
        //    //.
        //    //70
        //    //****************************
        //    var column = ((int)Math.Round((minUtmX - _2kUtmBoudingBox.XMin) / _2kUtmBlockWidth)).Number2String(true);

        //    var row = Math.Round((_2kUtmBoudingBox.YMax - maxUtmY) / _2kUtmBlockHeight);

        //    return $"{column}{row.ToString("00")}";
        //}

        public static UtmSheet GetIndexSheet(double longitude, double latitude, int utmZone, UtmIndexType type)
        {
            switch (type)
            {
                case UtmIndexType.Ncc2kBlock:
                    return UtmSheet.Create2kUtmBlock(longitude, latitude, utmZone);

                case UtmIndexType.Ncc2kSheet:
                    return UtmSheet.Create2kUtmSheet(longitude, latitude, utmZone);

                case UtmIndexType.Ncc1k:
                    return UtmSheet.Create1kUtmSheet(longitude, latitude, utmZone);

                case UtmIndexType.Ncc500:
                    return UtmSheet.Create500UtmSheet(longitude, latitude, utmZone);

                default:
                    throw new NotImplementedException();
            }
        }

        public static List<UtmSheet> GetIndexSheets(BoundingBox geographicIntersectRegion, UtmIndexType type, int utmZone)
        {
            switch (type)
            {
                case UtmIndexType.Ncc2kBlock:
                    return GetIndexSheets(geographicIntersectRegion, _2kUtmBlockWidth, _2kUtmBlockHeight, UtmIndexType.Ncc2kBlock, utmZone);

                case UtmIndexType.Ncc2kSheet:
                    return GetIndexSheets(geographicIntersectRegion, _2kUtmSheetWidth, _2kUtmSheetHeight, UtmIndexType.Ncc2kSheet, utmZone);

                case UtmIndexType.Ncc1k:
                    return GetIndexSheets(geographicIntersectRegion, _1kUtmSheetWidth, _1kUtmSheetHeight, UtmIndexType.Ncc1k, utmZone);

                case UtmIndexType.Ncc500:
                    return GetIndexSheets(geographicIntersectRegion, _500UtmSheetWidth, _500UtmSheetHeight, UtmIndexType.Ncc500, utmZone);

                default:
                    throw new NotImplementedException();
            }


        }

        private static List<UtmSheet> GetIndexSheets(BoundingBox geographicIntersectRegion, double utmWidth, double utmHeight, UtmIndexType type, int utmZone)
        {
            var geoBound = BoundingBoxHelper.UtmMbbToGeodeticWgs84Mbb(_2kUtmBoudingBox, utmZone)
                             .Intersect(geographicIntersectRegion);

            List<UtmSheet> result = new List<UtmSheet>();

            if (geoBound.IsNaN())
            {
                return result;
            }

            var utmBound = BoundingBoxHelper.GeodeticWgs84MbbToUtmMbb(geoBound, utmZone)
                            .Intersect(_2kUtmBoudingBox);

            if (utmBound.IsNaN())
            {
                return result;
            }

            int iStart = (int)Math.Floor((utmBound.XMin - _2kUtmXmin) / utmWidth);

            int iEnd = (int)Math.Ceiling((utmBound.XMax - _2kUtmXmin) / utmWidth);

            int jStart = (int)Math.Floor((utmBound.YMin - _2kUtmYmin) / utmHeight);

            int jEnd = (int)Math.Ceiling((utmBound.YMax - _2kUtmYmin) / utmHeight);

            for (int i = iStart; i < iEnd; i++)
            {
                for (int j = jStart; j < jEnd; j++)
                {
                    var startX = _2kUtmXmin + i * utmWidth;

                    var startY = _2kUtmYmin + j * utmHeight;

                    result.Add(UtmSheet.Create(new BoundingBox(startX, startY, startX + utmWidth, startY + utmHeight), type, utmZone));
                }
            }

            return result;
        }

        //for 2k, 1k, 1:500 scales
        public static List<Geometry<Point>> GetIndexLines(BoundingBox geographicIntersectRegion, UtmIndexType type, int utmZone)
        {
            switch (type)
            {
                case UtmIndexType.Ncc2kBlock:
                    return GetIndexLines(geographicIntersectRegion, _2kUtmBlockWidth, _2kUtmBlockHeight, utmZone);

                case UtmIndexType.Ncc2kSheet:
                    return GetIndexLines(geographicIntersectRegion, _2kUtmSheetWidth, _2kUtmSheetHeight, utmZone);

                case UtmIndexType.Ncc1k:
                    return GetIndexLines(geographicIntersectRegion, _1kUtmSheetWidth, _1kUtmSheetHeight, utmZone);

                case UtmIndexType.Ncc500:
                    return GetIndexLines(geographicIntersectRegion, _500UtmSheetWidth, _500UtmSheetHeight, utmZone);

                default:
                    throw new NotImplementedException();
            }
        }

        private static List<Geometry<Point>> GetIndexLines(BoundingBox geographicIntersectRegion, double utmWidth, double utmHeight, int utmZone)
        {
            ////this approach start the grid at upper Y of the actual region
            //var utmBound = geographicIntersectRegion.Transform(p => MapProjects.GeodeticToUTM(p, Ellipsoids.WGS84, 39, true));

            //utmBound = new BoundingBox(
            //    xMin: Math.Max(utmBound.XMin, _2kUtmXmin),
            //    yMin: Math.Max(utmBound.YMin, _2kUtmYmin),
            //    xMax: Math.Min(utmBound.XMax, _2kUtmXmax),
            //    yMax: Math.Min(utmBound.YMax, _2kUtmYmax));

            //var utmBound = new BoundingBox(_2kUtmXmin, _2kUtmYmin, _2kUtmXmax, _2kUtmYmax)
            //                    .Transform(p => MapProjects.UTMToGeodetic(p, utmZone))
            //                    .Intersect(geographicIntersectRegion)
            //                    .Transform(p => MapProjects.GeodeticToUTM(p, Ellipsoids.WGS84, utmZone));

            List<Geometry<Point>> result = new List<Geometry<Point>>();

            var geoBound = BoundingBoxHelper.UtmMbbToGeodeticWgs84Mbb(_2kUtmBoudingBox, utmZone)
                              .Intersect(geographicIntersectRegion);

            if (geoBound.IsNaN())
            {
                return result;
            }

            var utmBound = BoundingBoxHelper.GeodeticWgs84MbbToUtmMbb(geoBound, utmZone)
                            .Intersect(_2kUtmBoudingBox);

            if (utmBound.IsNaN())
            {
                return result;
            }

            var startX = _2kUtmXmin + Math.Floor((utmBound.XMin - _2kUtmXmin) / utmWidth) * utmWidth;

            var endX = _2kUtmXmin + Math.Ceiling((utmBound.XMax - _2kUtmXmin) / utmWidth) * utmWidth;

            var middleX = (startX + endX) / 2.0;

            var startY = _2kUtmYmin + Math.Floor((utmBound.YMin - _2kUtmYmin) / utmHeight) * utmHeight;

            var endY = _2kUtmYmin + Math.Ceiling((utmBound.YMax - _2kUtmYmin) / utmHeight) * utmHeight;

            for (double i = startX; i <= endX; i += utmWidth)
            {
                var p1 = new Point(i, startY);

                var p2 = new Point(i, endY);

                result.Add(new Geometry<Point>(new List<Point>() { p1, p2 }, GeometryType.LineString, 0));
            }

            for (double j = startY; j <= endY; j += utmHeight)
            {
                var p1 = new Point(startX, j);

                var p2 = new Point(middleX, j);

                var p3 = new Point(endX, j);

                result.Add(new Geometry<Point>(new List<Point>() { p1, p2, p3 }, GeometryType.LineString, 0));
            }

            return result.Select(g => g.Transform(p => MapProjects.UTMToGeodetic(p, utmZone), SridHelper.GeodeticWGS84)).ToList();
        }


    }
}
