using System;
namespace IRI.Sta.ShapefileFormat.EsriType
{
    interface IEsriPointsWithZ : IEsriPointsWithMeasure
    {
        double MaxZ { get; }
        double MinZ { get; }
        double[] ZValues { get; }
    }
}
