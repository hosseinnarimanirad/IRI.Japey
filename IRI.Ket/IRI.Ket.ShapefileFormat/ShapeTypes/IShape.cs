// besmellahe rahmane rahim
// Allahomma ajjel le-valiyek al-faraj

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace IRI.Ket.ShapefileFormat.EsriType
{

    public interface IShape
    {
        IRI.Ham.SpatialBase.BoundingBox MinimumBoundingBox { get; }

        byte[] WriteContentsToByte();

        /// <summary>
        /// Length of the record contents section measured in 16-bit words.
        /// </summary>
        int ContentLength { get; }

        ShapeType Type { get; }

        string AsSqlServerWkt();

        byte[] AsWkb();

        IRI.Ket.KmlFormat.Primitives.PlacemarkType AsPlacemark(Func<IRI.Ham.SpatialBase.Point, IRI.Ham.SpatialBase.Point> projectFunc = null, byte[] color = null);

        string AsKml(Func<IRI.Ham.SpatialBase.Point, IRI.Ham.SpatialBase.Point> projectToGeodeticFunc = null);

        IShape Transform(Func<Ham.SpatialBase.IPoint, Ham.SpatialBase.IPoint> transform);
    }
}
