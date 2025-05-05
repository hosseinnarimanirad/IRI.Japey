// besmellahe rahmane rahim
// Allahomma ajjel le-valiyek al-faraj

using System;
using System.Collections.Generic;
using IRI.Sta.Mathematics;
using IRI.Sta.DataStructures;
using System.Xml.Serialization;

namespace IRI.Ket.DigitalImageProcessing.ImageMatching
{

    public struct Extermas : IEnumerable<Exterma>
    {
        private List<Exterma> m_Values;

        public int Count
        {
            get { return m_Values.Count; }
        }

        public Exterma this[int index]
        {
            get { return this.m_Values[index]; }
            set { this.m_Values[index] = value; }
        }

        public void Add(double scaleLevel, double blureLevel, double column, double row, double sigma)
        {
            if (this.m_Values == null)
            {
                this.m_Values = new List<Exterma>();
            }

            Exterma value = new Exterma(scaleLevel, blureLevel, column, row, sigma);

            this.m_Values.Add(value);
        }

        public void Remove(int index)
        {
            this.m_Values.RemoveAt(index);
        }

        #region IEnumerable<Exterma> Members

        public IEnumerator<Exterma> GetEnumerator()
        {
            return this.m_Values.GetEnumerator();
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
