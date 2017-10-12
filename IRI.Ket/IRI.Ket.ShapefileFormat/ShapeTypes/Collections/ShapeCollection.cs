using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Runtime.Serialization;

namespace IRI.Ket.ShapefileFormat.EsriType
{
    
  
    public class ShapeCollection<T> : List<T>, IShapeCollection where T : IShape
    {
        private MainFileHeader mainHeader;

        public ShapeCollection()
        {
           
        }
        
        public ShapeCollection(MainFileHeader header)
        {
            this.mainHeader = header;
        }

        public ShapeCollection(MainFileHeader header, List<T> values)
        {
            this.AddRange(values);

            this.mainHeader = header;
        }

        public ShapeCollection(List<T> values)
        {
            if (typeof(T) is IRI.Ket.ShapefileFormat.EsriType.IPointsWithMeasure)
            {
                throw new NotImplementedException();
            }

            if (values.Select(i => i.Type).Distinct().Count() > 1)
            {
                throw new NotImplementedException();
            }

            this.AddRange(values);
             
            ////////this.mainHeader = new MainFileHeader(length, values[0].Type, minimumBoundingBox);

            var minimumBoundingBox = IRI.Ham.SpatialBase.BoundingBox.GetMergedBoundingBox(values.Select(i => i.MinimumBoundingBox));
           
            //The content length for a record is the length of the record contents section measured in
            //16-bit words. Each record, therefore, contributes (4 + content length) 16-bit words
            //toward the total length of the file, as stored at Byte 24 in the file header.
            var length = values.Sum(i => i.ContentLength + 4);

            this.mainHeader = new MainFileHeader(length, values.First().Type, minimumBoundingBox);
        }
         
        #region IShapeCollection Members

        public new IShape this[int index]
        {
            get
            {
                return (IShape)base[index];
            }
            set
            {
                base[index] = (T)value;
            }
        }

        public MainFileHeader MainHeader
        {
            get { return this.mainHeader; }
        }

        public string AsKml()
        {
            IRI.Ket.KmlFormat.Primitives.KmlType result = new KmlFormat.Primitives.KmlType();

            IRI.Ket.KmlFormat.Primitives.DocumentType document = new KmlFormat.Primitives.DocumentType();

            var placemarks = ((List<T>)this).Select(i => i.AsPlacemark());

            document.AbstractFeature = placemarks.OfType<KmlFormat.Primitives.AbstractFeatureType>().ToArray();

            result.KmlObjectExtensionGroup = new KmlFormat.Primitives.AbstractObjectType[] { document };

            return IRI.Ket.Common.Helpers.XmlHelper.Parse(result);
        }

        #endregion

        #region IEnumerable<IShape> Members

        public new IEnumerator<IShape> GetEnumerator()
        {
            for (int i = 0; i < this.Count; i++)
            {
                yield return base[i];
            }
        }

        #endregion



    }
}
