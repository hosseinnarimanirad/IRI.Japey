﻿using IRI.Maptor.Sta.Common.Primitives;
using IRI.Extensions;
using IRI.Maptor.Sta.Common.Helpers; 

namespace IRI.Maptor.Tag.SampleCodes.Geodesy;

public static class Geodesy_DegreeDistance
{
    public static void DegreeToEarthDistance(double latitude)
    {
        var basePoint = new Point(0, latitude);

        List<Point> equatorPoints =
        [
            new Point(1, latitude),
            new Point(0.1, latitude),
            new Point(0.01, latitude),
            new Point(0.001, latitude),
            new Point(0.0001, latitude),
            new Point(0.00001, latitude),
            new Point(0.000001, latitude),
            new Point(0.0000001, latitude),
            new Point(0.00000001, latitude),
        ];

        // Print table header
        Console.WriteLine($"| decimal places | degrees        | E/W at {latitude} N/S |");
        Console.WriteLine($"| -------------- | -------------- | -------------- |");

        for (int i = 0; i < equatorPoints.Count; i++)
        {
            var distance = basePoint.SphericalDistance(equatorPoints[i]);
            var degree = equatorPoints[i].X.ToString("0.##########");
            Console.WriteLine($"| {i,-14} | {degree,-14} | {FormatDistance(distance),-14}|");
        }

        Console.WriteLine(" -------------------------------------------------- ");
    }
     

    static string FormatDistance(double meters)
    {
        if (meters >= 1000)
            return $"{meters / 1000:000.##} km";
        if (meters >= 1)
            return $"{meters:000.##} m";
        if (meters >= 0.01)
            return $"{meters * 100:000.##} cm";
        return $"{meters * 1000:000.##} mm";
    }

}
