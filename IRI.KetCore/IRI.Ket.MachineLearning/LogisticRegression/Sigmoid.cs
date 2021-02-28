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

        // باید دقت داشت که اولین مقدار ایکس
        // برابر است با عدد یک به این ترتیب
        // طول بردار ظرایب و ایکس یکی می‌شود
        // صفحه ۹۵ کتاب زیر
        // Introduction to Algorithms for Data Mining and Machine Learning, Academic Press, 2019
        public static double CalculateLogesticFunction(double[] x, double[] weights)
        {
            var zValue = CalculateLogit(x, weights);

            if (zValue == null)
            {
                throw new NotImplementedException();
            }

            return CalculateSigmoid(zValue.Value);
        }

        public static double CalculateSigmoid(double zValue)
        {
            return 1.0 / (1.0 + Math.Exp(-zValue));
        }

        // https://philippmuens.com/logistic-regression-from-scratch
        public static double CalculateNegLogLikelihood(double y, double[] x, double[] weights)
        {
            var yPredicted = CalculateLogesticFunction(x, weights);

            return CalculateNegLogLikelihood(y, yPredicted);
        }

        public static double CalculateNegLogLikelihood(double y, double yPredicted)
        {
            return -1 * (y * Math.Log(yPredicted) + (1 - y) * Math.Log(1 - yPredicted));
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
