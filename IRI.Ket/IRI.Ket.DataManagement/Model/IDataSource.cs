using IRI.Ham.SpatialBase;

namespace IRI.Ket.DataManagement.Model
{
    public interface IDataSource
    {
        BoundingBox Extent { get; }
    }
}
