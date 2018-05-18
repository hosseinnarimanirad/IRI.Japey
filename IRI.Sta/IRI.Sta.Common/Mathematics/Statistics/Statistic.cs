using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using IRI.Ham.Algebra;

namespace IRI.Ham.Statistics
{
    public static class Statistics
    {
        #region Maximum

        public static double GetMax(Matrix values)
        {
            int width = values.NumberOfColumns;

            int height = values.NumberOfRows;

            double result = double.NegativeInfinity;

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (values[j, i] > result)
                    {
                        result = values[j, i];
                    }
                }
            }

            return result;
        }

        public static double GetMax(double[] values)
        {

            if (values.Length < 0)
            {
                throw new ZeroSizeArrayException();
            }

            double resultVlaue = values[0];

            for (int i = 1; i < values.Length; i++)
            {
                resultVlaue = Math.Max(resultVlaue, values[i]);
            }

            return resultVlaue;

        }

        public static int GetMax(int[] values)
        {

            if (values.Length < 0)
            {
                throw new ZeroSizeArrayException();
            }

            int resultVlaue = values[0];

            for (int i = 1; i < values.Length; i++)
            {
                resultVlaue = Math.Max(resultVlaue, values[i]);
            }

            return resultVlaue;

        }

        public static TValue GetMax<TObject, TValue>(IEnumerable<TObject> array, Func<TObject, TValue> mapFunction) where TValue : IComparable<TValue>
        {
            IEnumerator<TObject> enumerator = array.GetEnumerator();

            enumerator.MoveNext();

            TValue result = mapFunction(enumerator.Current);

            foreach (TObject item in array)
            {
                TValue temp = mapFunction(item);

                if (result.CompareTo(temp) < 0)
                {
                    result = temp;
                }
            }

            return result;
        }

        #endregion

        #region Minimum

        public static double GetMin(Matrix values)
        {
            int width = values.NumberOfColumns;

            int height = values.NumberOfRows;

            double result = double.PositiveInfinity;

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (values[j, i] < result)
                    {
                        result = values[j, i];
                    }
                }
            }

            return result;
        }

        public static double GetMin(double[] values)
        {

            if (values.Length < 0)
            {
                throw new ZeroSizeArrayException();
            }

            double resultVlaue = values[0];

            for (int i = 1; i < values.Length; i++)
            {

                if (resultVlaue > values[i])
                    resultVlaue = values[i];

            }

            return resultVlaue;

        }

        public static int GetMin(int[] values)
        {

            if (values.Length < 0)
            {
                throw new ZeroSizeArrayException();
            }

            int resultVlaue = values[0];

            for (int i = 1; i < values.Length; i++)
            {

                if (resultVlaue > values[i])
                    resultVlaue = values[i];

            }

            return resultVlaue;

        }

        public static TValue GetMin<TObject, TValue>(IEnumerable<TObject> array, Func<TObject, TValue> mapFunction) where TValue : IComparable<TValue>
        {
            IEnumerator<TObject> enumerator = array.GetEnumerator();

            enumerator.MoveNext();

            TValue result = mapFunction(enumerator.Current);

            foreach (TObject item in array)
            {
                TValue temp = mapFunction(item);

                if (result.CompareTo(temp) > 0)
                {
                    result = temp;
                }
            }

            return result;
        }
        #endregion

        #region Median

        #endregion

        #region Mode

        #endregion

        #region Sum

        public static double CalculateSum(double[] values)
        {
            if (values.Length < 0)
            {
                throw new ZeroSizeArrayException();
            }

            double result = 0;

            foreach (double item in values)
            {
                result += item;
            }

            return result;
        }

        private static double CalculateSum(List<double> values)
        {
            if (values.Count < 0)
            {
                throw new ZeroSizeArrayException();
            }

            double result = 0;

            foreach (double item in values)
            {
                result += item;
            }

            return result;

        }

        public static double CalculateSum(Matrix values)
        {
            double result = 0;

            for (int i = 0; i < values.NumberOfRows; i++)
            {
                for (int j = 0; j < values.NumberOfColumns; j++)
                {
                    result += values[i, j];
                }
            }

            return result;
        }

        #endregion

        #region Mean

        public static double CalculateMean(double[] values)
        {
            if (values.Length < 0)
            {
                throw new ZeroSizeArrayException();
            }

            return Statistics.CalculateSum(values) / values.Length;
        }

        private static double CalculateMean(List<double> values)
        {
            if (values.Count < 0)
            {
                throw new ZeroSizeArrayException();
            }

            return Statistics.CalculateSum(values) / values.Count;
        }

        public static double CalculateMean(Matrix values)
        {
            if ((object)values == null)
            {
                throw new NotImplementedException();
            }

            return Statistics.CalculateSum(values) / (values.NumberOfColumns * values.NumberOfRows);
        }

        #endregion

        #region StandardDeviation & Variance

        public static double CalculateStandardDeviation(double[] values)
        {
            if (values.Length < 0)
            {
                throw new ZeroSizeArrayException();
            }

            return Math.Sqrt(Statistics.CalculateVariance(values));
        }

        public static double CalculateStandardDeviation(List<double> values)
        {
            if (values.Count < 0)
            {
                throw new ZeroSizeArrayException();
            }

            return Math.Sqrt(Statistics.CalculateVariance(values));
        }

        public static double CalculateStandardDeviation(Matrix values)
        {
            if (((object)values) == null)
            {
                throw new ZeroSizeArrayException();
            }

            return Math.Sqrt(Statistics.CalculateVariance(values));
        }

        public static double CalculateVariance(double[] values)
        {
            if (values.Length < 0)
            {
                throw new ZeroSizeArrayException();
            }

            double result = 0;

            double mean = Statistics.CalculateMean(values);

            foreach (double item in values)
            {
                result += (item - mean) * (item - mean);
            }

            return result / values.Length;
        }

        public static double CalculateVariance(List<double> values)
        {
            if (values.Count < 0)
            {
                throw new ZeroSizeArrayException();
            }

            double result = 0;

            double mean = Statistics.CalculateMean(values);

            foreach (double item in values)
            {
                result += (item - mean) * (item - mean);
            }

            return result / values.Count;
        }

        public static double CalculateVariance(Matrix values)
        {
            if (((object)values) == null)
            {
                throw new ZeroSizeArrayException();
            }

            double result = 0;

            double mean = Statistics.CalculateMean(values);

            for (int i = 0; i < values.NumberOfRows; i++)
            {
                for (int j = 0; j < values.NumberOfColumns; j++)
                {
                    result += (values[i, j] - mean) * (values[i, j] - mean);
                }
            }

            return result / (values.NumberOfColumns * values.NumberOfRows);
        }

        #endregion

        #region Covariance & Correlation


        public static double CalculateCovariance(double[] firstValues, double[] secondValues)
        {
            if (firstValues.Length < 0)
            {
                throw new ZeroSizeArrayException();
            }

            int length = firstValues.Length;

            if (length != secondValues.Length)
            {
                throw new NotImplementedException();
            }

            double firstMean = Statistics.CalculateMean(firstValues);

            double secondMean = Statistics.CalculateMean(secondValues);

            double result = 0;

            for (int i = 0; i < length; i++)
            {
                result += (firstValues[i] - firstMean) * (secondValues[i] - secondMean);
            }

            return Math.Sqrt(result / length);
        }

        public static double CalculateCovariance(Matrix firstValues, Matrix secondValues)
        {
            if (((object)firstValues) == null || ((object)secondValues) == null)
            {
                throw new ZeroSizeArrayException();
            }

            if (!Matrix.AreTheSameSize(firstValues, secondValues))
            {
                throw new NotImplementedException();
            }

            double result = 0;

            double firstMean = Statistics.CalculateMean(firstValues);

            double secondMean = Statistics.CalculateMean(secondValues);

            for (int i = 0; i < firstValues.NumberOfRows; i++)
            {
                for (int j = 0; j < firstValues.NumberOfColumns; j++)
                {
                    result += (firstValues[i, j] - firstMean) * (secondValues[i, j] - secondMean);
                }
            }

            return result / (firstValues.NumberOfColumns * firstValues.NumberOfRows);
        }

        public static Matrix CalculateVarianceCovariance(Matrix[] values)
        {
            int numberOfArrays = values.Length;

            if (numberOfArrays < 0)
            {
                throw new ZeroSizeArrayException();
            }

            for (int i = 0; i < numberOfArrays; i++)
            {
                if (!Matrix.AreTheSameSize(values[0], values[i]))
                {
                    throw new NotImplementedException();
                }
            }

            Matrix result = new Matrix(numberOfArrays, numberOfArrays);

            for (int i = 0; i < numberOfArrays; i++)
            {
                for (int j = 0; j < numberOfArrays; j++)
                {
                    if (i > j)
                    {
                        result[i, j] = result[j, i];
                    }
                    else if (i == j)
                    {
                        result[i, j] = CalculateVariance(values[i]);
                    }
                    else
                    {
                        result[i, j] = CalculateCovariance(values[i], values[j]);
                    }
                }
            }

            return result;
        }

        public static Matrix CalculateVarianceCovariance(double[][] values)
        {
            int numberOfArrays = values.Length;

            if (numberOfArrays < 0)
            {
                throw new ZeroSizeArrayException();
            }

            int arrayLength = values[0].Length;

            if (arrayLength < 0)
            {
                throw new ZeroSizeArrayException();
            }

            foreach (double[] item in values)
            {
                if (item.Length != arrayLength)
                {
                    throw new NotImplementedException();
                }
            }

            Matrix result = new Matrix(numberOfArrays, numberOfArrays);

            for (int i = 0; i < numberOfArrays; i++)
            {
                for (int j = 0; j < numberOfArrays; j++)
                {
                    if (i > j)
                    {
                        result[i, j] = result[j, i];
                    }
                    else if (i == j)
                    {
                        result[i, j] = CalculateVariance(values[i]);
                    }
                    else
                    {
                        result[i, j] = CalculateCovariance(values[i], values[j]);
                    }
                }
            }

            return result;
        }

        public static Matrix CalculateCorrelation(Matrix[] values)
        {
            int numberOfArrays = values.Length;

            if (numberOfArrays < 0)
            {
                throw new ZeroSizeArrayException();
            }

            for (int i = 0; i < numberOfArrays; i++)
            {
                if (!Matrix.AreTheSameSize(values[0], values[i]))
                {
                    throw new NotImplementedException();
                }
            }

            double[] variances = new double[numberOfArrays];

            for (int i = 0; i < numberOfArrays; i++)
            {
                variances[i] = Statistics.CalculateVariance(values[i]);
            }

            Matrix result = new Matrix(numberOfArrays, numberOfArrays);

            for (int i = 0; i < numberOfArrays; i++)
            {
                for (int j = 0; j < numberOfArrays; j++)
                {
                    if (i > j)
                    {
                        result[i, j] = result[j, i];
                    }
                    else if (i == j)
                    {
                        result[i, j] = 1 / variances[i];
                    }
                    else
                    {
                        result[i, j] = CalculateCovariance(values[i], values[j]) / (variances[i] * variances[j]);
                    }
                }
            }

            return result;
        }

        public static Matrix CalculateCorrelation(double[][] values)
        {
            int numberOfArrays = values.Length;

            if (numberOfArrays < 0)
            {
                throw new ZeroSizeArrayException();
            }

            int arrayLength = values[0].Length;

            if (arrayLength < 0)
            {
                throw new ZeroSizeArrayException();
            }

            foreach (double[] item in values)
            {
                if (item.Length != arrayLength)
                {
                    throw new NotImplementedException();
                }
            }

            double[] variances = new double[numberOfArrays];

            for (int i = 0; i < numberOfArrays; i++)
            {
                variances[i] = Statistics.CalculateVariance(values[i]);
            }

            Matrix result = new Matrix(numberOfArrays, numberOfArrays);

            for (int i = 0; i < numberOfArrays; i++)
            {
                for (int j = 0; j < numberOfArrays; j++)
                {
                    if (i > j)
                    {
                        result[i, j] = result[j, i];
                    }
                    else if (i == j)
                    {
                        result[i, j] = 1 / variances[i];
                    }
                    else
                    {
                        result[i, j] = CalculateCovariance(values[i], values[j]) / (variances[i] * variances[j]);
                    }
                }
            }

            return result;
        }

        #endregion

    }
}
