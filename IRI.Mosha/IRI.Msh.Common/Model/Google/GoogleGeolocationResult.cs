﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Msh.Common.Model.Google
{
    public class GoogleGeolocationResult
    {
        public double accuracy { get; set; }

        public GoogleLocation location { get; set; }

        //public class Location
        //{
        //    public double lat { get; set; }

        //    public double lng { get; set; }
        //}
    }
}
