using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Msh.Common.Mapping
{
    public class Index5k : IndexBase
    {
        public override double Height { get => MapIndexes._5kHeight; }

        public override double Width { get => MapIndexes._5kWidth; }

        public string ShortSheetNumber { get { return SheetNumber?.Split(' ').Last(); } }
    }
}
