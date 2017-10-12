using System;
namespace IRI.Ket.DataStructure
{
    public interface IBinaryHeap<T> where T : IComparable<T>
    {
        int Length { get; }
        T ReleaseValue();
    }
}
