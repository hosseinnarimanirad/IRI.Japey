using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Msh.Statistics.Model
{
    public class StatisticsSummary
    {
        public double Min { get; set; }

        public double FirstQuartile { get; set; }

        //actually this is the second quartile
        public double Median { get; set; }

        public double ThirdQuartile { get; set; }

        public double Max { get; set; }

        public double Mean { get; set; }
    }
}
