﻿using IRI.Msh.Common.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Msh.Common.Mapping
{
    public class Index5k : IndexBase
    {
        public override double Height { get => GeodeticIndexes._5kHeight; }

        public override double Width { get => GeodeticIndexes._5kWidth; }

        public string ShortSheetNumber { get { return SheetNumber?.Split(' ').Last(); } }


        public override Feature AsFeature()
        {
            return new Feature()
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
                    {nameof(this.ShortSheetNumber), this.ShortSheetNumber},
                },
            };
        }
    }
}
