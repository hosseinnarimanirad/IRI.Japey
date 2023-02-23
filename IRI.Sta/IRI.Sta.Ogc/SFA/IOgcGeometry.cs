using IRI.Msh.Common.Ogc;
using System;

namespace IRI.Sta.Ogc.SFA
{
    public interface IOgcGeometry
    {
        WkbByteOrder ByteOrder { get; }

        byte[] ToWkb();

        WkbGeometryType Type { get; }
    }
}
