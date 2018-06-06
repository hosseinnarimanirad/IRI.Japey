using IRI.Ket.ShapefileFormat.EsriType;
using IRI.MainProjectWPF.LargeData.Model;
using IRI.Msh.Common.Mapping;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.MainProjectWPF.LargeData.Utilities
{
    public static class SimplifyAlgorithms
    {
        public static IEsriShapeCollection AdditiveSimplifyByArea(IEsriShapeCollection shapes, int zoomLevel)
        {
            var unitDistance = WebMercatorUtility.CalculateGroundResolution(zoomLevel, 35);

            var unitArea = unitDistance * unitDistance;

            return AdditiveSimplifyByArea(shapes, unitArea);
        }

        public static IEsriShapeCollection AdditiveSimplifyByArea(IEsriShapeCollection shapes, double threshold)
        {
            return Process(shapes, threshold, (i, t) => ShapeUtility.AdditiveSimplifyByArea(i, t));

            //ShapeCollection<Polygon> result = new ShapeCollection<Polygon>();

            //foreach (ISimplePoints feature in shapes)
            //{
            //    var points = feature.Parts.Select((i, index) => ShapeUtility.AdditiveSimplifyByArea(feature.GetPart(index), threshold)).ToArray();

            //    if (points == null)
            //        continue;

            //    result.Add(new Polygon(points));
            //}

            //return result;
        }

        public static IEsriShapeCollection AdditiveSimplifyByAreaPlus(IEsriShapeCollection shapes, double threshold)
        {
            return Process(shapes, threshold, (i, t) => ShapeUtility.AdditiveSimplifyByAreaPlus(i, t));
        }

        public static IEsriShapeCollection SimplifyByArea(IEsriShapeCollection shapes, double threshold)
        {
            return Process(shapes, threshold, (i, t) => ShapeUtility.SimplifyByArea(i, t));

            //ShapeCollection<PolyLine> result = new ShapeCollection<PolyLine>();

            //foreach (ISimplePoints feature in shapes)
            //{
            //    var temp = ShapeUtility.SimplifyByArea(feature.Points, threshold);

            //    if (temp == null)
            //        continue;

            //    if (temp.Length > 2)
            //    {
            //        result.Add(new PolyLine(temp));
            //    }
            //}

            //return result;
        }

        public static IEsriShapeCollection SimplifyByArea(IEsriShapeCollection shapes, int zoomLevel)
        {
            var unitDistance = WebMercatorUtility.CalculateGroundResolution(zoomLevel, 35);

            var unitArea = unitDistance * unitDistance;

            return SimplifyByArea(shapes, unitArea);
        }

        public static IEsriShapeCollection SimplifyByAngle(IEsriShapeCollection shapes, double threshold)
        {
            return Process(shapes, threshold, (i, t) => ShapeUtility.SimplifyByAngle(i, t));


            //if (shapes.First().Type == ShapeType.Polygon || shapes.First().Type == ShapeType.PolygonZ)
            //{
            //    var result = new ShapeCollection<Polygon>();

            //    foreach (ISimplePoints feature in shapes)
            //    {
            //        var temp = ShapeUtility.SimplifyByAngle(feature.Points, threshold);

            //        if (temp == null)
            //            continue;

            //        if (temp.Length > 2)
            //        {
            //            result.Add(new Polygon(temp));
            //        }
            //    }

            //    return result;
            //}
            //else if (shapes.First().Type == ShapeType.PolyLine || shapes.First().Type == ShapeType.PolyLineZ || shapes.First().Type == ShapeType.PolyLineM)
            //{
            //    var result = new ShapeCollection<PolyLine>();

            //    foreach (ISimplePoints feature in shapes)
            //    {
            //        var temp = ShapeUtility.SimplifyByAngle(feature.Points, threshold);

            //        if (temp == null)
            //            continue;

            //        if (temp.Length > 2)
            //        {
            //            result.Add(new PolyLine(temp));
            //        }
            //    }

            //    return result;
            //}
            //else
            //{
            //    throw new NotImplementedException();
            //}
        }

        public static IEsriShapeCollection AdditiveSimplifyByAngle(IEsriShapeCollection shapes, double threshold)
        {
            return Process(shapes, threshold, (i, t) => ShapeUtility.AdditiveSimplifyByAngle(i, t));

            //if (shapes.First().Type == ShapeType.Polygon || shapes.First().Type == ShapeType.PolygonZ)
            //{
            //    var result = new ShapeCollection<Polygon>();

            //    foreach (ISimplePoints feature in shapes)
            //    {
            //        //var temp = ShapeUtility.SimplifyByAngle(feature.Points, threshold);
            //        var temp = feature.Parts.Select((i, index) => ShapeUtility.AdditiveSimplifyByAngle(feature.GetPart(index), threshold)).ToArray();

            //        if (temp == null || temp[0].Length == 0)
            //            continue;

            //        //if (temp.Length > 2)
            //        //{
            //        result.Add(new Polygon(temp));
            //        //}
            //    }

            //    return result;
            //}
            //else if (shapes.First().Type == ShapeType.PolyLine || shapes.First().Type == ShapeType.PolyLineZ || shapes.First().Type == ShapeType.PolyLineM)
            //{
            //    var result = new ShapeCollection<PolyLine>();

            //    foreach (ISimplePoints feature in shapes)
            //    {
            //        //var temp = ShapeUtility.SimplifyByAngle(feature.Points, threshold);
            //        var temp = feature.Parts.Select((i, index) => ShapeUtility.AdditiveSimplifyByAngle(feature.GetPart(index), threshold)).ToArray();

            //        if (temp == null || temp.Length == 0 || temp.Sum(i => i.Length) == 0)
            //            continue;

            //        //if (temp.Length > 1)
            //        //{
            //        result.Add(new PolyLine(temp));
            //        //}
            //    }

            //    return result;
            //}
            //else
            //{
            //    throw new NotImplementedException();
            //}

        }

        public static IEsriShapeCollection AdditiveSimplifyByDistance(IEsriShapeCollection shapes, double threshold)
        {
            return Process(shapes, threshold, (i, t) => ShapeUtility.AdditiveSimplifyByDistance(i, t));
        }



        static void SuperSlim(string file)
        {
            var shapes = IRI.Ket.ShapefileFormat.Shapefile.ReadShapes(file);

            EsriShapeCollection<EsriPolygon> polygons = new EsriShapeCollection<EsriPolygon>();

            //var stat = new FeatureStatistics[120];

            int index = 0;

            foreach (EsriPolygon feature in shapes)
            {
                index = 0;

                //UpdateFeatureStat(ref stat[index++], feature.Points, -1);

                var temp = ShapeUtility.SimplifyByArea(feature.Points, 100);

                //UpdateFeatureStat(ref stat[index++], temp, 100);

                if (temp == null)
                    continue;

                for (int i = 1; i < 30; i++)
                {
                    var threshold = i * 100 + 100;

                    temp = ShapeUtility.SimplifyByArea(temp, threshold);

                    //UpdateFeatureStat(ref stat[index++], temp, threshold);
                }

                for (int i = 1; i < 11; i++)
                {
                    var threshold = (i * 500) + 5000;

                    temp = ShapeUtility.SimplifyByArea(temp, threshold);

                    //UpdateFeatureStat(ref stat[index++], temp, threshold);
                }

                for (int i = 1; i < 11; i++)
                {
                    var threshold = (i * 1000) + 10000;

                    temp = ShapeUtility.SimplifyByArea(temp, threshold);

                    //UpdateFeatureStat(ref stat[index++], temp, threshold);
                }

                for (int i = 1; i < 17; i++)
                {
                    var threshold = (i * 5000) + 20000;

                    temp = ShapeUtility.SimplifyByArea(temp, threshold);

                    //UpdateFeatureStat(ref stat[index++], temp, threshold);
                }

                for (int i = 1; i < 25; i++)
                {
                    var threshold = (i * 20000) + 100000;

                    temp = ShapeUtility.SimplifyByArea(temp, threshold);

                    //UpdateFeatureStat(ref stat[index++], temp, threshold);
                }

                if (temp.Length > 0)
                {
                    polygons.Add(new EsriPolygon(temp));
                }
            }

            IRI.Ket.ShapefileFormat.Shapefile.Save(@"E:\Data\0. Test\Large Data\94.04.10\IRI1OstanMercatorSuperSlim.shp", polygons, true);

            //Debug.Print("\n****************\n");

            //foreach (var item in stat)
            //{
            //    Debug.Print(item.ToString());
            //}

            //Debug.Print("\n****************\n");

            //foreach (var item in stat)
            //{
            //    Debug.Print(item.ToCSV());
            //}

        }

        public static IEsriShapeCollection Process(IEsriShapeCollection shapes, double threshold, Func<EsriPoint[], double, EsriPoint[]> method)
        {
            if (shapes.First().Type == EsriShapeType.EsriPolygon || shapes.First().Type == EsriShapeType.EsriPolygonZ)
            {
                var result = new List<EsriPolygon>();

                foreach (IEsriSimplePoints feature in shapes)
                {
                    var temp = feature.Parts.Select((i, index) => method(feature.GetPart(index), threshold)).Where(i => i != null && i.Length > 3).ToArray();

                    if (temp == null || temp.Length == 0 || temp.Sum(i => i.Length) == 0)
                        continue;

                    result.Add(new EsriPolygon(temp));
                }

                return new EsriShapeCollection<EsriPolygon>(result);
            }
            else if (shapes.First().Type == EsriShapeType.EsriPolyLine || shapes.First().Type == EsriShapeType.EsriPolyLineZ || shapes.First().Type == EsriShapeType.EsriPolyLineM)
            {
                var result = new List<EsriPolyline>();

                foreach (IEsriSimplePoints feature in shapes)
                {
                    var temp = feature.Parts.Select((i, index) => method(feature.GetPart(index), threshold)).Where(i => i != null && i.Length > 1).ToArray();

                    if (temp == null || temp.Length == 0 || temp.Sum(i => i.Length) == 0)
                        continue;

                    result.Add(new EsriPolyline(temp));
                }

                return new EsriShapeCollection<EsriPolyline>(result);
            }
            else
            {
                throw new NotImplementedException();
            }

        }

        //public static IShapeCollection IterativeSimplifyByArea(IShapeCollection shapes, double[] thresholds)
        //{
        //    var result = SimplifyByArea(shapes, thresholds[0]);

        //    for (int i = 1; i < thresholds.Length; i++)
        //    {
        //        result = SimplifyByArea(shapes, thresholds[i]);
        //    }

        //    return result;
        //}

    }
}
