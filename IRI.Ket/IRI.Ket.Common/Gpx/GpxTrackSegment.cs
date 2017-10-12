using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Ket.Common.Gpx
{
    public class GpxTrackSegment
    {
        public List<GpxTrackPoint> TrackPoints { get; set; }

        public GpxTrackSegment()
        {

        }

        public GpxTrackSegment(List<GpxTrackPoint> trackPoints)
        {
            this.TrackPoints = trackPoints;
        }
    }
}
