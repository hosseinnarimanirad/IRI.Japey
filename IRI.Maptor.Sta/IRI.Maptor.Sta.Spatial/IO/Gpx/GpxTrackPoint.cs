namespace IRI.Maptor.Sta.Common.IO.Gpx;

[Serializable]
public class GpxTrackPoint
{
    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public double Elevation { get; set; }

    public string Time { get; set; }
}
