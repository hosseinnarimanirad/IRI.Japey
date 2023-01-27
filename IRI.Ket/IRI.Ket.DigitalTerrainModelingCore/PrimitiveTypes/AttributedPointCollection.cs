﻿// besmellahe rahmane rahim
// Allahomma ajjel le-valiyek al-faraj

using System;
using System.Collections.Generic;
using System.Text;
using IRI.Ket.Geometry;
using System.Xml.Serialization;

//This class has some security issues: Points can be changed
//without the change in the values or visa versa
namespace IRI.Ket.DigitalTerrainModeling
{
    [Serializable]
    public class AttributedPointCollection : PointCollection, IEnumerable<AttributedPoint>
    {
        //Fields

        /*protected PointCollection points;*/

        protected List<double> values;

        //Constructors
        public AttributedPointCollection()
            : base()
        {
            this.values = new List<double>();
        }

        public AttributedPointCollection(List<double> x, List<double> y, List<double> values)
        {
            if (x.Count != y.Count || x.Count != values.Count)
            {
                throw new NotImplementedException();
            }

            for (int i = 0; i < x.Count; i++)
            {
                Add(x[i], y[i], values[i]);
            }
        }

        //Indexers

        public AttributedPoint this[int index]
        {
            get { return new AttributedPoint(points[index].X, points[index].Y, values[index]); }
        }

        /*public PointCollection Points
        //{
        //    get { return points; }
        }*/

        /*public int Count
        //{
        //    get { return this.values.Count; }
        }*/

        /*public double MinY
        //{
        //    get
        //    {
        //        double result = points[0].Y;

        //        foreach (Point item in points)
        //        {
        //            if (item.Y < result)
        //            {
        //                result = item.Y;
        //            }
        //        }

        //        return result;
        //    }
        }*/

        /*public double MinX
        //{
        //    get
        //    {
        //        double result = points[0].X;

        //        foreach (Point item in points)
        //        {
        //            if (item.X < result)
        //            {
        //                result = item.X;
        //            }
        //        }
        //        return result;
        //    }
        }*/

        /*public double MaxY
        //{
        //    get
        //    {
        //        double result = points[0].Y;

        //        foreach (Point item in points)
        //        {
        //            if (item.Y > result)
        //            {
        //                result = item.Y;
        //            }
        //        }
        //        return result;
        //    }
        }*/

        /*public double MaxX
        //{
        //    get
        //    {
        //        double result = points[0].X;

        //        foreach (Point item in points)
        //        {
        //            if (item.X > result)
        //            {
        //                result = item.X;
        //            }
        //        }
        //        return result;
        //    }
        }*/

        //public AttributedPoint LeftBound
        //{
        //    get
        //    {
        //        int index = 0;

        //        for (int i = 1; i < this.Count; i++)
        //        {
        //            if (this.points[i].Y < this.points[index].Y)
        //            {
        //                index = i;
        //            }
        //            else if (this.points[i].Y == this.points[index].Y)
        //            {
        //                if (this.points[i].X < this.points[index].X)
        //                {
        //                    index = i;
        //                }
        //            }
        //        }

        //        return new AttributedPoint(this.points[index].X, this.points[index].Y, this.values[index]);
        //    }
        //}

        //public AttributedPoint RightBound
        //{
        //    get
        //    {
        //        int index = 0;

        //        for (int i = 1; i < this.Count; i++)
        //        {
        //            if (this.points[i].Y > this.points[index].Y)
        //            {
        //                index = i;
        //            }
        //            else if (this.points[i].Y == this.points[index].Y)
        //            {
        //                if (this.points[i].X > this.points[index].X)
        //                {
        //                    index = i;
        //                }
        //            }
        //        }

        //        return new AttributedPoint(this.points[index].X, this.points[index].Y, this.values[index]);
        //    }
        //}

        //

        public override void Add(Point newPoint)
        {
            this.Add(new AttributedPoint(newPoint.X, newPoint.Y, 0));
        }

        private void Add(double x, double y, double value)
        {
            //int newCode = this.GetNewCode();

            Point temp = new Point(x, y);

            if (this.points.Contains(temp))
            {
                //this.queue.Enqueue(newCode);
                return;
                //throw new NotImplementedException();
            }
            else
            {
                //this.points.Add(temp);
                base.Add(temp);
                //this.codes.Add(newCode);

                this.values.Add(value);
            }
        }

        public void Add(AttributedPoint point)
        {
            //int newCode = this.GetNewCode();

            Point temp = new Point(point.X, point.Y);

            if (this.points.Contains(temp))
            {
                //this.queue.Enqueue(newCode);

                //throw new NotImplementedException();
                return;
            }
            else
            {
                //this.points.Add(temp);
                base.Add(temp);
                //this.codes.Add(newCode);

                this.values.Add(point.Value);
            }
        }

        public override void RemoveAt(int index)
        {
            if (index < 0 || index > this.Count - 1)
            {
                throw new NotImplementedException();
            }

            base.RemoveAt(index);

            this.values.RemoveAt(index);
        }

        public double GetValue(Point point)
        {
            int index = codes.IndexOf(point.GetHashCode());

            if (index > -1)
            {
                return values[index];
            }

            throw new NotImplementedException();
        }

        #region IEnumerable<AttributedPoint> Members

        public new IEnumerator<AttributedPoint> GetEnumerator()
        {
            for (int i = 0; i < this.Count; i++)
            {
                yield return new AttributedPoint(this.points[i].X, this.points[i].Y, this.values[i]);
            }
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion
    }
}
