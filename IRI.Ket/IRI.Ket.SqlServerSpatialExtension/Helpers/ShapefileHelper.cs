using IRI.Ket.SpatialExtensions;
using IRI.Ket.SqlServerSpatialExtension.Model;
using IRI.Msh.Common.Primitives;
using IRI.Msh.CoordinateSystem.MapProjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.SqlServerSpatialExtension.Helpers
{
    public class ShapefileHelper
    {

        public static List<SqlFeature> ReadShapefile(string shpFileName, Encoding dataEncoding, Encoding headerEncoding, bool correctFarsiCharacters, SrsBase targetSrs = null)
        {
            if (targetSrs != null)
            {
                var sourceSrs = IRI.Ket.ShapefileFormat.Shapefile.TryGetSrs(shpFileName);

                Func<IPoint, IPoint> map = p => p.Project(sourceSrs, targetSrs);

                return IRI.Ket.ShapefileFormat.Shapefile.Read<SqlFeature>(
                        shpFileName,
                        d => new SqlFeature() { Attributes = d },
                        (d, srid, feature) => feature.TheSqlGeometry = d.Transform(map, targetSrs.Srid).AsSqlGeometry(),
                        System.Text.Encoding.UTF8,
                        System.Text.Encoding.UTF8,
                        true);
            }
            else
            {

                return IRI.Ket.ShapefileFormat.Shapefile.Read<SqlFeature>(
                        shpFileName,
                        d => new SqlFeature() { Attributes = d },
                        (d, srid, feature) => feature.TheSqlGeometry = d.AsSqlGeometry(),
                        System.Text.Encoding.UTF8,
                        System.Text.Encoding.UTF8,
                        true);
            }
        }

        public static async Task<List<SqlFeature>> ReadShapefileAsync(string shpFileName, Encoding dataEncoding, Encoding headerEncoding, bool correctFarsiCharacters, SrsBase targetSrs = null)
        {
            if (targetSrs != null)
            {
                var sourceSrs = IRI.Ket.ShapefileFormat.Shapefile.TryGetSrs(shpFileName);

                Func<IPoint, IPoint> map = p => p.Project(sourceSrs, targetSrs);

                return await IRI.Ket.ShapefileFormat.Shapefile.ReadAsync<SqlFeature>(
                        shpFileName,
                        d => new SqlFeature() { Attributes = d },
                        (d, srid, feature) => feature.TheSqlGeometry = d.Transform(map, targetSrs.Srid).AsSqlGeometry(),
                        System.Text.Encoding.UTF8,
                        System.Text.Encoding.UTF8,
                        true);
            }
            else
            {

                return await IRI.Ket.ShapefileFormat.Shapefile.ReadAsync<SqlFeature>(
                        shpFileName,
                        d => new SqlFeature() { Attributes = d },
                        (d, srid, feature) => feature.TheSqlGeometry = d.AsSqlGeometry(),
                        System.Text.Encoding.UTF8,
                        System.Text.Encoding.UTF8,
                        true);
            }
        }

    }
}
