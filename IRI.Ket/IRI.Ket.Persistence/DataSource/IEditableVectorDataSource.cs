using IRI.Msh.Common.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.Persistence.DataSources;

public interface IEditableVectorDataSource<TGeometryAware, TPoint>
    where TGeometryAware : IGeometryAware<TPoint>
    where TPoint : IPoint, new()
{
    void Add(TGeometryAware newValue);

    void Remove(TGeometryAware value);

    void Update(TGeometryAware newValue);

    void SaveChanges();
}
