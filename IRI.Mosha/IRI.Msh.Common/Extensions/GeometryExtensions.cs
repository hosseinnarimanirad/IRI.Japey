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

        public static List<Geometry<T>> Project<T>(this List<Geometry<T>> values, SrsBase sourceSrs, SrsBase targetSrs) where T : IPoint, new()
        {
            List<Geometry<T>> result = new List<Geometry<T>>(values.Count);

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
