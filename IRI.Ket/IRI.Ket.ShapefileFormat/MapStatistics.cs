// besmellahe rahmane rahim
// Allahomma ajjel le-valiyek al-faraj

using System;
using System.Collections.Generic;
using System.Text;
using IRI.Ket.ShapefileFormat.EsriType;
using System.Linq;

namespace IRI.Ket.ShapefileFormat
{
    public static class MapStatistics
    {
        //public static double GetMaxX<T>(IEnumerable<T> points) where T : IPoint
        //{
        //    IEnumerator<T> enumerator = points.GetEnumerator();

        //    enumerator.MoveNext();

        //    double result = enumerator.Current.X;

        //    foreach (IPoint item in points)
        //    {
        //        if (result < item.X)
        //        {
        //            result = item.X;
        //        }
        //    }

        //    return result;
        //}

        //public static double GetMaxY<T>(IEnumerable<T> points) where T : IPoint
        //{
        //    IEnumerator<T> enumerator = points.GetEnumerator();

        //    enumerator.MoveNext();

        //    double result = enumerator.Current.Y;

        //    foreach (IPoint item in points)
        //    {
        //        if (result < item.Y)
        //        {
        //            result = item.Y;
        //        }
        //    }

        //    return result;
        //}

        //public static double GetMinX<T>(IEnumerable<T> points) where T : IPoint
        //{
        //    IEnumerator<T> enumerator = points.GetEnumerator();

        //    enumerator.MoveNext();

        //    double result = enumerator.Current.X;

        //    foreach (IPoint item in points)
        //    {
        //        if (result > item.X)
        //        {
        //            result = item.X;
        //        }
        //    }

        //    return result;
        //}

        //public static double GetMinY<T>(IEnumerable<T> points) where T : IPoint
        //{
        //    IEnumerator<T> enumerator = points.GetEnumerator();

        //    enumerator.MoveNext();

        //    double result = enumerator.Current.Y;

        //    foreach (IPoint item in points)
        //    {
        //        if (result > item.Y)
        //        {
        //            result = item.Y;
        //        }
        //    }

        //    return result;
        //}

        //public static double GetMaxX(List<Polygon> polygons)
        //{
        //    double result = GetMaxX(polygons[0].Points);

        //    for (int i = 1; i < polygons.Count; i++)
        //    {
        //        double temp = GetMaxX(polygons[i].Points);

        //        if (result < temp)
        //        {
        //            result = temp;
        //        }
        //    }

        //    return result;
        //}

        //public static double GetMaxY(List<Polygon> polygons)
        //{
        //    double result = GetMaxY(polygons[0].Points);

        //    for (int i = 1; i < polygons.Count; i++)
        //    {
        //        double temp = GetMaxY(polygons[i].Points);

        //        if (result < temp)
        //        {
        //            result = temp;
        //        }
        //    }

        //    return result;
        //}

        //public static double GetMinX(List<Polygon> polygons)
        //{
        //    double result = GetMinX(polygons[0].Points);

        //    for (int i = 1; i < polygons.Count; i++)
        //    {
        //        double temp = GetMinX(polygons[i].Points);

        //        if (result > temp)
        //        {
        //            result = temp;
        //        }
        //    }

        //    return result;
        //}

        //public static double GetMinY(List<Polygon> polygons)
        //{
        //    double result = GetMinY(polygons[0].Points);

        //    for (int i = 1; i < polygons.Count; i++)
        //    {
        //        double temp = GetMinY(polygons[i].Points);

        //        if (result > temp)
        //        {
        //            result = temp;
        //        }
        //    }

        //    return result;
        //}

        //public static double GetMaxX<T>(IEnumerable<T> values, Func<T, double> getInitialValue, Func<T, double> mapFunction)
        //{
        //    double result = getInitialValue(values.GetEnumerator().Current);

        //    foreach (T item in values)
        //    {
        //        double temp = mapFunction(item);

        //        if (result < temp)
        //        {
        //            result = temp;
        //        }
        //    }

        //    return result;
        //}

    }
}
