using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.Common.Extensions
{
    public static class ExceptionExtensions
    {
        public static string GetMessagePlus(this Exception ex)
        {
            if (ex == null)
            {
                return string.Empty;
            }

            return $"MESSAGE: { ex.Message} INNER: {ex.InnerException?.Message ?? string.Empty}";
        }

        public static string GetFullMessage(this Exception ex)
        {
            if (ex == null)
            {
                return string.Empty;
            }

            var innerException = ex?.Message + " - " + GetFullMessage(ex.InnerException);

            return $"{ex.Message}" + innerException != null ? $" {Environment.NewLine} {innerException}" : string.Empty;
        }
    }
}
