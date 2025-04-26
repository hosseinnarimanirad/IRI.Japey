using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Sta.Common.Gpx;

[Serializable]
public class GpxTrack
{
    public string Name { get; set; }

    public List<GpxTrackSegment> Segments { get; set; }

    public GpxTrack()
    {

    }

    public GpxTrack(string name, List<GpxTrackSegment> segments)
    {
        this.Name = name;

        this.Segments = segments;
    }
}
