using IRI.Maptor.Sta.Common.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Maptor.Sta.Common.Abstrations;

public interface ILocateable
{
    Point Location { get; set; }
}
