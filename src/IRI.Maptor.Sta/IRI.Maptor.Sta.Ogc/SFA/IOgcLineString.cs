using IRI.Maptor.Sta.Common.Enums;
using System;
namespace IRI.Maptor.Sta.Ogc.SFA;

interface IOgcLineString
{
    WkbByteOrder ByteOrder { get; }
    IOgcPointCollection Points { get; }
    byte[] ToWkb();
    WkbGeometryType Type { get; }
}
