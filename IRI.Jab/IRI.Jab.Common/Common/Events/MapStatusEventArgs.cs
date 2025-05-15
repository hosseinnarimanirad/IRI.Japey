using System; 
using IRI.Jab.Common.Model;

namespace IRI.Jab.Common;

public class MapStatusEventArgs : EventArgs
{
    public MapStatus Status { get; set; }

    public MapStatusEventArgs(MapStatus status)
    {
        Status = status;
    }
}
