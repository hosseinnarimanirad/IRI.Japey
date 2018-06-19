using IRI.Msh.Common.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Jab.Common
{
    public class GeometryEventArgs : EventArgs
    {
        public Geometry Geometry { get; set; }

        public GeometryEventArgs(Geometry geometry)
        {
            this.Geometry = geometry;
        }
    }
}
