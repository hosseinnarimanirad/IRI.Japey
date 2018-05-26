using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.ShapefileFormat.Indexing
{
    public struct ShpIndex
    {
        //public int Offset { get; set; }

        //public int ContentLength { get; set; }
        public int RecordNumber { get; set; }

        public IRI.Sta.Common.Primitives.BoundingBox MinimumBoundingBox { get; set; }
    }
}
