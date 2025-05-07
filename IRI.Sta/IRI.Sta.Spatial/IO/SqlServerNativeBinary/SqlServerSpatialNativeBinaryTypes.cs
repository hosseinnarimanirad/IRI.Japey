using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Sta.Spatial.Primitives.SqlServerNativeBinary;

public enum SqlServerSpatialNativeBinaryTypes : byte
{
    Point = 12,
    PointZ = 13,
    PointM = 14,
    PointZM = 15,

    MultiPoint = 4,
    MultiPointZ = 5,
    MultiPointM = 6,
    MultiPointZM = 7,
}
