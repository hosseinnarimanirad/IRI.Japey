using IRI.Sta.Common.Primitives;

namespace IRI.Ket.DataManagement.Model
{
    public interface IDataSource
    {
        BoundingBox Extent { get; }
    }
}
