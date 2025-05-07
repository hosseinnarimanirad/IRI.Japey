// besmellahe rahmane rahim
// Allahomma ajjel le-valiyek al-faraj

using IRI.Sta.Common.Primitives;
using IRI.Sta.ShapefileFormat.EsriType;
using IRI.Sta.Spatial.Primitives;
using System;

namespace IRI.Sta.ShapefileFormat.ShapeTypes.Abstractions;


public interface IEsriShape
{
    BoundingBox MinimumBoundingBox { get; }

    int Srid { get; set; }

    byte[] WriteContentsToByte();

    /// <summary>
    /// Length of the record contents section measured in 16-bit words.
    /// </summary>
    int ContentLength { get; }

    EsriShapeType Type { get; }

    string AsSqlServerWkt();

    byte[] AsWkb();

    Ket.KmlFormat.Primitives.PlacemarkType AsPlacemark(Func<Point, Point> projectFunc = null, byte[] color = null);

    string AsKml(Func<Point, Point> projectToGeodeticFunc = null);

    IEsriShape Transform(Func<IPoint, IPoint> transform, int newSrid);// where TPoint : IPoint, new(); 

    Geometry<Point> AsGeometry();

    bool IsNullOrEmpty();

    bool IsRingBase();
}
