using System;
namespace IRI.Maptor.Sta.ShapefileFormat.ShapeTypes.Abstractions;

interface IEsriPointsWithZ : IEsriPointsWithMeasure
{
    double MaxZ { get; }
    double MinZ { get; }
    double[] ZValues { get; }
}
