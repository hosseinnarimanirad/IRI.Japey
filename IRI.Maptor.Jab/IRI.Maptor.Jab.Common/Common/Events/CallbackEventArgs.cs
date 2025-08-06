using System; 

namespace IRI.Maptor.Jab.Common;

public class CallbackEventArgs<T> : System.EventArgs
{
    public Action Callback { get; private set; }

    public T Arg { get; set; }

    public CallbackEventArgs(T arg, Action callback)
    {
        this.Arg = arg;

        this.Callback = callback;
    }
}
