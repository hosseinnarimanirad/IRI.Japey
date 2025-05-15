using System; 
using IRI.Jab.Common.Model;

namespace IRI.Jab.Common;

public class MapActionEventArgs : EventArgs
{
    public MapAction Action { get; set; }

    public MapActionEventArgs(MapAction action)
    {
        Action = action;
    }
}
