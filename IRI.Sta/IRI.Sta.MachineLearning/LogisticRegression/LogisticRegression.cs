using IRI.Msh.Algebra;
using IRI.Msh.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Sta.MachineLearning
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


        LogisticRegressionOptions _options;

        // Mean and standard deviation of xs
        public List<BasicStatisticsInfo> xStatistics { get; set; }

        public LogisticRegression(LogisticRegressionOptions options)
        {
            _options = options;
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
            //beta = Enumerable.Range(1, numberOfParameters).Select(i => (double)1.0).ToArray();
            beta = Enumerable.Repeat(1.0, numberOfParameters).ToArray();


            // 1399.12.20
            // *******************************************************
            // ذخیره‌سازی پارامترها برای استفاده 
            // هنگام تخمین
            // *******************************************************
            xStatistics = xValues.GetStatisticsByColumns();

            //// 1399.12.27
            //// *******************************************************
            //// پیش پردازش داده
            //// به ارايه ایکس‌ها بایستی مقدار ۱ اضافه کرد
            //// این گام باید قبل از نرمال‌سازی باشه چون به 
            //// استتیستیک ستون یک هم نیاز هست بعدا برای 
            //// پردیکت. در غیر این صورت بردارها هم‌ساز 
            //// نخواهند شد.
            //// *******************************************************
            //var ones = Enumerable.Repeat(1.0, numberOfRow).ToArray();
            //xValues.InsertColumn(0, ones);

            // 1399.12.20
            // *******************************************************
            // ذخیره‌سازی پارامترها برای استفاده 
            // هنگام تخمین
            // *******************************************************
            //xStatistics = xValues.GetStatisticsByColumns();

            // *******************************************************
            // پیش پردازش داده
            // نرمال کردن داده‌ها
            // *******************************************************
            xValues = Normalization.NormalizeColumnsUsingZScore(xValues, this._options.VarianceCalculationMode);

            //System.Diagnostics.Debug.WriteLine(string.Join(",", xValues.GetColumn(0)));
            //System.Diagnostics.Debug.WriteLine(string.Join(",", xValues.GetColumn(1)));
            //System.Diagnostics.Debug.WriteLine(string.Join(",", xValues.GetColumn(2)));

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

            xStatistics.Insert(0, new BasicStatisticsInfo() { Mean = 1, StandardDeviation = 0 });


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


                    // 1400.03.13
                    //if (this._options.RegularizationMethod == RegularizationMethods.L2)
                    //{
                    //    //1400.03.12
                    //    // regularization
                    //    var lambda = 1.0;
                    //    double sigmaBeta = 0;
                    //    for (int j = 0; j < numberOfParameters; j++)
                    //    {
                    //        sigmaBeta += beta[j];
                    //    }

                    //    grad[j] += lambda
                    //}

                }

                // 1400.03.13
                // 1400.03.15
                // regularization strength
                var lambda = 1.0;
                //var sigmaBeta = this._options.RegularizationMethod == RegularizationMethods.None ? 0 : beta.Sum();

                for (int i = 0; i < numberOfParameters; i++)
                {
                    double regularizationComponent = 0;

                    if (i != 0)
                    {
                        switch (this._options.RegularizationMethod)
                        {
                            case RegularizationMethods.L1:
                                regularizationComponent = lambda / (beta.Length);
                                break;

                            case RegularizationMethods.L2:
                                regularizationComponent = lambda / beta.Length * beta[i];
                                break;

                            case RegularizationMethods.None:
                            default:
                                break;
                        }
                    }

                    grad[i] = 1.0 / beta.Length * grad[i] + regularizationComponent;

                    beta[i] = beta[i] - _learningRate * grad[i];
                }

                iteration++;
            }

            System.Diagnostics.Debug.WriteLine(string.Join(",", beta));

        }

        public double? Predict(List<double> xValues)
        {
            if (beta == null || xValues == null)
                return null;

            // 1400.03.14
            xValues.Insert(0, 1);

            // *******************************************************
            // پیش پردازش داده
            // نرمال کردن داده‌ها
            //
            // 1400.03.10
            // ستون اول چون مقادیر ۱ هستند
            // نیازی به نرمال کردن ندارند
            // *******************************************************
            for (int i = 1; i < xValues.Count; i++)
            {
                xValues[i] = Normalization.NormalizeUsingZScore(xValues[i], xStatistics[i].Mean, xStatistics[i].StandardDeviation);
            }

            return LogisticRegressionHelper.CalculateLogisticFunction(xValues.ToArray(), Beta);
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
