//// besmellahe rahmane rahim
//// Allahomma ajjel le-valiyek al-faraj

//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace IRI.Ket.DataStructure
//{

//    public class Triple<TFirst, TSecond, TThird>
//    {
//        List<TFirst> firstValues;

//        List<TSecond> secondValues;

//        List<TThird> thirdValues;

//        public int Count
//        {
//            get { return this.firstValues.Count; }
//        }

//        public Triple()
//        {
//            this.firstValues = new List<TFirst>();

//            this.secondValues = new List<TSecond>();

//            this.thirdValues = new List<TThird>();
//        }

//        public void Add(TFirst firstValue, TSecond secondValue,TThird thirdValue)
//        {
//            if (firstValues.Contains(firstValue) || secondValues.Contains(secondValue))
//            {
//                throw new NotImplementedException();
//            }

//            firstValues.Add(firstValue);

//            secondValues.Add(secondValue);
//        }

//        public TFirst this[TSecond secondValue]
//        {
//            get
//            {
//                return GetFirstValue(secondValue);
//            }
//        }

//        public TSecond this[TFirst firstValue]
//        {
//            get
//            {
//                int index = this.firstValues.IndexOf(firstValue);

//                return this.secondValues[index];
//            }
//        }

//        public TFirst GetFirstValue(TSecond secondValue)
//        {
//            int index = this.secondValues.IndexOf(secondValue);

//            return this.firstValues[index];
//        }

//        public TSecond GetSecondValue(TFirst firstValue)
//        {
//            int index = this.firstValues.IndexOf(firstValue);

//            return this.secondValues[index];
//        }

//        public bool FirstValuesContains(TFirst item)
//        {
//            return this.firstValues.Contains(item);
//        }

//        public bool SecondValuesContains(TSecond item)
//        {
//            return this.secondValues.Contains(item);
//        }

//    }
//}

using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Ket.DataStructure.MyStructures
{
    public struct Triple<TFirst, TSecond, TThird>
    {
        private TFirst m_First;

        private TSecond m_Second;

        private TThird m_Third;

        public TFirst First
        {
            get { return this.m_First; }
            set { this.m_First = value; }
        }

        public TSecond Second
        {
            get { return this.m_Second; }
            set { this.m_Second = value; }
        }

        public TThird Third
        {
            get { return this.m_Third; }
            set { this.m_Third = value; }
        }

        public Triple(TFirst first, TSecond second, TThird third)
        {
            this.m_First = first;

            this.m_Second = second;

            this.m_Third = third;
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}, {2}", this.First.ToString(), this.Second.ToString(), this.Third.ToString());
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(Triple<TFirst, TSecond, TThird>))
            {
                Triple<TFirst, TSecond, TThird> temp = (Triple<TFirst, TSecond, TThird>)obj;

                return this.First.Equals(temp.First) && this.Second.Equals(temp.Second) && this.Third.Equals(temp.Third);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
