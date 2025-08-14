using System; 
using IRI.Maptor.Jab.Common.Model;

namespace IRI.Maptor.Jab.Common;

public class MapStatusEventArgs : EventArgs
{
    public MapStatus Status { get; set; }

    public MapStatusEventArgs(MapStatus status)
    {
        Status = status;
    }
}
