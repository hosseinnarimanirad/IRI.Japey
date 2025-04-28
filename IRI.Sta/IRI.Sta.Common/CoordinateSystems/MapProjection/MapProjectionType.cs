using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Msh.CoordinateSystem.MapProjection;

public enum MapProjectionType
{
    None = 1,

    AlbersEqualAreaConic = 2,
    AzimuthalEquidistant = 3,
    CylindricalEqualArea = 4,
    LambertConformalConic = 5,
    Mercator = 6,
    TransverseMercator = 7,
    UTM = 8,
    WebMercator = 9,
}
