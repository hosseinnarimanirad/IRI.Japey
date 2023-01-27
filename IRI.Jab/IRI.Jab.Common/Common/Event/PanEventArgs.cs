using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace IRI.Jab.Common
{
    public class PanEventArgs : EventArgs
    {
        public Point Offset { get; set; }

        public PanEventArgs(Point offset)
        {
            this.Offset = offset;
        }
    }
}
