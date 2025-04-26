using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Sta.Common.Primitives
{
    public interface ILocateable
    {
        Point Location { get; set; }
    }
}
