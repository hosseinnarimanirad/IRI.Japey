using IRI.Msh.Common.Analysis;
using IRI.Msh.Common.Primitives;
using IRI.Msh.DataStructure.AdvancedStructures;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text; 

namespace IRI.Extensions;

public static class Sta_GeometryExtensions
{

    #region Simplification


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

    //public static Geometry<Point> Simplify(this Geometry<Point> geometry, SimplificationType type, SimplificationParamters parameters /*double threshold,  bool retain3Points, double areaThreshold = double.NaN*/)
    //{
    //    if (geometry.IsNotValidOrEmpty())
    //    {
    //        return geometry;
    //    }

    //    //var extractedGeometry = geometry.AsGeometry();

    //    var filteredGeometry = geometry.Simplify(type, parameters);

    //    return filteredGeometry;//.AsSqlGeometry().MakeValid();
    //}

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


    #endregion
}
