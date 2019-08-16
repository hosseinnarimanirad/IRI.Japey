using System;
namespace IRI.Standards.OGC.SFA
{
    interface IOgcLineString
    {
        WkbByteOrder ByteOrder { get; }
        IPointCollection Points { get; }
        byte[] ToWkb();
        WkbGeometryType Type { get; }
    }
}
