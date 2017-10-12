using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Ham.CoordinateSystem.MapProjection
{
    public enum MapProjectionType
    {
        None,

        AlbersEqualAreaConic,
        AzimuthalEquidistant,
        CylindricalEqualArea,
        LambertConformalConic,
        Mercator,
        TransverseMercator,
        UTM,
        WebMercator,
    }
}
