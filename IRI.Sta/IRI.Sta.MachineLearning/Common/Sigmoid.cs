﻿using IRI.Msh.Algebra;
using IRI.Msh.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Ket.MachineLearning.LogisticRegression
{
    public class Sigmoid
    {
        public static double CalculateSigmoid(double zValue)
        {
            return 1.0 / (1.0 + Math.Exp(-zValue));
        }

    }
}