﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Jab.Common
{
    public class CustomEventArgs<T> : System.EventArgs
    {
        public T Arg { get; set; }

        public CustomEventArgs(T arg)
        {
            this.Arg = arg;
        }
    }
}
