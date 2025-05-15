using System; 
using System.Windows;

namespace IRI.Jab.Common;

public class PanEventArgs : EventArgs
{
    public Point Offset { get; set; }

    public PanEventArgs(Point offset)
    {
        this.Offset = offset;
    }
}
