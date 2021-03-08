using IRI.Ket.MachineLearning.Common;
using IRI.Msh.Algebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Ket.MachineLearning.LogisticRegression
{

    // David Cox in 1958
    // The logistic regression is a form of supervised learning
    public class LogisticRegression
    {
        // 0.1 - 0.3
        private readonly double _learningRate = 0.1;

        private readonly int _maxIteration = 5000;

        private double[] beta = null;

        public double[] Beta { get { return beta; } }

        public LogisticRegression()
        {

        }

        public void Fit(Matrix xValues, double[] yValues)
        {
            // x: n*p
            // y: n*1

            var numberOfRow = xValues.NumberOfRows;
            var numberOfParameters = xValues.NumberOfColumns + 1;

            int iteration = 0;

            // *******************************************************
            // تخمین اولیه از ضرایب چند جمله‌ای
            // *******************************************************
            beta = Enumerable.Range(1, numberOfParameters).Select(i => (double)i).ToArray();


            // *******************************************************
            // پیش پردازش داده
            // نرمال کردن داده‌ها
            // *******************************************************
            xValues = Normalization.NormalizeColumnsUsingZScore(xValues);

            // *******************************************************
            // پیش پردازش داده
            // به ارايه ایکس‌ها بایستی مقدار ۱ اضافه کرد
            // *******************************************************
            var ones = Enumerable.Repeat(1.0, numberOfRow).ToArray();
            xValues.InsertColumn(0, ones);


            while (iteration < _maxIteration)
            {
                double[] yPredicted = new double[numberOfRow];

                double[] grad = new double[numberOfParameters];

                for (int i = 0; i < numberOfRow; i++)
                {
                    var xs = xValues.GetRow(i);

                    yPredicted[i] = LogisticRegressionHelper.CalculateLogisticFunction(xs, beta);

                    // sigma [(yPredicate-y)*x]
                    var error = yPredicted[i] - yValues[i];

                    for (int xi = 0; xi < xs.Length; xi++)
                    {
                        grad[xi] += error * xs[xi];
                    }
                }

                for (int i = 0; i < numberOfParameters; i++)
                {
                    grad[i] = 1.0 / beta.Length * grad[i];

                    beta[i] = beta[i] - _learningRate * grad[i];
                }

                //var loss = Sigmoid.CalculateLossByGradientDescent(yValues, yPredicted);

                iteration++;
            }

            System.Diagnostics.Debug.WriteLine(string.Join(",", beta));

        }

        public double? Predict(double[] xValues)
        {
            if (beta == null)
                return null;

            return LogisticRegressionHelper.CalculateLogisticFunction(xValues, Beta);
        }
    }
}
