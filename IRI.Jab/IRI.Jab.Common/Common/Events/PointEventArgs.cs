using System; 

namespace IRI.Jab.Common;

/// <summary>
/// Point is a (long, lat) pair on WGS84
/// </summary>
public class PointEventArgs: EventArgs
{
    public System.Windows.Point Point { get; set; }

    public PointEventArgs(System.Windows.Point point)
    {
        this.Point = point;
    }
}
