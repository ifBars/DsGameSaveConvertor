using System.Diagnostics;
using System.IO;

namespace DsGameSaveConvertor
{
    public static class FileConverter
    {
        public static void ConvertDszToDsv(string dszFilePath)
        {
            string extractedFilePath = Path.ChangeExtension(dszFilePath, "7z");
            string dsvFilePath = Path.ChangeExtension(dszFilePath, "dsv");

            // Extract the contents of the DSZ file using 7za.exe
            var processStartInfo = new ProcessStartInfo
            {
                FileName = "7za.exe",
                Arguments = $"e \"{dszFilePath}\" -o\"{Path.GetDirectoryName(dszFilePath)}\"",
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            var process = Process.Start(processStartInfo);
            process.WaitForExit();

            // Rename the extracted file to have a DSV extension
            File.Delete(dszFilePath);
        }

        public static void ConvertDsvToDsz(string dsvFilePath)
        {
            string dszFilePath = Path.ChangeExtension(dsvFilePath, "dsz");

            // Create a temporary directory for the 7za command
            string tempDirectory = Path.Combine(Path.GetDirectoryName(dsvFilePath), "7za_temp");
            Directory.CreateDirectory(tempDirectory);

            // Move the DSV file to the temporary directory
            string tempFilePath = Path.Combine(tempDirectory, Path.GetFileName(dsvFilePath));
            File.Move(dsvFilePath, tempFilePath);

            // Create a 7z archive with the contents of the temporary directory
            var processStartInfo = new ProcessStartInfo
            {
                FileName = "7za.exe",
                Arguments = $"a \"{dszFilePath}\" \"{tempDirectory}\\*\" -t7z -m0=LZMA",
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            var process = Process.Start(processStartInfo);
            process.WaitForExit();

            // Clean up the temporary directory
            Directory.Delete(tempDirectory, true);
        }
    }
}
