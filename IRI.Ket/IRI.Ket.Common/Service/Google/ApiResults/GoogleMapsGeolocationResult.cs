using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.Common.Service.Google.ApiResults
{
    public class GoogleMapsGeolocationJsonResult
    {
        public double accuracy { get; set; }

        public Location location { get; set; }

        public class Location
        {
            public double lat { get; set; }

            public double lng { get; set; }
        }
    }


}
