using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace IRI.Sta.Common.Primitives
{
    [DataContract]
    public struct BoundingBox
    {
        private double xMin, yMin, xMax, yMax;

        public double XMin
        {
            get { return xMin; }
        }

        public double YMin
        {
            get { return yMin; }
        }

        public double XMax
        {
            get { return xMax; }
        }

        public double YMax
        {
            get { return yMax; }
        }

        public BoundingBox(double xMin, double yMin, double xMax, double yMax)
        {
            this.xMin = xMin;

            this.xMax = xMax;

            this.yMin = yMin;

            this.yMax = yMax;
        }

        public double Width
        {
            get { return this.XMax - this.XMin; }
        }

        public double Height
        {
            get { return this.YMax - this.YMin; }
        }

        //The system is asumed to be right handed
        public Point TopLeft { get { return new Point(XMin, YMax); } }

        //The system is asumed to be right handed
        public Point BottomRight { get { return new Point(XMax, YMin); } }

        private bool IsInRange(double value, double minRange, double maxRange)
        {
            return value >= minRange && value <= maxRange;
        }

        public bool IsNaN()
        {
            return BoundingBox.IsNaN(this);
        }

        public bool IsValid()
        {
            return !double.IsNaN(this.Width + this.Height) &&
                    !double.IsInfinity(this.Width + this.Height);
        }

        public BoundingBox Add(BoundingBox secondBoundingBox)
        {
            return new BoundingBox(xMin: Math.Min(this.XMin, secondBoundingBox.XMin),
                                    yMin: Math.Min(this.YMin, secondBoundingBox.YMin),
                                    xMax: Math.Max(this.XMax, secondBoundingBox.XMax),
                                    yMax: Math.Max(this.YMax, secondBoundingBox.YMax));
        }

        //public static BoundingBox CalculateBoundingBox<T>(IEnumerable<T> spatialFeatures,
        //    Func<double> initialX, Func<double> initialY,
        //    Func<T, double> mapFuncX, Func<T, double> mapFuncY)
        //{
        //    double xMin = initialX();

        //    double yMin = initialY();

        //    double xMax = initialX();

        //    double yMax = initialY();

        //    foreach (T item in spatialFeatures)
        //    {
        //        xMin = Math.Min(xMin, mapFuncX(item));

        //        yMin = Math.Min(yMin, mapFuncY(item));

        //        xMax = Math.Max(xMax, mapFuncX(item));

        //        yMax = Math.Max(yMax, mapFuncY(item));
        //    }

        //    return new BoundingBox(xMin, yMin, xMax, yMax);
        //}
        public bool Intersects(BoundingBox other)
        {
            //var xIntersects = (this.XMin >= other.XMin && this.XMin <= other.XMax) ||
            //                    (this.XMax >= other.XMin && this.XMax <= other.XMax);

            //var yIntersects = (this.YMin >= other.YMin && this.YMin <= other.YMax) ||
            //                    (this.YMax >= other.YMin && this.YMax <= other.YMax);

            var xIntersects = IsInRange(this.XMin, other.XMin, other.XMax) ||
                                IsInRange(this.XMax, other.XMin, other.XMax) ||
                                IsInRange(other.XMin, this.XMin, this.XMax) ||
                                IsInRange(other.XMax, this.XMin, this.XMax);

            var yIntersects = IsInRange(this.YMin, other.YMin, other.YMax) ||
                                IsInRange(this.YMax, other.YMin, other.YMax) ||
                                IsInRange(other.YMin, this.YMin, this.YMax) ||
                                IsInRange(other.YMax, this.YMin, this.YMax);

            return xIntersects && yIntersects;
        }

        public bool Intersects(IPoint point)
        {
            return IsInRange(point.X, this.XMin, this.XMax) && IsInRange(point.Y, this.YMin, this.YMax);
        }

        public BoundingBox Intersect(BoundingBox other)
        {
            if (!Intersects(other))
            {
                return NaN;
            }

            return new BoundingBox(xMin: Math.Max(this.XMin, other.XMin),
                                    yMin: Math.Max(this.YMin, other.YMin),
                                    xMax: Math.Min(this.XMax, other.XMax),
                                    yMax: Math.Min(this.YMax, other.YMax));
        }

        public void Extend(BoundingBox secondBoundingBox)
        {
            this.xMin = Math.Min(this.XMin, secondBoundingBox.XMin);

            this.yMin = Math.Min(this.YMin, secondBoundingBox.YMin);

            this.xMax = Math.Max(this.XMax, secondBoundingBox.XMax);

            this.yMax = Math.Max(this.YMax, secondBoundingBox.YMax);
        }

        public BoundingBox Expand(double factor)
        {
            var xShift = this.Width / 2.0 * factor;

            var yShift = this.Height / 2.0 * factor;

            var center = this.Center;

            return new BoundingBox(center.X - xShift, center.Y - yShift, center.X + xShift, center.Y + yShift);
        }

        public static BoundingBox GetMergedBoundingBox(IEnumerable<BoundingBox> boundingBoxes, bool ignoreNanValues = false)
        {
            if (boundingBoxes == null || boundingBoxes.Count() == 0)
            {
                return BoundingBox.NaN;
            }

            if (ignoreNanValues)
            {
                return GetMergedBoundingBox(boundingBoxes.Where(i => !i.IsNaN()));
            }
            else
            {
                return new BoundingBox(
                xMin: boundingBoxes.Min(i => i.XMin),
                yMin: boundingBoxes.Min(i => i.YMin),
                xMax: boundingBoxes.Max(i => i.XMax),
                yMax: boundingBoxes.Max(i => i.YMax));
            }
            
        }

        public static BoundingBox CalculateBoundingBox(IEnumerable<IPoint> points)
        {
            if (points == null || points.Count() == 0)
            {
                return BoundingBox.NaN;
            }

            return new BoundingBox(xMin: points.Min(i => i.X),
                                                yMin: points.Min(i => i.Y),
                                                xMax: points.Max(i => i.X),
                                                yMax: points.Max(i => i.Y));
        }

        public static BoundingBox Add(BoundingBox first, BoundingBox second)
        {
            return GetMergedBoundingBox(new List<BoundingBox> { first, second });
        }

        public BoundingBox Transform(Func<Point, Point> func)
        {
            var newTopLeft = func(this.TopLeft);

            var newButtomRight = func(this.BottomRight);

            return new BoundingBox(newTopLeft.X, newButtomRight.Y, newButtomRight.X, newTopLeft.Y);
        }

        public override string ToString()
        {
            return string.Format("XMin: {0}, YMin: {1}, XMax: {2}, YMax: {3}", this.XMin, this.YMin, this.XMax, this.YMax);
        }

        public static BoundingBox Parse(string bbox)
        {
            try
            {
                var items = bbox.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(i => double.Parse(i)).ToList();

                if (items.Count != 4)
                {
                    throw new NotImplementedException("Invalid input in BoundingBox.Parse");
                }

                return new BoundingBox(xMin: items[0], yMin: items[1], xMax: items[2], yMax: items[3]);
            }
            catch (Exception)
            {
                throw new NotImplementedException("ERROR AT BoundingBox.Parse");
            }
        }

        public Point Center { get { return new Point((XMax + XMin) / 2.0, (YMax + YMin) / 2.0); } }

        public static BoundingBox NaN { get { return new BoundingBox(double.NaN, double.NaN, double.NaN, double.NaN); } }

        public static bool IsNaN(BoundingBox value)
        {
            return double.IsNaN(value.Width) || double.IsNaN(value.Height);
        }

        public static bool operator ==(BoundingBox first, BoundingBox second)
        {
            return first.XMin == second.XMin &&
                    first.XMax == second.XMax &&
                    first.YMin == second.YMin &&
                    first.YMax == second.YMax;
        }

        public static bool operator !=(BoundingBox first, BoundingBox second)
        {
            return !(first == second);
        }

        public override bool Equals(object obj)
        {
            return obj.GetType() == typeof(BoundingBox) && (BoundingBox)obj == this;
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }
    }
}
