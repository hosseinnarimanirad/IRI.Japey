using System;
using System.Collections.Generic;
using KnpCorp.DataStructure;
using System.Text;

namespace WindowsApplication1.DataStructure
{
    public class IndexValueCollection<T> : IEnumerable<IndexValue<T>> where T : IComparable<T>
    {
        IndexValue<T>[] values;

        public IndexValueCollection(IndexValue<T>[] values)
        {
            this.values = values;
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
}
