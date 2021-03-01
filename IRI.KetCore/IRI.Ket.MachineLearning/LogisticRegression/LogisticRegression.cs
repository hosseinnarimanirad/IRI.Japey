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

            while (iteration < _maxIteration)
            {
                double[] yPredicted = new double[numberOfRow];

                for (int i = 0; i < numberOfRow; i++)
                {
                    yPredicted[i] = Sigmoid.CalculateLogisticFunction(xValues.GetRow(i), beta);

                    //var error = 
                }

                var loss = Sigmoid.CalculateLossByGradientDescent(yValues, yPredicted);

                // Calculate the gradient

            }

        }
    }
}
