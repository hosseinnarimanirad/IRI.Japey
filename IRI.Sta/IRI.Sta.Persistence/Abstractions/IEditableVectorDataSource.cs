using IRI.Sta.Common.Abstrations;
using IRI.Sta.Spatial.Primitives;

namespace IRI.Sta.Persistence.Abstractions;

public interface IEditableVectorDataSource<TGeometryAware, TPoint>
    where TGeometryAware : IGeometryAware<TPoint>
    where TPoint : IPoint, new()
{
    void Add(TGeometryAware newValue);

    void Remove(TGeometryAware value);

    void Update(TGeometryAware newValue);

    void SaveChanges();
}
