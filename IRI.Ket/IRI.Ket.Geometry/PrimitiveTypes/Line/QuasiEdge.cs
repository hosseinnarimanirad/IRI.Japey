 //besmellahe rahmane rahim
 //Allahomma ajjel le-valiyek al-faraj

using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Ket.Geometry
{
    //public class EdgeChangedEventArg : EventArgs
    //{
    //    public int NewValue { get; set; }

    //    public EdgeChangedEventArg(int newValue)
    //    {
    //        this.NewValue = NewValue;
    //    }
    //}

    //public delegate void LeftChangedHandler(object sender, EdgeChangedEventArg e);

    //public delegate void RightChangedHandler(object sender, EdgeChangedEventArg e);

    public struct QuasiEdge
    {
        private int first, second;

        //private int m_left, m_right;

        public int First
        {
            get { return first; }
            set { first = value; }
        }

        public int Second
        {
            get { return second; }
            set { second = value; }
        }

        //public int Left
        //{
        //    get { return m_left; }

        //    set
        //    {
        //        m_left = value;
        //        if (OnLeftChanged!=null)
        //        {
        //            OnLeftChanged.Invoke(this, new EdgeChangedEventArg(value));
        //        }
        //    }
        //}

        //public int Right
        //{
        //    get { return m_right; }
        //    set
        //    {
        //        m_right = value;
        //        if (OnRightChanged != null)
        //        {
        //            OnRightChanged.Invoke(this, new EdgeChangedEventArg(value));
        //        }
        //    }
        //}

        public QuasiEdge(int firstVertex, int secondVertex)
        {
            this.first = firstVertex;

            this.second = secondVertex;
        }

        //public QuasiEdge(int firstVertex, int secondVertex, int leftPolygon, int rightPolygon)
        //{
        //    this.first = firstVertex;

        //    this.second = secondVertex;

        //    this.m_left = leftPolygon;

        //    this.m_right = rightPolygon;

        //    this.OnLeftChanged = null;

        //    this.OnRightChanged = null;
        //}

    }
}