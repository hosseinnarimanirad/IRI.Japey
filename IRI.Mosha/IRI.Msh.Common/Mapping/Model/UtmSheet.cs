using IRI.Msh.Common.Primitives;
using IRI.Msh.CoordinateSystem;
using IRI.Msh.CoordinateSystem.MapProjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Msh.Common.Mapping
{
    public class UtmSheet
    {
        internal const int _2kUtmSheetRowCount = 20;
        internal const int _2kUtmSheetColumnCount = 20;
        internal const int _2kUtmBlockRowCount = 70;
        internal const int _2kUtmBlockColumnCount = 21;

        internal static readonly char[] _2kUtmBlockColumns;
        internal static readonly char[] _2kUtmSheetColumns;

        public BoundingBox UtmExtent { get; set; }

        public BoundingBox GeodeticExtent
        {
            get
            {
                return UtmExtent.Transform(p => IRI.Msh.CoordinateSystem.MapProjection.MapProjects.UTMToGeodetic(p, UtmZone));
            }
        }

        public int Row { get; set; }

        public int Column { get; set; }

        public string Title { get; set; }

        public NccIndexType Type { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }

        public int UtmZone { get; set; }

        static UtmSheet()
        {
            _2kUtmBlockColumns = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U' };

            _2kUtmSheetColumns = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T' };
        }

        private UtmSheet()
        {

        }


        //From lat/long
        public static UtmSheet Create2kUtmBlock(double longitude, double latitude, int utmZone)
        {
            var utmPoint = MapProjects.GeodeticToUTM(new Point(longitude, latitude), Ellipsoids.WGS84, utmZone);

            if (!MapIndexes._2kUtmBoudingBox.Intersects(utmPoint))
            {
                return null;
            }

            var rowColumn = CalculateRowColumn(MapIndexes._2kUtmBoudingBox, utmPoint, MapIndexes._2kUtmBlockWidth, MapIndexes._2kUtmBlockHeight);

            return UtmSheet.Create2kBlock(rowColumn.Row, rowColumn.Column, utmZone);
        }

        public static UtmSheet Create2kUtmSheet(double longitude, double latitude, int utmZone)
        {
            var utmPoint = MapProjects.GeodeticToUTM(new Point(longitude, latitude), Ellipsoids.WGS84, utmZone);

            if (!MapIndexes._2kUtmBoudingBox.Intersects(utmPoint))
            {
                return null;
            }

            var block = Create2kUtmBlock(longitude, latitude, utmZone);

            var rowColumn = CalculateRowColumn(block.UtmExtent, utmPoint, MapIndexes._2kUtmSheetWidth, MapIndexes._2kUtmSheetHeight);

            return UtmSheet.Create2kSheet(block, rowColumn.Row, rowColumn.Column);
        }

        public static UtmSheet Create1kUtmSheet(double longitude, double latitude, int utmZone)
        {
            var utmPoint = MapProjects.GeodeticToUTM(new Point(longitude, latitude), Ellipsoids.WGS84, utmZone);

            if (!MapIndexes._2kUtmBoudingBox.Intersects(utmPoint))
            {
                return null;
            }

            var utm2kSheet = Create2kUtmSheet(longitude, latitude, utmZone);

            var rowColumn = CalculateRowColumn(utm2kSheet.UtmExtent, utmPoint, MapIndexes._1kUtmSheetWidth, MapIndexes._1kUtmSheetHeight);

            return UtmSheet.Create1kSheet(utm2kSheet, rowColumn.Row, rowColumn.Column);
        }

        public static UtmSheet Create500UtmSheet(double longitude, double latitude, int utmZone)
        {
            var utmPoint = MapProjects.GeodeticToUTM(new Point(longitude, latitude), Ellipsoids.WGS84, utmZone);

            if (!MapIndexes._2kUtmBoudingBox.Intersects(utmPoint))
            {
                return null;
            }

            var utm1kSheet = Create1kUtmSheet(longitude, latitude, utmZone);

            var rowColumn = CalculateRowColumn(utm1kSheet.UtmExtent, utmPoint, MapIndexes._500UtmSheetWidth, MapIndexes._500UtmSheetHeight);

            return UtmSheet.Create500Sheet(utm1kSheet, rowColumn.Row, rowColumn.Column);
        }


        //From row/column
        private static UtmSheet Create2kBlock(int row, int column, int utmZone)
        {
            UtmSheet result = new UtmSheet()
            {
                Row = row,
                Column = column,
                UtmZone = utmZone,
                Title = $"{utmZone}{_2kUtmBlockColumns[column]}{row.ToString("00")}",
                Type = NccIndexType.NccUtmBased2kBlock,
                Width = MapIndexes._2kUtmBlockWidth,
                Height = MapIndexes._2kUtmBlockHeight,
                UtmExtent = CreateBound(row, column, MapIndexes._2kUtmXmin, MapIndexes._2kUtmYmax, MapIndexes._2kUtmBlockWidth, MapIndexes._2kUtmBlockHeight)
                //Extent = new BoundingBox(MapIndexes._2kUtmXmin + column * MapIndexes._2kUtmBlockWidth,
                //                         MapIndexes._2kUtmYmax - (row + 1) * MapIndexes._2kUtmBlockHeight,
                //                         MapIndexes._2kUtmXmin + (column + 1) * MapIndexes._2kUtmBlockWidth,
                //                         MapIndexes._2kUtmYmax - row * MapIndexes._2kUtmBlockHeight)
            };

            return result;
        }

        private static UtmSheet Create2kSheet(UtmSheet utm2kBlock, int row, int column)
        {
            if (utm2kBlock.Type != NccIndexType.NccUtmBased2kBlock)
            {
                throw new NotImplementedException();
            }

            UtmSheet result = new UtmSheet()
            {
                Row = row,
                Column = column,
                UtmZone = utm2kBlock.UtmZone,
                Title = $"{utm2kBlock.Title}{_2kUtmSheetColumns[column]}{row.ToString("00")}",
                Type = NccIndexType.NccUtmBased2kSheet,
                Width = MapIndexes._2kUtmSheetWidth,
                Height = MapIndexes._2kUtmSheetHeight,
                UtmExtent = CreateBound(row, column, utm2kBlock.UtmExtent.XMin, utm2kBlock.UtmExtent.YMax, MapIndexes._2kUtmSheetWidth, MapIndexes._2kUtmSheetHeight)
            };

            return result;
        }

        private static UtmSheet Create1kSheet(UtmSheet utm2kSheet, int row, int column)
        {
            if (utm2kSheet.Type != NccIndexType.NccUtmBased2kSheet)
            {
                throw new NotImplementedException();
            }

            UtmSheet result = new UtmSheet()
            {
                Row = row,
                Column = column,
                UtmZone = utm2kSheet.UtmZone,
                Title = $"{utm2kSheet.Title}{RowColumnToSheetNumber(row, column)}",
                Type = NccIndexType.NccUtmBased1k,
                Width = MapIndexes._1kUtmSheetWidth,
                Height = MapIndexes._1kUtmSheetHeight,
                UtmExtent = CreateBound(row, column, utm2kSheet.UtmExtent.XMin, utm2kSheet.UtmExtent.YMax, MapIndexes._1kUtmSheetWidth, MapIndexes._1kUtmSheetHeight)
            };

            return result;
        }

        private static UtmSheet Create500Sheet(UtmSheet utm1kSheet, int row, int column)
        {
            if (utm1kSheet.Type != NccIndexType.NccUtmBased1k)
            {
                throw new NotImplementedException();
            }

            UtmSheet result = new UtmSheet()
            {
                Row = row,
                Column = column,
                UtmZone = utm1kSheet.UtmZone,
                Title = $"{utm1kSheet.Title}{RowColumnToSheetNumber(row, column)}",
                Type = NccIndexType.NccUtmBased500,
                Width = MapIndexes._500UtmSheetWidth,
                Height = MapIndexes._500UtmSheetHeight,
                UtmExtent = CreateBound(row, column, utm1kSheet.UtmExtent.XMin, utm1kSheet.UtmExtent.YMax, MapIndexes._500UtmSheetWidth, MapIndexes._500UtmSheetHeight)
            };

            return result;
        }




        private static BoundingBox CreateBound(int row, int column, double xMin, double yMax, double width, double height)
        {
            return new BoundingBox(xMin + column * width,
                                     yMax - (row + 1) * height,
                                     xMin + (column + 1) * width,
                                     yMax - row * height);
        }

        private static int RowColumnToSheetNumber(int row, int column)
        {
            if (row == 0 && column == 1)
            {
                return 1;
            }
            else if (row == 1 && column == 1)
            {
                return 2;
            }
            else if (row == 1 && column == 0)
            {
                return 3;
            }
            else if (row == 0 && column == 0)
            {
                return 4;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private static RowColumn CalculateRowColumn(BoundingBox utmBoundingBox, Point utmPoint, double columnWidth, double rowHeight)
        {
            //if (!utmBoundingBox.Intersects(utmPoint))
            //{
            //    return null;
            //}

            var column = (int)Math.Floor((utmPoint.X - utmBoundingBox.XMin) / columnWidth);

            var row = (int)Math.Floor((utmBoundingBox.YMax - utmPoint.Y) / rowHeight);

            return new RowColumn() { Row = row, Column = column };
        }
    }
}
