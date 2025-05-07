using System;
namespace IRI.Sta.ShapefileFormat.ShapeTypes.Abstractions;

interface IEsriPointsWithZ : IEsriPointsWithMeasure
{
    double MaxZ { get; }
    double MinZ { get; }
    double[] ZValues { get; }
}
