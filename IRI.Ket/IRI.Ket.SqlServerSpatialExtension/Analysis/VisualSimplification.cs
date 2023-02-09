//using IRI.Msh.Common.Primitives;
//using IRI.Ket.SpatialExtensions;
//using IRI.Ket.SqlServerSpatialExtension.GeoStatistics;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using IRI.Msh.Common.Analysis;

//namespace Microsoft.SqlServer.Types
//{
//    public static class VisualSimplification
//    {
//        public static List<SqlGeometry> Simplify(
//            this List<SqlGeometry> geometries
//            , SimplificationType type
//            , int zoomLevel 
//            , SimplificationParamters paramters
//            , bool reduceToPoint = true 
//            )
//        {
//            try
//            {
//                Debug.WriteLine($"SIMPLIFY MEHTOD START FOR Z:{zoomLevel}, COUNT OF GEOMETRIES:{geometries?.Count}");

//                if (geometries == null)
//                {
//                    return null;
//                }

//                var threshold = IRI.Msh.Common.Mapping.WebMercatorUtility.CalculateGroundResolution(zoomLevel, paramters.AverageLatitude ?? 0); //0 seconds!

//                //watch.Stop();
//                //System.Diagnostics.Debug.WriteLine($"CALCULATE threshold {watch.ElapsedMilliseconds / 1000} s", "PYRAMID");
//                //watch.Restart();

//                var areaThreshold = threshold * threshold;

//                var result = new List<SqlGeometry>(); //0 seconds!

//                //watch.Stop();
//                //System.Diagnostics.Debug.WriteLine($"CREATE EMPTY LIST {watch.ElapsedMilliseconds / 1000} s", "PYRAMID");
//                //watch.Restart();

//                Stopwatch watch = Stopwatch.StartNew();


//                for (int i = 0; i < geometries.Count; i++)
//                {
//                    try
//                    {
//                        result.Add(geometries[i].Simplify(type, paramters));
//                        //result.Add(FilterPoints(geometries[i], filter));
//                    }
//                    catch (Exception ex)
//                    {

//                        throw;
//                    }
//                }

//                watch.Stop();
//                var tSimplify = watch.ElapsedMilliseconds / 1000; // System.Diagnostics.Debug.WriteLine($"CALL PROCESS {watch.ElapsedMilliseconds / 1000} s", "PYRAMID");
//                watch.Restart();

//                result = result.Select(i => i.MakeValid()).Where(i => !i.IsNullOrEmpty()).ToList();   //0 seconds!

//                //watch.Stop();
//                //System.Diagnostics.Debug.WriteLine($"FILTERING INVALID RESULTS {watch.ElapsedMilliseconds / 1000} s", "PYRAMID");
//                //watch.Restart();

//                ///////////////////////////////////////////////////////////*********************************
//                if (reduceToPoint)
//                {
//                    for (int g = 0; g < result.Count; g++)
//                    {
//                        try
//                        {
//                            var length = result[g].STLength().Value;

//                            if (length < threshold)
//                            {
//                                result[g] = result[g].STPointOnSurface();
//                            }
//                        }
//                        catch (Exception ex)
//                        {

//                            throw;
//                        }
//                    }
//                    //*********************************** 0-1 second //////////////////////////////////////////

//                    //watch.Stop();
//                    //System.Diagnostics.Debug.WriteLine($"CONVERT TO POINT {watch.ElapsedMilliseconds / 1000} s", "PYRAMID");
//                    //watch.Restart();

//                    var tMid = watch.ElapsedMilliseconds / 100;

//                    watch.Restart();

//                    result = result.RemoveOverlappingPoints(threshold);

//                    watch.Stop();
//                    var tRemovePoints = watch.ElapsedMilliseconds / 1000;
//                    Debug.WriteLine($"Total:{tSimplify + tMid + tRemovePoints}, Simplify: {tSimplify}, RemoveExteraPoints: {tRemovePoints}", "PYRAMID");

//                }

//                return result;

//            }
//            catch (Exception ex)
//            {
//                throw;
//            }
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="geometry"></param>
//        /// <param name="threshold"></param>
//        /// <param name="type"></param>
//        /// <param name="areaThreshold"></param>
//        /// <returns></returns>
//        public static SqlGeometry Simplify(this SqlGeometry geometry, SimplificationType type, SimplificationParamters parameters /*double threshold,  bool retain3Points, double areaThreshold = double.NaN*/)
//        {
//            if (geometry.IsNotValidOrEmpty())
//            {
//                return geometry;
//            }

//            var extractedGeometry = geometry.AsGeometry();

//            var filteredGeometry = extractedGeometry.Simplify(type, parameters);

//            return filteredGeometry.AsSqlGeometry().MakeValid();             
//        }
         
//        public static List<SqlGeometry> RemoveOverlappingPoints(this List<SqlGeometry> source, double minDistance)
//        {
//            try
//            {
//                List<SqlGeometry> result = new List<SqlGeometry>();

//                if (source == null || source.Count < 1)
//                    return result;

//                var points = source.Where(i => i.GetOpenGisType() == OpenGisGeometryType.Point).Select(i => new Point(i.STX.Value, i.STY.Value)).ToList();

//                Stopwatch watch = Stopwatch.StartNew();

//                //var cFast = KdTreePointClusters<Point>.GetClusterCenters(points, Point.NaN, minDistance).Count;


//                ////************************************************************************************************
//                //watch.Stop();
//                //var tFast = watch.ElapsedMilliseconds / 1000;
//                //watch.Restart();
//                ////************************************************************************************************


//                //var clusters = new PointClusters<Point>(points);
//                //var cSlow = clusters.GetClusters((p1, p2) => Point.EuclideanDistance(p1, p2) < minDistance).Count;


//                ////************************************************************************************************
//                //watch.Stop();
//                //var tSlow = watch.ElapsedMilliseconds / 1000;
//                //watch.Restart();
//                ////************************************************************************************************


//                var kdtreeCluster = new KdTreePointClusters<Point>(points, new Group<Point>(Point.NaN));
//                //kdtreeCluster.GetClusters((p1, p2) => Point.EuclideanDistance(p1, p2) < minDistance);
//                kdtreeCluster.GetClusters((p1, p2) => SpatialUtility.GetEuclideanDistance(p1, p2) < minDistance);


//                //************************************************************************************************
//                watch.Stop();
//                var tNormal = watch.ElapsedMilliseconds / 1000;
//                watch.Restart();
//                //************************************************************************************************


//                var centers = kdtreeCluster.GetGroupCenters();


//                //************************************************************************************************
//                watch.Stop();
//                var tGetGroupCenters = watch.ElapsedMilliseconds / 1000;
//                watch.Restart();
//                //************************************************************************************************


//                for (int i = 0; i < source.Count; i++)
//                {
//                    try
//                    {
//                        if (source[i].IsNullOrEmpty())
//                            continue;

//                        if (source[i].GetOpenGisType() != OpenGisGeometryType.Point)
//                        {
//                            result.Add(source[i]);
//                        }
//                    }
//                    catch (Exception ex)
//                    {
//                        throw;
//                    }
//                }



//                //************************************************************************************************
//                watch.Stop();
//                var tAddNonPoints = watch.ElapsedMilliseconds / 1000;//System.Diagnostics.Debug.WriteLine($"\t\tADDNONPOINTS {watch.ElapsedMilliseconds / 1000} s", "PYRAMID");
//                watch.Restart();
//                //************************************************************************************************



//                var srid = source.FirstOrDefault(i => !i.IsNullOrEmpty()).STSrid.Value;

//                result.AddRange(centers.Select(i => i.AsSqlGeometry(srid)));



//                //************************************************************************************************
//                watch.Stop();
//                var tAddPoints = watch.ElapsedMilliseconds / 1000;//System.Diagnostics.Debug.WriteLine($"\t\tADDPOINTS {centers.Count} - {watch.ElapsedMilliseconds / 1000} s", "PYRAMID");
//                watch.Restart();
//                //************************************************************************************************


//                //Debug.WriteLine($"\t\t [Points :{points.Count}] , [Slow-(c: {cSlow} , t: {tSlow})], [Normal-(c:{centers.Count} , t: {tNormal})]", "PYRAMID");

//                Debug.WriteLine($"\t\t GetGroupCenters: {tGetGroupCenters}, AddNonPoints: {tAddNonPoints}, AddPoints: {tAddPoints}", "PYRAMID");

//                return result;
//            }
//            catch (Exception ex)
//            {

//                throw;
//            }
//        }

         
//    }
//}
