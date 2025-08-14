namespace IRI.Maptor.Sta.Common.IO.Gpx;

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
        Name = name;

        Segments = segments;
    }
}
