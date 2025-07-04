using IRI.Sta.Spatial.Primitives;

namespace IRI.Sta.Spatial.MapIndexes;

public class Index5k : IndexBase
{
    public override double Height { get => GeodeticIndexes._5kHeight; }

    public override double Width { get => GeodeticIndexes._5kWidth; }

    public string ShortSheetNumber { get { return SheetNumber?.Split(' ').LastOrDefault() ?? string.Empty; } }


    public override Feature AsFeature()
    {
        return new Feature()
        {
            TheGeometry = TheGeometry,
            LabelAttribute = nameof(SheetNameEn),
            Attributes = new Dictionary<string, object>()
            {
                {nameof(Height), Height },
                {nameof(Id), Id},
                {nameof(MinLatitude), MinLatitude},
                {nameof(MinLongitude), MinLongitude },
                {nameof(SheetNameEn), SheetNameEn },
                {nameof(SheetNameFa), SheetNameFa },
                {nameof(SheetNumber), SheetNumber },
                {nameof(Width), Width },
                {nameof(ShortSheetNumber), ShortSheetNumber},
            },
        };
    }
}
