// besmellahe rahmane rahim
// Allahoma ajjel le-valiyek al-faraj

using System;
using IRI.Ham.MeasurementUnit;
using System.Collections.Generic;

namespace IRI.Ham.CoordinateSystem
{
    public struct Ellipsoid<TLinear, TAngular> : IEllipsoid
        where TLinear : LinearUnit, new()
        where TAngular : AngularUnit, new()
    {
        #region Fields

        private Cartesian3DPoint<TLinear> m_DatumTranslation;

        //private AngularUnit m_Omega, m_Phi, m_Kappa;

        private OrientationParameter m_DatumMisalignment;

        private LinearUnit m_SemiMajorAxis;

        private LinearUnit m_SemiMinorAxis;

        private string m_Name;

        private double m_FirstEccentricity;

        private double m_SecondEccentricity;

        #endregion

        #region Properties

        public ICartesian3DPoint DatumTranslation
        {
            get { return this.m_DatumTranslation; }
        }

        public OrientationParameter DatumMisalignment
        {
            get { return this.m_DatumMisalignment; }
        }

        public LinearUnit SemiMajorAxis
        {
            get { return m_SemiMajorAxis; }
        }

        public LinearUnit SemiMinorAxis
        {
            get { return m_SemiMinorAxis; }
        }

        public string Name
        {
            get { return m_Name; }
        }

        public string EsriName { get; set; }

        public double FirstEccentricity
        {
            get { return m_FirstEccentricity; }
        }

        public double SecondEccentricity
        {
            get { return m_SecondEccentricity; }
        }

        public double Flattening
        {
            get
            {
                return ((this.SemiMajorAxis.Value - this.SemiMinorAxis.Value) / this.SemiMajorAxis.Value);
            }
        }

        public double InverseFlattening
        {
            get
            {
                return (this.SemiMajorAxis.Value / (this.SemiMajorAxis.Value - this.SemiMinorAxis.Value));
            }
        }

        #endregion

        #region Constructors

        public Ellipsoid(string name, LinearUnit semiMajorAxis, double inverseFlattening)
            : this(name,
                    semiMajorAxis,
                    inverseFlattening,
                    new Cartesian3DPoint<TLinear>(new TLinear(), new TLinear(), new TLinear()),
                    new OrientationParameter(new Radian(), new Radian(), new Radian()))
        { }

        public Ellipsoid(string name, LinearUnit semiMajorAxis, LinearUnit semiMinorAxis,
                            ICartesian3DPoint datumTranslation, OrientationParameter datumMisalignment)
            : this(name,
                    semiMajorAxis,
                    1 / ((semiMajorAxis.Subtract(semiMinorAxis)).Divide(semiMajorAxis)).Value,
                    datumTranslation,
                    datumMisalignment)
        { }

        public Ellipsoid(string name, LinearUnit semiMajorAxis, double inverseFlattening,
                            ICartesian3DPoint datumTranslation, OrientationParameter datumMisalignment)
        {
            this.m_DatumTranslation = new Cartesian3DPoint<TLinear>(datumTranslation.X, datumTranslation.Y, datumTranslation.Z);

            this.m_DatumMisalignment = new OrientationParameter(datumMisalignment.Omega.ChangeTo<TAngular>(),
                                                                datumMisalignment.Phi.ChangeTo<TAngular>(),
                                                                datumMisalignment.Kappa.ChangeTo<TAngular>());

            this.m_Name = name;

            this.m_SemiMajorAxis = semiMajorAxis.ChangeTo<TLinear>();

            double tempSemiMajor = this.m_SemiMajorAxis.Value;

            if (inverseFlattening == 0)
            {
                this.m_SemiMinorAxis = new TLinear() { Value = tempSemiMajor };
            }
            else
            {
                this.m_SemiMinorAxis = new TLinear() { Value = tempSemiMajor - tempSemiMajor / inverseFlattening };
            }

            double tempSemiMinor = this.m_SemiMinorAxis.Value;

            this.m_FirstEccentricity = Math.Sqrt((tempSemiMajor * tempSemiMajor - tempSemiMinor * tempSemiMinor)
                                                   /
                                                (tempSemiMajor * tempSemiMajor));

            this.m_SecondEccentricity = Math.Sqrt((tempSemiMajor * tempSemiMajor - tempSemiMinor * tempSemiMinor)
                                                   /
                                                 (tempSemiMinor * tempSemiMinor));

            this.EsriName = string.Empty;
        }

        #endregion

        #region Methods

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="latitudeInRadian">Latitude Value in Radian</param>
        ///// <returns></returns>
        //public double CalculateN(double latitudeInRadian)
        //{
        //    var sin = Math.Sin(latitudeInRadian);

        //    return  (this.m_SemiMajorAxis.Value
        //                    /
        //            Math.Sqrt(1 - this.FirstEccentricity * this.FirstEccentricity * sin * sin));
        //}

        public double CalculateN(double Latitude)
        {
            double sin = Math.Sin(Latitude * Math.PI / 180);

            return (this.m_SemiMajorAxis.Value
                           /
                           Math.Sqrt(1 - this.FirstEccentricity * this.FirstEccentricity * sin * sin));
        }

        public LinearUnit CalculateN(AngularUnit Latitude)
        {
            TLinear result = new TLinear();

            result.Value = (this.m_SemiMajorAxis.Value
                            /
                            Math.Sqrt(1 - this.FirstEccentricity * this.FirstEccentricity * Latitude.Sin * Latitude.Sin));

            return result;
        }

        public LinearUnit CalculateM(AngularUnit Latitude)
        {
            TLinear result = new TLinear();

            result.Value = (this.m_SemiMajorAxis.Value * (1 - this.FirstEccentricity * this.FirstEccentricity)
                            /
                            Math.Pow((1 - this.FirstEccentricity * this.FirstEccentricity * Latitude.Sin * Latitude.Sin), 3.0 / 2.0));

            return result;
        }

        public bool AreTheSame(IEllipsoid other)
        {
            return
                other.SemiMajorAxis.GetType() == this.SemiMajorAxis.GetType() &&
                this.SemiMajorAxis.Value == other.SemiMajorAxis.Value &&
                    this.FirstEccentricity == other.FirstEccentricity;
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(IEllipsoid))
            {
                return this == (IEllipsoid)obj;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public override string ToString()
        {
            return this.Name;
        }

        public Ellipsoid<TNewLinearType, TNewAngularType> ChangeTo<TNewLinearType, TNewAngularType>()
            where TNewLinearType : LinearUnit, new()
            where TNewAngularType : AngularUnit, new()
        {
            return new Ellipsoid<TNewLinearType, TNewAngularType>(string.Empty,
                                                                this.SemiMajorAxis,
                                                                this.SemiMinorAxis,
                                                                this.DatumTranslation,
                                                                this.DatumMisalignment);
        }

        public Ellipsoid<TLinear, TAngular> GetGeocentricVersion()
        {
            return new Ellipsoid<TLinear, TAngular>(this.Name + "_Geocentric", this.SemiMajorAxis, this.InverseFlattening);
        }

        #endregion

        public static bool operator ==(Ellipsoid<TLinear, TAngular> firstEllipsoid, IEllipsoid secondEllipsoid)
        {
            return (firstEllipsoid.DatumTranslation == secondEllipsoid.DatumTranslation &&
                    firstEllipsoid.DatumMisalignment == secondEllipsoid.DatumMisalignment &&
                    firstEllipsoid.Name == secondEllipsoid.Name &&
                    firstEllipsoid.SemiMajorAxis == secondEllipsoid.SemiMajorAxis &&
                    firstEllipsoid.SemiMinorAxis == secondEllipsoid.SemiMinorAxis);
        }

        public static bool operator !=(Ellipsoid<TLinear, TAngular> firstEllipsoid, IEllipsoid secondEllipsoid)
        {
            return !(firstEllipsoid == secondEllipsoid);
        }

    }
}
