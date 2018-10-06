using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GZipTest
{
    class Program
    {
        public const int Megabyte = 1024 * 1024;

        static void Main(string[] args)
        {
            var pathToSourceFile = TestFileManager.path + "Test-Clastering_4-Size_100.txt";
            var pathToCompressedFile = TestFileManager.path + "Test-Clastering_4-zip.gz";
            var pathToDecompressedFile = TestFileManager.path + "Test-Clastering_4-unzip.txt";

            //Compressor.CompressFileToFile(pathToSourceFile, pathToCompressedFile);

            //Compressor.DecompressFileToFile(pathToCompressedFile, pathToDecompressedFile);

            Console.WriteLine(TestFileManager.Compare("Test-Clastering_4-Size_100.txt", "Test-Clastering_4-unzip.txt"));

            Console.ReadKey();

        }
    }
}
