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

        private Cartesian3DPoint<TLinear> _datumTranslation;

        //private AngularUnit m_Omega, m_Phi, m_Kappa;

        private OrientationParameter _datumMisalignment;

        private LinearUnit _semiMajorAxis;

        private LinearUnit _semiMinorAxis;

        private string _name;

        private double _firstEccentricity;

        private double _secondEccentricity;

        private int _srid;

        #endregion

        #region Properties

        public ICartesian3DPoint DatumTranslation
        {
            get { return this._datumTranslation; }
        }

        public OrientationParameter DatumMisalignment
        {
            get { return this._datumMisalignment; }
        }

        public LinearUnit SemiMajorAxis
        {
            get { return _semiMajorAxis; }
        }

        public LinearUnit SemiMinorAxis
        {
            get { return _semiMinorAxis; }
        }

        public string Name
        {
            get { return _name; }
        }

        public string EsriName { get; set; }

        public double FirstEccentricity
        {
            get { return _firstEccentricity; }
        }

        public double SecondEccentricity
        {
            get { return _secondEccentricity; }
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

        public int Srid
        {
            get
            {
                return _srid;
            }
        }

        #endregion

        #region Constructors

        public Ellipsoid(string name, LinearUnit semiMajorAxis, double inverseFlattening, int srid)
            : this(name,
                    semiMajorAxis,
                    inverseFlattening,
                    new Cartesian3DPoint<TLinear>(new TLinear(), new TLinear(), new TLinear()),
                    new OrientationParameter(new Radian(), new Radian(), new Radian()),
                    srid)
        { }

        public Ellipsoid(string name, LinearUnit semiMajorAxis, LinearUnit semiMinorAxis,
                            ICartesian3DPoint datumTranslation, OrientationParameter datumMisalignment, int srid)
            : this(name,
                    semiMajorAxis,
                    1.0 / ((semiMajorAxis.Subtract(semiMinorAxis)).Divide(semiMajorAxis)).Value,
                    datumTranslation,
                    datumMisalignment,
                    srid)
        { }

        public Ellipsoid(string name, LinearUnit semiMajorAxis, double inverseFlattening,
                            ICartesian3DPoint datumTranslation, OrientationParameter datumMisalignment, int srid)
        {
            this._datumTranslation = new Cartesian3DPoint<TLinear>(datumTranslation.X, datumTranslation.Y, datumTranslation.Z);

            this._datumMisalignment = new OrientationParameter(datumMisalignment.Omega.ChangeTo<TAngular>(),
                                                                datumMisalignment.Phi.ChangeTo<TAngular>(),
                                                                datumMisalignment.Kappa.ChangeTo<TAngular>());

            this._name = name;

            this._srid = srid;

            this._semiMajorAxis = semiMajorAxis.ChangeTo<TLinear>();

            double tempSemiMajor = this._semiMajorAxis.Value;

            if (inverseFlattening == 0)
            {
                this._semiMinorAxis = new TLinear() { Value = tempSemiMajor };
            }
            else
            {
                this._semiMinorAxis = new TLinear() { Value = tempSemiMajor - tempSemiMajor / inverseFlattening };
            }

            double tempSemiMinor = this._semiMinorAxis.Value;

            this._firstEccentricity = Math.Sqrt((tempSemiMajor * tempSemiMajor - tempSemiMinor * tempSemiMinor)
                                                   /
                                                (tempSemiMajor * tempSemiMajor));

            this._secondEccentricity = Math.Sqrt((tempSemiMajor * tempSemiMajor - tempSemiMinor * tempSemiMinor)
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

            return (this._semiMajorAxis.Value
                           /
                           Math.Sqrt(1 - this.FirstEccentricity * this.FirstEccentricity * sin * sin));
        }

        public LinearUnit CalculateN(AngularUnit Latitude)
        {
            TLinear result = new TLinear();

            result.Value = (this._semiMajorAxis.Value
                            /
                            Math.Sqrt(1 - this.FirstEccentricity * this.FirstEccentricity * Latitude.Sin * Latitude.Sin));

            return result;
        }

        public LinearUnit CalculateM(AngularUnit Latitude)
        {
            TLinear result = new TLinear();

            result.Value = (this._semiMajorAxis.Value * (1 - this.FirstEccentricity * this.FirstEccentricity)
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
                                                                this.DatumMisalignment,
                                                                this.Srid);
        }

        public Ellipsoid<TLinear, TAngular> GetGeocentricVersion(int newSrid)
        {
            return new Ellipsoid<TLinear, TAngular>(this.Name + "_Geocentric", this.SemiMajorAxis, this.InverseFlattening, newSrid);
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
