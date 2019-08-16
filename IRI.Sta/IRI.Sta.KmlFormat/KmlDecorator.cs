﻿using IRI.Ket.KmlFormat.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Ket.KmlFormat
{
    public static class KmlDecorator
    {
        public static string DecorateWithExtendedData<T>(List<PlacemarkType> placemarks, List<T> attributes, List<string> attributeNames, List<Func<T, string>> extractFuncs)
        {
            int numberOfFeatures = placemarks.Count;

            if (numberOfFeatures != attributes.Count)
            {
                throw new NotImplementedException();
            }

            IRI.Ket.KmlFormat.Primitives.KmlType result =
               new KmlFormat.Primitives.KmlType();

            IRI.Ket.KmlFormat.Primitives.DocumentType document =
                new KmlFormat.Primitives.DocumentType();

            for (int i = 0; i < numberOfFeatures; i++)
            {
                placemarks[i].id = i.ToString();

                IRI.Ket.KmlFormat.Primitives.SimpleDataType[] elements = new IRI.Ket.KmlFormat.Primitives.SimpleDataType[attributeNames.Count];

                for (int j = 0; j < attributeNames.Count; j++)
                {
                    elements[j] =
                            new Primitives.SimpleDataType()
                            {
                                name = attributeNames[j],
                                Value = extractFuncs[j](attributes[i])
                            };
                }

                IRI.Ket.KmlFormat.Primitives.SchemaDataType[] data =
                    new Primitives.SchemaDataType[1];

                data[0] = new Primitives.SchemaDataType() { SimpleData = elements };

                IRI.Ket.KmlFormat.Primitives.ExtendedDataType extendedData =
                    new Primitives.ExtendedDataType();

                extendedData.SchemaData = data;

                placemarks[i].ExtendedData = extendedData;

                placemarks[i].description = i.ToString();
            }

            document.AbstractFeature = placemarks.OfType<KmlFormat.Primitives.AbstractFeatureType>().ToArray();

            result.KmlObjectExtensionGroup = new KmlFormat.Primitives.AbstractObjectType[] { document };

            return IRI.Ket.Common.Helpers.XmlHelper.Parse(result);
        }

        public static string DecorateWithSybmols(List<PlacemarkType> placemark)
        {
            return string.Empty;
        }
    }
}
