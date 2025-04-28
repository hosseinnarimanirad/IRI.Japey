using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace IRI.Msh.CoordinateSystem;

public enum SpatialReferenceType
{ 
    None,

    AlbersEqualAreaConic,
    CylindricalEqualArea,
    Geodetic,
    LambertConformalConic,
    Mercator,
    TransverseMercator,
    UTM,
    WebMercator,
}
  