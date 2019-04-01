using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Msh.Common.Mapping
{
    public class Index50k : IndexBase
    {
        public override double Height { get => GeodeticIndexes._50kSize; }

        public override double Width { get => GeodeticIndexes._50kSize; }

        public string ShortSheetNumber { get { return SheetNumber?.Split(' ').Last(); } }
    }
}
