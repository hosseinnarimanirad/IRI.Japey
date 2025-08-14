using System;
using System.Collections.Generic;
using System.Text;
using IRI.Maptor.Sta.ShapefileFormat.ShapeTypes.Abstractions;

namespace IRI.Maptor.Sta.ShapefileFormat.EsriType;

public interface IEsriShapeCollection : IEnumerable<IEsriShape>
{
    MainFileHeader MainHeader { get; }

    IEsriShape this[int index] { get; set; }

    int Count { get; }

    string AsKml();        
}
