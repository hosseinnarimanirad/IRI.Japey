using System;
namespace IRI.Ket.ShapefileFormat.EsriType
{
    interface IPointsWithMeasure : ISimplePoints
    {
        double MaxMeasure { get; }
        double[] Measures { get; }
        double MinMeasure { get; }
    }
}
