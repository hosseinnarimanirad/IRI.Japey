﻿using IRI.Sta.Common.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Sta.Common.Mapping
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