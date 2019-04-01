using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Msh.Common.Mapping
{
    public class Index10k : IndexBase
    {
        public override double Height { get => GeodeticIndexes._10kHeight; }

        public override double Width { get => GeodeticIndexes._10kWidth; }

        public string ShortSheetNumber { get { return SheetNumber?.Split(' ').Last(); } }
    }
}
