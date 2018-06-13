﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Msh.Common.Mapping
{
    public class Index25k : IndexBase
    {
        public override double Height { get => MapIndexes._25kSize; }

        public override double Width { get => MapIndexes._25kSize; }

        public string ShortSheetNumber { get { return SheetNumber?.Split(' ').Last(); } }
    }
}