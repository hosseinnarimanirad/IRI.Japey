using System;
namespace IRI.Sta.ShapefileFormat.EsriType
{
    interface IEsriPointsWithMeasure : IEsriSimplePoints
    {
        double MaxMeasure { get; }
        double[] Measures { get; }
        double MinMeasure { get; }
    }
}
