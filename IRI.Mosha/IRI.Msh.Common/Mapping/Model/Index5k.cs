using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Msh.Common.Mapping
{
    public class Index5k : IndexBase
    {
        public override double Height { get => GeodeticIndexes._5kHeight; }

        public override double Width { get => GeodeticIndexes._5kWidth; }

        public string ShortSheetNumber { get { return SheetNumber?.Split(' ').Last(); } }
    }
}
