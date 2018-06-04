using IRI.Msh.Common.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Msh.Common.Model.Mapping
{
    public class IndexSheet
    {
        public BoundingBox Extent { get; set; }

        public string SheetName { get; set; }

        public string Note { get; set; }
    }
}
