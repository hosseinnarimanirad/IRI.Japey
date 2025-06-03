using IRI.Sta.SpatialReferenceSystem.MapProjections;
using IRI.Sta.Common.Primitives;

namespace IRI.Sta.Spatial.Primitives;

public interface IGeometry
{
    int NumberOfGeometries { get; }

    int NumberOfPoints { get; }

    int Srid { get; set; }

    int TotalNumberOfPoints { get; }

    GeometryType Type { get; set; }

    string AsWkt();

    byte[] AsWkb();

    SrsBase GetSrs();
     
}