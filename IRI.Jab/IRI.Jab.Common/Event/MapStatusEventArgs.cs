using IRI.Jab.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Jab.Common
{
    public class MapStatusEventArgs : EventArgs
    {
        public MapStatus Status { get; set; }

        public MapStatusEventArgs(MapStatus status)
        {
            Status = status;
        }
    }
}
