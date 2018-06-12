using IRI.Msh.Common.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Msh.Common.Mapping
{
    public class Index100k : IndexBase
    {
        public override double Height { get => MapIndexes._100kSize; }

        public override double Width { get => MapIndexes._100kSize; }

        public string BlockName { get; set; }

        public string BlockNumber { get; set; }

        //public BoundingBox GetBoundingBox()
        //{
        //    return new BoundingBox(MinLongitude, MinLatitude, MinLongitude + MapIndexes._100kSize, MinLatitude + MapIndexes._100kSize);
        //}
    }
}
