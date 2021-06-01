using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Msh.Statistics
{
    public class BasicStatisticsInfo
    {
        public double Mean { get; set; }

        public double StandardDeviation { get; set; }

        public BasicStatisticsInfo()
        {

        }

        public BasicStatisticsInfo(double[] values)
        {
            this.Mean = IRI.Msh.Statistics.Statistics.CalculateMean(values);

            this.StandardDeviation = Statistics.CalculateStandardDeviation(values);
        }
    }
}
