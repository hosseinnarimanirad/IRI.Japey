using System; 

namespace IRI.Maptor.Jab.Common;

public class ZoomEventArgs : EventArgs
{
    public double ZoomLevel { get; set; }

    public double MapScale { get; set; }

    public ZoomEventArgs(double zoomLevel, double mapScale)
    {
        this.ZoomLevel = zoomLevel;

        this.MapScale = mapScale;
    }
}
