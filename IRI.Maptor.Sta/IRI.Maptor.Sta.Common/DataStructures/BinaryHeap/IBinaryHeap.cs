using System;
namespace IRI.Maptor.Sta.DataStructures;

public interface IBinaryHeap<T> where T : IComparable<T>
{
    int Length { get; }
    T ReleaseValue();
}
