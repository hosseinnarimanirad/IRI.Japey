using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.Common.Service
{
    public class Response<T>
    {
        public bool IsFailed { get; set; }

        public string ErrorMessage { get; set; }

        public T Result { get; set; }


    }

    public static class ResponseFactory
    {
        public static Response<T> Create<T>(T result)
        {
            return new Response<T>() { Result = result, ErrorMessage = string.Empty, IsFailed = false };
        }

        public static Response<T> CreateError<T>(string errorMessage)
        {
            return new Response<T> { ErrorMessage = errorMessage, IsFailed = true };
        }
    }
}
