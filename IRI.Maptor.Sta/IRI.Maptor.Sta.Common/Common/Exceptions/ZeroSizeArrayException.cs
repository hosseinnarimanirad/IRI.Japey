using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace IRI.Maptor.Sta.Common.Exceptions;

[DataContract]
public class ZeroSizeArrayException : Exception
{

    public ZeroSizeArrayException() : base("Array size is zero") { }
    public ZeroSizeArrayException(string message) : base(message) { }
    public ZeroSizeArrayException(string message, Exception inner) : base(message, inner) { }

}
