using IRI.Sta.Common.Primitives;
using IRI.Sta.Spatial.Primitives;

using System.Threading.Tasks;
using System.Collections.Generic;

namespace IRI.Sta.Persistence.DataSources;

public interface IScaleDependentDataSource : IDataSource
{
    List<Geometry<Point>> GetGeometries(double mapScale);

    List<Geometry<Point>> GetGeometries(double mapScale, BoundingBox boundingBox);

    Task<List<Geometry<Point>>> GetGeometriesAsync(double mapScale);

    Task<List<Geometry<Point>>> GetGeometriesAsync(double mapScale, BoundingBox boundingBox);

}
