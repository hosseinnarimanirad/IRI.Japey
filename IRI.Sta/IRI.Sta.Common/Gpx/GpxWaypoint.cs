using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Sta.Common.Gpx
{
    [Serializable]
    public class GpxWaypoint
    {
        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string Name { get; set; }

        public double Elevation { get; set; }

        public string Description { get; set; }

        public string Symbol { get; set; }
    }
}
