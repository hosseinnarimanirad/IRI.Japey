using IRI.Msh.Common.Ogc;
using System;
namespace IRI.Sta.Ogc.SFA
{
    interface IOgcLineString
    {
        WkbByteOrder ByteOrder { get; }
        IOgcPointCollection Points { get; }
        byte[] ToWkb();
        WkbGeometryType Type { get; }
    }
}
