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

        public LogisticRegression()
        {

        }



        public void Learn(Matrix xValues, double[] yValues)
        {
            // x: n*p
            // y: n*1

            var numberOfRow = xValues.NumberOfRows;
            var numberOfParameters = xValues.NumberOfColumns;

            int iteration = 0;

            // تخمین اولیه از ضرایب چند جمله‌ای
            double[] beta = Enumerable.Range(1, numberOfParameters + 1).Select(i => (double)i).ToArray();

            // به ارايه ایکس‌ها بایستی مقدار ۱ اضافه کرد
            var ones = Enumerable.Repeat(1.0, numberOfRow).ToArray();
            xValues.InsertColumn(0, ones);

            // نرمال کردن داده‌ها
            xValues = Sigmoid.NormalizeUsingZScore(xValues);

            while (iteration < _maxIteration)
            {
                double[] yPredicted = new double[numberOfRow];

                double[] grad = new double[beta.Length];

                for (int i = 0; i < numberOfRow; i++)
                {
                    var row = xValues.GetRow(i);

                    yPredicted[i] = Sigmoid.CalculateLogisticFunction(row, beta);

                    var error = yPredicted[i] - yValues[i];

                    for (int xi = 0; xi < row.Length; xi++)
                    {
                        grad[xi] += error * row[xi];
                    }
                }

                for (int i = 0; i < grad.Length; i++)
                {
                    grad[i] = 1.0 / beta.Length * grad[i];
                }

                //var loss = Sigmoid.CalculateLossByGradientDescent(yValues, yPredicted);

                for (int i = 0; i < beta.Length; i++)
                {
                    beta[i] = beta[i] - _learningRate * grad[i];
                }
            }

        }
    }
}
