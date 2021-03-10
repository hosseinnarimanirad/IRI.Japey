using System;
namespace IRI.Ket.ShapefileFormat.EsriType
{
    interface IEsriPointsWithMeasure : IEsriSimplePoints
    {
        double MaxMeasure { get; }
        double[] Measures { get; }
        double MinMeasure { get; }
    }
}
