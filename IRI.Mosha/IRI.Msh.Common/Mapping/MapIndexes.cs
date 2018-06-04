using IRI.Msh.Common.Model.Mapping;
using IRI.Msh.Common.Primitives;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace IRI.Msh.Common.Mapping
{
    public static class MapIndexes
    {
        const double _250kWidth = 1.5;

        const double _250kHeight = 1.0;

        //size: width and height (degree)
        const double _100kSize = 0.5;

        //size: width and height (degree)
        const double _50kSize = 0.25;

        //size: width and height (degree)
        const double _25kSize = 0.125;

        const double _10kWidth = 5.0 / 60.0;

        const double _10kHeight = 3.0 / 60.0;

        const double _5kWidth = 2.5 / 60.0;

        const double _5kHeight = 1.5 / 60.0;

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
                SheetName = _100kSheetNamingFunc(minLongitude, minLatitude)
            };
        }

        public static IndexSheet Get50kIndexSheet(double longitude, double latitude)
        {
            var minLongitude = Math.Floor(longitude / _50kSize) * _50kSize;

            var minLatitude = Math.Floor(latitude / _50kSize) * _50kSize;

            return new IndexSheet()
            {
                Extent = new BoundingBox(minLongitude, minLatitude, minLongitude + _50kSize, minLatitude + _50kSize),
                SheetName = _50kSheetNamingFunc(minLongitude, minLatitude)
            };
        }

        public static IndexSheet Get25kIndexSheet(double longitude, double latitude)
        {
            var minLongitude = Math.Floor(longitude / _25kSize) * _25kSize;

            var minLatitude = Math.Floor(latitude / _25kSize) * _25kSize;

            return new IndexSheet()
            {
                Extent = new BoundingBox(minLongitude, minLatitude, minLongitude + _25kSize, minLatitude + _25kSize),
                SheetName = _25kSheetNamingFunc(minLongitude, minLatitude)
            };
        }

        private static Func<double, double, string> _100kSheetNamingFunc = (minLongitude, minLatitude) => FormattableString.Invariant($"{minLongitude * 2 - 40}{minLatitude * 2 - 10}");

        private static Func<double, double, string> _50kSheetNamingFunc = (minLongitude, minLatitude) =>
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
        };

        private static Func<double, double, string> _25kSheetNamingFunc = (minLongitude, minLatitude) =>
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
        };

        public static List<IndexSheet> FindNcc100kIndexes(BoundingBox geographicIntersectRegion)
        {
            return FindIndexes(geographicIntersectRegion, _100kSize, _100kSize, (minLongitude, minLatitude) => _100kSheetNamingFunc(minLongitude, minLatitude));
        }

        public static List<IndexSheet> FindNcc50kIndexes(BoundingBox geographicIntersectRegion)
        {
            return FindIndexes(geographicIntersectRegion, _50kSize, _50kSize, (minLongitude, minLatitude) => _50kSheetNamingFunc(minLongitude, minLatitude));
        }

        public static List<IndexSheet> FindNcc25kIndexes(BoundingBox geographicIntersectRegion)
        {
            return FindIndexes(geographicIntersectRegion, _25kSize, _25kSize, (minLongitude, minLatitude) => _25kSheetNamingFunc(minLongitude, minLatitude));
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
                    result.Add(new IndexSheet()
                    {
                        Extent = new BoundingBox(i, j, i + indexWidth, j + indexHeight),
                        SheetName = namingFunc(i, j)
                    });
                }
            }

            return result;
        }
    }
}
