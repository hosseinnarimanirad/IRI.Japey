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

        static MapIndexes()
        {
            _2kUtmBoudingBox = new BoundingBox(_2kUtmXmin, _2kUtmYmin, _2kUtmXmax, _2kUtmYmax);
        }


        #region Get SheetNames (LAT/LONG > SHEET NAME)

        private static string Get100kSheetName(double minLongitude, double minLatitude)
        {
            return FormattableString.Invariant($"{minLongitude * 2 - 40}{minLatitude * 2 - 10}");
        }

        private static string Get50kSheetName(double minLongitude, double minLatitude)
        {
            var sheet100k = GetIndexSheet(minLongitude, minLatitude, NccIndexType.Ncc100k);

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
            var sheet50k = GetIndexSheet(minLongitude, minLatitude, NccIndexType.Ncc50k);

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
            var sheet50k = GetIndexSheet(minLongitude, minLatitude, NccIndexType.Ncc50k);

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
            var sheet25k = GetIndexSheet(minLongitude, minLatitude, NccIndexType.Ncc25k);

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

        public static IndexSheet GetIndexSheet(double longitude, double latitude, NccIndexType type)
        {
            switch (type)
            {
                case NccIndexType.Ncc100k:
                    return GetIndexSheet(longitude, latitude, type, _100kSize, _100kSize, Get100kSheetName);

                case NccIndexType.Ncc50k:
                    return GetIndexSheet(longitude, latitude, type, _50kSize, _50kSize, Get50kSheetName);

                case NccIndexType.Ncc25k:
                    return GetIndexSheet(longitude, latitude, type, _25kSize, _25kSize, Get25kSheetName);

                case NccIndexType.Ncc10k:
                    return GetIndexSheet(longitude, latitude, type, _10kWidth, _10kHeight, Get10kSheetName);

                case NccIndexType.Ncc5k:
                    return GetIndexSheet(longitude, latitude, type, _5kWidth, _5kHeight, Get5kSheetName);

                case NccIndexType.Ncc250k:
                case NccIndexType.NccUtmBased2kBlock:
                case NccIndexType.NccUtmBased2kSheet:
                case NccIndexType.NccUtmBased1k:
                case NccIndexType.NccUtmBased500:
                default:
                    throw new NotImplementedException();
            }
        }

        private static IndexSheet GetIndexSheet(double longitude, double latitude, NccIndexType type, double width, double height, Func<double, double, string> namingFunc)
        {
            var minLongitude = Math.Floor(longitude / width) * width;

            var minLatitude = Math.Floor(latitude / height) * height;

            return new IndexSheet(new BoundingBox(minLongitude, minLatitude, minLongitude + width, minLatitude + height), type)
            {
                SheetName = namingFunc(minLongitude, minLatitude)
            };
        }

        //public static IndexSheet Get100kIndexSheet(double longitude, double latitude)
        //{
        //    var minLongitude = Math.Floor(longitude / _100kSize) * _100kSize;

        //    var minLatitude = Math.Floor(latitude / _100kSize) * _100kSize;

        //    return new IndexSheet(new BoundingBox(minLongitude, minLatitude, minLongitude + _100kSize, minLatitude + _100kSize), NccIndexType.Ncc100k)
        //    {
        //        SheetName = Get100kSheetName(minLongitude, minLatitude)
        //    };
        //}

        //public static IndexSheet Get50kIndexSheet(double longitude, double latitude)
        //{
        //    var minLongitude = Math.Floor(longitude / _50kSize) * _50kSize;

        //    var minLatitude = Math.Floor(latitude / _50kSize) * _50kSize;

        //    return new IndexSheet(new BoundingBox(minLongitude, minLatitude, minLongitude + _50kSize, minLatitude + _50kSize), NccIndexType.Ncc50k)
        //    {
        //        SheetName = Get50kSheetName(minLongitude, minLatitude)
        //    };
        //}

        //public static IndexSheet Get25kIndexSheet(double longitude, double latitude)
        //{
        //    var minLongitude = Math.Floor(longitude / _25kSize) * _25kSize;

        //    var minLatitude = Math.Floor(latitude / _25kSize) * _25kSize;

        //    return new IndexSheet(new BoundingBox(minLongitude, minLatitude, minLongitude + _25kSize, minLatitude + _25kSize), NccIndexType.Ncc25k)
        //    {
        //        SheetName = Get25kSheetName(minLongitude, minLatitude)
        //    };
        //}

        //public static IndexSheet Get10kIndexSheet(double longitude, double latitude)
        //{
        //    var minLongitude = Math.Floor(longitude / _10kWidth) * _10kWidth;

        //    var minLatitude = Math.Floor(latitude / _10kHeight) * _10kHeight;

        //    return new IndexSheet(new BoundingBox(minLongitude, minLatitude, minLongitude + _10kWidth, minLatitude + _10kHeight), NccIndexType.Ncc10k)
        //    {
        //        SheetName = Get10kSheetName(minLongitude, minLatitude)
        //    };
        //}

        //public static IndexSheet Get5kIndexSheet(double longitude, double latitude)
        //{
        //    var minLongitude = Math.Floor(longitude / _5kWidth) * _5kWidth;

        //    var minLatitude = Math.Floor(latitude / _5kHeight) * _5kHeight;

        //    return new IndexSheet(new BoundingBox(minLongitude, minLatitude, minLongitude + _5kWidth, minLatitude + _5kHeight), NccIndexType.Ncc5k)
        //    {
        //        SheetName = Get5kSheetName(minLongitude, minLatitude)
        //    };
        //}

        //2k, 1k, 500
        public static IndexSheet GetUtmIndexSheet(double longitude, double latitude, int utmZone, NccIndexType type)
        {
            switch (type)
            {
                case NccIndexType.NccUtmBased2kBlock:
                    var utm2kBlock = UtmSheet.Create2kUtmBlock(longitude, latitude, utmZone);
                    return new IndexSheet(utm2kBlock.GeodeticExtent, NccIndexType.NccUtmBased2kBlock) { SheetName = utm2kBlock.Title };

                case NccIndexType.NccUtmBased2kSheet:
                    var utm2kSheet = UtmSheet.Create2kUtmSheet(longitude, latitude, utmZone);
                    return new IndexSheet(utm2kSheet.GeodeticExtent, NccIndexType.NccUtmBased2kBlock) { SheetName = utm2kSheet.Title };

                case NccIndexType.NccUtmBased1k:
                    var utm1kSheet = UtmSheet.Create1kUtmSheet(longitude, latitude, utmZone);
                    return new IndexSheet(utm1kSheet.GeodeticExtent, NccIndexType.NccUtmBased2kBlock) { SheetName = utm1kSheet.Title };

                case NccIndexType.NccUtmBased500:
                    var utm500Sheet = UtmSheet.Create500UtmSheet(longitude, latitude, utmZone);
                    return new IndexSheet(utm500Sheet.GeodeticExtent, NccIndexType.NccUtmBased2kBlock) { SheetName = utm500Sheet.Title };

                case NccIndexType.Ncc250k:
                case NccIndexType.Ncc100k:
                case NccIndexType.Ncc50k:
                case NccIndexType.Ncc25k:
                case NccIndexType.Ncc10k:
                case NccIndexType.Ncc5k:
                default:
                    throw new NotImplementedException();
            }
        }

        //public static IndexSheet Get2kIndexBlock(double longitude, double latitude, int utmZone)
        //{
        //    var utmSheet = UtmSheet.Create2kUtmBlock(longitude, latitude, utmZone);

        //    return new IndexSheet(utmSheet.GeodeticExtent, NccIndexType.NccUtmBased2kBlock) { SheetName = utmSheet.Title };
        //}

        //public static UtmSheet Get2kUtmBlock(double longitude, double latitude, int utmZone)
        //{
        //    return UtmSheet.Create2kUtmBlock(longitude, latitude, utmzone);
        //    //var utmPoint = MapProjects.GeodeticToUTM(new Point(longitude, latitude), Ellipsoids.WGS84, utmZone);

        //    //if (!_2kUtmBoudingBox.Intersects(utmPoint))
        //    //{
        //    //    return null;
        //    //}

        //    //var column = (int)Math.Floor((utmPoint.X - _2kUtmXmin) / _2kUtmBlockWidth);

        //    //var row = (int)Math.Floor((_2kUtmYmax - utmPoint.Y) / _2kUtmBlockHeight);

        //    //return UtmSheet.Create2kBlock(row, column, utmZone);

        //    //return new IndexSheet(geoBound, NccIndexType.NccUtmBased2kBlock) { SheetName = $"{utmZone.ToString("00")}{column}{row.ToString("00")}" };
        //}

        //public static UtmSheet Get2kUtmSheet(double longitude, double latitude, int utmZone)
        //{
        //    var utmPoint = MapProjects.GeodeticToUTM(new Point(longitude, latitude), Ellipsoids.WGS84, utmZone);

        //    if (!_2kUtmBoudingBox.Intersects(utmPoint))
        //    {
        //        return null;
        //    }

        //    var blockColumnRow = CalculateRowColumn(_2kUtmBoudingBox, utmPoint, _2kUtmBlockWidth, _2kUtmBlockHeight);

        //    var blockBound = CalculateUtm2kBlockBoundingBox((int)blockColumnRow.Y, (int)blockColumnRow.X);

        //    var sheetColumnRow = CalculateRowColumn(blockBound, utmPoint, _2kUtmSheetWidth, _2kUtmSheetHeight);

        //    var geoBound = CalculateUtm2kBlockBoundingBox((int)sheetColumnRow.Y, (int)sheetColumnRow.X)
        //                    .Transform(p => MapProjects.UTMToGeodetic(p, utmZone));

        //    return new UtmSheet(geoBound, NccIndexType.NccUtmBased2kSheet)
        //    {
        //        SheetName = $"{utmZone.ToString("00")}{_2kUtmBlockColumns[(int)blockColumnRow.X]}{blockColumnRow.Y.ToString("00")}{_2kUtmSheetColumns[(int)sheetColumnRow.X]}{sheetColumnRow.Y.ToString("00")}"
        //    };
        //}


        //private static BoundingBox CalculateUtm2kBlockBoundingBox(int row, int column)
        //{
        //    return new BoundingBox(_2kUtmXmin + column * _2kUtmBlockWidth,
        //                                _2kUtmYmax - (row + 1) * _2kUtmBlockHeight,
        //                                _2kUtmXmin + (column + 1) * _2kUtmBlockWidth,
        //                                _2kUtmYmax - row * _2kUtmBlockHeight);
        //}

        //private static Point CalculateRowColumn(BoundingBox utmBoundingBox, Point utmPoint, double columnWidth, double rowHeight)
        //{
        //    if (!utmBoundingBox.Intersects(utmPoint))
        //    {
        //        return null;
        //    }

        //    var column = Math.Floor((utmPoint.X - utmBoundingBox.XMin) / columnWidth);

        //    var row = Math.Floor((utmBoundingBox.YMax - utmPoint.Y) / rowHeight);

        //    return new Point(column, row);
        //}

        ////for 1:1000 and 1:500 sheets
        //private static int? CalculateMapNumber(BoundingBox utmSheetBoundingBox, Point utmPoint)
        //{
        //    if (!utmSheetBoundingBox.Intersects(utmPoint))
        //        return null;

        //    var center = utmSheetBoundingBox.Center;

        //    if (utmPoint.X >= center.X)
        //    {
        //        if (utmPoint.Y >= center.Y)
        //        {
        //            return 1;
        //        }
        //        else
        //        {
        //            return 2;
        //        }
        //    }
        //    else
        //    {
        //        if (utmPoint.Y >= center.Y)
        //        {
        //            return 4;
        //        }
        //        else
        //        {
        //            return 3;
        //        }
        //    }
        //}

        #endregion


        #region Get IndexSheets in a Region (BoundingBox > List<IndexSheet>)

        //no function to calculate 250k sheet names
        public static List<IndexSheet> Find250kIndexSheets(BoundingBox geographicIntersectRegion)
        {
            return FindIndexes(geographicIntersectRegion, _250kWidth, _250kHeight, (minLongitude, minLatitude) => string.Empty, NccIndexType.Ncc250k);
        }

        public static List<IndexSheet> Find100kIndexSheets(BoundingBox geographicIntersectRegion)
        {
            return FindIndexes(geographicIntersectRegion, _100kSize, _100kSize, (minLongitude, minLatitude) => Get100kSheetName(minLongitude, minLatitude), NccIndexType.Ncc100k);
        }

        public static List<IndexSheet> Find50kIndexSheets(BoundingBox geographicIntersectRegion)
        {
            return FindIndexes(geographicIntersectRegion, _50kSize, _50kSize, (minLongitude, minLatitude) => Get50kSheetName(minLongitude, minLatitude), NccIndexType.Ncc50k);
        }

        public static List<IndexSheet> Find25kIndexSheets(BoundingBox geographicIntersectRegion)
        {
            return FindIndexes(geographicIntersectRegion, _25kSize, _25kSize, (minLongitude, minLatitude) => Get25kSheetName(minLongitude, minLatitude), NccIndexType.Ncc25k);
        }

        public static List<IndexSheet> Find10kIndexSheets(BoundingBox geographicIntersectRegion)
        {
            return FindIndexes(geographicIntersectRegion, _10kWidth, _10kHeight, (minLongitude, minLatitude) => Get10kSheetName(minLongitude, minLatitude), NccIndexType.Ncc10k);
        }

        public static List<IndexSheet> Find5kIndexSheets(BoundingBox geographicIntersectRegion)
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
        private static List<IndexSheet> FindIndexes(BoundingBox geographicIntersectRegion, double indexWidth, double indexHeight, Func<double, double, string> namingFunc, NccIndexType type)
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

        public static List<BoundingBox> Find250kIndexMbbs(BoundingBox geographicIntersectRegion)
        {
            return FindIndexeMbbs(geographicIntersectRegion, _250kWidth, _250kHeight);
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

        public static List<BoundingBox> Find100kIndexMbbs(BoundingBox geographicIntersectRegion)
        {
            return FindIndexeMbbs(geographicIntersectRegion, _100kSize, _100kSize);
        }

        public static List<BoundingBox> Find50kIndexMbbs(BoundingBox geographicIntersectRegion)
        {
            return FindIndexeMbbs(geographicIntersectRegion, _50kSize, _50kSize);
        }

        public static List<BoundingBox> Find25kIndexMbbs(BoundingBox geographicIntersectRegion)
        {
            return FindIndexeMbbs(geographicIntersectRegion, _25kSize, _25kSize);
        }

        public static List<BoundingBox> Find10kIndexMbbs(BoundingBox geographicIntersectRegion)
        {
            return FindIndexeMbbs(geographicIntersectRegion, _10kWidth, _10kHeight);
        }

        public static List<BoundingBox> Find5kIndexMbbs(BoundingBox geographicIntersectRegion)
        {
            return FindIndexeMbbs(geographicIntersectRegion, _5kWidth, _5kHeight);
        }

        private static List<BoundingBox> FindIndexeMbbs(BoundingBox geographicIntersectRegion, double indexWidth, double indexHeight)
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

        //for 250k, 100k, 50k, 25k, 10k, 5k scales
        public static List<Geometry> GetIndexLines(BoundingBox geographicIntersectRegion, NccIndexType type)
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

                default:
                    throw new NotImplementedException();
            }
        }

        //for 2k, 1k, 1:500 scales
        public static List<Geometry> GetIndexLines(BoundingBox geographicIntersectRegion, NccIndexType type, int utmZone)
        {
            switch (type)
            {
                case NccIndexType.NccUtmBased2kBlock:
                    return GetIndexLines(geographicIntersectRegion, _2kUtmBlockWidth, _2kUtmBlockHeight, utmZone);

                case NccIndexType.NccUtmBased2kSheet:
                    return GetIndexLines(geographicIntersectRegion, _2kUtmSheetWidth, _2kUtmSheetHeight, utmZone);

                case NccIndexType.NccUtmBased1k:
                    return GetIndexLines(geographicIntersectRegion, _1kUtmSheetWidth, _1kUtmSheetHeight, utmZone);

                case NccIndexType.NccUtmBased500:
                    return GetIndexLines(geographicIntersectRegion, _500UtmSheetWidth, _500UtmSheetHeight, utmZone);

                default:
                    throw new NotImplementedException();
            }
        }

        private static List<Geometry> GetIndexLines(BoundingBox geographicIntersectRegion, double indexWidth, double indexHeight)
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
         
        private static List<Geometry> GetIndexLines(BoundingBox geographicIntersectRegion, double utmWidth, double utmHeight, int utmZone)
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


            //var control1 = new BoundingBox(_2kUtmXmin, _2kUtmYmin, _2kUtmXmax, _2kUtmYmax)
            //                    .Transform(p => MapProjects.UTMToGeodetic(p, utmZone));

            //var control11 = BoundingBoxHelper.UtmMbbToGeodeticWgs84Mbb(new BoundingBox(_2kUtmXmin, _2kUtmYmin, _2kUtmXmax, _2kUtmYmax), utmZone);

            //var control2 = new BoundingBox(_2kUtmXmin, _2kUtmYmin, _2kUtmXmax, _2kUtmYmax)
            //                    .Transform(p => MapProjects.UTMToGeodetic(p, utmZone))
            //                    .Intersect(geographicIntersectRegion);

            //var control22 = control11.Intersect(geographicIntersectRegion);


            //var control4 = control2.Transform(p => MapProjects.GeodeticToUTM(p, Ellipsoids.WGS84, utmZone));
            //var control44 = BoundingBoxHelper.GeodeticWgs84MbbToUtmMbb(control2);

            //var control5 = control22.Transform(p => MapProjects.GeodeticToUTM(p, Ellipsoids.WGS84, utmZone));
            //var control55 = BoundingBoxHelper.GeodeticWgs84MbbToUtmMbb(control22);


            var geoBound = BoundingBoxHelper.UtmMbbToGeodeticWgs84Mbb(_2kUtmBoudingBox, utmZone)
                              .Intersect(geographicIntersectRegion);

            var utmBound = BoundingBoxHelper.GeodeticWgs84MbbToUtmMbb(geoBound, utmZone)
                            .Intersect(_2kUtmBoudingBox);

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
