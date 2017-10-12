using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Ket.DigitalTerrainModeling
{
    public class GRDFileFormat
    {
        public int NumberOfColumns;

        public int NumberOfRows;

        public double NoDataValue;

        public double LowerLeftX;

        public double LowerLeftY;

        public double CellSize;

        public string Path;

        public IRI.Ham.Algebra.Matrix Values;

        public GRDFileFormat(string path)
        {
            if (!System.IO.File.Exists(path))
                throw new Exception("File does not exists!");

            System.IO.StreamReader reader = new System.IO.StreamReader(path);
            try
            {
                this.Path = path;

                this.NumberOfColumns = int.Parse((reader.ReadLine().Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries))[1]);

                this.NumberOfRows = int.Parse(reader.ReadLine().Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)[1]);

                this.LowerLeftX = double.Parse(reader.ReadLine().Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)[1]);

                this.LowerLeftY = double.Parse(reader.ReadLine().Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)[1]);

                this.CellSize = double.Parse(reader.ReadLine().Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)[1]);

                this.NoDataValue = double.Parse(reader.ReadLine().Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)[1]);

                this.ReadToMatrix();
            }
            catch
            {
                throw new Exception("Error while openning grd file");
            }
            finally
            {
                reader.Close();
            }

        }

        private void ReadToMatrix()
        {

            System.IO.StreamReader reader = new System.IO.StreamReader(this.Path);

            Values = new IRI.Ham.Algebra.Matrix(this.NumberOfRows, this.NumberOfColumns);

            for (int i = 0; i < 6; i++)
                reader.ReadLine();

            for (int i = 0; i < NumberOfRows; i++)
            {
                string line = reader.ReadLine();

                string[] lineValues = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                if (lineValues.Length != this.NumberOfColumns)
                    throw new Exception("File was corrupted");

                for (int j = 0; j < NumberOfColumns; j++)
                {
                    double tempValue = double.Parse(lineValues[j]);

                    if (tempValue == this.NoDataValue)
                        Values[i, j] = double.NaN;
                    else
                        Values[i, j] = tempValue;
                }
            }

            reader.Close();
        }

        public double[] ReadToDouble()
        {
            System.IO.StreamReader reader = new System.IO.StreamReader(this.Path);

            double[] result = new double[this.NumberOfRows * this.NumberOfColumns];

            for (int i = 0; i < 6; i++)
                reader.ReadLine();

            for (int i = 6; i < this.NumberOfRows; i++)
            {
                string line = reader.ReadLine();

                string[] lineValues = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                if (lineValues.Length != this.NumberOfColumns)
                    throw new Exception("File was corrupted");

                for (int j = 0; j < lineValues.Length; j++)
                {
                    double tempValue = double.Parse(lineValues[j]);

                    if (tempValue == this.NoDataValue)
                        result[(i - 6) * this.NumberOfColumns + j] = double.NaN;
                    else
                        result[(i - 6) * this.NumberOfColumns + j] = tempValue;
                }

                GC.Collect();
            }

            reader.Close();

            return result;
        }

        public IRI.Ham.Algebra.Matrix ReadQuarter(QuarterPart part)
        {
            int midRow = this.NumberOfRows % 2 == 0 ? this.NumberOfRows / 2 - 1 : this.NumberOfRows / 2;

            int midColumn = this.NumberOfColumns % 2 == 0 ? this.NumberOfColumns / 2 - 1 : this.NumberOfColumns / 2;

            switch (part)
            {
                case QuarterPart.UpperLeft:
                    return this.ReadRegion(0, 0, midRow, midColumn);

                case QuarterPart.UpperRight:
                    return this.ReadRegion(0, midColumn + 1, midRow, this.NumberOfColumns - 1);

                case QuarterPart.LowerLeft:
                    return this.ReadRegion(midRow + 1, 0, this.NumberOfRows - 1, midColumn);

                case QuarterPart.LowerRight:
                    return this.ReadRegion(midRow + 1, midColumn + 1, this.NumberOfRows - 1, this.NumberOfColumns - 1);

                default:
                    return null;
            }
        }

        public IRI.Ham.Algebra.Matrix ReadRegion(int startRow, int startColumn, int endRow, int endColumn)
        {
            return this.Values.SubMatrix(startRow, startColumn, endRow, endColumn);
        }

        public void SaveQuarter(string fileName, QuarterPart part)
        {
            int midRow = this.NumberOfRows % 2 == 0 ? this.NumberOfRows / 2 - 1 : this.NumberOfRows / 2;

            int midColumn = this.NumberOfColumns % 2 == 0 ? this.NumberOfColumns / 2 - 1 : this.NumberOfColumns / 2;

            IRI.Ham.Algebra.Matrix values;

            switch (part)
            {
                case QuarterPart.UpperLeft:

                    values = this.ReadRegion(0, 0, midRow, midColumn);

                    SaveAsGRD(fileName,
                                values,
                                0 + this.LowerLeftX,
                                (this.NumberOfRows - values.NumberOfRows) * this.CellSize + this.LowerLeftY,
                                this.CellSize,
                                this.NoDataValue);
                    break;

                case QuarterPart.UpperRight:

                    values = this.ReadRegion(0, midColumn + 1, midRow, this.NumberOfColumns - 1);

                    SaveAsGRD(fileName,
                                values,
                                (this.NumberOfColumns - values.NumberOfColumns) * this.CellSize + this.LowerLeftX,
                                (this.NumberOfRows - values.NumberOfRows) * this.CellSize + this.LowerLeftY,
                                this.CellSize,
                                this.NoDataValue);

                    break;

                case QuarterPart.LowerLeft:

                    values = this.ReadRegion(midRow + 1, 0, this.NumberOfRows - 1, midColumn);


                    SaveAsGRD(fileName,
                                values,
                                0 + this.LowerLeftX,
                                0 + this.LowerLeftY,
                                this.CellSize,
                                this.NoDataValue);

                    break;

                case QuarterPart.LowerRight:

                    values = this.ReadRegion(midRow + 1, midColumn + 1, this.NumberOfRows - 1, this.NumberOfColumns - 1);

                    SaveAsGRD(fileName,
                                values,
                                (this.NumberOfColumns - values.NumberOfColumns) * this.CellSize + this.LowerLeftX,
                                0 + this.LowerLeftY,
                                this.CellSize,
                                this.NoDataValue);

                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        public void SaveAsGRD(string path, double[,] values, double lowerLeftX, double lowerLeftY, double cellSize, double noDataValue)
        {
            System.IO.StreamWriter writer = new System.IO.StreamWriter(path);

            int numberOfColumns = values.GetLength(1);

            int numberOfRows = values.GetLength(0);

            writer.WriteLine(string.Format("ncols         {0}", numberOfColumns));

            writer.WriteLine(string.Format("nrows         {0}", numberOfRows));

            writer.WriteLine(string.Format("xllcorner     {0}", lowerLeftX));

            writer.WriteLine(string.Format("yllcorner     {0}", lowerLeftY));

            writer.WriteLine(string.Format("cellsize      {0}", cellSize));

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

        public void SaveAsGRD(string path, IRI.Ham.Algebra.Matrix values, double lowerLeftX, double lowerLeftY, double cellSize, double noDataValue)
        {
            System.IO.StreamWriter writer = new System.IO.StreamWriter(path);

            int numberOfColumns = values.NumberOfColumns;

            int numberOfRows = values.NumberOfRows;

            writer.WriteLine(string.Format("ncols         {0}", numberOfColumns));

            writer.WriteLine(string.Format("nrows         {0}", numberOfRows));

            writer.WriteLine(string.Format("xllcorner     {0}", lowerLeftX));

            writer.WriteLine(string.Format("yllcorner     {0}", lowerLeftY));

            writer.WriteLine(string.Format("cellsize      {0}", cellSize));

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

    }
}