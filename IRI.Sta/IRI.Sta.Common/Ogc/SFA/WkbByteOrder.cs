﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Ham.SpatialBase.Ogc
{
    public enum WkbByteOrder : byte
    {
        /// <summary>
        /// BigEndian
        /// </summary>
        WkbXdr = 0,    //Big Endian
        /// <summary>
        /// Little Endian
        /// </summary>
        WkbNdr = 1      //Little Endian
    }
}
