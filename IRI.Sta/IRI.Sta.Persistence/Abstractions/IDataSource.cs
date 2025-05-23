using IRI.Sta.Common.Primitives;

namespace IRI.Sta.Persistence.Abstractions;

public interface IDataSource
{
    BoundingBox WebMercatorExtent { get; }

    public int Srid { get; }
}
