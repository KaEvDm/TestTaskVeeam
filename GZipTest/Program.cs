using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GZipTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //var pathToSourceFile = TestFileManager.path + "Test-Clastering_4-Size_100.txt";
            //var pathToCompressedFile = TestFileManager.path + "Test-Clastering_4-zip.gz";
            //var pathToDecompressedFile = TestFileManager.path + "Test-Clastering_4-unzip.txt";
            //Compressor.CompressFileToFile(pathToSourceFile, pathToCompressedFile);
            //Compressor.DecompressFileToFile(pathToCompressedFile, pathToDecompressedFile);
            //Console.WriteLine(TestFileManager.Compare("Test-Clastering_4-Size_100.txt", "Test-Clastering_4-unzip.txt"));

            Parameters.Parse(args);

            new Program().Run(Parameters.PathToSourceFile, Parameters.PathToResultFile);
        }

        void Run(string sourceFile, string resultFile)
        {
            var fileSize = (new FileInfo(sourceFile)).Length;

            Console.WriteLine("Ждите!");

            if (fileSize < Parameters.Megabyte && Parameters.Mode == ProcessMode.compress)
            {
                Compressor.CompressFileToFile(sourceFile, resultFile);
            }
            else
            {
                int processors = Environment.ProcessorCount;
                MultiThreading.Run(processors);

                MultiThreading.Stop();
            }

            Console.WriteLine("Успешно!");
            Console.ReadLine();
        }

        static void ProcessException(object sender, UnhandledExceptionEventArgs args)
        {
            Console.WriteLine((args.ExceptionObject as Exception).Message);
            Environment.Exit(1);
        }
    }
}
