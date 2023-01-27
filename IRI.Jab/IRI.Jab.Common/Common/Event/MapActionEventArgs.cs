using IRI.Jab.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Jab.Common
{
    public class MapActionEventArgs : EventArgs
    {
        public MapAction Action { get; set; }

        public MapActionEventArgs(MapAction action)
        {
            Action = action;
        }
    }
}
