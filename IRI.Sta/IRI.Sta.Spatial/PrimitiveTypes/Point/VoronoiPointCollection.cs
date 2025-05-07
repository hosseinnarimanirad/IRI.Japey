// besmellahe rahmane rahim
// Allahomma ajjel le-valiyek al-faraj

using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Sta.Spatial;

public class VoronoiPointCollection : IEnumerable<VoronoiPoint>
{
    List<VoronoiPoint> points;

    List<int> codes;

    public VoronoiPointCollection()
    {
        this.points = new List<VoronoiPoint>();

        this.codes = new List<int>();
    }

    public VoronoiPointCollection(int length)
    {
        this.points = new List<VoronoiPoint>(length);

        this.codes = new List<int>(length);
    }

    public VoronoiPointCollection(List<VoronoiPoint> points)
    {
        this.points = points;

        this.codes = new List<int>();

        RefreshCodes();
    }

    public VoronoiPoint this[int index]
    {
        get { return points[index]; }
    }

    public int Count
    {
        get { return points.Count; }
    }

    private void RefreshCodes()
    {
        this.codes.RemoveRange(0, this.codes.Count);

        foreach (VoronoiPoint item in points)
        {
            int tempCode = item.GetHashCode();

            if (!this.codes.Contains(tempCode))
            {
                this.points.Add(item);

                this.codes.Add(tempCode);
            }
        }
    }

    public bool ContainsCode(int pointCode)
    {
        return this.codes.Contains(pointCode);
    }

    public void Add(VoronoiPoint newVoronoiPoint)
    {
        int tempHashCode = newVoronoiPoint.GetHashCode();
        
        if (codes.Contains(tempHashCode))
        {
            throw new NotImplementedException();
           
        }

        points.Add(newVoronoiPoint);

        codes.Add(tempHashCode);
    }

    public void RemoveAt(int index)
    {
        if (index < 0 || index >= codes.Count)
        {
            throw new NotImplementedException();
        }

        this.codes.RemoveAt(index);

        this.points.RemoveAt(index);
    }

    public void RemoveByCode(int pointCode)
    {
        int index = codes.IndexOf(pointCode);

        if (index == -1)
        {
            throw new NotImplementedException();
        }

        RemoveAt(index);
    }

    public VoronoiPoint GetPointByCode(int pointCode)
    {
        return points[codes.IndexOf(pointCode)];
    }

    public VoronoiPoint GetPointByTriangleCode(int triangleCode)
    {
        for (int i = 0; i < this.points.Count; i++)
        {
            if (this.points[i].TriangleCode==triangleCode)
            {
                return this.points[i];
            }
        }

        throw new NotImplementedException();
    }

    #region IEnumerable<VoronoiPoint> Members

    public IEnumerator<VoronoiPoint> GetEnumerator()
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
