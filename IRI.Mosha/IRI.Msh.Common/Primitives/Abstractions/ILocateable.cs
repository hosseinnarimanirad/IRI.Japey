using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Msh.Common.Primitives
{
    public interface ILocateable
    {
        Point Location { get; set; }
    }
}
