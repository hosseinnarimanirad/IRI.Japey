using IRI.Sta.Common.IO.OgcSFA;
using System;
namespace IRI.Sta.Ogc.SFA;

interface IOgcLineString
{
    WkbByteOrder ByteOrder { get; }
    IOgcPointCollection Points { get; }
    byte[] ToWkb();
    WkbGeometryType Type { get; }
}
