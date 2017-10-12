using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Standards.OGC.SFA
{
    public class PointCollection<T> : List<T>, IPointCollection where T : IPoint
    {

        public new IPoint this[int index]
        {
            get { return base[index]; }

            set { base[index] = (T)value; }
        }
        
        public PointCollection(int capacity)
            : base(capacity)
        {

        }

        public new IEnumerator<IPoint> GetEnumerator()
        {
            for (int i = 0; i < this.Count; i++)
            {
                yield return base[i];
            }
        }
    }
}
