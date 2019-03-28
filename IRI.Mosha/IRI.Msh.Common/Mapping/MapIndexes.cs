using IRI.Msh.Common.Primitives;
using IRI.Msh.CoordinateSystem;
using IRI.Msh.CoordinateSystem.MapProjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace IRI.Msh.Common.Mapping
{
    public static class MapIndexes
    {
        #region Constants

        internal const double _250kWidth = 1.5;

        internal const double _250kHeight = 1.0;

        //size: width and height (degree)
        internal const double _100kSize = 0.5;

        //size: width and height (degree)
        internal const double _50kSize = 0.25;

        //size: width and height (degree)
        internal const double _25kSize = 0.125;

        internal const double _10kWidth = 5.0 / 60.0;

        internal const double _10kHeight = 3.0 / 60.0;

        internal const double _5kWidth = 2.5 / 60.0;

        internal const double _5kHeight = 1.5 / 60.0;

        internal const double _2kUtmXmin = 164000;
        internal const double _2kUtmXmax = 836000;
        internal const double _2kUtmYmin = 2752800;
        internal const double _2kUtmYmax = 4432800;
        internal const double _2kUtmBlockWidth = 32000;
        internal const double _2kUtmBlockHeight = 24000;
        internal const double _2kUtmMapWidth = 1600;
        internal const double _2kUtmMapHeight = 1200;

        #endregion

        #region Get SheetNames (LAT/LONG > SHEET NAME)

        private static string Get100kSheetName(double minLongitude, double minLatitude)
        {
            return FormattableString.Invariant($"{minLongitude * 2 - 40}{minLatitude * 2 - 10}");
        }

        private static string Get50kSheetName(double minLongitude, double minLatitude)
        {
            var sheet100k = Get100kIndexSheet(minLongitude, minLatitude);

            var center100k = sheet100k.Extent.Center;

            if (minLongitude == sheet100k.Extent.XMin)
            {
                if (minLatitude == sheet100k.Extent.YMin)
                {
                    return FormattableString.Invariant($"{sheet100k.SheetName} III");
                }
                else if (minLatitude == center100k.Y)
                {
                    return FormattableString.Invariant($"{sheet100k.SheetName} IV");
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            else if (minLongitude == center100k.X)
            {
                if (minLatitude == sheet100k.Extent.YMin)
                {
                    return FormattableString.Invariant($"{sheet100k.SheetName} II");
                }
                else if (minLatitude == center100k.Y)
                {
                    return FormattableString.Invariant($"{sheet100k.SheetName} I");
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private static string Get25kSheetName(double minLongitude, double minLatitude)
        {
            var sheet50k = Get50kIndexSheet(minLongitude, minLatitude);

            var center50k = sheet50k.Extent.Center;

            if (minLongitude == sheet50k.Extent.XMin)
            {
                if (minLatitude == sheet50k.Extent.YMin)
                {
                    return FormattableString.Invariant($"{sheet50k.SheetName} SW");
                }
                else if (minLatitude == center50k.Y)
                {
                    return FormattableString.Invariant($"{sheet50k.SheetName} NW");
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            else if (minLongitude == center50k.X)
            {
                if (minLatitude == sheet50k.Extent.YMin)
                {
                    return FormattableString.Invariant($"{sheet50k.SheetName} SE");
                }
                else if (minLatitude == center50k.Y)
                {
                    return FormattableString.Invariant($"{sheet50k.SheetName} NE");
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private static string Get10kSheetName(double minLongitude, double minLatitude)
        {
            var sheet50k = Get50kIndexSheet(minLongitude, minLatitude);

            //  K   L   M
            //1
            //2
            //3
            //4
            //5
            //************

            //var center50k = sheet50k.Extent.Center;
            var xKmin = sheet50k.Extent.XMin;
            var xLmin = sheet50k.Extent.XMin + _10kWidth;
            var xMmin = sheet50k.Extent.XMax - _10kWidth;

            //var yMin5 = sheet50k.Extent.YMin;
            //var yMin4 = sheet50k.Extent.YMin + 1 * _10kHeight;
            //var yMin3 = sheet50k.Extent.YMin + 2 * _10kHeight;
            //var yMin2 = sheet50k.Extent.YMin + 3 * _10kHeight;
            //var yMin1 = sheet50k.Extent.YMin + 4 * _10kHeight;

            //row is between 1 and 5
            var row = 5 - Math.Round((minLongitude - sheet50k.Extent.YMin) / _10kHeight);

            var column = xKmin == minLatitude ? "K" : (xLmin == minLatitude) ? "L" : (xMmin == minLatitude) ? "M" : throw new Exception();

            return FormattableString.Invariant($"{sheet50k.SheetName} {column}{row}");
        }

        private static string Get5kSheetName(double minLongitude, double minLatitude)
        {
            var sheet25k = Get25kIndexSheet(minLongitude, minLatitude);

            //  A   B   C
            //1
            //2
            //3
            //4
            //5
            //************

            //var center50k = sheet50k.Extent.Center;
            var xAmin = sheet25k.Extent.XMin;
            var xBmin = sheet25k.Extent.XMin + _10kWidth;
            var xCmin = sheet25k.Extent.XMax - _10kWidth;

            //var yMin5 = sheet50k.Extent.YMin;
            //var yMin4 = sheet50k.Extent.YMin + 1 * _10kHeight;
            //var yMin3 = sheet50k.Extent.YMin + 2 * _10kHeight;
            //var yMin2 = sheet50k.Extent.YMin + 3 * _10kHeight;
            //var yMin1 = sheet50k.Extent.YMin + 4 * _10kHeight;

            //row is between 1 and 5
            var row = 5 - Math.Round((minLongitude - sheet25k.Extent.YMin) / _5kHeight);

            var column = xAmin == minLatitude ? "A" : (xBmin == minLatitude) ? "B" : (xCmin == minLatitude) ? "C" : throw new Exception();

            return FormattableString.Invariant($"{sheet25k.SheetName} {column}{row}");
        }

        #endregion


        #region Get IndexSheet by name or lat/long (LAT/LONG OR NAME > IndexSheet)

        public static IndexSheet Get100kIndexSheet(string ncc100kSheetName)
        {
            if (!string.IsNullOrWhiteSpace(ncc100kSheetName) && ncc100kSheetName.Length == 4)
            {
                double firstPart, secondPart;

                if (double.TryParse(ncc100kSheetName.Substring(0, 2), out firstPart))
                {
                    if (double.TryParse(ncc100kSheetName.Substring(2, 2), out secondPart))
                    {
                        var xMin = (firstPart + 40) / 2.0;

                        var yMin = (secondPart + 10) / 2.0;

                        return new IndexSheet(new BoundingBox(xMin, yMin, xMin + _100kSize, yMin + _100kSize), NccIndexType.Ncc100k)
                        {
                            SheetName = ncc100kSheetName,
                        };

                    }
                }
            }

            return null;
        }

        public static IndexSheet Get100kIndexSheet(double longitude, double latitude)
        {
            var minLongitude = Math.Floor(longitude / _100kSize) * _100kSize;

            var minLatitude = Math.Floor(latitude / _100kSize) * _100kSize;

            return new IndexSheet(new BoundingBox(minLongitude, minLatitude, minLongitude + _100kSize, minLatitude + _100kSize), NccIndexType.Ncc100k)
            {
                SheetName = Get100kSheetName(minLongitude, minLatitude)
            };
        }

        public static IndexSheet Get50kIndexSheet(double longitude, double latitude)
        {
            var minLongitude = Math.Floor(longitude / _50kSize) * _50kSize;

            var minLatitude = Math.Floor(latitude / _50kSize) * _50kSize;

            return new IndexSheet(new BoundingBox(minLongitude, minLatitude, minLongitude + _50kSize, minLatitude + _50kSize), NccIndexType.Ncc50k)
            {
                SheetName = Get50kSheetName(minLongitude, minLatitude)
            };
        }

        public static IndexSheet Get25kIndexSheet(double longitude, double latitude)
        {
            var minLongitude = Math.Floor(longitude / _25kSize) * _25kSize;

            var minLatitude = Math.Floor(latitude / _25kSize) * _25kSize;

            return new IndexSheet(new BoundingBox(minLongitude, minLatitude, minLongitude + _25kSize, minLatitude + _25kSize), NccIndexType.Ncc25k)
            {
                SheetName = Get25kSheetName(minLongitude, minLatitude)
            };
        }

        #endregion


        #region Get IndexSheets in a Region (BoundingBox > List<IndexSheet>)

        public static List<IndexSheet> FindNcc100kIndexes(BoundingBox geographicIntersectRegion)
        {
            return FindIndexes(geographicIntersectRegion, _100kSize, _100kSize, (minLongitude, minLatitude) => Get100kSheetName(minLongitude, minLatitude), NccIndexType.Ncc100k);
        }

        public static List<IndexSheet> FindNcc50kIndexes(BoundingBox geographicIntersectRegion)
        {
            return FindIndexes(geographicIntersectRegion, _50kSize, _50kSize, (minLongitude, minLatitude) => Get50kSheetName(minLongitude, minLatitude), NccIndexType.Ncc50k);
        }

        public static List<IndexSheet> FindNcc25kIndexes(BoundingBox geographicIntersectRegion)
        {
            return FindIndexes(geographicIntersectRegion, _25kSize, _25kSize, (minLongitude, minLatitude) => Get25kSheetName(minLongitude, minLatitude), NccIndexType.Ncc25k);
        }

        public static List<IndexSheet> FindNcc10kIndexes(BoundingBox geographicIntersectRegion)
        {
            return FindIndexes(geographicIntersectRegion, _10kWidth, _10kHeight, (minLongitude, minLatitude) => Get10kSheetName(minLongitude, minLatitude), NccIndexType.Ncc10k);
        }

        public static List<IndexSheet> FindNcc5kIndexes(BoundingBox geographicIntersectRegion)
        {
            return FindIndexes(geographicIntersectRegion, _5kWidth, _5kHeight, (minLongitude, minLatitude) => Get5kSheetName(minLongitude, minLatitude), NccIndexType.Ncc5k);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="geographicIntersectRegion"></param>
        /// <param name="indexWidth"></param>
        /// <param name="indexHeight"></param>
        /// <param name="namingFunc">Based on Lower Left Coordinate</param>
        /// <returns></returns>
        public static List<IndexSheet> FindIndexes(BoundingBox geographicIntersectRegion, double indexWidth, double indexHeight, Func<double, double, string> namingFunc, NccIndexType type)
        {
            var startLongitude = Math.Floor(geographicIntersectRegion.XMin / indexWidth) * indexWidth;

            var endLongitude = Math.Ceiling(geographicIntersectRegion.XMax / indexWidth) * indexWidth;

            var startLatitdue = Math.Floor(geographicIntersectRegion.YMin / indexHeight) * indexHeight;

            var endLatitdue = Math.Ceiling(geographicIntersectRegion.YMax / indexHeight) * indexHeight;

            List<IndexSheet> result = new List<IndexSheet>();

            for (double i = startLongitude; i < endLongitude; i += indexWidth)
            {
                for (double j = startLatitdue; j < endLatitdue; j += indexHeight)
                {
                    var sheetName = namingFunc(i, j);

                    result.Add(new IndexSheet(new BoundingBox(i, j, i + indexWidth, j + indexHeight), type)
                    {
                        SheetName = sheetName
                    });
                }
            }

            return result;
        }

        #endregion


        #region Get Index BoundingBoxes in a Region (BoundingBox > List<BoundingBox>)

        public static List<BoundingBox> Find250kIndexes(BoundingBox geographicIntersectRegion)
        {
            return FindIndexes(geographicIntersectRegion, _250kWidth, _250kHeight);
            //var startLongitude = (int)Math.Floor(geographicIntersectRegion.XMin / _nioc250kWidth);

            //var endLongitude = (int)Math.Ceiling(geographicIntersectRegion.XMax / _nioc250kWidth);

            //var startLatitdue = (int)Math.Floor(geographicIntersectRegion.YMin / _nioc250kHeight);

            //var endLatitdue = (int)Math.Floor(geographicIntersectRegion.YMax / _nioc250kHeight);

            //List<BoundingBox> result = new List<BoundingBox>();

            //for (int i = startLongitude; i < endLongitude; i++)
            //{
            //    for (int j = startLatitdue; j < endLatitdue; j++)
            //    {
            //        result.Add(new BoundingBox(i, j, i + _nioc250kWidth, j + _nioc250kHeight));
            //    }
            //}

            //return result;
        }

        public static List<BoundingBox> Find100kIndexes(BoundingBox geographicIntersectRegion)
        {
            return FindIndexes(geographicIntersectRegion, _100kSize, _100kSize);
        }

        public static List<BoundingBox> Find50kIndexes(BoundingBox geographicIntersectRegion)
        {
            return FindIndexes(geographicIntersectRegion, _50kSize, _50kSize);
        }

        public static List<BoundingBox> Find25kIndexes(BoundingBox geographicIntersectRegion)
        {
            return FindIndexes(geographicIntersectRegion, _25kSize, _25kSize);
        }

        public static List<BoundingBox> Find10kIndex(BoundingBox geographicIntersectRegion)
        {
            return FindIndexes(geographicIntersectRegion, _10kWidth, _10kHeight);
        }

        public static List<BoundingBox> Find5kIndex(BoundingBox geographicIntersectRegion)
        {
            return FindIndexes(geographicIntersectRegion, _5kWidth, _5kHeight);
        }

        public static List<BoundingBox> FindIndexes(BoundingBox geographicIntersectRegion, double indexWidth, double indexHeight)
        {
            var startLongitude = Math.Floor(geographicIntersectRegion.XMin / indexWidth) * indexWidth;

            var endLongitude = Math.Ceiling(geographicIntersectRegion.XMax / indexWidth) * indexWidth;

            var startLatitdue = Math.Floor(geographicIntersectRegion.YMin / indexHeight) * indexHeight;

            var endLatitdue = Math.Ceiling(geographicIntersectRegion.YMax / indexHeight) * indexHeight;

            List<BoundingBox> result = new List<BoundingBox>();

            for (double i = startLongitude; i < endLongitude; i += indexWidth)
            {
                for (double j = startLatitdue; j < endLatitdue; j += indexHeight)
                {
                    result.Add(new BoundingBox(i, j, i + indexWidth, j + indexHeight));
                }
            }

            return result;
        }

        #endregion


        #region Get Index Lines (BoundingBox > List<Geometry>)

        public static List<Geometry> GetIndexLines(BoundingBox geographicIntersectRegion, NccIndexType type, int utmZone = 0)
        {
            switch (type)
            {
                case NccIndexType.Ncc250k:
                    return GetIndexLines(geographicIntersectRegion, _250kWidth, _250kHeight);

                case NccIndexType.Ncc100k:
                    return GetIndexLines(geographicIntersectRegion, _100kSize, _100kSize);

                case NccIndexType.Ncc50k:
                    return GetIndexLines(geographicIntersectRegion, _50kSize, _50kSize);

                case NccIndexType.Ncc25k:
                    return GetIndexLines(geographicIntersectRegion, _25kSize, _25kSize);

                case NccIndexType.Ncc10k:
                    return GetIndexLines(geographicIntersectRegion, _10kWidth, _10kHeight);

                case NccIndexType.Ncc5k:
                    return GetIndexLines(geographicIntersectRegion, _5kWidth, _5kHeight);

                case NccIndexType.Ncc2k:
                    return Get2kIndexBlockLines(geographicIntersectRegion, utmZone);

                default:
                    throw new NotImplementedException();
            }
        }

        //private static List<Geometry> Get250kIndexLines(BoundingBox geographicIntersectRegion)
        //{
        //    return GetIndexLines(geographicIntersectRegion, _250kWidth, _250kHeight);
        //}

        //private static List<Geometry> Get100kIndexLines(BoundingBox geographicIntersectRegion)
        //{
        //    return GetIndexLines(geographicIntersectRegion, _100kSize, _100kSize);
        //}

        //private static List<Geometry> Get50kIndexLines(BoundingBox geographicIntersectRegion)
        //{
        //    return GetIndexLines(geographicIntersectRegion, _50kSize, _50kSize);
        //}

        //private static List<Geometry> Get25kIndexLines(BoundingBox geographicIntersectRegion)
        //{
        //    return GetIndexLines(geographicIntersectRegion, _25kSize, _25kSize);
        //}

        //private static List<Geometry> Get10kIndexLines(BoundingBox geographicIntersectRegion)
        //{
        //    return GetIndexLines(geographicIntersectRegion, _10kWidth, _10kHeight);
        //}

        //private static List<Geometry> Get5kIndexLines(BoundingBox geographicIntersectRegion)
        //{
        //    return GetIndexLines(geographicIntersectRegion, _5kWidth, _5kHeight);
        //}

        public static List<Geometry> GetIndexLines(BoundingBox geographicIntersectRegion, double indexWidth, double indexHeight)
        {
            var startLongitude = Math.Floor(geographicIntersectRegion.XMin / indexWidth) * indexWidth;

            var endLongitude = Math.Ceiling(geographicIntersectRegion.XMax / indexWidth) * indexWidth;

            var startLatitdue = Math.Floor(geographicIntersectRegion.YMin / indexHeight) * indexHeight;

            var endLatitdue = Math.Ceiling(geographicIntersectRegion.YMax / indexHeight) * indexHeight;

            List<Geometry> result = new List<Geometry>();

            for (double i = startLongitude; i < endLongitude; i += indexWidth)
            {
                var p1 = new Point(i, geographicIntersectRegion.YMin);

                var p2 = new Point(i, geographicIntersectRegion.YMax);

                result.Add(new Geometry(new Point[] { p1, p2 }, GeometryType.LineString, SridHelper.GeodeticWGS84));
            }

            for (double j = startLatitdue; j < endLatitdue; j += indexHeight)
            {
                var p1 = new Point(geographicIntersectRegion.XMin, j);

                var p2 = new Point(geographicIntersectRegion.XMax, j);

                result.Add(new Geometry(new Point[] { p1, p2 }, GeometryType.LineString, SridHelper.GeodeticWGS84));
            }

            return result;
        }


        public static List<Geometry> Get2kIndexBlockLines(BoundingBox geographicIntersectRegion, int utmZone)
        {
            //return GetUtmBasedIndexLines(geographicIntersectRegion, _2kUtmMapWidth, _2kUtmMapHeight);
            return GetUtmBasedIndexLines(geographicIntersectRegion, _2kUtmBlockWidth, _2kUtmBlockHeight, utmZone);



            //var utmBound = geographicIntersectRegion.Transform(p => MapProjects.GeodeticToUTM(p, Ellipsoids.WGS84, 39, true));

            //utmBound = new BoundingBox(
            //    xMin: Math.Max(utmBound.XMin, _2kUtmXmin),
            //    yMin: Math.Max(utmBound.YMin, _2kUtmYmin),
            //    xMax: Math.Min(utmBound.XMax, _2kUtmXmax),
            //    yMax: Math.Min(utmBound.YMax, _2kUtmYmax));

            //var startX = _2kUtmXmin + Math.Floor((utmBound.XMin - _2kUtmXmin) / _2kUtmBlockWidth) * _2kUtmBlockWidth;

            //var endX = _2kUtmXmin + Math.Ceiling((utmBound.XMax - _2kUtmXmin) / _2kUtmBlockWidth) * _2kUtmBlockWidth;

            //var startY = _2kUtmYmin + Math.Floor((utmBound.YMin - _2kUtmYmin) / _2kUtmBlockHeight) * _2kUtmBlockHeight;

            //var endY = _2kUtmYmin + Math.Ceiling((utmBound.YMax - _2kUtmYmin) / _2kUtmBlockHeight) * _2kUtmBlockHeight;

            //List<Geometry> result = new List<Geometry>();

            //for (double i = startX; i < endX; i += _2kUtmBlockWidth)
            //{
            //    var p1 = new Point(i, utmBound.YMin);

            //    var p2 = new Point(i, utmBound.YMax);

            //    result.Add(new Geometry(new Point[] { p1, p2 }, GeometryType.LineString, SridHelper.GeodeticWGS84));
            //}

            //for (double j = startY; j < endY; j += _2kUtmBlockHeight)
            //{
            //    var p1 = new Point(utmBound.XMin, j);

            //    var p2 = new Point(utmBound.XMax, j);

            //    result.Add(new Geometry(new Point[] { p1, p2 }, GeometryType.LineString, SridHelper.GeodeticWGS84));
            //}

            //return result.Select(g => g.Transform(p => MapProjects.UTMToGeodetic(p, 39))).ToList();
        }

        public static List<Geometry> GetUtmBasedIndexLines(BoundingBox geographicIntersectRegion, double utmWidth, double utmHeight, int utmZone)
        {
            ////this approach start the grid at upper Y of the actual region
            //var utmBound = geographicIntersectRegion.Transform(p => MapProjects.GeodeticToUTM(p, Ellipsoids.WGS84, 39, true));

            //utmBound = new BoundingBox(
            //    xMin: Math.Max(utmBound.XMin, _2kUtmXmin),
            //    yMin: Math.Max(utmBound.YMin, _2kUtmYmin),
            //    xMax: Math.Min(utmBound.XMax, _2kUtmXmax),
            //    yMax: Math.Min(utmBound.YMax, _2kUtmYmax));


            //var meanX = (_2kUtmXmin + _2kUtmXmax) / 2.0;
            //var meanY = (_2kUtmYmin + _2kUtmYmax) / 2.0;

            //var minLatitude1 = MapProjects.UTMToGeodetic(new Point(_2kUtmXmin, _2kUtmYmin), Ellipsoids.WGS84, utmZone).Y; //min
            ////var minLatitude2 = MapProjects.UTMToGeodetic(new Point(meanX, _2kUtmYmin), Ellipsoids.WGS84, utmZone).Y;
            ////var minLatitude3 = MapProjects.UTMToGeodetic(new Point(_2kUtmXmax, _2kUtmYmin), Ellipsoids.WGS84, utmZone).Y;

            //var maxLatitude1 = MapProjects.UTMToGeodetic(new Point(_2kUtmXmin, _2kUtmYmax), Ellipsoids.WGS84, utmZone).Y;
            ////var maxLatitude2 = MapProjects.UTMToGeodetic(new Point(meanX, _2kUtmYmax), Ellipsoids.WGS84, utmZone).Y;       //max
            ////var maxLatitude3 = MapProjects.UTMToGeodetic(new Point(_2kUtmXmax, _2kUtmYmax), Ellipsoids.WGS84, utmZone).Y;

            //var minLongitude1 = MapProjects.UTMToGeodetic(new Point(_2kUtmXmin, _2kUtmYmin), Ellipsoids.WGS84, utmZone).X;
            ////var minLongitude2 = MapProjects.UTMToGeodetic(new Point(_2kUtmXmin, meanY), Ellipsoids.WGS84, utmZone).X;
            ////var minLongitude3 = MapProjects.UTMToGeodetic(new Point(_2kUtmXmin, _2kUtmYmax), Ellipsoids.WGS84, utmZone).X;  //min

            //var maxLongitude1 = MapProjects.UTMToGeodetic(new Point(_2kUtmXmax, _2kUtmYmin), Ellipsoids.WGS84, utmZone).X; //max
            ////var maxLongitude2 = MapProjects.UTMToGeodetic(new Point(_2kUtmXmax, meanY), Ellipsoids.WGS84, utmZone).X;
            ////var maxLongitude3 = MapProjects.UTMToGeodetic(new Point(_2kUtmXmax, _2kUtmYmax), Ellipsoids.WGS84, utmZone).X;



            ////var geoXMin = Math.Max(Math.Min(minLongitude1, minLongitude3), geographicIntersectRegion.XMin);
            ////var geoXMax = Math.Min(Math.Max(maxLongitude1, maxLongitude3), geographicIntersectRegion.XMax);
            ////var geoMeanX = (geoXMin + geoXMax) / 2.0;

            ////var geoYMin = Math.Max(Math.Min(minLatitude1, minLatitude2), geographicIntersectRegion.YMin);
            ////var geoYMax = Math.Min(Math.Max(maxLatitude1, maxLatitude2), geographicIntersectRegion.YMax);

            //var geoXMin = Math.Max(minLongitude1, geographicIntersectRegion.XMin);
            //var geoXMax = Math.Min(maxLongitude1, geographicIntersectRegion.XMax);

            //var geoYMin = Math.Max(minLatitude1, geographicIntersectRegion.YMin);
            //var geoYMax = Math.Min(maxLatitude1, geographicIntersectRegion.YMax);


            //var xMin = MapProjects.GeodeticToUTM(new Point(geoXMin, geoYMin), Ellipsoids.WGS84, utmZone).X;
            //var xMax = MapProjects.GeodeticToUTM(new Point(geoXMax, geoYMin), Ellipsoids.WGS84, utmZone).X;

            //var yMin = MapProjects.GeodeticToUTM(new Point(geoXMin, geoYMin), Ellipsoids.WGS84, utmZone).Y;
            //var yMax = MapProjects.GeodeticToUTM(new Point(geoXMin, geoYMax), Ellipsoids.WGS84, utmZone).Y;


            //to get the actual ymin/ymax consider middle of lower/uper edge of rectangle
            //var yMin = MapProjects.GeodeticToUTM(new Point(geographicIntersectRegion.Center.X, geoYMin), Ellipsoids.WGS84, utmZone).Y;
            //var yMax = MapProjects.GeodeticToUTM(new Point(geographicIntersectRegion.Center.X, geoYMax), Ellipsoids.WGS84, utmZone).Y;


            //var utmBound = new BoundingBox(
            //    xMin: Math.Max(xMin, _2kUtmXmin),
            //    yMin: Math.Max(yMin, _2kUtmYmin),
            //    xMax: Math.Min(xMax, _2kUtmXmax),
            //    yMax: Math.Min(yMax, _2kUtmYmax));


            var utmBound = new BoundingBox(_2kUtmXmin, _2kUtmYmin, _2kUtmXmax, _2kUtmYmax)
                                .Transform(p => MapProjects.UTMToGeodetic(p, utmZone))
                                .Intersect(geographicIntersectRegion)
                                .Transform(p => MapProjects.GeodeticToUTM(p, Ellipsoids.WGS84, utmZone));


            List<Geometry> result = new List<Geometry>();

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

                result.Add(new Geometry(new Point[] { p1, p2 }, GeometryType.LineString, 0));
            }

            for (double j = startY; j <= endY; j += utmHeight)
            {
                var p1 = new Point(startX, j);

                var p2 = new Point(middleX, j);

                var p3 = new Point(endX, j);

                result.Add(new Geometry(new Point[] { p1, p2, p3 }, GeometryType.LineString, 0));
            }

            return result.Select(g => g.Transform(p => MapProjects.UTMToGeodetic(p, utmZone), SridHelper.GeodeticWGS84)).ToList();
        }

        #endregion



        public static List<Index50k> Generate50kIndexes(BoundingBox geographicIntersectRegion)
        {
            return GenerateIndexes<Index50k>(geographicIntersectRegion, _50kSize, _50kSize, (lng, lat) => new Index50k()
            {
                MinLatitude = lat,
                MinLongitude = lng,
                SheetNameEn = Get50kSheetName(lng, lat)
            });
        }

        public static List<Index25k> Generate25kIndexes(BoundingBox geographicIntersectRegion)
        {
            return GenerateIndexes<Index25k>(geographicIntersectRegion, _25kSize, _25kSize, (lng, lat) => new Index25k()
            {
                MinLatitude = lat,
                MinLongitude = lng,
                SheetNameEn = Get50kSheetName(lng, lat)
            });
        }

        public static List<Index10k> Generate10kIndexes(BoundingBox geographicIntersectRegion)
        {
            return GenerateIndexes<Index10k>(geographicIntersectRegion, _10kWidth, _10kHeight, (lng, lat) => new Index10k()
            {
                MinLatitude = lat,
                MinLongitude = lng,
                SheetNameEn = Get10kSheetName(lng, lat)
            });
        }

        public static List<Index5k> Generate5kIndexes(BoundingBox geographicIntersectRegion)
        {
            return GenerateIndexes<Index5k>(geographicIntersectRegion, _5kWidth, _5kHeight, (lng, lat) => new Index5k()
            {
                MinLatitude = lat,
                MinLongitude = lng,
                SheetNameEn = Get5kSheetName(lng, lat)
            });
        }


        private static List<T> GenerateIndexes<T>(BoundingBox geographicIntersectRegion, double indexWidth, double indexHeight, Func<double, double, T> generateFunc)
        {
            var startLongitude = Math.Floor(geographicIntersectRegion.XMin / indexWidth) * indexWidth;

            var endLongitude = Math.Ceiling(geographicIntersectRegion.XMax / indexWidth) * indexWidth;

            var startLatitdue = Math.Floor(geographicIntersectRegion.YMin / indexHeight) * indexHeight;

            var endLatitdue = Math.Ceiling(geographicIntersectRegion.YMax / indexHeight) * indexHeight;

            List<T> result = new List<T>();

            for (double i = startLongitude; i < endLongitude; i += indexWidth)
            {
                for (double j = startLatitdue; j < endLatitdue; j += indexHeight)
                {
                    result.Add(generateFunc(i, j));
                }
            }

            return result;
        }
    }
}
