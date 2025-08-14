﻿using IRI.Maptor.Sta.Common.Primitives; 

namespace IRI.Maptor.Sta.ShapefileFormat.Indexing;

public struct ShpIndex
{
    //public int Offset { get; set; }

    //public int ContentLength { get; set; }
    public int RecordNumber { get; set; }

    public BoundingBox MinimumBoundingBox { get; set; }
}
