using IRI.Msh.Common.Primitives;

namespace IRI.Ket.DataManagement.DataSource
{
    public interface IDataSource
    {
        BoundingBox Extent { get; }
    }
}
