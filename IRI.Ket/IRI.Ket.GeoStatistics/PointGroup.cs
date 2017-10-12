using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IRI.Ham.SpatialBase;

namespace IRI.Ket.GeoStatistics
{
    public class Group<T>
    {
        public T Center { get; set; }

        public int Frequency { get { return this.Members.Count; } }



        public Group(T center)
        {
            this.Members = new List<T>();

            this.Center = center;

            this.Members.Add(center);
        }

        public bool DoseBelongTo(T item, Func<T, T, bool> groupLogic)
        {
            return groupLogic(this.Center, item);
        }

        public void Add(T item)
        {
            this.Members.Add(item);
        }

        //public void Increment()
        //{
        //    this.Frequency++;
        //}

        private List<T> _members;

        public List<T> Members
        {
            get { return this._members; }
            private set { this._members = value; }
        }
    }
}
