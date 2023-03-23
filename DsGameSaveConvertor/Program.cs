using DsGameSaveConvertor;
using System;

namespace DsSaveConvertor
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: DsSaveConvertor dsz/dsv filePath");
                return;
            }

            string command = args[0];
            string filePath = args[1];

            switch (command.ToLower())
            {
                case "dsz":
                    FileConverter.ConvertDszToDsv(filePath);
                    break;
                case "dsv":
                    FileConverter.ConvertDsvToDsz(filePath);
                    break;
                default:
                    Console.WriteLine($"Unknown command: {command}");
                    break;
            }
        }
    }
}
