// besmellahe rahmane rahim
// Allahomma ajjel le-valiyek al-faraj

using IRI.Sta.Common.Primitives;
using IRI.Sta.Common.Enums;

namespace IRI.Sta.Spatial;

[Serializable]
public class PointCollection : IEnumerable<Point>
{

    public static PointComparisonPriority ComparisonPriority = PointComparisonPriority.YBased;

    #region Fields

    protected List<Point> points = new List<Point>();

    protected List<int> codes = new List<int>();

    protected Queue<int> queue = new Queue<int>();

    #endregion

    #region Constructors

    public PointCollection()
        : this(new List<Point>())
    {
        this.queue.Enqueue(0);
    }

    public PointCollection(List<Point> points)
    {
        foreach (Point item in points)
        {
            Add(item);
        }
    }

    public PointCollection(List<double> x, List<double> y)
    {
        if (x.Count != y.Count)
        {
            throw new NotImplementedException();
        }

        for (int i = 0; i < x.Count; i++)
        {
            Point tempValue = new Point(x[i], y[i]);

            if (!this.points.Contains(tempValue))
            {
                Add(tempValue);
            }
        }
    }

    #endregion

    #region Indexers & Properties

    public virtual Point this[int index]
    {
        get { return points[index]; }

        //set 
        //{
        //    points[index] = value; 
        //}
    }

    public int Count
    {
        get { return this.points.Count; }
    }

    public int LowerBoundIndex
    {
        get
        {
            int index = 0;

            double x = points[index].X;

            double y = points[index].Y;

            for (int i = 1; i < this.Count; i++)
            {
                if (this.points[i].Y < y)
                {
                    index = i;

                    x = this.points[i].X;

                    y = this.points[i].Y;
                }
                else if (this.points[i].Y == y)
                {
                    if (this.points[i].X < x)
                    {
                        index = i;

                        x = this.points[i].X;

                        y = this.points[i].Y;
                    }
                }
            }

            return index;
        }
    }

    public int UpperBoundIndex
    {
        get
        {
            int index = 0;

            double x = points[index].X;

            double y = points[index].Y;

            for (int i = 1; i < this.Count; i++)
            {
                if (this.points[i].Y > y)
                {
                    index = i;

                    x = this.points[i].X;

                    y = this.points[i].Y;
                }
                else if (this.points[i].Y == y)
                {
                    if (this.points[i].X > x)
                    {
                        index = i;

                        x = this.points[i].X;

                        y = this.points[i].Y;
                    }
                }
            }

            return index;
        }
    }

    public double MinY
    {
        get
        {
            double result = points[0].Y;

            foreach (Point item in points)
            {
                if (item.Y < result)
                {
                    result = item.Y;
                }
            }

            return result;
        }
    }

    public double MinX
    {
        get
        {
            double result = points[0].X;

            foreach (Point item in points)
            {
                if (item.X < result)
                {
                    result = item.X;
                }
            }
            return result;
        }
    }

    public double MaxY
    {
        get
        {
            double result = points[0].Y;

            foreach (Point item in points)
            {
                if (item.Y > result)
                {
                    result = item.Y;
                }
            }
            return result;
        }
    }

    public double MaxX
    {
        get
        {
            double result = points[0].X;

            foreach (Point item in points)
            {
                if (item.X > result)
                {
                    result = item.X;
                }
            }
            return result;
        }
    }

    #endregion

    #region Methods

    public virtual void Add(Point newPoint)
    {
        if (this.points.Contains(newPoint))
        {
            throw new NotImplementedException();
            //return;
        }

        int newCode = this.GetNewCode();

        Point temp = new Point(newPoint.X, newPoint.Y/*, newCode*/);

        this.points.Add(temp);

        if (this.codes.Contains(newCode))
        {
            throw new NotImplementedException();
        }

        this.codes.Add(newCode);
    }

    public void Remove(Point point)
    {
        int index = this.points.IndexOf(point);

        this.RemoveAt(index);
    }

    public virtual void RemoveAt(int index)
    {
        if (index < 0 || index >= points.Count)
        {
            throw new NotImplementedException();
        }

        this.queue.Enqueue(this.codes[index]);

        this.points.RemoveAt(index);

        this.codes.RemoveAt(index);

    }

    public void RemoveByCode(int pointCode)
    {
        int index = codes.IndexOf(pointCode);

        RemoveAt(index);
    }

    public bool Contains(Point point)
    {
        return this.points.Contains(point);
    }

    public int IndexOf(Point point)
    {
        return this.points.LastIndexOf(point);
    }

    public int IndexOf(int pointCode)
    {
        return this.codes.IndexOf(pointCode);
    }

    public Point GetPoint(int pointCode)
    {
        return this.points[this.IndexOf(pointCode)];
    }

    public int GetNewCode()
    {
        if (this.queue.Count < 1)
        {
            return this.Count;
        }
        else
        {
            return this.queue.Dequeue();
        }
    }

    #endregion
    
    /*Sort
    //public void Sort(IRI.Sta.DataStructures.SortDirection direction, PointComparisonPriority priority)
    //{
    //    Point.ComparisonPriority = priority;

    //    IRI.Sta.DataStructures.SortAlgorithm.Heapsort<Point>(ref this.points, direction);

    //    //RefreshCodes();
    //}
    */
     
    #region IEnumerable<Point> Members

    public IEnumerator<Point> GetEnumerator()
    {
        return points.GetEnumerator();
    }

    #endregion

    #region IEnumerable Members

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }

    #endregion
}