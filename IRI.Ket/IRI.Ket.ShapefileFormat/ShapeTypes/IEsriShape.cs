// besmellahe rahmane rahim
// Allahomma ajjel le-valiyek al-faraj

using IRI.Msh.Common.Primitives;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace IRI.Ket.ShapefileFormat.EsriType
{

    public interface IEsriShape
    {
        IRI.Msh.Common.Primitives.BoundingBox MinimumBoundingBox { get; }

        int Srid { get; }

        byte[] WriteContentsToByte();

        /// <summary>
        /// Length of the record contents section measured in 16-bit words.
        /// </summary>
        int ContentLength { get; }

        EsriShapeType Type { get; }

        string AsSqlServerWkt();

        byte[] AsWkb();

        IRI.Ket.KmlFormat.Primitives.PlacemarkType AsPlacemark(Func<IRI.Msh.Common.Primitives.Point, IRI.Msh.Common.Primitives.Point> projectFunc = null, byte[] color = null);

        string AsKml(Func<IRI.Msh.Common.Primitives.Point, IRI.Msh.Common.Primitives.Point> projectToGeodeticFunc = null);

        IEsriShape Transform(Func<IPoint, IPoint> transform, int newSrid);

        Geometry AsGeometry();
    }
}
