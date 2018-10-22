using IRI.Msh.Common.Primitives;
using IRI.Ket.SpatialExtensions;
using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;

//namespace IRI.Ket.DataManagement.DataSource
//{
//    public class SqlServerScaleDependentDataSource : SqlServerDataSource
//    {
//        //Dictionary<double, List<SqlGeometry>> source;
//        public SqlServerScaleDependentDataSource(string connectionString, string tableName, string spatialColumnName = null, string labelColumnName = null, string _queryString = null)
//        {
//            this._connectionString = connectionString;

//            this._tableName = tableName;

//            this._spatialColumnName = spatialColumnName;

//            this._labelColumnName = labelColumnName;

//            this._queryString = _queryString;

//            if (spatialColumnName == null)
//            {
//                this.Extent = BoundingBox.NaN;
//            }
//            else
//            {
//                //IMPORTANT!
//                //this.Extent = GetGeometries().GetBoundingBox();
//            }

//        }

//        //public static SqlServerScaleDependentDataSource CreateForQueryString(string connectionString, string queryString, string spatialColumnName, string labelColumnName = null)
//        //{
//        //    SqlServerScaleDependentDataSource result = new SqlServerScaleDependentDataSource(connectionString, null, spatialColumnName, labelColumnName, queryString);

//        //    return result;
//        //}

//        //average latitude is assumed to be 30
//        //public SqlServerScaleDependentDataSource(List<SqlGeometry> geometries)
//        //{
//        //    //source = new Dictionary<double, List<SqlGeometry>>();

//        //    var boundingBox = geometries.GetBoundingBox();

//        //    var fitLevel = IRI.Msh.Common.Mapping.GoogleMapsUtility.GetZoomLevel(Max(boundingBox.Width, boundingBox.Height));

//        //    var simplifiedByAngleGeometries = geometries.Select(g => g.Simplify(.98, SqlServerSpatialExtension.Analysis.SimplificationType.AdditiveSimplifyByAngle)).Where(g => !g.IsNullOrEmpty()).ToList();

//        //    for (int i = fitLevel; i < 18; i += 4)
//        //    {
//        //        var threshold = IRI.Msh.Common.Mapping.WebMercatorUtility.CalculateGroundResolution(i, 0);

//        //        Debug.Print($"threshold: {threshold}, level:{i}");

//        //        //var scale = IRI.Msh.Common.Mapping.WebMercatorUtility.CalculateMapScale(i, 30);
//        //        var inverseScale = IRI.Msh.Common.Mapping.GoogleMapsUtility.ZoomLevels.Single(z => z.ZoomLevel == i).InverseScale;

//        //        source.Add(inverseScale, simplifiedByAngleGeometries.Select(g => g.Simplify(threshold, SqlServerSpatialExtension.Analysis.SimplificationType.AdditiveSimplifyByArea)).Where(g => !g.IsNotValidOrEmpty()).ToList());
//        //    }
//        //}

//        //POTENTIONALLY ERROR PRONE. What if geometries where in lat/long so ground distance is wrong for calcualting ZoomLevel
//        public override List<SqlGeometry> GetGeometries(BoundingBox boundingBox)
//        {
//            var srid = GetSrid();

//            var properLevel = IRI.Msh.Common.Mapping.WebMercatorUtility.GetZoomLevel(Max(boundingBox.Width, boundingBox.Height), 30, 1500);

//            var levels = GetLevels();

//            if (properLevel > levels.Max())
//            {
//                return base.GetGeometries(boundingBox);
//            }
//            else
//            {
//                var wktBoundingBox = boundingBox.AsWkt();

//                var query = $"SELECT Geo FROM {_tableName}_P WHERE Level = {properLevel} AND (MBB.STIntersects(GEOMETRY::STPolyFromText('{wktBoundingBox}',{srid})) = 1) ";

//                return this.SelectGeometries(query);
//            }

//        }


//        public List<int> GetLevels()
//        {
//            return Select<int>($"SELECT DISTINCT(Level) FROM {_tableName}_P");
//        }
//        //public Task<List<SqlGeometry>> GetGeometriesAsync(double scale, BoundingBox boundingBox)
//        //{
//        //    return Task.Run(() => { return GetGeometries(scale, boundingBox); });
//        //}
//    }
//}
