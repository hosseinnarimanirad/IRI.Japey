// besmellahe rahmane rahim
// Allahoma ajjel le-valiyek al-faraj

using System;
using IRI.Sta.MeasurementUnit;
using System.Collections.Generic;
using IRI.Sta.Algebra;

namespace IRI.Sta.CoordinateSystem
{
    public class Projected<T> : IProjected
        where T : LinearUnit, new()
    {

        #region Fields

        private string m_Name;

        private AxisType m_Handedness;

        private const int m_Dimension = 2;

        private ILinearCollection m_X, m_Y;

        #endregion

        #region Properties

        public string Name
        {
            get { return this.m_Name; }
        }

        public int Dimension
        {
            get { return m_Dimension; }
        }

        public int NumberOfPoints
        {
            get { return this.X.Length; }
        }

        public AxisType Handedness
        {
            get { return this.m_Handedness; }
        }

        public LinearMode LinearMode
        {
            get { return this.X.Mode; }
        }

        public ILinearCollection X
        {
            get { return this.m_X; }
        }

        public ILinearCollection Y
        {
            get { return this.m_Y; }
        }

        #endregion

        public ICartesian2DPoint this[int index]
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public IEnumerator<ICartesian2DPoint> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public ICartesian2D Rotate(AngularUnit rotateAngle, RotateDirection direction)
        {
            throw new NotImplementedException();
        }

        public ICartesian2D Shift(ICartesian2DPoint newBase)
        {
            throw new NotImplementedException();
        }

        public ICartesian2D Clone()
        {
            throw new NotImplementedException();
        }

        public Matrix ToMatrix()
        {
            throw new NotImplementedException();
        }

        public Polar<TLinear, TAngular> ToPolar<TLinear, TAngular>(AngleRange range)
            where TLinear : LinearUnit, new()
            where TAngular : AngularUnit, new()
        {
            throw new NotImplementedException();
        }

        public Cartesian2D<TNewType> ChangeTo<TNewType>() where TNewType : LinearUnit, new()
        {
            throw new NotImplementedException();
        }

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
