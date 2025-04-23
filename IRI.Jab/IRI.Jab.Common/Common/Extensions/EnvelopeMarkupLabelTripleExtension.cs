//using IRI.Msh.Common.Primitives;
//using IRI.Extensions;
//using Microsoft.SqlServer.Types;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Data.SqlTypes;

//namespace IRI.Jab.Common.Model.Spatialable
//{
//    public static class EnvelopeMarkupLabelTripleExtension
//    {
//        public static BoundingBox GetBoundingBox(this EnvelopeMarkupLabelTriple value, int srid)
//        {
//            var geometry = SqlGeometry.STGeomFromWKB(new SqlBytes(value.GetEnvelopeWkbWm()), srid);
            
//            return geometry.GetBoundingBox();
//        }
//    }
//}
