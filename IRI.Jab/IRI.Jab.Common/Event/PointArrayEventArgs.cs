using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Jab.Common
{
    public class PointArrayEventArgs : EventArgs
    {
        public List<IRI.Msh.Common.Primitives.Point> Coordinates { get; set; }

        public bool IsClosed { get; set; }

        public PointArrayEventArgs(List<IRI.Msh.Common.Primitives.Point> coordinates, bool isClosed)
        {
            this.Coordinates = coordinates;

            this.IsClosed = isClosed;
        }
    }
}
