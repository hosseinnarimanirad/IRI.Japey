using IRI.Sta.Spatial.Primitives;

namespace IRI.Sta.Spatial.MapIndexes;

public class Index100k : IndexBase
{
    public override double Height { get => GeodeticIndexes._100kSize; }

    public override double Width { get => GeodeticIndexes._100kSize; }

    public string BlockName { get; set; }

    public string BlockNumber { get; set; }


    public override Feature AsFeature()
    {
        return new Feature()
        {
            TheGeometry = TheGeometry,
            LabelAttribute = nameof(SheetNumber),
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
                {nameof(BlockName), BlockName },
                {nameof(BlockNumber), BlockNumber },
            },
        };
    }
}
