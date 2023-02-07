using IRI.Ket.SpatialExtensions;
using IRI.Ket.SqlServerSpatialExtension.Model;
using IRI.Msh.Common.Primitives;
using IRI.Msh.CoordinateSystem.MapProjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRI.Msh.Common.Extensions;

namespace IRI.Ket.SqlServerSpatialExtension.Helpers
{
    public static class ShapefileHelper
    {
        public static List<SqlFeature> ReadAsSqlFeature(string shpFileName, Encoding dataEncoding, SrsBase targetSrs = null, Encoding headerEncoding = null, bool correctFarsiCharacters = true, string label = null)
        {
            if (targetSrs != null)
            {
                var sourceSrs = IRI.Ket.ShapefileFormat.Shapefile.TryGetSrs(shpFileName);

                Func<Point, Point> map = p => p.Project(sourceSrs, targetSrs);

                return IRI.Ket.ShapefileFormat.Shapefile.Read<SqlFeature>(
                        shpFileName,
                        (d, ies) => new SqlFeature() { Attributes = d, LabelAttribute = label, TheSqlGeometry = ies.Transform(map as Func<IPoint, IPoint>, targetSrs.Srid).AsSqlGeometry() },
                        //(d, srid, feature) => feature.TheSqlGeometry = d.Transform(map, targetSrs.Srid).AsSqlGeometry(),
                        true,
                        dataEncoding,
                        null,
                        headerEncoding
                        );
            }
            else
            {

                return IRI.Ket.ShapefileFormat.Shapefile.Read<SqlFeature>(
                        shpFileName,
                        (d, ies) => new SqlFeature() { Attributes = d, LabelAttribute = label, TheSqlGeometry = ies.AsSqlGeometry() },
                        //d => new SqlFeature() { Attributes = d, LabelAttribute = label },
                        //(d, srid, feature) => feature.TheSqlGeometry = d.AsSqlGeometry(),
                        true,
                        dataEncoding,
                        null,
                        headerEncoding);
            }
        }

        public static async Task<List<SqlFeature>> ReadAsSqlFeatureAsync(string shpFileName, Encoding dataEncoding, SrsBase targetSrs = null, Encoding headerEncoding = null, bool correctFarsiCharacters = true, string label = null)
        {
            if (targetSrs != null)
            {
                var sourceSrs = IRI.Ket.ShapefileFormat.Shapefile.TryGetSrs(shpFileName);

                Func<Point, Point> map = p => p.Project(sourceSrs, targetSrs);

                return await IRI.Ket.ShapefileFormat.Shapefile.ReadAsync<SqlFeature>(
                        shpFileName,
                        (d, ies) => new SqlFeature() { Attributes = d, LabelAttribute = label, TheSqlGeometry = ies.Transform(map as Func<IPoint, IPoint>, targetSrs.Srid).AsSqlGeometry() },
                        //d => new SqlFeature() { Attributes = d, LabelAttribute = label },
                        //(d, srid, feature) => feature.TheSqlGeometry = d.Transform(map, targetSrs.Srid).AsSqlGeometry(),
                        true,
                        dataEncoding,
                        null,
                        headerEncoding);
            }
            else
            {
                return await IRI.Ket.ShapefileFormat.Shapefile.ReadAsync<SqlFeature>(
                        shpFileName,
                        (d, ies) => new SqlFeature() { Attributes = d, LabelAttribute = label, TheSqlGeometry = ies.AsSqlGeometry() },
                        //d => new SqlFeature() { Attributes = d, LabelAttribute = label },
                        //(d, srid, feature) => feature.TheSqlGeometry = d.AsSqlGeometry(),
                        true,
                        dataEncoding,
                        null,
                        headerEncoding);
            }
        }
    }
}
