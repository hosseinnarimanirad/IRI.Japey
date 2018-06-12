using IRI.Msh.Common.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Msh.Common.Model.Mapping
{
    public class IndexSheet
    {
        public BoundingBox Extent { get; set; }

        public string SheetName { get; set; }

        public string SubTitle { get { return SheetName.Contains(" ") ? SheetName.Split(' ')?.Last() : string.Empty; } }

        public string Note { get; set; }
    }
}
