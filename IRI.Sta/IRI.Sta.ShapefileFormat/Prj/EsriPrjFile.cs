using IRI.Msh.CoordinateSystem.MapProjection;
using IRI.Extensions;
using IRI.Sta.ShapefileFormat.Prj;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ellipsoid = IRI.Msh.CoordinateSystem.Ellipsoid<IRI.Msh.MeasurementUnit.Meter, IRI.Msh.MeasurementUnit.Degree>;
using IRI.Msh.MeasurementUnit;

namespace IRI.Sta.ShapefileFormat.Prj
{
    public class EsriPrjFile
    {
        private EsriPrjTreeNode _root;

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
        public const string _authority = "AUTHORITY";
        public const string _toWgs84 = "TOWGS84";

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
        public const string _epsg = "EPSG";

        #endregion

        public EsriPrjFile(EsriPrjTreeNode root)
        {
            this._root = root;

            //sample: AUTHORITY["EPSG", "4326"]
            //var authorityInfo = root.Children.SingleOrDefault(i => i.Name == _authority)?.Values;

            //int srid = 0;

            //if (authorityInfo != null && authorityInfo.Count == 2 && authorityInfo?[0] == _epsg)
            //{
            //    int.TryParse(authorityInfo[1], out srid);
            //}

            this._srid = GetCrsSrid();
        }

        public EsriPrjFile(string prjFileName)
        {
            this._root = EsriPrjTreeNode.Parse(System.IO.File.ReadAllText(prjFileName));

            this._srid = GetCrsSrid();

        }

        public static EsriPrjFile Parse(string esriWktPrj)
        {
            return new EsriPrjFile(EsriPrjTreeNode.Parse(esriWktPrj));
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

        private EsriPrjTreeNode Geogcs
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

        private EsriPrjTreeNode Datum
        {
            get
            {
                return Geogcs.Children.Single(i => i.Name.EqualsIgnoreCase(_datum));
            }
        }

        private int _srid;
        public int Srid
        {
            get { return _srid; }
        }

        //Ellipsoid
        public string EllipsoidName
        {
            get
            {
                return Datum.Children.Single(i => i.Name.EqualsIgnoreCase(_spheroid)).Values?.First();
            }
        }

        //TODO: TEST EPSG
        public Ellipsoid Ellipsoid
        {
            get
            {
                var spheroidValues = Datum.Children.Single(i => i.Name.EqualsIgnoreCase(_spheroid)).Values;

                var toWgs84Values = Datum.Children.SingleOrDefault(i => i.Name.EqualsIgnoreCase(_toWgs84))?.Values;

                var srid = GetEllipsoidSrid();

                if (toWgs84Values == null)
                {
                    return new Ellipsoid(spheroidValues.First(),
                                        new Msh.MeasurementUnit.Meter(double.Parse(spheroidValues.Skip(1).First(), CultureInfo.InvariantCulture)),
                                        double.Parse(spheroidValues.Skip(2).First(), CultureInfo.InvariantCulture), srid)
                    {
                        EsriName = spheroidValues.First(),
                    };
                }
                else
                {
                    var dx = double.Parse(toWgs84Values[0], CultureInfo.InvariantCulture);
                    var dy = double.Parse(toWgs84Values[1], CultureInfo.InvariantCulture);
                    var dz = double.Parse(toWgs84Values[2], CultureInfo.InvariantCulture);

                    var drx = double.Parse(toWgs84Values[3], CultureInfo.InvariantCulture);
                    var dry = double.Parse(toWgs84Values[4], CultureInfo.InvariantCulture);
                    var drz = double.Parse(toWgs84Values[5], CultureInfo.InvariantCulture);

                    return new Ellipsoid(spheroidValues.First(),
                                        new Msh.MeasurementUnit.Meter(double.Parse(spheroidValues.Skip(1).First(), CultureInfo.InvariantCulture)),
                                        double.Parse(spheroidValues.Skip(2).First(), CultureInfo.InvariantCulture),
                                        new IRI.Msh.CoordinateSystem.Cartesian3DPoint<Meter>(new Meter(dx), new Meter(dy), new Meter(dz)),
                                        new Msh.CoordinateSystem.OrientationParameter(new Degree(drx), new Degree(dry), new Degree(drz)),
                                        srid)
                    {
                        EsriName = spheroidValues.First(),
                    };
                }


            }
        }

        private int GetCrsSrid()
        {
            var crsAuthorityNode = _root.Children.SingleOrDefault(i => i.Name == _authority);

            var srid = GetSridFromAuthorityNode(crsAuthorityNode);

            //1399.06.13
            //in the authority field was not available
            if (srid == 0)
            {
                if (Type == EsriSrType.Geogcs && Ellipsoid.Name.ToUpper() == "WGS_1984")
                {
                    srid = SridHelper.GeodeticWGS84;
                }
            }

            return srid;
        }

        private int GetEllipsoidSrid()
        {
            var ellipsoidAuthorityNode = Geogcs.Children.SingleOrDefault(i => i.Name.EqualsIgnoreCase(_authority));

            return GetSridFromAuthorityNode(ellipsoidAuthorityNode);
        }

        private int GetSridFromAuthorityNode(EsriPrjTreeNode authorityNode)
        {
            int srid = 0;

            if (authorityNode?.Values?.Count == 2 && authorityNode?.Values?[0] == _epsg)
            {
                int.TryParse(authorityNode.Values[1], out srid);
            }

            return srid;
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

        public SrsBase AsMapProjection()
        {
            switch (ProjectionType)
            {
                case MapProjectionType.None:
                    return new NoProjection(this.Title, this.Ellipsoid) { DatumName = this.Geogcs.Values?.First() };

                case MapProjectionType.AlbersEqualAreaConic:
                case MapProjectionType.AzimuthalEquidistant:
                    throw new NotImplementedException();

                case MapProjectionType.CylindricalEqualArea:
                    return new CylindricalEqualArea(this.Title, this.Ellipsoid, Srid) { DatumName = this.Geogcs.Values?.First() };

                case MapProjectionType.LambertConformalConic:
                    return new LambertConformalConic2P(
                        this.Ellipsoid,
                        GetParameter(EsriPrjParameterType.StandardParallel_1, double.NaN),
                        GetParameter(EsriPrjParameterType.StandardParallel_2, double.NaN),
                        GetParameter(EsriPrjParameterType.CentralMeridian, 0),
                        GetParameter(EsriPrjParameterType.LatitudeOfOrigin, 0),
                        GetParameter(EsriPrjParameterType.FalseEasting, 0),
                        GetParameter(EsriPrjParameterType.FalseNorthing, 0),
                        GetParameter(EsriPrjParameterType.ScaleFactor, 1),
                        Srid)
                    {
                        Title = this.Title,
                        DatumName = this.Geogcs.Values?.First()
                    };

                case MapProjectionType.Mercator:
                    return new Mercator(this.Ellipsoid, Srid)
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
                        GetParameter(EsriPrjParameterType.ScaleFactor, 1),
                        Srid)
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
