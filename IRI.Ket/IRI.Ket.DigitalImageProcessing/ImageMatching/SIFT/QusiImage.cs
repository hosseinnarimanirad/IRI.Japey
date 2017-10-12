// besmellahe rahmane rahim
// Allahomma ajjel le-valiyek al-faraj

using System;
using System.Collections.Generic;
using IRI.Ham.Algebra;
using IRI.Ket.DataStructure;
using System.Xml.Serialization;

namespace IRI.Ket.DigitalImageProcessing.ImageMatching
{

    public class QusiImage
    {
        private Matrix m_Values;

        private int m_Scale, m_Blure;

        private double m_Sigma;

        public int Width
        {
            get { return this.m_Values.NumberOfColumns; }
        }

        public int Height
        {
            get { return this.m_Values.NumberOfRows; }
        }

        public int Scale
        {
            get { return this.m_Scale; }

            set { this.m_Scale = value; }
        }

        public int Blure
        {
            get { return this.m_Blure; }

            set { this.m_Blure = value; }
        }

        public double Sigma
        {
            get { return this.m_Sigma; }

            set { this.m_Sigma = value; }
        }

        public double this[int column, int row]
        {
            get { return this.m_Values[row, column]; }

            set { this.m_Values[row, column] = value; }
        }

        public double MaxGrayValue
        {
            get
            {
                return IRI.Ham.Statistics.Statistics.GetMax(this.m_Values);
            }
        }

        public double MinGrayValue
        {
            get
            {
                return IRI.Ham.Statistics.Statistics.GetMin(this.m_Values);
            }
        }

        public QusiImage(Matrix values, int scale, int blure, double sigma)
        {
            if (values == null)
            {
                throw new NotImplementedException();
            }

            this.m_Scale = scale;

            this.m_Blure = blure;

            this.m_Sigma = sigma;

            this.m_Values = values;
        }
    }
}
