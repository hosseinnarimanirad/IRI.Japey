using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Ham.SpatialBase
{
    public class GeoReferencedImage
    {
        public BoundingBox GeodeticWgs84BoundingBox { get; set; }
         
        public byte[] Image { get; set; }

        public GeoReferencedImage(byte[] image, BoundingBox geodeticBoundingBox, bool isValid = true)
        {
            this.Image = image;

            this.GeodeticWgs84BoundingBox = geodeticBoundingBox;

            this.IsValid = isValid;
        }
        
        public bool IsValid { get; set; }
    }
}
