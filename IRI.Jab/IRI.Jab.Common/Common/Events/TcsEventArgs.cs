using System.Threading.Tasks;

namespace IRI.Jab.Common;

public class TcsEventArgs<T1, T2> : System.EventArgs
{
    public TaskCompletionSource<T1> Tcs { get; private set; }

    public T2 Arg { get; set; }

    public TcsEventArgs(TaskCompletionSource<T1> tcs, T2 arg)
    {
        this.Tcs = tcs;

        this.Arg = arg;
    }
}
