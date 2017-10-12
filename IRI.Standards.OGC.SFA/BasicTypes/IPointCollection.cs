using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Standards.OGC.SFA
{
    public interface IPointCollection : IEnumerable<IPoint>
    {
        IPoint this[int index] { get; set; }

        int Count { get; }
    }
}
