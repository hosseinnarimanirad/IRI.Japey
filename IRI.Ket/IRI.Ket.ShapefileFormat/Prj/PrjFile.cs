using IRI.Sta.CoordinateSystem.MapProjection;
using IRI.Ket.Common.Extensions;
using IRI.Ket.ShapefileFormat.Prj;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ellipsoid = IRI.Sta.CoordinateSystem.Ellipsoid<IRI.Sta.MeasurementUnit.Meter, IRI.Sta.MeasurementUnit.Degree>;

namespace IRI.Ket.ShapefileFormat.Prj
{
    public class PrjFile
    {
        private PrjTree _root;

        #region Constants

        public const string _esriLambertConformalConic = "Lambert_Conformal_Conic";
        public const string _esriTransverseMercator = "Transverse_Mercator";
        public const string _esriMercator = "Mercator";
        public const string _esriAzimuthalEquidistant = "Azimuthal_Equidistant";
        public const string _esriCylindricalEqualArea = "Cylindrical_Equal_Area";
        public const string _esriWebMercator = "Mercator_Auxiliary_Sphere";

        public const string _projcs = "PROJCS";
        public const string _geogcs = "GEOGCS";
        public const string _spheroid = "SPHEROID";
        public const string _datum = "DATUM";
        public const string _projection = "PROJECTION";
        public const string _unit = "UNIT";
        public const string _parameter = "PARAMETER";
        public const string _primem = "PRIMEM";

        public const string _falseEasting = "False_Easting";
        public const string _falseNorthing = "False_Northing";
        public const string _centralMeridian = "Central_Meridian";
        public const string _scaleFactor = "Scale_Factor";
        public const string _latitudeOfOrigin = "Latitude_Of_Origin";
        public const string _standardParallel1 = "Standard_Parallel_1";
        public const string _standardParallel2 = "Standard_Parallel_2";
        public const string _auxiliarySphereType = "Auxiliary_Sphere_Type";
        public const string _greenwich = "Greenwich";
        public const string _degree = "Degree";
        public const string _degreeValue = "0.0174532925199433";

        #endregion

        public PrjFile(PrjTree root)
        {
            this._root = root;
        }

        public PrjFile(string prjFileName)
        {
            this._root = PrjTree.Parse(System.IO.File.ReadAllText(prjFileName));
        }

        public static PrjFile Parse(string esriWktPrj)
        {
            return new PrjFile(PrjTree.Parse(esriWktPrj));
        }

        //Prj file type: GEOGCS, PROJCS
        public EsriSrType Type
        {
            get
            {
                switch (_root?.Name)
                {
                    case _projcs:
                        return EsriSrType.Projcs;

                    case _geogcs:
                        return EsriSrType.Geogcs;

                    default:
                        throw new NotImplementedException();
                }
            }
        }

        //Projection Type
        public string ProjectionName
        {
            get
            {
                switch (Type)
                {
                    case EsriSrType.Projcs:
                        return _root.Children.Single(i => i.Name.EqualsIgnoreCase(_projection)).Values.First();

                    case EsriSrType.Geogcs:
                        return "None";

                    default:
                        throw new NotImplementedException();
                }
            }
        }

        public MapProjectionType ProjectionType
        {
            get
            {
                switch (ProjectionName)
                {
                    case _esriLambertConformalConic:
                        return MapProjectionType.LambertConformalConic;
                    case _esriTransverseMercator:
                        return MapProjectionType.TransverseMercator;
                    case _esriMercator:
                        return MapProjectionType.Mercator;
                    case _esriAzimuthalEquidistant:
                        return MapProjectionType.AzimuthalEquidistant;
                    case _esriCylindricalEqualArea:
                        return MapProjectionType.CylindricalEqualArea;
                    case _esriWebMercator:
                        return MapProjectionType.WebMercator;
                    case "None":
                        return MapProjectionType.None;
                    default:
                        throw new NotImplementedException();
                }
            }
        }


        public string Title
        {
            get { return _root?.Values?.First(); }
        }

        private PrjTree Geogcs
        {
            get
            {
                switch (Type)
                {
                    case EsriSrType.Projcs:
                        return _root.Children.Single(i => i.Name.EqualsIgnoreCase(_geogcs));

                    case EsriSrType.Geogcs:
                        return _root;

                    default:
                        throw new NotImplementedException();
                }
            }
        }

        private PrjTree Datum
        {
            get
            {
                return Geogcs.Children.Single(i => i.Name.EqualsIgnoreCase(_datum));
            }
        }


        //Ellipsoid
        public string EllipsoidName
        {
            get
            {
                return Datum.Children.Single(i => i.Name.EqualsIgnoreCase(_spheroid)).Values?.First();
            }
        }

        //EPSG not available
        public Ellipsoid Ellipsoid
        {
            get
            {
                var values = Datum.Children.Single(i => i.Name.EqualsIgnoreCase(_spheroid)).Values;

                return new Ellipsoid(values.First(),
                                        new Sta.MeasurementUnit.Meter(double.Parse(values.Skip(1).First(), CultureInfo.InvariantCulture)),
                                        double.Parse(values.Skip(2).First(), CultureInfo.InvariantCulture), int.MaxValue)
                {
                    EsriName = values.First()
                };
            }
        }


        public bool IsSISystem()
        {
            var isDegree = Geogcs.Children.Single(i => i.Name.EqualsIgnoreCase(_unit)).Values.First().ToLower() == "degree";

            switch (Type)
            {
                case EsriSrType.Projcs:
                    return isDegree && _root.Children.Single(i => i.Name.EqualsIgnoreCase(_unit)).Values.First().ToLower() == "meter";

                case EsriSrType.Geogcs:
                    return isDegree;

                default:
                    throw new NotImplementedException();
            }
        }

        //public string TypeName
        //{
        //    get
        //    {
        //        return _root.Name;
        //    }
        //}

        private bool HasParameter(EsriPrjParameterType parameter)
        {
            switch (parameter)
            {
                case EsriPrjParameterType.FalseEasting:
                    return HasParameter(_falseEasting);

                case EsriPrjParameterType.FalseNorthing:
                    return HasParameter(_falseNorthing);

                case EsriPrjParameterType.CentralMeridian:
                    return HasParameter(_centralMeridian);

                case EsriPrjParameterType.ScaleFactor:
                    return HasParameter(_scaleFactor);

                case EsriPrjParameterType.LatitudeOfOrigin:
                    return HasParameter(_latitudeOfOrigin);

                case EsriPrjParameterType.StandardParallel_1:
                    return HasParameter(_standardParallel1);

                case EsriPrjParameterType.StandardParallel_2:
                    return HasParameter(_standardParallel2);

                default:
                    throw new NotImplementedException();
            }
        }

        private bool HasParameter(string parameterName)
        {
            var parameters = _root.Children.Where(i => i.Name.EqualsIgnoreCase(_parameter)).ToList();

            return parameters.Any(i => i.Values.First().EqualsIgnoreCase(parameterName));
        }

        private double GetParameter(EsriPrjParameterType parameter, double defaultValue)
        {
            if (!HasParameter(parameter))
            {
                return defaultValue;
            }

            switch (parameter)
            {
                case EsriPrjParameterType.FalseEasting:
                    return GetParameter(_falseEasting);

                case EsriPrjParameterType.FalseNorthing:
                    return GetParameter(_falseNorthing);

                case EsriPrjParameterType.CentralMeridian:
                    return GetParameter(_centralMeridian);

                case EsriPrjParameterType.ScaleFactor:
                    return GetParameter(_scaleFactor);

                case EsriPrjParameterType.LatitudeOfOrigin:
                    return GetParameter(_latitudeOfOrigin);

                case EsriPrjParameterType.StandardParallel_1:
                    return GetParameter(_standardParallel1);

                case EsriPrjParameterType.StandardParallel_2:
                    return GetParameter(_standardParallel2);

                default:
                    throw new NotImplementedException();
            }
        }

        private double GetParameter(string parameterName)
        {
            var parameters = _root.Children.Where(i => i.Name.EqualsIgnoreCase(_parameter)).ToList();

            return double.Parse(parameters.Single(i => i.Values.First().EqualsIgnoreCase(parameterName)).Values.Skip(1).First());
        }

        public CoordinateReferenceSystemBase AsMapProjection()
        {
            switch (ProjectionType)
            {
                case MapProjectionType.None:
                    return new NoProjection(this.Title, this.Ellipsoid) { DatumName = this.Geogcs.Values?.First() };

                case MapProjectionType.AlbersEqualAreaConic:
                case MapProjectionType.AzimuthalEquidistant:
                    throw new NotImplementedException();

                case MapProjectionType.CylindricalEqualArea:
                    return new CylindricalEqualArea(this.Title, this.Ellipsoid) { DatumName = this.Geogcs.Values?.First() };

                case MapProjectionType.LambertConformalConic:
                    return new LambertConformalConic(
                        this.Ellipsoid,
                        GetParameter(EsriPrjParameterType.StandardParallel_1, double.NaN),
                        GetParameter(EsriPrjParameterType.StandardParallel_2, double.NaN),
                        GetParameter(EsriPrjParameterType.CentralMeridian, 0),
                        GetParameter(EsriPrjParameterType.LatitudeOfOrigin, 0),
                        GetParameter(EsriPrjParameterType.FalseEasting, 0),
                        GetParameter(EsriPrjParameterType.FalseNorthing, 0),
                        GetParameter(EsriPrjParameterType.ScaleFactor, 1))
                    {
                        Title = this.Title,
                        DatumName = this.Geogcs.Values?.First()
                    };

                case MapProjectionType.Mercator:
                    return new Mercator(this.Ellipsoid)
                    {
                        Title = this.Title,
                        DatumName = this.Geogcs.Values?.First()
                    };

                case MapProjectionType.TransverseMercator:
                case MapProjectionType.UTM:
                    return new TransverseMercator(
                        this.Ellipsoid,
                        GetParameter(EsriPrjParameterType.CentralMeridian, 0),
                        GetParameter(EsriPrjParameterType.LatitudeOfOrigin, 0),
                        GetParameter(EsriPrjParameterType.FalseEasting, 0),
                        GetParameter(EsriPrjParameterType.FalseNorthing, 0),
                        GetParameter(EsriPrjParameterType.ScaleFactor, 1))
                    {
                        Title = this.Title,
                        DatumName = this.Geogcs.Values?.First()
                    };

                case MapProjectionType.WebMercator:
                    return new WebMercator()
                    {
                        Title = this.Title,
                        DatumName = this.Geogcs.Values?.First()
                    };

                default:
                    throw new NotImplementedException();
            }
        }

        public string AsEsriCrsWkt()
        {
            return _root.AsEsriCrsWkt();
        }

        public void Save(string prjFileName)
        {
            if (System.IO.Path.GetExtension(prjFileName).ToLower() != ".prj")
                throw new NotImplementedException();

            System.IO.File.WriteAllText(prjFileName, AsEsriCrsWkt());
        }
    }
}
