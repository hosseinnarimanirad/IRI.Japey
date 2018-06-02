using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Jab.Common.Extensions
{
    public static class SpatialBaseExtensions
    {
        public static System.Windows.Point AsWpfPoint(this IRI.Msh.Common.Primitives.IPoint value)
        {
            return new System.Windows.Point(value.X, value.Y);
        }
    }
}
