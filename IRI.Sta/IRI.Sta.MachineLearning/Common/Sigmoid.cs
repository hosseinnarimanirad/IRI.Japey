using IRI.Sta.Mathematics;
using IRI.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Sta.MachineLearning;

public class Sigmoid
{
    public static double CalculateSigmoid(double zValue)
    {
        return 1.0 / (1.0 + Math.Exp(-zValue));
    }

}
