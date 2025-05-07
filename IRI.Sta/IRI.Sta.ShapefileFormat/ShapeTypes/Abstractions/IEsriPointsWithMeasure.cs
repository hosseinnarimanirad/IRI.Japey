using System;

namespace IRI.Sta.ShapefileFormat.ShapeTypes.Abstractions;

interface IEsriPointsWithMeasure : IEsriSimplePoints
{
    double MaxMeasure { get; }
    double[] Measures { get; }
    double MinMeasure { get; }
}
