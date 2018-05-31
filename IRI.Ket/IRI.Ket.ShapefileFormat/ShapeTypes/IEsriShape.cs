// besmellahe rahmane rahim
// Allahomma ajjel le-valiyek al-faraj

using IRI.Sta.Common.Primitives;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace IRI.Ket.ShapefileFormat.EsriType
{

    public interface IEsriShape
    {
        IRI.Sta.Common.Primitives.BoundingBox MinimumBoundingBox { get; }

        byte[] WriteContentsToByte();

        /// <summary>
        /// Length of the record contents section measured in 16-bit words.
        /// </summary>
        int ContentLength { get; }

        EsriShapeType Type { get; }

        string AsSqlServerWkt();

        byte[] AsWkb();

        IRI.Ket.KmlFormat.Primitives.PlacemarkType AsPlacemark(Func<IRI.Sta.Common.Primitives.Point, IRI.Sta.Common.Primitives.Point> projectFunc = null, byte[] color = null);

        string AsKml(Func<IRI.Sta.Common.Primitives.Point, IRI.Sta.Common.Primitives.Point> projectToGeodeticFunc = null);

        IEsriShape Transform(Func<IPoint, IPoint> transform);

        Geometry AsGeometry();
    }
}
