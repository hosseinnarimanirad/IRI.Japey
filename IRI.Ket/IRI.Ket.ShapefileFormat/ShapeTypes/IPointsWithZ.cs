using System;
namespace IRI.Ket.ShapefileFormat.EsriType
{
    interface IPointsWithZ : IPointsWithMeasure
    {
        double MaxZ { get; }
        double MinZ { get; }
        double[] ZValues { get; }
    }
}
