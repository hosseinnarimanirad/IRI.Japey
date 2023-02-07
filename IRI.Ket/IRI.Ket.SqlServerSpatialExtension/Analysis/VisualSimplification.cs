using IRI.Msh.Common.Primitives;
using IRI.Ket.SpatialExtensions;
using IRI.Ket.SqlServerSpatialExtension.GeoStatistics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using IRI.Msh.Common.Analysis;

namespace Microsoft.SqlServer.Types
{
    public static class VisualSimplification
    {
        public static List<SqlGeometry> Simplify(
            this List<SqlGeometry> geometries
            , SimplificationType type
            , int zoomLevel
            //bool retain3Points,
            , SimplificationParamters paramters
            , bool reduceToPoint = true
            //double averageLatitude = 35,
            //double angleThreshold = .98
            )
        {
            try
            {
                Debug.WriteLine($"SIMPLIFY MEHTOD START FOR Z:{zoomLevel}, COUNT OF GEOMETRIES:{geometries?.Count}");

                if (geometries == null)
                {
                    return null;
                }

                var threshold = IRI.Msh.Common.Mapping.WebMercatorUtility.CalculateGroundResolution(zoomLevel, paramters.AverageLatitude ?? 0); //0 seconds!

                //watch.Stop();
                //System.Diagnostics.Debug.WriteLine($"CALCULATE threshold {watch.ElapsedMilliseconds / 1000} s", "PYRAMID");
                //watch.Restart();

                var areaThreshold = threshold * threshold;

                var result = new List<SqlGeometry>(); //0 seconds!

                //watch.Stop();
                //System.Diagnostics.Debug.WriteLine($"CREATE EMPTY LIST {watch.ElapsedMilliseconds / 1000} s", "PYRAMID");
                //watch.Restart();

                Stopwatch watch = Stopwatch.StartNew();


                for (int i = 0; i < geometries.Count; i++)
                {
                    try
                    {
                        result.Add(geometries[i].Simplify(type, paramters));
                        //result.Add(FilterPoints(geometries[i], filter));
                    }
                    catch (Exception ex)
                    {

                        throw;
                    }
                }

                watch.Stop();
                var tSimplify = watch.ElapsedMilliseconds / 1000; // System.Diagnostics.Debug.WriteLine($"CALL PROCESS {watch.ElapsedMilliseconds / 1000} s", "PYRAMID");
                watch.Restart();

                result = result.Select(i => i.MakeValid()).Where(i => !i.IsNullOrEmpty()).ToList();   //0 seconds!

                //watch.Stop();
                //System.Diagnostics.Debug.WriteLine($"FILTERING INVALID RESULTS {watch.ElapsedMilliseconds / 1000} s", "PYRAMID");
                //watch.Restart();

                ///////////////////////////////////////////////////////////*********************************
                if (reduceToPoint)
                {
                    for (int g = 0; g < result.Count; g++)
                    {
                        try
                        {
                            var length = result[g].STLength().Value;

                            if (length < threshold)
                            {
                                result[g] = result[g].STPointOnSurface();
                            }
                        }
                        catch (Exception ex)
                        {

                            throw;
                        }
                    }
                    //*********************************** 0-1 second //////////////////////////////////////////

                    //watch.Stop();
                    //System.Diagnostics.Debug.WriteLine($"CONVERT TO POINT {watch.ElapsedMilliseconds / 1000} s", "PYRAMID");
                    //watch.Restart();

                    var tMid = watch.ElapsedMilliseconds / 100;

                    watch.Restart();

                    result = result.RemoveOverlappingPoints(threshold);

                    watch.Stop();
                    var tRemovePoints = watch.ElapsedMilliseconds / 1000;
                    Debug.WriteLine($"Total:{tSimplify + tMid + tRemovePoints}, Simplify: {tSimplify}, RemoveExteraPoints: {tRemovePoints}", "PYRAMID");

                }

                return result;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="geometry"></param>
        /// <param name="threshold"></param>
        /// <param name="type"></param>
        /// <param name="areaThreshold"></param>
        /// <returns></returns>
        public static SqlGeometry Simplify(this SqlGeometry geometry, SimplificationType type, SimplificationParamters parameters /*double threshold,  bool retain3Points, double areaThreshold = double.NaN*/)
        {
            if (geometry.IsNotValidOrEmpty())
            {
                return geometry;
            }

            var extractedGeometry = geometry.AsGeometry();

            var filteredGeometry = extractedGeometry.Simplify(type, parameters);

            return filteredGeometry.AsSqlGeometry().MakeValid();

            //switch (type)
            //{
            //    case SimplificationType.ByArea:
            //        return FilterPoints(geometry, pList => IRI.Msh.Common.Analysis.VisualSimplification.SimplifyByArea(pList, threshold));

            //    case SimplificationType.AdditiveByArea:
            //        return FilterPoints(geometry, pList => IRI.Msh.Common.Analysis.VisualSimplification.AdditiveSimplifyByArea(pList, threshold));

            //    case SimplificationType.AdditiveByAreaPlus:
            //        return FilterPoints(geometry, pList => IRI.Msh.Common.Analysis.VisualSimplification.AdditiveSimplifyByAreaPlus(pList, threshold));

            //    case SimplificationType.ByAngle:
            //        return FilterPoints(geometry, pList => IRI.Msh.Common.Analysis.VisualSimplification.SimplifyByAngle(pList, threshold));

            //    case SimplificationType.AdditiveByAngle:
            //        return FilterPoints(geometry, pList => IRI.Msh.Common.Analysis.VisualSimplification.AdditiveSimplifyByAngle(pList, threshold));

            //    case SimplificationType.AdditiveByDistance:
            //        return FilterPoints(geometry, pList => IRI.Msh.Common.Analysis.VisualSimplification.AdditiveSimplifyByDistance(pList, threshold));

            //    case SimplificationType.AdditiveByAreaAngle:
            //        return FilterPoints(geometry, pList => IRI.Msh.Common.Analysis.VisualSimplification.AdditiveSimplifyByAngleArea(pList, threshold, areaThreshold));

            //    default:
            //        throw new NotImplementedException();
            //}
        }

        //private static SqlGeometry FilterPoints(SqlGeometry geometry, Func<IPoint[], IPoint[]> filter)
        //{
        //    if (geometry.IsNotValidOrEmpty())
        //    {
        //        return geometry;
        //    }

        //    var extractedGeometry = geometry.AsGeometry();

        //    var filteredGeometry = extractedGeometry.SelectPoints(filter);

        //    return filteredGeometry.AsSqlGeometry().MakeValid();
        //}


        //private static List<SqlGeometry> FilterPoints(List<SqlGeometry> geometries, Func<IPoint[], IPoint[]> filter)
        //{
        //    List<SqlGeometry> result = new List<SqlGeometry>(geometries.Count);

        //    for (int i = 0; i < geometries.Count; i++)
        //    {
        //        try
        //        {
        //            //result.Add(FilterPoints(geometries[i], filter));
        //        }
        //        catch (Exception ex)
        //        {

        //            throw;
        //        }
        //    }

        //    return result;
        //}

        //public static List<SqlGeometry> Simplify(this List<SqlGeometry> source, bool reduceToPoint = false)
        //{
        //    var boundingBox = source.GetBoundingBox();

        //    var fitLevel = IRI.Msh.Common.Mapping.WebMercatorUtility.GetZoomLevel(Math.Max(boundingBox.Width, boundingBox.Height), 30, 1500);

        //    var simplifiedByAngleGeometries = source.Select(g => g.Simplify(.98, SimplificationType.AdditiveByAngle)).Where(g => !g.IsNullOrEmpty()).ToList();

        //    var threshold = IRI.Msh.Common.Mapping.WebMercatorUtility.CalculateGroundResolution(fitLevel, 40);

        //    var temp = simplifiedByAngleGeometries.Select(g => g.Simplify(threshold, SimplificationType.AdditiveByAreaPlus).MakeValid())
        //                                            .Where(g => !g.IsNullOrEmpty())
        //                                            .ToList();

        //    if (reduceToPoint)
        //    {
        //        for (int g = 0; g < temp.Count; g++)
        //        {
        //            var length = temp[g].STLength().Value;

        //            if (length < threshold)
        //            {
        //                temp[g] = temp[g].STPointN(1);
        //            }
        //        }
        //    }

        //    return temp;
        //}

        public static List<SqlGeometry> RemoveOverlappingPoints(this List<SqlGeometry> source, double minDistance)
        {
            try
            {
                List<SqlGeometry> result = new List<SqlGeometry>();

                if (source == null || source.Count < 1)
                    return result;

                var points = source.Where(i => i.GetOpenGisType() == OpenGisGeometryType.Point).Select(i => new Point(i.STX.Value, i.STY.Value)).ToList();

                Stopwatch watch = Stopwatch.StartNew();

                //var cFast = KdTreePointClusters<Point>.GetClusterCenters(points, Point.NaN, minDistance).Count;


                ////************************************************************************************************
                //watch.Stop();
                //var tFast = watch.ElapsedMilliseconds / 1000;
                //watch.Restart();
                ////************************************************************************************************


                //var clusters = new PointClusters<Point>(points);
                //var cSlow = clusters.GetClusters((p1, p2) => Point.EuclideanDistance(p1, p2) < minDistance).Count;


                ////************************************************************************************************
                //watch.Stop();
                //var tSlow = watch.ElapsedMilliseconds / 1000;
                //watch.Restart();
                ////************************************************************************************************


                var kdtreeCluster = new KdTreePointClusters<Point>(points, new Group<Point>(Point.NaN));
                //kdtreeCluster.GetClusters((p1, p2) => Point.EuclideanDistance(p1, p2) < minDistance);
                kdtreeCluster.GetClusters((p1, p2) => SpatialUtility.GetEuclideanDistance(p1, p2) < minDistance);


                //************************************************************************************************
                watch.Stop();
                var tNormal = watch.ElapsedMilliseconds / 1000;
                watch.Restart();
                //************************************************************************************************


                var centers = kdtreeCluster.GetGroupCenters();


                //************************************************************************************************
                watch.Stop();
                var tGetGroupCenters = watch.ElapsedMilliseconds / 1000;
                watch.Restart();
                //************************************************************************************************


                for (int i = 0; i < source.Count; i++)
                {
                    try
                    {
                        if (source[i].IsNullOrEmpty())
                            continue;

                        if (source[i].GetOpenGisType() != OpenGisGeometryType.Point)
                        {
                            result.Add(source[i]);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }



                //************************************************************************************************
                watch.Stop();
                var tAddNonPoints = watch.ElapsedMilliseconds / 1000;//System.Diagnostics.Debug.WriteLine($"\t\tADDNONPOINTS {watch.ElapsedMilliseconds / 1000} s", "PYRAMID");
                watch.Restart();
                //************************************************************************************************



                var srid = source.FirstOrDefault(i => !i.IsNullOrEmpty()).STSrid.Value;

                result.AddRange(centers.Select(i => i.AsSqlGeometry(srid)));



                //************************************************************************************************
                watch.Stop();
                var tAddPoints = watch.ElapsedMilliseconds / 1000;//System.Diagnostics.Debug.WriteLine($"\t\tADDPOINTS {centers.Count} - {watch.ElapsedMilliseconds / 1000} s", "PYRAMID");
                watch.Restart();
                //************************************************************************************************


                //Debug.WriteLine($"\t\t [Points :{points.Count}] , [Slow-(c: {cSlow} , t: {tSlow})], [Normal-(c:{centers.Count} , t: {tNormal})]", "PYRAMID");

                Debug.WriteLine($"\t\t GetGroupCenters: {tGetGroupCenters}, AddNonPoints: {tAddNonPoints}, AddPoints: {tAddPoints}", "PYRAMID");

                return result;
            }
            catch (Exception ex)
            {

                throw;
            }
        }





        public static List<Geometry<Point>>? Simplify(
          this List<Geometry<Point>> geometries
          , SimplificationType type
          , int zoomLevel
          , SimplificationParamters paramters
          , bool reduceToPoint = true)
        {
            try
            {
                if (geometries == null)
                    return null;

                var threshold = IRI.Msh.Common.Mapping.WebMercatorUtility.CalculateGroundResolution(zoomLevel, paramters.AverageLatitude ?? 0); //0 seconds!

                var areaThreshold = threshold * threshold;

                var result = new List<Geometry<Point>>(); //0 seconds!

                Stopwatch watch = Stopwatch.StartNew();

                for (int i = 0; i < geometries.Count; i++)
                {
                    try
                    {
                        result.Add(geometries[i].Simplify(type, paramters));
                        //result.Add(FilterPoints(geometries[i], filter));
                    }
                    catch (Exception ex) { throw; }
                }

                result = result/*.Select(i => i.MakeValid())*/.Where(i => !i.IsNullOrEmpty()).ToList();   //0 seconds!

                if (reduceToPoint)
                {
                    for (int g = 0; g < result.Count; g++)
                    {
                        try
                        {
                            var length = result[g].CalculateEuclideanLength();//.STLength().Value;

                            if (length < threshold)
                            {
                                //result[g] = result[g].STPointOnSurface();
                                result[g] = result[g].GetLastPoint().AsGeometry(result[g].Srid);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw;
                        }
                    }
                    var tMid = watch.ElapsedMilliseconds / 100;

                    watch.Restart();

                    result = result.RemoveOverlappingPoints(threshold);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="geometry"></param>
        /// <param name="threshold"></param>
        /// <param name="type"></param>
        /// <param name="areaThreshold"></param>
        /// <returns></returns>
        public static Geometry<Point> Simplify(this Geometry<Point> geometry, SimplificationType type, SimplificationParamters parameters /*double threshold,  bool retain3Points, double areaThreshold = double.NaN*/)
        {
            if (geometry.IsNotValidOrEmpty())
            {
                return geometry;
            }

            //var extractedGeometry = geometry.AsGeometry();

            var filteredGeometry = geometry.Simplify(type, parameters);

            return filteredGeometry;//.AsSqlGeometry().MakeValid();

        }

        public static List<Geometry<Point>> RemoveOverlappingPoints(this List<Geometry<Point>> source, double minDistance)
        {
            try
            {
                List<Geometry<Point>> result = new List<Geometry<Point>>();

                if (source == null || source.Count < 1)
                    return result;

                var points = source.Where(i => i.Type == GeometryType.Point).Select(i => new Point(i.Points[0].X, i.Points[0].Y)).ToList();

                Stopwatch watch = Stopwatch.StartNew();

                //var cFast = KdTreePointClusters<Point>.GetClusterCenters(points, Point.NaN, minDistance).Count;


                ////************************************************************************************************
                //watch.Stop();
                //var tFast = watch.ElapsedMilliseconds / 1000;
                //watch.Restart();
                ////************************************************************************************************


                //var clusters = new PointClusters<Point>(points);
                //var cSlow = clusters.GetClusters((p1, p2) => Point.EuclideanDistance(p1, p2) < minDistance).Count;


                ////************************************************************************************************
                //watch.Stop();
                //var tSlow = watch.ElapsedMilliseconds / 1000;
                //watch.Restart();
                ////************************************************************************************************


                var kdtreeCluster = new KdTreePointClusters<Point>(points, new Group<Point>(Point.NaN));
                //kdtreeCluster.GetClusters((p1, p2) => Point.EuclideanDistance(p1, p2) < minDistance);
                kdtreeCluster.GetClusters((p1, p2) => SpatialUtility.GetEuclideanDistance(p1, p2) < minDistance);


                //************************************************************************************************
                watch.Stop();
                var tNormal = watch.ElapsedMilliseconds / 1000;
                watch.Restart();
                //************************************************************************************************


                var centers = kdtreeCluster.GetGroupCenters();


                //************************************************************************************************
                watch.Stop();
                var tGetGroupCenters = watch.ElapsedMilliseconds / 1000;
                watch.Restart();
                //************************************************************************************************


                for (int i = 0; i < source.Count; i++)
                {
                    try
                    {
                        if (source[i].IsNullOrEmpty())
                            continue;

                        if (source[i].Type == GeometryType.Point)
                        {
                            result.Add(source[i]);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }



                //************************************************************************************************
                watch.Stop();
                var tAddNonPoints = watch.ElapsedMilliseconds / 1000;//System.Diagnostics.Debug.WriteLine($"\t\tADDNONPOINTS {watch.ElapsedMilliseconds / 1000} s", "PYRAMID");
                watch.Restart();
                //************************************************************************************************



                var srid = source.FirstOrDefault(i => !i.IsNullOrEmpty()).Srid;

                result.AddRange(centers.Select(i => i.AsGeometry(srid)));



                //************************************************************************************************
                watch.Stop();
                var tAddPoints = watch.ElapsedMilliseconds / 1000;//System.Diagnostics.Debug.WriteLine($"\t\tADDPOINTS {centers.Count} - {watch.ElapsedMilliseconds / 1000} s", "PYRAMID");
                watch.Restart();
                //************************************************************************************************


                //Debug.WriteLine($"\t\t [Points :{points.Count}] , [Slow-(c: {cSlow} , t: {tSlow})], [Normal-(c:{centers.Count} , t: {tNormal})]", "PYRAMID");

                Debug.WriteLine($"\t\t GetGroupCenters: {tGetGroupCenters}, AddNonPoints: {tAddNonPoints}, AddPoints: {tAddPoints}", "PYRAMID");

                return result;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

    }
}
