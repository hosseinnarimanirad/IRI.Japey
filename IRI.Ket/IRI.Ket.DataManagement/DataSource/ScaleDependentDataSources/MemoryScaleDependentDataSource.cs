using static System.Math;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRI.Ham.SpatialBase;
using Microsoft.SqlServer.Types;
using System.Diagnostics;
using IRI.Ket.SpatialExtensions;
using IRI.Ket.DataManagement.Model;

namespace IRI.Ket.DataManagement.DataSource
{
    public class MemoryScaleDependentDataSource<T> : MemoryDataSource<T>, IScaleDependentDataSource
    {
        Dictionary<double, List<SqlGeometry>> source;

        double averageLatitude = 30;

        //average latitude is assumed to be 30
        public MemoryScaleDependentDataSource(List<SqlGeometry> geometries)
        {
            source = new Dictionary<double, List<SqlGeometry>>();

            var boundingBox = geometries.GetBoundingBox();

            var fitLevel = IRI.Ham.SpatialBase.Mapping.WebMercatorUtility.GetZoomLevel(Max(boundingBox.Width, boundingBox.Height), averageLatitude, 900);

            var simplifiedByAngleGeometries = geometries.Select(g => g.Simplify(.98, SqlServerSpatialExtension.Analysis.SimplificationType.AdditiveByAngle)).Where(g => !g.IsNullOrEmpty()).ToList();

            for (int i = fitLevel; i < 18; i += 4)
            {
                var threshold = IRI.Ham.SpatialBase.Mapping.WebMercatorUtility.CalculateGroundResolution(i, 0);

                Debug.Print($"threshold: {threshold}, level:{i}");

                var inverseScale = IRI.Ham.SpatialBase.Mapping.WebMercatorUtility.ZoomLevels.Single(z => z.ZoomLevel == i).InverseScale;

                source.Add(inverseScale, simplifiedByAngleGeometries.Select(g => g.Simplify(threshold, SqlServerSpatialExtension.Analysis.SimplificationType.AdditiveByArea)).Where(g => !g.IsNotValidOrEmpty()).ToList());
            }
        }

        public List<SqlGeometry> GetGeometries(double scale)
        { 
            var availableScales = source.Select(k => k.Key).ToList();

            var selectedScale = IRI.Ham.SpatialBase.Mapping.WebMercatorUtility.GetUpperLevel(scale, availableScales);
             
            return source[selectedScale];
        }

        public List<SqlGeometry> GetGeometries(double scale, BoundingBox boundingBox)
        {
            SqlGeometry boundary = boundingBox.AsSqlGeometry();

            return GetGeometries(scale).AsParallel().Where(i => i.STIntersects(boundary).IsTrue).ToList();

        }

        public Task<List<SqlGeometry>> GetGeometriesAsync(double scale, BoundingBox boundingBox)
        {
            return Task.Run(() => { return GetGeometries(scale, boundingBox); });
        }

    }
}
