using System;
using IRI.Maptor.Sta.Common.Abstrations;
using IRI.Maptor.Sta.ShapefileFormat.EsriType;

namespace IRI.Maptor.Sta.ShapefileFormat.ShapeTypes.Abstractions;

public interface IEsriSimplePoints : IEsriShape
{
    EsriPoint[] Points { get; }

    int[] Parts { get; }

    int NumberOfPoints { get; }

    int NumberOfParts { get; }

    EsriPoint[] GetPart(int partNo);
}
