using IRI.Sta.Common.Primitives;
using IRI.Sta.Spatial.Primitives;

namespace IRI.Sta.Persistence.Abstractions;

public interface IEditableVectorDataSource//<TGeometryAware, TPoint>
                                          //where TGeometryAware : IGeometryAware<TPoint>
                                          //where TPoint : IPoint, new()
{
    void Add(Feature<Point> newValue);

    void Remove(Feature<Point> value);

    void Update(Feature<Point> newValue);

    void SaveChanges();
}
