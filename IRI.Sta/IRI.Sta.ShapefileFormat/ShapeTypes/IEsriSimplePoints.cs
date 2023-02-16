using System;


namespace IRI.Sta.ShapefileFormat.EsriType
{
    public interface IEsriSimplePoints : IEsriShape
    {
        EsriPoint[] Points { get; }

        int[] Parts { get; }

        int NumberOfPoints { get; }

        int NumberOfParts { get; }

        EsriPoint[] GetPart(int partNo);
    }
}
