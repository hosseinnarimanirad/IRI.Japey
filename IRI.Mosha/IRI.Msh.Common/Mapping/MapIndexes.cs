using IRI.Msh.Common.Model.Mapping;
using IRI.Msh.Common.Primitives;
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


        #region BoundingBox

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


        #region IndexSheet

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

                        return new IndexSheet()
                        {
                            SheetName = ncc100kSheetName,
                            Extent = new BoundingBox(xMin, yMin, xMin + _100kSize, yMin + _100kSize)
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

            return new IndexSheet()
            {
                Extent = new BoundingBox(minLongitude, minLatitude, minLongitude + _100kSize, minLatitude + _100kSize),
                SheetName = Get100kSheetName(minLongitude, minLatitude)
            };
        }

        public static IndexSheet Get50kIndexSheet(double longitude, double latitude)
        {
            var minLongitude = Math.Floor(longitude / _50kSize) * _50kSize;

            var minLatitude = Math.Floor(latitude / _50kSize) * _50kSize;

            return new IndexSheet()
            {
                Extent = new BoundingBox(minLongitude, minLatitude, minLongitude + _50kSize, minLatitude + _50kSize),
                SheetName = Get50kSheetName(minLongitude, minLatitude)
            };
        }

        public static IndexSheet Get25kIndexSheet(double longitude, double latitude)
        {
            var minLongitude = Math.Floor(longitude / _25kSize) * _25kSize;

            var minLatitude = Math.Floor(latitude / _25kSize) * _25kSize;

            return new IndexSheet()
            {
                Extent = new BoundingBox(minLongitude, minLatitude, minLongitude + _25kSize, minLatitude + _25kSize),
                SheetName = Get25kSheetName(minLongitude, minLatitude)
            };
        }

        #endregion


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



        public static List<IndexSheet> FindNcc100kIndexes(BoundingBox geographicIntersectRegion)
        {
            return FindIndexes(geographicIntersectRegion, _100kSize, _100kSize, (minLongitude, minLatitude) => Get100kSheetName(minLongitude, minLatitude));
        }

        public static List<IndexSheet> FindNcc50kIndexes(BoundingBox geographicIntersectRegion)
        {
            return FindIndexes(geographicIntersectRegion, _50kSize, _50kSize, (minLongitude, minLatitude) => Get50kSheetName(minLongitude, minLatitude));
        }

        public static List<IndexSheet> FindNcc25kIndexes(BoundingBox geographicIntersectRegion)
        {
            return FindIndexes(geographicIntersectRegion, _25kSize, _25kSize, (minLongitude, minLatitude) => Get25kSheetName(minLongitude, minLatitude));
        }

        public static List<IndexSheet> FindNcc10kIndexes(BoundingBox geographicIntersectRegion)
        {
            return FindIndexes(geographicIntersectRegion, _10kWidth, _10kHeight, (minLongitude, minLatitude) => Get10kSheetName(minLongitude, minLatitude));
        }

        public static List<IndexSheet> FindNcc5kIndexes(BoundingBox geographicIntersectRegion)
        {
            return FindIndexes(geographicIntersectRegion, _5kWidth, _5kHeight, (minLongitude, minLatitude) => Get5kSheetName(minLongitude, minLatitude));
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="geographicIntersectRegion"></param>
        /// <param name="indexWidth"></param>
        /// <param name="indexHeight"></param>
        /// <param name="namingFunc">Based on Lower Left Coordinate</param>
        /// <returns></returns>
        public static List<IndexSheet> FindIndexes(BoundingBox geographicIntersectRegion, double indexWidth, double indexHeight, Func<double, double, string> namingFunc)
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

                    result.Add(new IndexSheet()
                    {
                        Extent = new BoundingBox(i, j, i + indexWidth, j + indexHeight),
                        SheetName = sheetName
                    });
                }
            }

            return result;
        }

        #region Index Lines

        public static List<Geometry> GetIndexLines(BoundingBox geographicIntersectRegion, Indexes type)
        {
            switch (type)
            {
                case Indexes.Ncc250k:
                    return Get250kIndexLines(geographicIntersectRegion);

                case Indexes.Ncc100k:
                    return Get100kIndexLines(geographicIntersectRegion);

                case Indexes.Ncc50k:
                    return Get50kIndexLines(geographicIntersectRegion);

                case Indexes.Ncc25k:
                    return Get25kIndexLines(geographicIntersectRegion);

                case Indexes.Ncc10k:
                    return Get10kIndexLines(geographicIntersectRegion);

                case Indexes.Ncc5k:
                    return Get5kIndexLines(geographicIntersectRegion);

                default:
                    throw new NotImplementedException();
            }
        }

        private static List<Geometry> Get250kIndexLines(BoundingBox geographicIntersectRegion)
        {
            return GetIndexLines(geographicIntersectRegion, _250kWidth, _250kHeight);
        }

        private static List<Geometry> Get100kIndexLines(BoundingBox geographicIntersectRegion)
        {
            return GetIndexLines(geographicIntersectRegion, _100kSize, _100kSize);
        }

        private static List<Geometry> Get50kIndexLines(BoundingBox geographicIntersectRegion)
        {
            return GetIndexLines(geographicIntersectRegion, _50kSize, _50kSize);
        }

        private static List<Geometry> Get25kIndexLines(BoundingBox geographicIntersectRegion)
        {
            return GetIndexLines(geographicIntersectRegion, _25kSize, _25kSize);
        }

        private static List<Geometry> Get10kIndexLines(BoundingBox geographicIntersectRegion)
        {
            return GetIndexLines(geographicIntersectRegion, _10kWidth, _10kHeight);
        }

        private static List<Geometry> Get5kIndexLines(BoundingBox geographicIntersectRegion)
        {
            return GetIndexLines(geographicIntersectRegion, _5kWidth, _5kHeight);
        }

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
