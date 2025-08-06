using IRI.Maptor.Sta.Mathematics;
using IRI.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Maptor.Sta.MachineLearning;

public class Sigmoid
{
    public static double CalculateSigmoid(double zValue)
    {
        return 1.0 / (1.0 + Math.Exp(-zValue));
    }

}
