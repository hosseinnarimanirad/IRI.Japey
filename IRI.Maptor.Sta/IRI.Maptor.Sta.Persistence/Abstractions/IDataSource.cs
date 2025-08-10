using IRI.Maptor.Sta.Common.Primitives;

namespace IRI.Maptor.Sta.Persistence.Abstractions;

public interface IDataSource
{
    BoundingBox WebMercatorExtent { get; }

    public int Srid { get; }
}
