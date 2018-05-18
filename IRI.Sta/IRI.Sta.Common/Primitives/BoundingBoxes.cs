using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Ham.SpatialBase.Primitives
{
    public static class BoundingBoxes
    {
        public static BoundingBox IranMercatorBoundingBox
        {
            get
            {
                return new BoundingBox(4840000, 2800000, 7080000, 4900000);
            }
        }
    }
}
