using IRI.Sta.Common.Primitives;

namespace IRI.Sta.Persistence.DataSources;

public interface IDataSource
{
    BoundingBox WebMercatorExtent { get; }

    public int Srid { get; }
}
