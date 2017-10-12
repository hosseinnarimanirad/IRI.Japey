using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Ket.ShapefileFormat.EsriType
{
    public interface IShapeCollection : IEnumerable<IShape>
    {
        MainFileHeader MainHeader { get; }

        IShape this[int index] { get; set; }

        int Count { get; }

        string AsKml();        
    }
}
