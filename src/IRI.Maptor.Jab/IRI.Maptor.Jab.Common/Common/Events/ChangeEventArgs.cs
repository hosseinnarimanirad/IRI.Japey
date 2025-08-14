using System; 

namespace IRI.Maptor.Jab.Common;

public class ChangeEventArgs<T> : EventArgs
{
    public T OldValue { get; private set; }

    public T NewValue { get; private set; }

    public ChangeEventArgs(T oldValue, T newValue)
    {
        this.OldValue = oldValue;

        this.NewValue = newValue;
    }
}
