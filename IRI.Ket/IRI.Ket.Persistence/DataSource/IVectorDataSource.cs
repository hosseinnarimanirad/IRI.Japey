using IRI.Msh.Common.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.Persistence.DataSources;

public interface IVectorDataSource : IDataSource
{
    GeometryType? GeometryType { get; }


    // Get Geometries *********************************************************

    List<Geometry<Point>> GetGeometries();
    List<Geometry<Point>> GetGeometries(BoundingBox boundingBox);
    List<Geometry<Point>> GetGeometries(Geometry<Point>? geometry);

    Task<List<Geometry<Point>>> GetGeometriesAsync();
    Task<List<Geometry<Point>>> GetGeometriesAsync(BoundingBox boundingBox);
    Task<List<Geometry<Point>>> GetGeometriesAsync(Geometry<Point>? geometry);


    // Get Named-Geometries ***************************************************

    List<NamedGeometry> GetNamedGeometries();
    List<NamedGeometry> GetNamedGeometries(BoundingBox boundary);
    List<NamedGeometry> GetNamedGeometries(Geometry<Point>? geometry);
    List<NamedGeometry> GetNamedGeometries(double mapScale, BoundingBox boundingBox);

    Task<List<NamedGeometry>> GetNamedGeometriesAsync();
    Task<List<NamedGeometry>> GetNamedGeometriesAsync(BoundingBox boundary);
    Task<List<NamedGeometry>> GetNamedGeometriesAsync(Geometry<Point>? geometry);
    Task<List<NamedGeometry>> GetNamedGeometriesAsync(double mapScale, BoundingBox boundingBox);



    // Get as FeatureSet of Point *********************************************

    FeatureSet<Point> GetAsFeatureSetOfPoint();
    FeatureSet<Point> GetAsFeatureSetOfPoint(BoundingBox boundary);
    FeatureSet<Point> GetAsFeatureSetOfPoint(Geometry<Point>? geometry);
    FeatureSet<Point> GetAsFeatureSetOfPoint(double mapScale, BoundingBox boundingBox);

    Task<FeatureSet<Point>> GetAsFeatureSetOfPointAsync();
    Task<FeatureSet<Point>> GetAsFeatureSetOfPointAsync(BoundingBox boundary);
    Task<FeatureSet<Point>> GetAsFeatureSetOfPointAsync(Geometry<Point>? geometry);
    Task<FeatureSet<Point>> GetAsFeatureSetOfPointAsync(double mapScale, BoundingBox boundingBox);




    // Other ******************************************************************
    FeatureSet<Point> Search(string searchText);


    // CRUD ******************************************************************

    void Add(IGeometryAware<Point> newValue);

    void Remove(IGeometryAware<Point> value);

    void Update(IGeometryAware<Point> newValue);

    void SaveChanges();
}
