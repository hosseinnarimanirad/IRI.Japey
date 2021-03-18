using IRI.Ket.MachineLearning.Common;
using IRI.Msh.Algebra;
using IRI.Msh.Statistics.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Ket.MachineLearning.Regressions
{

    // David Cox in 1958
    // The logistic regression is a form of supervised learning
    public class LogisticRegression
    {
        // 0.1 - 0.3
        private readonly double _learningRate = 0.1;

        private readonly int _maxIteration = 5000;

        private double[] beta = null;

        // پارامترهای ضرایب
        public double[] Beta { get { return beta; } }


        // Mean and standard deviation of xs
        public List<BasicStatisticsInfo> xStatistics { get; set; }

        public LogisticRegression()
        {

        }

        public LogisticRegression(double[] beta, List<BasicStatisticsInfo> xStatistics)
        {
            this.beta = beta;

            this.xStatistics = xStatistics;
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


            // 1399.12.27
            // *******************************************************
            // پیش پردازش داده
            // به ارايه ایکس‌ها بایستی مقدار ۱ اضافه کرد
            // این گام باید قبل از نرمال‌سازی باشه چون به 
            // استتیستیک ستون یک هم نیاز هست بعدا برای 
            // پردیکت. در غیر این صورت بردارها هم‌ساز 
            // نخواهند شد.
            // *******************************************************
            var ones = Enumerable.Repeat(1.0, numberOfRow).ToArray();
            xValues.InsertColumn(0, ones);


            // 1399.12.20
            // *******************************************************
            // ذخیره‌سازی پارامترها برای استفاده 
            // هنگام تخمین
            // *******************************************************
            xStatistics = xValues.GetStatisticsByColumns();

            // *******************************************************
            // پیش پردازش داده
            // نرمال کردن داده‌ها
            // *******************************************************
            xValues = Normalization.NormalizeColumnsUsingZScore(xValues);


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

            // *******************************************************
            // پیش پردازش داده
            // نرمال کردن داده‌ها
            // *******************************************************
            for (int i = 0; i < xValues.Length; i++)
            {
                xValues[i] = Normalization.NormalizeUsingZScore(xValues[i], xStatistics[i].Mean, xStatistics[i].StandardDeviation);
            }

            return LogisticRegressionHelper.CalculateLogisticFunction(xValues, Beta);
        }

        public void Serialize(string fileName)
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(this);

            System.IO.File.WriteAllText(fileName, jsonString);
        }

        public static LogisticRegression Deserialize(string fileName)
        {
            var jsonString = System.IO.File.ReadAllText(fileName);

            return Newtonsoft.Json.JsonConvert.DeserializeObject<LogisticRegression>(jsonString);
        }
    }
}
