using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Ket.DataManagement.Infrastructure
{
    public static class ZippedJsonArchiveInfrastructure
    {
        public static string ReadAsString(string zippedArchiveFile, string fileName)
        {
            var archive = System.IO.Compression.ZipFile.OpenRead(zippedArchiveFile);

            var file = archive.Entries.Where(i => i.FullName.Equals(fileName, StringComparison.OrdinalIgnoreCase));

            if (!(file?.Count() == 1))
            {
                return null;
            }

            var stream = file.First().Open();

            return IRI.Msh.Common.Helpers.StreamHelper.ToString(stream, Encoding.UTF8);

        }

        public static byte[] ReadAsBinary(string zippedArchiveFile, string fileName)
        {
            var archive = System.IO.Compression.ZipFile.OpenRead(zippedArchiveFile);

            using (var stream = archive.Entries.Single(e => e.FullName.Equals(fileName, StringComparison.OrdinalIgnoreCase)).Open())
            {
                return IRI.Msh.Common.Helpers.StreamHelper.ToByteArray(stream);
            }            
        }
    }
}
