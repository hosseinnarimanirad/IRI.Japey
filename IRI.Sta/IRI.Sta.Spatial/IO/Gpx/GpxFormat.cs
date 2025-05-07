using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace IRI.Sta.Common.IO.Gpx;

public static class GpxFormat
{
    public static List<GpxWaypoint> GetWaypoints(string gpxFileName, string xNamespace = null)
    {
        XDocument document = XDocument.Load(gpxFileName);

        XNamespace gpxNamespace = string.IsNullOrEmpty(xNamespace) ? XNamespace.Get(@"http://www.topografix.com/GPX/1/1") : XNamespace.Get(xNamespace);

        //XNamespace gpxNamespace = XNamespace.Get(@"http://www.topografix.com/GPX/1/1");

        return document.Root.Descendants(gpxNamespace + "wpt").Select(i => new GpxWaypoint()
        {
            Latitude = double.Parse(i.Attribute("lat").Value),
            Longitude = double.Parse(i.Attribute("lon").Value),
            Elevation = double.Parse((string)i.Element(gpxNamespace + "ele") ?? "-9999"),
            Name = (string)i.Element(gpxNamespace + "name"),
            Symbol = (string)i.Element(gpxNamespace + "sym"),
            Description = (string)i.Element(gpxNamespace + "desc")
        }).ToList();
    }

    public static IEnumerable<GpxTrack> GetTracks(string gpxFileName, string xNamespace = null)
    {
        XDocument document = XDocument.Load(gpxFileName);

        XNamespace gpxNamespace = string.IsNullOrEmpty(xNamespace) ? XNamespace.Get(@"http://www.topografix.com/GPX/1/1") : XNamespace.Get(xNamespace);

        return document.Root
                .Descendants(gpxNamespace + "trk")
                .Select(i => new GpxTrack()
                {
                    Name = i.Element(gpxNamespace + "name").Value,
                    Segments = i.Elements(gpxNamespace + "trkseg")
                                .Select(j => new GpxTrackSegment()
                                {
                                    TrackPoints = j.Elements(gpxNamespace + "trkpt")
                                                    .Select(k => new GpxTrackPoint()
                                                    {
                                                        Latitude = double.Parse(k.Attribute("lat").Value),
                                                        Longitude = double.Parse(k.Attribute("lon").Value),
                                                        Elevation = double.Parse((string)k.Element(gpxNamespace + "ele") ?? "-9999"),
                                                        Time = (string)k.Element(gpxNamespace + "time"),
                                                    }).ToList()
                                }).ToList()
                }).ToList();
    }
}
