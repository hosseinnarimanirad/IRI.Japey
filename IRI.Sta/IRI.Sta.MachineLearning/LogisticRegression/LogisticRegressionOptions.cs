using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Sta.MachineLearning
{
    public class LogisticRegressionOptions
    {
        public RegularizationMethods RegularizationMethod { get; set; }

        public LogisticRegressionOptions()
        {
            this.RegularizationMethod = RegularizationMethods.None;
        }
    }
}
