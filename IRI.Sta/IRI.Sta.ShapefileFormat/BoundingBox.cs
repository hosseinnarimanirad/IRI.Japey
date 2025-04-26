//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace IRI.Sta.ShapefileFormat
//{
//    public struct BoundingBox
//    {
//        private double xMin, yMin, xMax, yMax;

//        public double XMin
//        {
//            get { return xMin; }
//        }

//        public double YMin
//        {
//            get { return yMin; }
//        }

//        public double XMax
//        {
//            get { return xMax; }
//        }

//        public double YMax
//        {
//            get { return yMax; }
//        }

//        public BoundingBox(double xMin, double yMin, double xMax, double yMax)
//        {
//            this.xMin = xMin;

//            this.xMax = xMax;

//            this.yMin = yMin;

//            this.yMax = yMax;
//        }

//        public double Width
//        {
//            get { return this.XMax - this.XMin; }
//        }

//        public double Height
//        {
//            get { return this.YMax - this.YMin; }
//        }

//        public static IRI.Sta.Common.Primitives.BoundingBox CalculateBoundingBox(Point[] points)
//        {
//            double xMin = points[0].X, yMin = points[0].Y, xMax = points[0].X, yMax = points[0].Y;

//            for (int i = 1; i < points.Length; i++)
//            {
//                xMin = Math.Min(xMin, points[i].X);

//                yMin = Math.Min(yMin, points[i].Y);

//                xMax = Math.Max(xMax, points[i].X);

//                yMax = Math.Max(yMax, points[i].Y);
//            }

//            return new BoundingBox(xMin, yMin, xMax, yMax);
//        }

//        public IRI.Sta.Common.Primitives.BoundingBox Add(BoundingBox secondBoundingBox)
//        {
//            double t = Math.Min(this.XMin, secondBoundingBox.XMin);

//            t = Math.Min(this.YMin, secondBoundingBox.YMin);

//            t = Math.Max(this.XMax, secondBoundingBox.XMax);

//            t = Math.Max(this.YMax, secondBoundingBox.YMax);

//            return new BoundingBox(xMin: Math.Min(this.XMin, secondBoundingBox.XMin),
//                                    yMin: Math.Min(this.YMin, secondBoundingBox.YMin),
//                                    xMax: Math.Max(this.XMax, secondBoundingBox.XMax),
//                                    yMax: Math.Max(this.YMax, secondBoundingBox.YMax));
//        }
//    }
//}
