using System;

namespace IRI.Maptor.Sta.ShapefileFormat.ShapeTypes.Abstractions;

interface IEsriPointsWithMeasure : IEsriSimplePoints
{
    double MaxMeasure { get; }
    double[] Measures { get; }
    double MinMeasure { get; }
}
