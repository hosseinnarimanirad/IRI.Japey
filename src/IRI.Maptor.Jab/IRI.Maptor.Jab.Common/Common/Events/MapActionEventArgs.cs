using System; 
using IRI.Maptor.Jab.Common.Model;

namespace IRI.Maptor.Jab.Common;

public class MapActionEventArgs : EventArgs
{
    public MapAction Action { get; set; }

    public MapActionEventArgs(MapAction action)
    {
        Action = action;
    }
}
