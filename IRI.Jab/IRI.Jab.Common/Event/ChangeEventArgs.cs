using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Jab.Common
{
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
}
