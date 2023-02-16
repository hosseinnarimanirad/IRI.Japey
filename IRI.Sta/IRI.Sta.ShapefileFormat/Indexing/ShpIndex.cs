using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Sta.ShapefileFormat.Indexing
{
    public struct ShpIndex
    {
        //public int Offset { get; set; }

        //public int ContentLength { get; set; }
        public int RecordNumber { get; set; }

        public IRI.Msh.Common.Primitives.BoundingBox MinimumBoundingBox { get; set; }
    }
}
