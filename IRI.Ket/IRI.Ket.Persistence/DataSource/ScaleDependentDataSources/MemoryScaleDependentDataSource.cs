using static System.Math;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRI.Sta.Common.Primitives;
using System.Diagnostics;
using IRI.Extensions;

using IRI.Sta.Common.Analysis;

namespace IRI.Ket.Persistence.DataSources
{
    public class MemoryScaleDependentDataSource<TGeometryAware> : MemoryDataSource<TGeometryAware, Point>, IScaleDependentDataSource where TGeometryAware : class, IGeometryAware<Point>
    {
        Dictionary<double, List<Geometry<Point>>> source;

        double averageLatitude = 30;

        //average latitude is assumed to be 30
        public MemoryScaleDependentDataSource(List<Geometry<Point>> geometries)
        {
            source = new Dictionary<double, List<Geometry<Point>>>();

            var boundingBox = geometries.GetBoundingBox();

            var fitLevel = IRI.Sta.Common.Mapping.WebMercatorUtility.EstimateZoomLevel(Max(boundingBox.Width, boundingBox.Height), /*averageLatitude,*/ 900);

            this.GeometryType = geometries.First().Type;

            var simplifiedByAngleGeometries = geometries
                                                .Select(g => g.Simplify(SimplificationType.CumulativeAngle, new SimplificationParamters()
                                                {
                                                    AngleThreshold = .98,
                                                    Retain3Points = true
                                                }))
                                                .Where(g => !g.IsNullOrEmpty())
                                                .ToList();

            for (int i = fitLevel; i < 18; i += 4)
            {
                var threshold = IRI.Sta.Common.Mapping.WebMercatorUtility.CalculateGroundResolution(i, 0);

                Debug.Print($"threshold: {threshold}, level:{i}");

                var inverseScale = IRI.Sta.Common.Mapping.WebMercatorUtility.ZoomLevels.Single(z => z.ZoomLevel == i).InverseScale;

                source.Add(inverseScale, simplifiedByAngleGeometries.Select(g => g.Simplify(SimplificationType.CumulativeTriangleRoutine, new SimplificationParamters()
                {
                    AreaThreshold = threshold * threshold,
                    Retain3Points = true
                }))
                                                                    .Where(g => !g.IsNotValidOrEmpty())
                                                                    .ToList());
            }
        }

        public List<Geometry<Point>> GetGeometries(double scale)
        {
            var availableScales = source.Select(k => k.Key).ToList();

            var selectedScale = IRI.Sta.Common.Mapping.WebMercatorUtility.GetUpperLevel(scale, availableScales);

            return source[selectedScale];
        }

        public List<Geometry<Point>> GetGeometries(double scale, BoundingBox boundingBox)
        {
            Geometry<Point> boundary = boundingBox.AsGeometry<Point>(GetSrid());

            return GetGeometries(scale, boundary);

        }

        public List<Geometry<Point>> GetGeometries(double scale, Geometry<Point> geometry)
        {
            return GetGeometries(scale).AsParallel().Where(i => i.Intersects(geometry)).ToList();
        }



        public Task<List<Geometry<Point>>> GetGeometriesAsync(double scale)
        {
            return Task.Run(() => { return GetGeometries(scale); });
        }

        public Task<List<Geometry<Point>>> GetGeometriesAsync(double scale, BoundingBox boundingBox)
        {
            return Task.Run(() => { return GetGeometries(scale, boundingBox); });
        }

    }

    public class MemoryScaleDependentDataSource : MemoryDataSource, IScaleDependentDataSource
    {
        Dictionary<double, List<Geometry<Point>>> source;

        double averageLatitude = 30;

        //average latitude is assumed to be 30
        public MemoryScaleDependentDataSource(List<Geometry<Point>> geometries)
        {
            source = new Dictionary<double, List<Geometry<Point>>>();

            this.GeometryType = geometries.First().Type;

            var boundingBox = geometries.GetBoundingBox();

            var fitLevel = IRI.Sta.Common.Mapping.WebMercatorUtility.EstimateZoomLevel(Max(boundingBox.Width, boundingBox.Height), /*averageLatitude, */900);

            var simplifiedByAngleGeometries = geometries.Select(g => g.Simplify(SimplificationType.CumulativeAngle, new SimplificationParamters() { AngleThreshold = .98, Retain3Points = true })).Where(g => !g.IsNullOrEmpty()).ToList();

            for (int i = fitLevel; i < 18; i += 4)
            {
                var threshold = IRI.Sta.Common.Mapping.WebMercatorUtility.CalculateGroundResolution(i, 0);

                Debug.Print($"threshold: {threshold}, level:{i}");

                var inverseScale = IRI.Sta.Common.Mapping.WebMercatorUtility.ZoomLevels.Single(z => z.ZoomLevel == i).InverseScale;

                source.Add(inverseScale, simplifiedByAngleGeometries.Select(g => g.Simplify(SimplificationType.CumulativeTriangleRoutine, new SimplificationParamters() { AreaThreshold = threshold * threshold, Retain3Points = true })).Where(g => !g.IsNotValidOrEmpty()).ToList());
            }
        }

        public List<Geometry<Point>> GetGeometries(double scale)
        {
            var availableScales = source.Select(k => k.Key).ToList();

            var selectedScale = IRI.Sta.Common.Mapping.WebMercatorUtility.GetUpperLevel(scale, availableScales);

            return source[selectedScale];
        }

        public List<Geometry<Point>> GetGeometries(double scale, BoundingBox boundingBox)
        {
            Geometry<Point> boundary = boundingBox.AsGeometry<Point>(GetSrid());

            return GetGeometries(scale, boundary);
        }

        public List<Geometry<Point>> GetGeometries(double scale, Geometry<Point> geometry)
        {
            return GetGeometries(scale).AsParallel().Where(i => i.Intersects(geometry)).ToList();
        }



        public Task<List<Geometry<Point>>> GetGeometriesAsync(double scale)
        {
            return Task.Run(() => { return GetGeometries(scale); });
        }

        public Task<List<Geometry<Point>>> GetGeometriesAsync(double scale, BoundingBox boundingBox)
        {
            return Task.Run(() => { return GetGeometries(scale, boundingBox); });
        }

    }
}
