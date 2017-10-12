using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRI.Ket.Common.Helpers
{
    public static class PathHelper
    {
        public static string GetExtensionWithoutDot(string path)
        {
            return System.IO.Path.GetExtension(path).Replace(".", "");
        }
    }
}
