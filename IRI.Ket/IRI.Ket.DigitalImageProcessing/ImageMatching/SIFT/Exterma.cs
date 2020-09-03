﻿// besmellahe rahmane rahim
// Allahomma ajjel le-valiyek al-faraj

using System;
using System.Collections.Generic;
using IRI.Msh.Algebra;
using IRI.Msh.DataStructure;
using System.Xml.Serialization;

namespace IRI.Ket.DigitalImageProcessing.ImageMatching
{
    [Serializable()]
    public struct Exterma
    {
        
        private double m_ScaleLevel;
        
        private double m_BlureLevel;
        
        private double m_Row;

        private double m_Column;

        private double m_Sigma;

        public Exterma(double scaleLevel, double blureLevel, double column, double row, double sigma)
        {
            this.m_ScaleLevel = scaleLevel;

            this.m_BlureLevel = blureLevel;

            this.m_Row = row;

            this.m_Column = column;

            this.m_Sigma = sigma;
        }

        public double ScaleLevel
        {
            get { return m_ScaleLevel; }
            set { m_ScaleLevel = value; }
        }
       
        public double BlureLevel
        {
            get { return m_BlureLevel; }

            set { m_BlureLevel = value; }
        }
        
        public double Row
        {
            get { return m_Row; }

            set { m_Row = value; }
        }

        public double Column
        {
            get { return m_Column; }

            set { m_Column = value; }
        }

        public double Sigma
        {
            get { return this.m_Sigma; }
            set { this.m_Sigma = value; }
        }

        public override string ToString()
        {
            return string.Format("scale:{0}, blure:{1}, x:{2} y:{3}, sigma:{4}", ScaleLevel, BlureLevel, Column, Row, Sigma);
        }

    }

}
