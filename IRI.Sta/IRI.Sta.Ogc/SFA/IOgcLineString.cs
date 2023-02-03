using IRI.Msh.Common.Ogc;
using System;
namespace IRI.Standards.OGC.SFA
{
    interface IOgcLineString
    {
        WkbByteOrder ByteOrder { get; }
        IOgcPointCollection Points { get; }
        byte[] ToWkb();
        WkbGeometryType Type { get; }
    }
}
