using IRI.Msh.Common.Primitives;
using IRI.Ket.SpatialExtensions;
using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Jab.Common.Model.Spatialable
{
    public static class EnvelopeMarkupLabelTripleExtension
    {
        public static BoundingBox GetBoundingBox(this EnvelopeMarkupLabelTriple value, int srid)
        {
            var geometry = SqlGeometry.STGeomFromWKB(new System.Data.SqlTypes.SqlBytes(value.GetEnvelopeWkbWm()), srid);

            return geometry.GetBoundingBox();
        }
    }
}
