// besmellahe rahmane rahim
// Allahomma ajjel le-valiyek al-faraj

using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Sta.Common;


public class QuasiVoronoiCellCollection: IEnumerable<QuasiVoronoiCell>
{
    List<QuasiVoronoiCell> polygons;

    List<int> codes;

    public QuasiVoronoiCellCollection()
    {
        polygons = new List<QuasiVoronoiCell>();

        this.codes = new List<int>();
    }

    public QuasiVoronoiCellCollection(int length)
    {
        this.polygons = new List<QuasiVoronoiCell>(length);

        this.codes = new List<int>(length);
    }

    public QuasiVoronoiCellCollection(List<QuasiVoronoiCell> polygons)
    {
        this.polygons = polygons;

        this.codes = new List<int>();

        RefreshCodes(polygons);
    }

    public QuasiVoronoiCell this[int index]
    {
        get { return polygons[index]; }
    }

    public int Count
    {
        get { return polygons.Count; }
    }

    private void RefreshCodes(List<QuasiVoronoiCell> polygons)
    {
        this.codes.RemoveRange(0, this.codes.Count);

        foreach (QuasiVoronoiCell item in polygons)
        {
            int tempCode = item.GetHashCode();

            if (!this.codes.Contains(tempCode))
            {
                this.polygons.Add(item);

                this.codes.Add(tempCode);
            }
        }
    }

    public bool ContainsCode(int polygonCode)
    {
        return this.codes.Contains(polygonCode);
    }

    public void Add(QuasiVoronoiCell newPolygon)
    {
        int tempHashCode = newPolygon.GetHashCode();

        if (codes.Contains(tempHashCode))
        {
            throw new NotImplementedException();
            //return;
        }

        polygons.Add(newPolygon);

        codes.Add(tempHashCode);
    }

    public void RemoveAt(int index)
    {
        if (index < 0 || index >= codes.Count)
        {
            throw new NotImplementedException();
        }

        this.codes.RemoveAt(index);

        this.polygons.RemoveAt(index);
    }

    public void RemoveByCode(int polygonCode)
    {
        int index = codes.IndexOf(polygonCode);

        if (index == -1)
        {
            throw new NotImplementedException();
        }

        RemoveAt(index);
    }

    public QuasiVoronoiCell GetCell(int polygonCode)
    {
        int index = codes.IndexOf(polygonCode);

        return (index == -1 ? null : polygons[codes.IndexOf(polygonCode)]);
    }

    #region IEnumerable<QuasiVoronoiPolygon> Members

    public IEnumerator<QuasiVoronoiCell> GetEnumerator()
    {
        return this.polygons.GetEnumerator();
    }

    #endregion

    #region IEnumerable Members

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }

    #endregion
}
