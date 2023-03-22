using System;
using System.IO;
using System.Linq;
using SharpCompress.Archives;
using SharpCompress.Archives.SevenZip;
using SharpCompress.Common;

namespace DsSaveConvertor
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: DsSaveConvertor.exe (filePath)");
                return;
            }

            string filePath = args[0];
            string extension = Path.GetExtension(filePath);
            if (extension.ToLower() != ".dsz")
            {
                Console.WriteLine("File is not a .dsz file.");
                return;
            }

            string newFilePath = Path.ChangeExtension(filePath, ".dsv");

            using (var archive = SevenZipArchive.Open(filePath))
            {
                var entries = archive.Entries.Where(e => !e.IsDirectory);

                if (entries.Count() != 1)
                {
                    Console.WriteLine("Error extracting file.");
                    return;
                }

                var entry = entries.First();
                entry.WriteToDirectory(Path.GetDirectoryName(filePath), new ExtractionOptions()
                {
                    ExtractFullPath = true,
                    Overwrite = true
                });
            }

            File.Move(Path.Combine(Path.GetDirectoryName(filePath), Path.GetFileNameWithoutExtension(filePath)), newFilePath);
            File.Delete(filePath);

            Console.WriteLine("File successfully converted.");
        }
    }
}
