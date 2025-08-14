using IRI.Maptor.Sta.Common.Enums;
using System;

namespace IRI.Maptor.Sta.Ogc.SFA;

public interface IOgcGeometry
{
    WkbByteOrder ByteOrder { get; }

    byte[] ToWkb();

    WkbGeometryType Type { get; }
}
