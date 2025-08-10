using IRI.Maptor.Sta.Common.Primitives;
using IRI.Maptor.Sta.Spatial.Primitives;
 

namespace IRI.Maptor.Sta.Spatial.MapIndexes;

public class Index250k : IndexBase
{
    public override double Height { get => GeodeticIndexes._250kHeight; }

    public override double Width { get => GeodeticIndexes._250kWidth; }


    public override Feature<Point> AsFeature()
    {
        return new Feature<Point>()
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
                {nameof(Width), Width }
            },
        };
    }
}
