using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Jab.Common
{
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
}
