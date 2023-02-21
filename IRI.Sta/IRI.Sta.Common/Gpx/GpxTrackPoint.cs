using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Sta.Common.Gpx
{
    [Serializable]
    public class GpxTrackPoint
    {
        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public double Elevation { get; set; }

        public string Time { get; set; }
    }
}
