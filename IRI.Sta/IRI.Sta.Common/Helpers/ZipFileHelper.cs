﻿using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRI.Sta.Common.Helpers
{
    public static class ZipFileHelper
    {
        public static string ReadAsString(ZipArchive archive, string fileName)
        {
            var file = archive.Entries.Where(i => i.FullName.Equals(fileName, StringComparison.OrdinalIgnoreCase));

            if (!(file?.Count() == 1))
            {
                return null;
            }

            var stream = file.First().Open();

            return IRI.Msh.Common.Helpers.StreamHelper.ToString(stream, Encoding.UTF8);
        }

        public static string OpenAndReadAsString(string zippedArchiveFile, string fileName)
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

        public static byte[] OpenAndReadAsBinary(string zippedArchiveFile, string fileName)
        {
            var archive = System.IO.Compression.ZipFile.OpenRead(zippedArchiveFile);

            using (var stream = archive.Entries.Single(e => e.FullName.Equals(fileName, StringComparison.OrdinalIgnoreCase)).Open())
            {
                return IRI.Msh.Common.Helpers.StreamHelper.ToByteArray(stream);
            }
        }

        public static void WriteString(string zippedArchiveFile, string content, string fileName)
        {
            using (FileStream zipToOpen = new FileStream(zippedArchiveFile, FileMode.Open))
            {
                using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                {
                    ZipArchiveEntry newEntry = archive.CreateEntry(fileName);

                    using (StreamWriter writer = new StreamWriter(newEntry.Open()))
                    {
                        writer.WriteLine(content);
                    }
                }
            }
        }

        public static void WriteString(ZipArchive archive, string content, string fileName)
        {
            ZipArchiveEntry newEntry = archive.CreateEntry(fileName);

            using (StreamWriter writer = new StreamWriter(newEntry.Open()))
            {
                writer.WriteLine(content);
            }
        }
    }
}
