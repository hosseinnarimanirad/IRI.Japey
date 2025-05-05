using System;
namespace IRI.Sta.DataStructures;

public interface IBinaryHeap<T> where T : IComparable<T>
{
    int Length { get; }
    T ReleaseValue();
}
