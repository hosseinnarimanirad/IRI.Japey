using IRI.Msh.Algebra;
using IRI.Msh.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Ket.MachineLearning.LogisticRegression
{
    public class Sigmoid
    {
        // logit function
        // f(x) = b_0 + b_1 * x_1 + b_2 * x_2 + … + b_n * x_n
        public static double? CalculateLogit(double[] x, double[] weights)
        {
            return x.DotProduct(weights);
        }

        public static double CalculateSigmoid(double zValue)
        {
            return 1.0 / (1.0 + Math.Exp(-zValue));
        }

        // باید دقت داشت که اولین مقدار ایکس
        // برابر است با عدد یک به این ترتیب
        // طول بردار ظرایب و ایکس یکی می‌شود
        // صفحه ۹۵ کتاب زیر
        // Introduction to Algorithms for Data Mining and Machine Learning, Academic Press, 2019
        public static double CalculateLogisticFunction(double[] x, double[] weights)
        {
            var zValue = CalculateLogit(x, weights);

            if (zValue == null)
            {
                throw new NotImplementedException("Sigmoid > CalculateLogisticFunction");
            }

            return CalculateSigmoid(zValue.Value);
        }

        public static double CalculateNegLogLikelihood(double y, double yPredicted)
        {
            return -1 * (y * Math.Log(yPredicted) + (1 - y) * Math.Log(1 - yPredicted));
        }

        // link: https://philippmuens.com/logistic-regression-from-scratch
        public static double CalculateNegLogLikelihood(double y, double[] x, double[] weights)
        {
            var yPredicted = CalculateLogisticFunction(x, weights);

            return CalculateNegLogLikelihood(y, yPredicted);
        }

        public static double CalculateNegLogLikelihood(double[] y, double[][] x, double[] weights)
        {
            double length = y.Length;

            double result = 0;

            for (int i = 0; i < length; i++)
            {
                result += CalculateNegLogLikelihood(y[i], x[i], weights);
            }

            return 1.0 / length * result;
        }

        public static double CalculateLossByGradientDescent(double[] y, double[] yPredicted)
        {
            if (y.Length != yPredicted.Length)
            {
                throw new NotImplementedException("Sigmoid > ErrorFunctionByGradientDescent");
            }

            var length = y.Length;

            double sum = 0.0;

            for (int i = 0; i < y.Length; i++)
            {
                sum += CalculateNegLogLikelihood(y[i], yPredicted[i]);
            }

            return 1.0 / length * sum;
        }


        public static Matrix NormalizeUsingZScore(Matrix values)
        {
            if (values == null)
                return null;

            int numberOfColumns = values.NumberOfColumns;

            Matrix result = new Matrix(values.NumberOfRows, values.NumberOfColumns);

            for (int i = 0; i < numberOfColumns; i++)
            {
                var columnValues = values.GetColumn(i);

                result.SetColumn(i, NormalizeUsingZScore(columnValues));
            }

            return result;
        }

        public static double[] NormalizeUsingZScore(double[] values)
        {
            if (values.IsNullOrEmpty())
            {
                return null;
            }

            double[] result = new double[values.Length];

            values.CopyTo(result, 0);

            var mean = IRI.Msh.Statistics.Statistics.CalculateMean(values);

            var std = IRI.Msh.Statistics.Statistics.CalculateStandardDeviation(values);

            // 1399.12.12
            // to prevent divide by zero
            if (std > 0)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    result[i] = (values[i] - mean) / std;
                }
            }

            return result;
        }

        //// nabla
        //// nabla or gradient for log-likelihood
        //public static double CalculateGradient(double[][] x, double[] y)
        //{
        //    double result = 0;

        //    for (int i = 0; i < y.Length; i++)
        //    {

        //    }
        //}


        //public static double Cal(double[][] x, double[] y, double[] beta)
        //{
        //    double[] beta = new double[x[0].Length + 1];

        //    double[] result = new double[y.Length];

        //    for (int i = 0; i < y.Length; i++)
        //    {

        //        for (int j = 0; j < x[0].Length; j++)
        //        {

        //        }
        //        x[i] * (1 / (1 + CalculateSigmoid(x[i], beta)))
        //    }
        //}

    }
}
