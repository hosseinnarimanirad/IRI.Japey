using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Msh.Common.Analysis
{
    public class SimplificationParamters
    {
        public double? DistanceThreshold { get; set; }

        public double? AreaThreshold { get; set; }

        public double? AngleThreshold { get; set; }

        public bool Retain3Points { get; set; } = false;

        //public bool? IsRing { get; set; }

        // used in Lang algorithm
        public int? LookAhead { get; set; }

        public double? AverageLatitude { get; set; }
    }
}
