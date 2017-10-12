// besmellahe rahmane rahim
// Allahomma ajjel le-valiyek al-faraj

using System;
using System.Collections.Generic;
using System.Text;
using IRI.Ket.Geometry;
using IRI.Ham.Algebra;
using IRI.Ket.DataStructure;

namespace IRI.Ket.DigitalTerrainModeling
{
    [Serializable]
    public class RegularDtm
    {
        #region Field & Properties

        private double cellWidth;

        private double cellHeight;

        //coordinate of the center of upper left cell
        private Point lowerLeftCoordinate;

        protected IRI.Ham.Algebra.Matrix values;

        public double CellWidth
        {
            get { return cellWidth; }
        }
        
        public double CellHeight
        {
            get { return cellHeight; }
        }
        
        public int NumberOfRows
        {
            get { return this.values.NumberOfRows; }
        }

        public int NumberOfColumns
        {
            get { return this.values.NumberOfColumns; }
        }

        public AttributedPoint LowerLeft
        {
            get { return this.GetPoint(NumberOfRows - 1, 0); }
        }

        public AttributedPoint LoweRight
        {
            get { return this.GetPoint(NumberOfRows - 1, NumberOfColumns - 1); }
        }

        public AttributedPoint UpperLeft
        {
            get { return this.GetPoint(0, 0); }
        }

        public AttributedPoint UppeRight
        {
            get { return this.GetPoint(0, NumberOfColumns - 1); }
        }

        public Matrix Values
        {
            get { return this.values; }
        }

        public double MinX
        {
            get { return lowerLeftCoordinate.X - CellWidth / 2; }
        }

        public double MinY
        {
            get { return lowerLeftCoordinate.Y - cellHeight / 2; }
        }

        public double MaxY
        {
            get { return this.MinY + this.NumberOfRows * CellHeight; }
        }

        public double MaX
        {
            get { return this.MinX + this.NumberOfColumns * CellWidth; }
        }

        public AttributedPoint this[int row, int column]
        {
            get { return GetPoint(column, row); }
            //set { /* set the specified index to value here */ }
        }

        #endregion

        #region Constructor

        public RegularDtm(double[,] values, double cellSize, Point lowerLeft)
            : this(new Matrix(values), cellSize, cellSize, lowerLeft) { }

        public RegularDtm(double[,] values, double cellWidth, double cellHeight, Point lowerLeft)
            : this(new Matrix(values), cellWidth, cellHeight, lowerLeft) { }

        public RegularDtm(Matrix values, double cellSize, Point lowerLeft)
            : this(values, cellSize, cellSize, lowerLeft) { }

        public RegularDtm(Matrix values, double cellWidth, double cellHeight, Point lowerLeft)
        {
            this.values = values;

            this.cellHeight = cellHeight;

            this.cellWidth = cellWidth;

            this.lowerLeftCoordinate = lowerLeft;
        }

        public RegularDtm(GRDFileFormat file)
            : this(file.Values, file.CellSize, file.CellSize, new Point(file.LowerLeftX, file.LowerLeftY)) { }

        #endregion

        #region Methods

        public AttributedPoint GetPoint(int row, int column)
        {
            return new AttributedPoint(lowerLeftCoordinate.X + column * cellWidth,
                                        lowerLeftCoordinate.Y + (this.NumberOfRows - 1 - row) * cellHeight,
                                        values[row, column]);
        }

        public RegularDtm Subtract(RegularDtm dtm)
        {
            Matrix resultValues = Differece(dtm);

            return new RegularDtm(resultValues, this.CellWidth, this.CellHeight, new Point(this.MinX, this.MinY));
        }

        public Matrix Differece(RegularDtm dtm)
        {
            if (!this.AreTheSameRegion(dtm))
            {
                throw new NotImplementedException();
            }

            return this.values - dtm.values;
        }

        public RegularDtm SubtractFromFinerDtm(RegularDtm finerDtm)
        {
            Matrix result = DifferenceFromFinerDtm(finerDtm);

            return new RegularDtm(result, finerDtm.CellWidth, finerDtm.CellHeight, new Point(finerDtm.MinX, finerDtm.MinY));
        }

        public Matrix DifferenceFromFinerDtm(RegularDtm finerDtm)
        {
            if (!AreTheSameRegion(finerDtm))
            {
                throw new NotImplementedException();
            }

            if (this.CellHeight < finerDtm.CellHeight || this.CellWidth < finerDtm.CellWidth)
            {
                throw new NotImplementedException();
            }

            Matrix result = new Matrix(finerDtm.NumberOfRows, finerDtm.NumberOfColumns);

            double widthRatio = finerDtm.CellWidth / this.CellWidth;

            double heightRatio = finerDtm.CellHeight / this.CellHeight;

            for (int i = 0; i < finerDtm.NumberOfRows; i++)
            {
                for (int j = 0; j < finerDtm.NumberOfColumns; j++)
                {
                    int tempI = (int)((i * finerDtm.CellHeight + finerDtm.CellHeight / 2) / this.CellHeight);

                    int tempJ = (int)((j * finerDtm.CellWidth + finerDtm.CellWidth / 2) / this.CellWidth);

                    result[i, j] = this.values[tempI, tempJ] - finerDtm.values[i, j];
                }
            }

            return result;
        }

        public bool AreTheSameRegion(RegularDtm dtm)
        {
            return this.MinX - this.CellWidth / 2 == dtm.MinX - dtm.CellWidth / 2 &&
                    this.MinY - this.CellHeight / 2 == dtm.MinY - dtm.CellHeight / 2 &&
                    this.MaX + this.CellWidth / 2 == dtm.MaX + dtm.CellWidth / 2 &&
                    this.MaxY + this.CellHeight / 2 == dtm.MaxY + dtm.CellHeight / 2;
        }

        #endregion

        #region SpecialMethods

        public double CalculateSecondDifference(int row, int column, RasterDirection direction)
        {
            switch (direction)
            {
                case RasterDirection.EastWest:
                    return 2 * values[row, column] - values[row, column - 1] - values[row, column + 1];

                case RasterDirection.SouthNorth:
                    return 2 * values[row, column] - values[row - 1, column] - values[row + 1, column];

                case RasterDirection.SouthernWestNorthernEast:
                    return 2 * values[row, column] - values[row + 1, column - 1] - values[row - 1, column + 1];

                case RasterDirection.NorthernWestSouthernEast:
                    return 2 * values[row, column] - values[row - 1, column - 1] - values[row + 1, column + 1];

                default:
                    throw new NotImplementedException();
            }
        }

        public double CalculateWestEastSlope(int row, int column)
        {
            double coef = Math.Sqrt(2);

            return ((values[row + 1, column + 1] + coef * values[row, column + 1] + values[row - 1, column + 1]) -
                     (values[row - 1, column + 1] + coef * values[row, column - 1] + values[row - 1, column - 1])) /
                     ((4 + 2 * coef) * cellWidth);
        }

        public double CalculateSouthNorthSlope(int row, int column)
        {
            double coef = Math.Sqrt(2);

            return ((values[row - 1, column + 1] + coef * values[row - 1, column] + values[row - 1, column - 1]) -
                    (values[row + 1, column + 1] + coef * values[row + 1, column] + values[row + 1, column - 1])) /
                    ((4 + 2 * coef) * cellHeight);
        }

        public IRI.Ham.Algebra.Matrix GetSlopeMatrix()
        {
            Matrix result = new Matrix(this.NumberOfRows - 2, this.NumberOfColumns - 2);

            for (int i = 1; i < this.NumberOfRows - 1; i++)
            {
                for (int j = 1; j < this.NumberOfColumns - 1; j++)
                {
                    double xSlope = CalculateWestEastSlope(i, j);

                    double ySlope = CalculateSouthNorthSlope(i, j);

                    result[i - 1, j - 1] = Math.Atan(Math.Sqrt(xSlope * xSlope + ySlope * ySlope));
                }
            }

            return result;
        }

        public IRI.Ham.Algebra.Matrix GetAspectMatrix()
        {
            Matrix result = new Matrix(this.NumberOfRows - 2, this.NumberOfColumns - 2);

            for (int i = 1; i < this.NumberOfRows - 1; i++)
            {
                for (int j = 1; j < this.NumberOfColumns - 1; j++)
                {
                    double xSlope = CalculateWestEastSlope(i, j);

                    double ySlope = CalculateSouthNorthSlope(i, j);

                    double temp = Math.Atan2(ySlope, xSlope);

                    result[i - 1, j - 1] = temp < 0 ? temp + 2 * Math.PI : temp;
                }
            }

            return result;
        }

        #endregion

        #region Conversions & Exports

        /// <summary>
        /// Chen and Guevara 77
        /// </summary>
        /// <param name="numberOfPoints"></param>
        /// <returns></returns>
        public AttributedPointCollection SelectPointsBaesdOnCAG(int numberOfPoints)
        {
            int numberOfRow = this.NumberOfRows;

            int numberOfColumns = this.NumberOfColumns;

            IndexValue<double>[] significanceValues = new IndexValue<double>[numberOfRow * numberOfColumns];

            for (int i = 1; i < numberOfRow - 1; i++)
            {
                for (int j = 1; j < numberOfColumns - 1; j++)
                {
                    double first = CalculateSecondDifference(i, j, RasterDirection.EastWest);

                    double second = CalculateSecondDifference(i, j, RasterDirection.SouthNorth);

                    double third = CalculateSecondDifference(i, j, RasterDirection.NorthernWestSouthernEast);

                    double fourth = CalculateSecondDifference(i, j, RasterDirection.SouthernWestNorthernEast);

                    significanceValues[i * numberOfColumns + j] =
                        new IndexValue<double>(i * numberOfColumns + j, first + second + third + fourth);
                }
            }

            IndexValue<double>[] sortedSignificanceValues = SortAlgorithm.Heapsort<IndexValue<double>>(significanceValues, SortDirection.Ascending);

            AttributedPointCollection irregularPoints = new AttributedPointCollection();

            for (int i = 0; i < numberOfPoints; i++)
            {
                int row = (int)Math.Floor((double)sortedSignificanceValues[i].Index / numberOfColumns);

                int column = sortedSignificanceValues[i].Index % numberOfColumns;

                irregularPoints.Add(this.GetPoint(row, column));
            }

            //AddBorder
            irregularPoints.Add(this.LowerLeft);

            irregularPoints.Add(this.LoweRight);

            irregularPoints.Add(this.UpperLeft);

            irregularPoints.Add(this.UppeRight);

            return irregularPoints;
        }

        public IrregularDtm ToIrregularDtmBaesdOnCAG(int numberOfPoints)
        {
            AttributedPointCollection irregularPoints = SelectPointsBaesdOnCAG(numberOfPoints);

            return new IrregularDtm(irregularPoints);
        }

        public AttributedPointCollection SelectPointsBasedOnLi(double threshold)
        {
            int numberOfRow = this.NumberOfRows;

            int numberOfColumns = this.NumberOfColumns;

            AttributedPointCollection irregularPoints = new AttributedPointCollection();

            Matrix values = this.values.Clone();

            for (int i = 1; i < numberOfRow - 1; i++)
            {
                for (int j = 1; j < numberOfColumns - 1; j++)
                {
                    double firstDifference = values[i, j] - ((values[i, j - 1] + values[i, j + 1]) / 2);

                    double secondDifference = values[i, j] - ((values[i - 1, j] + values[i + 1, j]) / 2);

                    double thirdDifference = values[i, j] - ((values[i + 1, j - 1] + values[i - 1, j + 1]) / 2);

                    double fourthDifference = values[i, j] - ((values[i - 1, j - 1] + values[i + 1, j + 1]) / 2);

                    if ((Math.Abs(firstDifference) + Math.Abs(secondDifference) + Math.Abs(thirdDifference) + Math.Abs(fourthDifference)) / 4 > threshold)
                    {
                        irregularPoints.Add(this.GetPoint(i, j));
                    }
                    else
                    {
                        values[i, j] = (4 * values[i, j] - firstDifference - secondDifference - thirdDifference - fourthDifference) / 4;
                    }
                }
            }

            //AddBorder
            irregularPoints.Add(this.LowerLeft);

            irregularPoints.Add(this.LoweRight);

            irregularPoints.Add(this.UpperLeft);

            irregularPoints.Add(this.UppeRight);

            return irregularPoints;
        }

        public IrregularDtm ToIrregularDtmBasedOnLi(double threshold)
        {
            AttributedPointCollection irregularPoints = SelectPointsBasedOnLi(threshold);

            return new IrregularDtm(irregularPoints);
        }

        public void SaveAsGRD(string fileName, double noDataValue)
        {
            if ((object)values == null)
            {
                throw new NotImplementedException();
            }

            if (this.cellHeight != this.cellWidth)
            {

                throw new NotImplementedException();
            }

            double numberOfRows = this.NumberOfRows;

            double numberOfColumns = this.NumberOfColumns;

            System.IO.StreamWriter writer = new System.IO.StreamWriter(fileName);

            writer.WriteLine(string.Format("ncols         {0}", numberOfColumns));

            writer.WriteLine(string.Format("nrows         {0}", numberOfRows));

            writer.WriteLine(string.Format("xllcorner     {0}", this.MinX));

            writer.WriteLine(string.Format("yllcorner     {0}", this.MinY));

            writer.WriteLine(string.Format("cellsize      {0}", this.cellHeight));

            writer.WriteLine(string.Format("NODATA_value {0}", noDataValue));

            for (int i = 0; i < numberOfRows; i++)
            {
                for (int j = 0; j < numberOfColumns; j++)
                {
                    writer.Write(string.Format("{0} ", values[i, j]));
                }

                writer.WriteLine();
            }

            writer.Close();

        }

        #endregion

        //public RegularDtm MakeCourser(ushort Scale)
        //{
        //    int numberOfColumns = Math.Ceiling(this.NumberOfColumns / Scale);

        //    int numberOfRows = Math.Ceiling(this.numberOfRows / Scale);

        //    Matrix result = new Matrix(numberOfRows, numberOfColumns);

        //    int endRow = numberOfRows - numberOfRows % Scale;

        //    int endColumn = numberOfRows - numberOfColumns % Scale;

        //    for (int i = 0; i < numberOfRows - endRow; i++)
        //    {
        //        for (int j = 0; j < numberOfColumns - endColumn; j++)
        //        {

        //            for (int k = 0; k < Scale; k++)
        //            {

        //            }
        //        }
        //    }

        //}
    }
}
