using IRI.Msh.CoordinateSystem.MapProjection;

namespace IRI.Msh.Common.Primitives;

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

    bool Intersects(IGeometry other);
}