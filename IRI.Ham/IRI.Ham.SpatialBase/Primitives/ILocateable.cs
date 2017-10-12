using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Ham.SpatialBase.Primitives
{
    public interface ILocateable
    {
        Point Location { get; set; }
    }
}
