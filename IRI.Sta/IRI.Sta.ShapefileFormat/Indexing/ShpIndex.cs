using IRI.Sta.Common.Primitives; 

namespace IRI.Sta.ShapefileFormat.Indexing;

public struct ShpIndex
{
    //public int Offset { get; set; }

    //public int ContentLength { get; set; }
    public int RecordNumber { get; set; }

    public BoundingBox MinimumBoundingBox { get; set; }
}
