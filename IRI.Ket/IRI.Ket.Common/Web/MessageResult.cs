using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.Common.Web
{
    public class MessageResult<T>
    {
        public bool Succeed { get; set; }

        public T Result { get; set; }

        public string ErrorMessage { get; set; }


        public MessageResult(T value)
        {
            this.Result = value;
        }

        public MessageResult()
        {

        }
    }

    public static class MessageResult
    {
        public static MessageResult<string> CreateFailMessage(string error)
        {
            return new MessageResult<string>(string.Empty) { ErrorMessage = error, Succeed = false };
        }

        public static MessageResult<string> CreateOkMessage(string message)
        {
            return new MessageResult<string>(message) { Succeed = true };
        }

        public static MessageResult<T> CreateOkMessage<T>(T result)
        {
            return new MessageResult<T>(result) { Succeed = true };
        }
    }
}
