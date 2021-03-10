// besmellahe rahmane rahim
// Allahomma ajjel le-valiyek al-faraj

using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Ket.ShapefileFormat.EsriType
{
    public enum EsriShapeType : int
    {
        NullShape = 0,
        EsriPoint = 1,
        EsriPolyLine = 3,
        EsriPolygon = 5,
        EsriMultiPoint = 8,
        EsriPointZ = 11,
        EsriPolyLineZ = 13,
        EsriPolygonZ = 15,
        EsriMultiPointZ = 18,
        EsriPointM = 21,
        EsriPolyLineM = 23,
        EsriPolygonM = 25,
        EsriMultiPointM = 28,
        EsriMultiPatch = 31
    }
}
