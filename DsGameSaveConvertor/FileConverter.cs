using System.Diagnostics;
using System.IO;
using SevenZip;

namespace DsGameSaveConvertor
{
    public static class FileConverter
    {
        public static void ConvertDsvToDsz(string dsvFilePath)
        {
            string dszFilePath = Path.ChangeExtension(dsvFilePath, "dsz");
            string fileName = Path.GetFileName(dsvFilePath);

            // Create a temporary directory for the 7za command
            string tempDirectory = Path.Combine(Path.GetDirectoryName(dsvFilePath), "7za_temp");
            Directory.CreateDirectory(tempDirectory);

            // Move the DSV file to the temporary directory
            string tempFilePath = Path.Combine(tempDirectory, Path.GetFileName(dsvFilePath));
            File.Move(dsvFilePath, tempFilePath);

            // Create a 7z archive with the contents of the temporary directory
            SevenZipCompressor compressor = new SevenZipCompressor();
            compressor.CompressFiles(dszFilePath, tempDirectory + "\\" + fileName);

            // Clean up the temporary directory
            Directory.Delete(tempDirectory, true);
            Console.WriteLine("Converted " + Path.GetFileName(dszFilePath) + " to dsz.");
        }

        public static void ConvertDszToDsv(string dszFilePath)
        {
            string extractedFilePath = Path.ChangeExtension(dszFilePath, "7z");
            string dsvFilePath = Path.ChangeExtension(dszFilePath, "dsv");

            // Extract the contents of the DSZ file using Squid-Box.SevenZipSharp
            using (var extractor = new SevenZipExtractor(dszFilePath))
            {
                extractor.ExtractArchive(Path.GetDirectoryName(dszFilePath));
            }

            // Rename the extracted file to have a DSV extension
            // File.Move(extractedFilePath, dsvFilePath);
            File.Delete(dszFilePath);
            Console.WriteLine("Converted " + Path.GetFileName(dszFilePath) + " to dsv.");
        }
    }
}
