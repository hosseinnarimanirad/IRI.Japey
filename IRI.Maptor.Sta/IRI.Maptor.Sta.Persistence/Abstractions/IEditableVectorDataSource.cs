using IRI.Maptor.Sta.Common.Primitives;
using IRI.Maptor.Sta.Spatial.Primitives;

namespace IRI.Maptor.Sta.Persistence.Abstractions;

public interface IEditableVectorDataSource//<TGeometryAware, TPoint>
                                          //where TGeometryAware : IGeometryAware<TPoint>
                                          //where TPoint : IPoint, new()
{
    void Add(Feature<Point> newValue);

    void Remove(Feature<Point> value);

    void Update(Feature<Point> newValue);

    void SaveChanges();
}
