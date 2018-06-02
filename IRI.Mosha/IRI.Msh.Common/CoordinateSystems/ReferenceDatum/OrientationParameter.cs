// besmellahe rahmane rahim
// Allahoma ajjel le-valiyek al-faraj

using System;
using IRI.Sta.MeasurementUnit;
using System.Collections.Generic;

namespace IRI.Sta.CoordinateSystem
{
    public struct OrientationParameter
    {
        private AngularUnit m_Omega, m_Phi, m_Kappa;

        public AngularUnit Omega
        {
            get { return this.m_Omega; }
        }

        public AngularUnit Phi
        {
            get { return this.m_Omega; }
        }

        public AngularUnit Kappa
        {
            get { return this.m_Omega; }
        }

        public OrientationParameter(AngularUnit omega, AngularUnit phi, AngularUnit kappa)
        {
            this.m_Omega = omega;

            this.m_Phi = phi;

            this.m_Kappa = kappa;
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(OrientationParameter))
            {
                return this == (OrientationParameter)obj;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public override string ToString()
        {
            return string.Format(System.Globalization.CultureInfo.InvariantCulture,
                                    "Omega: {0}, Phi: {1}, Kappa: {2}",
                                    this.Omega,
                                    this.Phi,
                                    this.Kappa);
        }

        public static bool operator ==(OrientationParameter firstValue, OrientationParameter secondValue)
        {
            return (firstValue.Omega == secondValue.Omega &&
                    firstValue.Phi == secondValue.Phi &&
                    firstValue.Kappa == secondValue.Kappa);
        }

        public static bool operator !=(OrientationParameter firstValue, OrientationParameter secondValue)
        {
            return !(firstValue == secondValue);
        }
    }
}
