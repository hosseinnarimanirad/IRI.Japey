using static System.Math;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRI.Msh.Common.Primitives;
using Microsoft.SqlServer.Types;
using System.Diagnostics;
using IRI.Ket.SpatialExtensions;
using IRI.Ket.DataManagement.Model;
using IRI.Ket.SqlServerSpatialExtension.Model;
using IRI.Msh.Common.Analysis;

namespace IRI.Ket.DataManagement.DataSource
{
    public class MemoryScaleDependentDataSource<T> : MemoryDataSource<T>, IScaleDependentDataSource where T : class, ISqlGeometryAware
    {
        Dictionary<double, List<SqlGeometry>> source;

        double averageLatitude = 30;

        //average latitude is assumed to be 30
        public MemoryScaleDependentDataSource(List<SqlGeometry> geometries)
        {
            source = new Dictionary<double, List<SqlGeometry>>();

            var boundingBox = geometries.GetBoundingBox();

            var fitLevel = IRI.Msh.Common.Mapping.WebMercatorUtility.EstimateZoomLevel(Max(boundingBox.Width, boundingBox.Height), /*averageLatitude,*/ 900);

            var simplifiedByAngleGeometries = geometries
                                                .Select(g => g.Simplify(SimplificationType.AdditiveByAngle, new SimplificationParamters()
                                                {
                                                    AngleThreshold = .98,
                                                    Retain3Points = true
                                                }))
                                                .Where(g => !g.IsNullOrEmpty())
                                                .ToList();

            for (int i = fitLevel; i < 18; i += 4)
            {
                var threshold = IRI.Msh.Common.Mapping.WebMercatorUtility.CalculateGroundResolution(i, 0);

                Debug.Print($"threshold: {threshold}, level:{i}");

                var inverseScale = IRI.Msh.Common.Mapping.WebMercatorUtility.ZoomLevels.Single(z => z.ZoomLevel == i).InverseScale;

                source.Add(inverseScale, simplifiedByAngleGeometries.Select(g => g.Simplify(SimplificationType.AdditiveByArea, new SimplificationParamters()
                                                                    {
                                                                        AreaThreshold = threshold * threshold,
                                                                        Retain3Points = true
                                                                    }))
                                                                    .Where(g => !g.IsNotValidOrEmpty())
                                                                    .ToList());
            }
        }

        public List<SqlGeometry> GetGeometries(double scale)
        {
            var availableScales = source.Select(k => k.Key).ToList();

            var selectedScale = IRI.Msh.Common.Mapping.WebMercatorUtility.GetUpperLevel(scale, availableScales);

            return source[selectedScale];
        }

        public List<SqlGeometry> GetGeometries(double scale, BoundingBox boundingBox)
        {
            SqlGeometry boundary = boundingBox.AsSqlGeometry(GetSrid());

            return GetGeometries(scale, boundary);

        }

        public List<SqlGeometry> GetGeometries(double scale, SqlGeometry geometry)
        {
            return GetGeometries(scale).AsParallel().Where(i => i.STIntersects(geometry).IsTrue).ToList();
        }



        public Task<List<SqlGeometry>> GetGeometriesAsync(double scale)
        {
            return Task.Run(() => { return GetGeometries(scale); });
        }

        public Task<List<SqlGeometry>> GetGeometriesAsync(double scale, BoundingBox boundingBox)
        {
            return Task.Run(() => { return GetGeometries(scale, boundingBox); });
        }

    }

    public class MemoryScaleDependentDataSource : MemoryDataSource, IScaleDependentDataSource
    {
        Dictionary<double, List<SqlGeometry>> source;

        double averageLatitude = 30;

        //average latitude is assumed to be 30
        public MemoryScaleDependentDataSource(List<SqlGeometry> geometries)
        {
            source = new Dictionary<double, List<SqlGeometry>>();

            var boundingBox = geometries.GetBoundingBox();

            var fitLevel = IRI.Msh.Common.Mapping.WebMercatorUtility.EstimateZoomLevel(Max(boundingBox.Width, boundingBox.Height), /*averageLatitude, */900);

            var simplifiedByAngleGeometries = geometries.Select(g => g.Simplify(SimplificationType.AdditiveByAngle, new SimplificationParamters() { AngleThreshold = .98, Retain3Points = true })).Where(g => !g.IsNullOrEmpty()).ToList();

            for (int i = fitLevel; i < 18; i += 4)
            {
                var threshold = IRI.Msh.Common.Mapping.WebMercatorUtility.CalculateGroundResolution(i, 0);

                Debug.Print($"threshold: {threshold}, level:{i}");

                var inverseScale = IRI.Msh.Common.Mapping.WebMercatorUtility.ZoomLevels.Single(z => z.ZoomLevel == i).InverseScale;

                source.Add(inverseScale, simplifiedByAngleGeometries.Select(g => g.Simplify(SimplificationType.AdditiveByArea, new SimplificationParamters() { AreaThreshold = threshold * threshold, Retain3Points = true })).Where(g => !g.IsNotValidOrEmpty()).ToList());
            }
        }

        public List<SqlGeometry> GetGeometries(double scale)
        {
            var availableScales = source.Select(k => k.Key).ToList();

            var selectedScale = IRI.Msh.Common.Mapping.WebMercatorUtility.GetUpperLevel(scale, availableScales);

            return source[selectedScale];
        }

        public List<SqlGeometry> GetGeometries(double scale, BoundingBox boundingBox)
        {
            SqlGeometry boundary = boundingBox.AsSqlGeometry(GetSrid());

            return GetGeometries(scale, boundary);

        }

        public List<SqlGeometry> GetGeometries(double scale, SqlGeometry geometry)
        {
            return GetGeometries(scale).AsParallel().Where(i => i.STIntersects(geometry).IsTrue).ToList();
        }



        public Task<List<SqlGeometry>> GetGeometriesAsync(double scale)
        {
            return Task.Run(() => { return GetGeometries(scale); });
        }

        public Task<List<SqlGeometry>> GetGeometriesAsync(double scale, BoundingBox boundingBox)
        {
            return Task.Run(() => { return GetGeometries(scale, boundingBox); });
        }

    }
}
