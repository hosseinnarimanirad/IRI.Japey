namespace IRI.Maptor.Jab.Common;

public class CustomEventArgs<T> : System.EventArgs
{
    public T Arg { get; set; }

    public CustomEventArgs(T arg)
    {
        this.Arg = arg;
    }
}
