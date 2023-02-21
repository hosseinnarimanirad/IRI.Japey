using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IRI.Ket.KmlFormat;
using IRI.Ket.KmlFormat.Primitives;

namespace IRI.Sta.ShapefileFormat
{
    static class OgcKmlMapFunctions
    {
        internal static string AsKml(PlacemarkType placemark)
        {
            return AsKml(new AbstractFeatureType[] { placemark });
        }

        internal static string AsKml(AbstractFeatureType[] abstractFeatureType)
        {
            IRI.Ket.KmlFormat.Primitives.KmlType result = new KmlType();

            IRI.Ket.KmlFormat.Primitives.DocumentType document = new DocumentType();

            document.AbstractFeature = abstractFeatureType;

            result.KmlObjectExtensionGroup = new AbstractObjectType[] { document };

            return IRI.Sta.Common.Helpers.XmlHelper.Parse(result);
        }
    }
}
