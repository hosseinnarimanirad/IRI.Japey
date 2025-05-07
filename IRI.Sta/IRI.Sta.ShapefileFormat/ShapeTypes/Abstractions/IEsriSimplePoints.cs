using System;
using IRI.Sta.ShapefileFormat.EsriType;

namespace IRI.Sta.ShapefileFormat.ShapeTypes.Abstractions;

public interface IEsriSimplePoints : IEsriShape
{
    EsriPoint[] Points { get; }

    int[] Parts { get; }

    int NumberOfPoints { get; }

    int NumberOfParts { get; }

    EsriPoint[] GetPart(int partNo);
}
