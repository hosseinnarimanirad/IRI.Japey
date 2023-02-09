using IRI.Msh.Common.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Msh.Common.Mapping
{
    public class Index100k : IndexBase
    {
        public override double Height { get => GeodeticIndexes._100kSize; }

        public override double Width { get => GeodeticIndexes._100kSize; }

        public string BlockName { get; set; }

        public string BlockNumber { get; set; }
         

        public override Feature<Point> AsFeature()
        {
            return new Feature<Point>()
            {
                TheGeometry = this.TheGeometry,
                LabelAttribute = nameof(this.SheetNameEn),
                Attributes = new Dictionary<string, object>()
                {
                    {nameof(this.Height), this.Height },
                    {nameof(this.Id), this.Id},
                    {nameof(this.MinLatitude), this.MinLatitude},
                    {nameof(this.MinLongitude), this.MinLongitude },
                    {nameof(this.SheetNameEn), this.SheetNameEn },
                    {nameof(this.SheetNameFa), this.SheetNameFa },
                    {nameof(this.SheetNumber), this.SheetNumber },
                    {nameof(this.Width), this.Width },
                    {nameof(this.BlockName), this.BlockName },
                    {nameof(this.BlockNumber), this.BlockNumber },
                },
            };
        }
    }
}
