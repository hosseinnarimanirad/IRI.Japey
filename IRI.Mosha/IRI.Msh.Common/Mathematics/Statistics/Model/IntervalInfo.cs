//using IRI.Msh.Common.Model;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace IRI.Msh.Statistics.Model
//{
//    public class IntervalInfo<T> : IComparable<IntervalInfo<T>>
//    {
//        public Interval Interval { get; set; }

//        public List<(T @class, int count)> ClassFrequencies { get; set; }

//        public IntervalInfo(Interval interval, IEnumerable<T> classes)
//        {
//            this.ClassFrequencies = classes.GroupBy(c => c).Select(g => (@class: g.Key, count: g.Count()))?.ToList();

//            this.Interval = interval;
//        }

//        public int CompareTo(IntervalInfo<T> other)
//        {
//            if (other == null)
//            {
//                return this.CompareTo(other);
//            }

//            return this.Interval.Mid.CompareTo(other.Interval.Mid);
//        }
//    }
//}
