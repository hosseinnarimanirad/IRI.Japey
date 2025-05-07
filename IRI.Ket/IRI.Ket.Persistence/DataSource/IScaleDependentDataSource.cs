using IRI.Sta.Common.Primitives;
using IRI.Sta.Spatial.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.Persistence.DataSources;

public interface IScaleDependentDataSource : IDataSource
{
    List<Geometry<Point>> GetGeometries(double mapScale);

    List<Geometry<Point>> GetGeometries(double mapScale, BoundingBox boundingBox);

    Task<List<Geometry<Point>>> GetGeometriesAsync(double mapScale);

    Task<List<Geometry<Point>>> GetGeometriesAsync(double mapScale, BoundingBox boundingBox);
     
}
