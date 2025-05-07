using System;
using System.Collections.Generic;
using System.Text;
using IRI.Sta.ShapefileFormat.ShapeTypes.Abstractions;

namespace IRI.Sta.ShapefileFormat.EsriType;

public interface IEsriShapeCollection : IEnumerable<IEsriShape>
{
    MainFileHeader MainHeader { get; }

    IEsriShape this[int index] { get; set; }

    int Count { get; }

    string AsKml();        
}
