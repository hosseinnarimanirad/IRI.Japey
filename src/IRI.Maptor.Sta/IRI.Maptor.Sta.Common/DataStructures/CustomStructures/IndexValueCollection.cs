// besmellahe rahmane rahim
// Allahomma ajjel le-valiyek al-faraj

using System;
using System.Collections.Generic;
using IRI.Maptor.Sta.DataStructures;
using System.Text;

namespace IRI.Maptor.Sta.DataStructures.CustomStructures;

public class IndexValueCollection<T> : IEnumerable<IndexValue<T>> where T : IComparable<T>
{
    IndexValue<T>[] values;

    public IndexValueCollection(IndexValue<T>[] values)
    {
        this.values = values;
    }

    public IndexValueCollection(int[] indexes,T[] values)
    {
        if (indexes.Length!=values.Length)
        {
            throw new NotImplementedException();
        }

        this.values = new IndexValue<T>[indexes.Length];

        for (int i = 0; i < indexes.Length; i++)
        {
            this.values[i] = new IndexValue<T>(indexes[i], values[i]);
        }
    }

    #region IEnumerable<T> Members

    public IEnumerator<IndexValue<T>> GetEnumerator()
    {
        foreach (IndexValue<T> var in values)
        {
            yield return var;
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
