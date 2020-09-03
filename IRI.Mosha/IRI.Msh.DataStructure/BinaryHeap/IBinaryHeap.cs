using System;
namespace IRI.Msh.DataStructure
{
    public interface IBinaryHeap<T> where T : IComparable<T>
    {
        int Length { get; }
        T ReleaseValue();
    }
}
