using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Ket.ShapefileFormat
{
    static class OgcKmlMapFunctions
    {
        internal static string AsKml(KmlFormat.Primitives.PlacemarkType placemark)
        {
            return AsKml(new KmlFormat.Primitives.AbstractFeatureType[] { placemark });
        }

        internal static string AsKml(KmlFormat.Primitives.AbstractFeatureType[] abstractFeatureType)
        {
            IRI.Ket.KmlFormat.Primitives.KmlType result = new KmlFormat.Primitives.KmlType();

            IRI.Ket.KmlFormat.Primitives.DocumentType document = new KmlFormat.Primitives.DocumentType();

            document.AbstractFeature = abstractFeatureType;

            result.KmlObjectExtensionGroup = new KmlFormat.Primitives.AbstractObjectType[] { document };

            return IRI.Ket.Common.Helpers.XmlHelper.Parse(result);
        }
    }
}
