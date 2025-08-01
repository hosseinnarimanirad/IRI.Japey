﻿using IRI.Sta.SpatialReferenceSystem;
using System.Globalization;
using IRI.Sta.Common.Primitives;
using IRI.Sta.Spatial.Helpers;

namespace IRI.Sta.Spatial.Model;

public class TileInfo
{
    public int RowNumber { get; private set; }

    public int ColumnNumber { get; private set; }

    public int ZoomLevel { get; private set; }
     
    public BoundingBox GeodeticExtent { get; private set; }
     
    public BoundingBox WebMercatorExtent
    {
        get
        {
            return GeodeticExtent.Transform(i => MapProjects.GeodeticWgs84ToWebMercator(i));
        }
    }

    /// <summary>
    /// Extent based on sphere
    /// </summary>
    public BoundingBox GeocentricExtent
    {
        get
        {
            return GeodeticExtent.Transform(i => Transformations.ChangeDatum(i, Ellipsoids.WGS84, Ellipsoids.Sphere));
        }
    }

    public TileInfo(int row, int column, int zoomLevel)
    {
        this.RowNumber = row;

        this.ColumnNumber = column;

        this.ZoomLevel = zoomLevel;

        //this.UsedMapScale = usedMapScale;

        this.GeodeticExtent = WebMercatorUtility.GetWgs84ImageBoundingBox(row, column, zoomLevel);
    }

    public override string ToString()
    {
        return $"Z:{ZoomLevel}, R: {RowNumber}, C: {ColumnNumber}, W:{WebMercatorExtent.Width}, H:{WebMercatorExtent.Height}, X:{WebMercatorExtent.TopLeft.X}, Y:{WebMercatorExtent.TopLeft.Y}";
    }

    public string ToShortString()
    {
        return $"{ZoomLevel.ToString(CultureInfo.InvariantCulture)}, {RowNumber.ToString(CultureInfo.InvariantCulture)}, {ColumnNumber.ToString(CultureInfo.InvariantCulture)}";
    }

    public override bool Equals(object obj)
    {
        var other = obj as TileInfo;

        if (other == null)
        {
            return false;
        }

        return this.ZoomLevel == other.ZoomLevel && this.RowNumber == other.RowNumber && this.ColumnNumber == other.ColumnNumber;
    }

    public override int GetHashCode()
    {
        return $"{ZoomLevel}{RowNumber}{ColumnNumber}".GetHashCode();
    }

    public TileInfo Clone()
    {
        return new TileInfo(this.RowNumber, this.ColumnNumber, this.ZoomLevel);
    }
}
