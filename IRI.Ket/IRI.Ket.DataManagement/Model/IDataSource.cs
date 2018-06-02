using IRI.Msh.Common.Primitives;

namespace IRI.Ket.DataManagement.Model
{
    public interface IDataSource
    {
        BoundingBox Extent { get; }
    }
}
