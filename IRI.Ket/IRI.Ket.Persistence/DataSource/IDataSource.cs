using IRI.Sta.Common.Primitives;

namespace IRI.Ket.Persistence.DataSources;

public interface IDataSource
{
    BoundingBox WebMercatorExtent { get; }

    public int Srid { get; }
}
