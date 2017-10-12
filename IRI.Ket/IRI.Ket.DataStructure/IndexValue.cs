using System;
using System.Collections.Generic;
using System.Text;

namespace KnpCorp.DataStructure
{
    public struct IndexValue<T> : IComparable<IndexValue<T>> where T : IComparable<T>
    {
        int m_Index;

        T m_Value;

        public int Index
        {
            get { return this.m_Index; }
        }

        public T Value
        {
            get { return this.m_Value; }
        }


        public IndexValue(int index, T values)
        {
            this.m_Index = index;

            this.m_Value = values;
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}", Index, Value.ToString()); ;
        }

        //public static bool operator <(IndexValue<T> firstValue, IndexValue<T> secondValue)
        //{
        //    return firstValue.CompareTo(secondValue) < 0;
        //}

        //public static bool operator >(IndexValue<T> firstValue, IndexValue<T> secondValue)
        //{
        //    return firstValue.CompareTo(secondValue) > 0;
        //}

        //public static bool operator ==(IndexValue<T> firstValue, IndexValue<T> secondValue)
        //{
        //    return firstValue.CompareTo(secondValue) == 0;
        //}

        //public static bool operator !=(IndexValue<T> firstValue, IndexValue<T> secondValue)
        //{
        //    return firstValue.CompareTo(secondValue) != 0;
        //}

        //#region IComparable<T> Members

        //public int CompareTo(T other)
        //{
        //    return this.values.CompareTo(other);
        //}

        //#endregion

        #region IComparable<IndexValue<T>> Members

        public int CompareTo(IndexValue<T> other)
        {
            return this.Value.CompareTo(other.Value);
        }

        #endregion
    }
}
