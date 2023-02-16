using IRI.Msh.Common.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.DataManagement.DataSource;

public interface IVectorDataSource : IDataSource
{
    #region Get Geometries

    List<Geometry<Point>> GetGeometries();
    List<Geometry<Point>> GetGeometries(BoundingBox boundingBox);
    List<Geometry<Point>> GetGeometries(Geometry<Point> geometry);

    #endregion



    #region Get Geometry Label Pairs

    List<NamedGeometry> GetNamedGeometries();
    List<NamedGeometry> GetNamedGeometries(BoundingBox boundary);
    List<NamedGeometry> GetNamedGeometries(Geometry<Point>? geometry);

    Task<List<NamedGeometry>> GetNamedGeometriesAsync(BoundingBox boundary);

    #endregion



    #region GetAsFeatureSet

    FeatureSet<Point> GetAsFeatureSet();
    FeatureSet<Point> GetAsFeatureSet(BoundingBox boundary);
    FeatureSet<Point> GetAsFeatureSet(Geometry<Point>? geometry);
    Task<FeatureSet<Point>> GetAsFeatureSetForDisplayAsync(double mapScale, BoundingBox boundingBox);

    #endregion

    #region CRUD

    void Add(IGeometryAware<Point> newValue);

    void Remove(IGeometryAware<Point> value);

    void Update(IGeometryAware<Point> newValue);

    void SaveChanges();

    #endregion
}
