using System;
namespace IRI.Ket.ShapefileFormat.EsriType
{
    interface IEsriPointsWithZ : IEsriPointsWithMeasure
    {
        double MaxZ { get; }
        double MinZ { get; }
        double[] ZValues { get; }
    }
}
