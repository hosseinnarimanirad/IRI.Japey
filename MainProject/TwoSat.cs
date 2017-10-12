using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.MainProject
{
    class TwoSat : IComparable
    {
        public bool _isNegate;
        public long _index;

        public TwoSat(long index, bool value, bool isNegate)
        {
            this._index = index;

            this._isNegate = isNegate;
        }

        public override bool Equals(object obj)
        {
            TwoSat o = (TwoSat)obj;

            return this._isNegate == o._isNegate && this._index == o._index;
        }

        public int CompareTo(object obj)
        {
            TwoSat o = (TwoSat)obj;

            if (o.Equals(this))
            {
                return 0;
            }
            else
            {
                return this._index.CompareTo(o._index);
            }
        }
    }

    class Clauses
    {
        public TwoSat first;
        public TwoSat second;

    }
}
