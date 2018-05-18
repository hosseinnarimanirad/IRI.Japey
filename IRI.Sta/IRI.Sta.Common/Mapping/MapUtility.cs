﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Ham.SpatialBase.Mapping
{
    public class MapUtility
    {
        public static Func<Point, Point> GetMapToScreen(BoundingBox mapExtent, double screenWidth, double screeenHeight)
        {
            double scaleX = screenWidth / mapExtent.Width;

            double scaleY = screeenHeight / mapExtent.Height;

            return p => new Point((p.X - mapExtent.XMin) * scaleX, screeenHeight - (p.Y - mapExtent.YMin) * scaleY);
        }
         
    }
}
