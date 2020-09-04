using IRI.Msh.Common.Analysis;
using IRI.Msh.Common.Primitives;
using IRI.Msh.CoordinateSystem.MapProjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Msh.Common.Primitives
{
    public static class GeometryExtensions
    {

        public static List<Geometry> Project(this List<Geometry> values, SrsBase sourceSrs, SrsBase targetSrs)
        {
            List<Geometry> result = new List<Geometry>(values.Count);

            if (sourceSrs.Ellipsoid.AreTheSame(targetSrs.Ellipsoid))
            {
                for (int i = 0; i < values.Count; i++)
                {
                    var c1 = values[i].Transform(p => sourceSrs.ToGeodetic(p), SridHelper.GeodeticWGS84);

                    result.Add(c1.Transform(p => targetSrs.FromGeodetic(p), targetSrs.Srid));
                }
            }
            else
            {
                for (int i = 0; i < values.Count; i++)
                {
                    var c1 = values[i].Transform(p => sourceSrs.ToGeodetic(p), SridHelper.GeodeticWGS84);

                    result.Add(c1.Transform(p => targetSrs.FromGeodetic(p, sourceSrs.Ellipsoid), targetSrs.Srid));
                }
            }

            return result;
        }
         
    }
}
