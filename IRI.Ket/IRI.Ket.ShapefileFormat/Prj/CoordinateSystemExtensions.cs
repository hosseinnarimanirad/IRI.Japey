using IRI.Sta.CoordinateSystem.MapProjection;
using IRI.Ket.ShapefileFormat.Prj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.ShapefileFormat
{
    public static class CoordinateSystemExtensions
    {
        public static PrjFile AsEsriPrj(this CoordinateReferenceSystemBase mapProjection)
        {
            switch (mapProjection.Type)
            {
                case MapProjectionType.None:
                    return new PrjFile(new PrjTree(mapProjection.Ellipsoid, mapProjection.Title));

                case MapProjectionType.AlbersEqualAreaConic:
                case MapProjectionType.AzimuthalEquidistant:
                    throw new NotImplementedException();

                case MapProjectionType.CylindricalEqualArea:
                    return AsEsriPrjFile((CylindricalEqualArea)mapProjection);

                case MapProjectionType.LambertConformalConic:
                    return AsEsriPrjFile((LambertConformalConic)mapProjection);

                case MapProjectionType.Mercator:
                    return AsEsriPrjFile((Mercator)mapProjection);

                case MapProjectionType.TransverseMercator:
                    return AsEsriPrjFile((TransverseMercator)mapProjection);

                case MapProjectionType.UTM:
                    return AsEsriPrjFile((UTM)mapProjection);

                case MapProjectionType.WebMercator:
                    return AsEsriPrjFile((WebMercator)mapProjection);

                default:
                    throw new NotImplementedException();
            }
        }

        private static PrjFile AsEsriPrjFile(LambertConformalConic lcc)
        {
            PrjTree root = new PrjTree();

            root.Name = PrjFile._projcs;

            root.Values = new List<string>() { string.IsNullOrWhiteSpace(lcc.Title) ? "LAMBERT CONFORMAL CONIC" : lcc.Title };

            var geogcs = new PrjTree(lcc.Ellipsoid, lcc.DatumName);

            var projection = new PrjTree(PrjFile._projection, PrjFile._esriLambertConformalConic);

            var falseEasting = new PrjTree(PrjFile._parameter, PrjFile._falseEasting, lcc.FalseEasting.AsExactString());

            var falseNorthing = new PrjTree(PrjFile._parameter, PrjFile._falseNorthing, lcc.FalseNorthing.AsExactString());

            var centralMeridian = new PrjTree(PrjFile._parameter, PrjFile._centralMeridian, lcc.CentralMeridian.AsExactString());

            var standardParallel1 = new PrjTree(PrjFile._parameter, PrjFile._standardParallel1, lcc.StandardParallel1.AsExactString());

            var standardParallel2 = new PrjTree(PrjFile._parameter, PrjFile._standardParallel2, lcc.StandardParallel2.AsExactString());

            var scaleFactor = new PrjTree(PrjFile._parameter, PrjFile._scaleFactor, lcc.StandardParallel2.AsExactString());

            var latitudeOfOrigin = new PrjTree(PrjFile._parameter, PrjFile._latitudeOfOrigin, lcc.StandardParallel2.AsExactString());

            var unit = PrjTree.MeterUnit;

            root.Children = new List<PrjTree>() { geogcs, projection, falseEasting, falseNorthing, centralMeridian, standardParallel1, standardParallel2, scaleFactor, latitudeOfOrigin, unit };

            return new PrjFile(root);
        }

        private static PrjFile AsEsriPrjFile(UTM utm)
        {
            PrjTree root = new PrjTree();

            root.Name = PrjFile._projcs;

            string ns = utm.FalseNorthing == 0 ? "N" : "S";

            var zone = IRI.Sta.CoordinateSystem.MapProjection.MapProjects.FindZone(utm.CentralMeridian);

            root.Values = new List<string>() { string.IsNullOrWhiteSpace(utm.Title) ? $"WGS_1984_UTM_Zone_{zone}{ns}" : utm.Title };

            var geogcs = new PrjTree(utm.Ellipsoid, utm.DatumName);

            var projection = new PrjTree(PrjFile._projection, PrjFile._esriTransverseMercator);

            var falseEasting = new PrjTree(PrjFile._parameter, PrjFile._falseEasting, utm.FalseEasting.AsExactString());

            var falseNorthing = new PrjTree(PrjFile._parameter, PrjFile._falseNorthing, utm.FalseNorthing.AsExactString());

            var centralMeridian = new PrjTree(PrjFile._parameter, PrjFile._centralMeridian, utm.CentralMeridian.AsExactString());

            var scaleFactor = new PrjTree(PrjFile._parameter, PrjFile._scaleFactor, utm.ScaleFactor.AsExactString());

            var latitudeOfOrigin = new PrjTree(PrjFile._parameter, PrjFile._latitudeOfOrigin, utm.StandardParallel2.AsExactString());

            var unit = PrjTree.MeterUnit;

            root.Children = new List<PrjTree>() { geogcs, projection, falseEasting, falseNorthing, centralMeridian, scaleFactor, latitudeOfOrigin, unit };

            return new PrjFile(root);
        }

        private static PrjFile AsEsriPrjFile(WebMercator webMercator)
        {
            PrjTree root = new PrjTree();

            root.Name = PrjFile._projcs;

            root.Values = new List<string>() { string.IsNullOrWhiteSpace(webMercator.Title) ? $"{webMercator.Ellipsoid.EsriName}_Web_Mercator_Auxiliary_Sphere" : webMercator.Title };

            var geogcs = new PrjTree(webMercator.Ellipsoid, webMercator.DatumName);

            var projection = new PrjTree(PrjFile._projection, PrjFile._esriWebMercator);

            var falseEasting = new PrjTree(PrjFile._parameter, PrjFile._falseEasting, webMercator.FalseEasting.AsExactString());

            var falseNorthing = new PrjTree(PrjFile._parameter, PrjFile._falseNorthing, webMercator.FalseNorthing.AsExactString());

            var centralMeridian = new PrjTree(PrjFile._parameter, PrjFile._centralMeridian, webMercator.CentralMeridian.AsExactString());

            var standardParallel1 = new PrjTree(PrjFile._parameter, PrjFile._standardParallel1, webMercator.StandardParallel1.AsExactString());

            var auxiliarySphereType = new PrjTree(PrjFile._parameter, PrjFile._auxiliarySphereType, webMercator.AuxiliarySphereType.AsExactString());

            var unit = PrjTree.MeterUnit;

            root.Children = new List<PrjTree>() { geogcs, projection, falseEasting, falseNorthing, centralMeridian, standardParallel1, auxiliarySphereType, unit };

            return new PrjFile(root);
        }

        private static PrjFile AsEsriPrjFile(Mercator mercator)
        {
            PrjTree root = new PrjTree();

            root.Name = PrjFile._projcs;

            root.Values = new List<string>() { string.IsNullOrWhiteSpace(mercator.Title) ? "WGS_1984_World_Mercator" : mercator.Title };

            var geogcs = new PrjTree(mercator.Ellipsoid, mercator.DatumName);

            var projection = new PrjTree(PrjFile._projection, PrjFile._esriMercator);

            var falseEasting = new PrjTree(PrjFile._parameter, PrjFile._falseEasting, mercator.FalseEasting.AsExactString());

            var falseNorthing = new PrjTree(PrjFile._parameter, PrjFile._falseNorthing, mercator.FalseNorthing.AsExactString());

            var centralMeridian = new PrjTree(PrjFile._parameter, PrjFile._centralMeridian, mercator.CentralMeridian.AsExactString());

            var standardParallel1 = new PrjTree(PrjFile._parameter, PrjFile._standardParallel1, mercator.StandardParallel1.AsExactString());

            var unit = PrjTree.MeterUnit;

            root.Children = new List<PrjTree>() { geogcs, projection, falseEasting, falseNorthing, centralMeridian, standardParallel1, unit };

            return new PrjFile(root);
        }

        private static PrjFile AsEsriPrjFile(CylindricalEqualArea cylindricalEqualArea)
        {
            PrjTree root = new PrjTree();

            root.Name = PrjFile._projcs;

            root.Values = new List<string>() { string.IsNullOrWhiteSpace(cylindricalEqualArea.Title) ? "World_Cylindrical_Equal_Area" : cylindricalEqualArea.Title };

            var geogcs = new PrjTree(cylindricalEqualArea.Ellipsoid, cylindricalEqualArea.DatumName);

            var projection = new PrjTree(PrjFile._projection, PrjFile._esriCylindricalEqualArea);

            var falseEasting = new PrjTree(PrjFile._parameter, PrjFile._falseEasting, cylindricalEqualArea.FalseEasting.AsExactString());

            var falseNorthing = new PrjTree(PrjFile._parameter, PrjFile._falseNorthing, cylindricalEqualArea.FalseNorthing.AsExactString());

            var centralMeridian = new PrjTree(PrjFile._parameter, PrjFile._centralMeridian, cylindricalEqualArea.CentralMeridian.AsExactString());

            var standardParallel1 = new PrjTree(PrjFile._parameter, PrjFile._standardParallel1, cylindricalEqualArea.StandardParallel1.AsExactString());

            var unit = PrjTree.MeterUnit;

            root.Children = new List<PrjTree>() { geogcs, projection, falseEasting, falseNorthing, centralMeridian, standardParallel1, unit };

            return new PrjFile(root);
        }

        private static PrjFile AsEsriPrjFile(TransverseMercator transverseMercator)
        {
            PrjTree root = new PrjTree();

            root.Name = PrjFile._projcs;

            root.Values = new List<string>() { string.IsNullOrWhiteSpace(transverseMercator.Title) ? "Transverse Mercator" : transverseMercator.Title };

            var geogcs = new PrjTree(transverseMercator.Ellipsoid, transverseMercator.DatumName);

            var projection = new PrjTree(PrjFile._projection, PrjFile._esriTransverseMercator);

            var falseEasting = new PrjTree(PrjFile._parameter, PrjFile._falseEasting, transverseMercator.FalseEasting.AsExactString());

            var falseNorthing = new PrjTree(PrjFile._parameter, PrjFile._falseNorthing, transverseMercator.FalseNorthing.AsExactString());

            var centralMeridian = new PrjTree(PrjFile._parameter, PrjFile._centralMeridian, transverseMercator.CentralMeridian.AsExactString());

            var scaleFactor = new PrjTree(PrjFile._parameter, PrjFile._scaleFactor, transverseMercator.ScaleFactor.AsExactString());

            var latitudeOfOrigin = new PrjTree(PrjFile._parameter, PrjFile._latitudeOfOrigin, transverseMercator.StandardParallel2.AsExactString());

            var unit = PrjTree.MeterUnit;

            root.Children = new List<PrjTree>() { geogcs, projection, falseEasting, falseNorthing, centralMeridian, scaleFactor, latitudeOfOrigin, unit };

            return new PrjFile(root);
        }

    }
}
