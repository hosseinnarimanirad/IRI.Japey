using static System.Math;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRI.Sta.Common.Primitives;
using Microsoft.SqlServer.Types;
using System.Diagnostics;
using IRI.Ket.SpatialExtensions;
using IRI.Ket.DataManagement.Model;
using IRI.Ket.SqlServerSpatialExtension.Model;

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

            var fitLevel = IRI.Sta.Common.Mapping.WebMercatorUtility.GetZoomLevel(Max(boundingBox.Width, boundingBox.Height), averageLatitude, 900);

            var simplifiedByAngleGeometries = geometries.Select(g => g.Simplify(.98, SqlServerSpatialExtension.Analysis.SimplificationType.AdditiveByAngle)).Where(g => !g.IsNullOrEmpty()).ToList();

            for (int i = fitLevel; i < 18; i += 4)
            {
                var threshold = IRI.Sta.Common.Mapping.WebMercatorUtility.CalculateGroundResolution(i, 0);

                Debug.Print($"threshold: {threshold}, level:{i}");

                var inverseScale = IRI.Sta.Common.Mapping.WebMercatorUtility.ZoomLevels.Single(z => z.ZoomLevel == i).InverseScale;

                source.Add(inverseScale, simplifiedByAngleGeometries.Select(g => g.Simplify(threshold, SqlServerSpatialExtension.Analysis.SimplificationType.AdditiveByArea)).Where(g => !g.IsNotValidOrEmpty()).ToList());
            }
        }

        public List<SqlGeometry> GetGeometries(double scale)
        {
            var availableScales = source.Select(k => k.Key).ToList();

            var selectedScale = IRI.Sta.Common.Mapping.WebMercatorUtility.GetUpperLevel(scale, availableScales);

            return source[selectedScale];
        }

        public List<SqlGeometry> GetGeometries(double scale, BoundingBox boundingBox)
        {
            SqlGeometry boundary = boundingBox.AsSqlGeometry();

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
