using System;
using System.Linq;
using System.IO;

namespace GZipTest
{
    public static class TestFileManager
    {
        public static readonly string path = @"C:\Users\evGenius\source\repos\TestTaskVeeam\GZipTest\TestFiles\";

        public static void CreateFile(int size, int clusteringLevel, string fileName)
        {
            using (var stream = new FileStream(path + fileName, FileMode.CreateNew))
            {
                var rnd = new Random();
                var buffer = new byte[clusteringLevel];

                for (int i = 0; i < size / (clusteringLevel * 2); i++)
                {
                    rnd.NextBytes(buffer);
                    for (int j = 0; j < clusteringLevel; j++)
                    {
                        stream.Write(buffer, 0, buffer.Length);
                    }

                }
            }
        }

        public static void ShowFileInConsole(string fileName)
        {
            using (var stream = new FileStream(path + fileName, FileMode.Open))
            {
                var charactersInString = 40;
                var buffer = new byte[charactersInString];

                while (stream.Read(buffer, 0, buffer.Length) > 0)
                {
                    for (int i = 0; i < charactersInString; i++)
                    {
                        Console.Write(buffer[i]);
                        Console.Write(" ");
                    }
                    Console.WriteLine();
                }
            }
        }

        public static bool Compare(string sourceFileNeme, string targetFileName)
        {
            var pathToSourceFile = path + sourceFileNeme;
            var pathToTargetFile = path + targetFileName;

            var sourceFileSize = (new FileInfo(pathToSourceFile)).Length;
            var targetFileSize = (new FileInfo(pathToTargetFile)).Length;
            Console.WriteLine($"sourceFileSize = {sourceFileSize}");
            Console.WriteLine($"targetFileSize = {targetFileSize}");

            if (sourceFileSize == targetFileSize)
            {
                using (var sourceStream = new FileStream(path + sourceFileNeme, FileMode.Open))
                using (var targetStream = new FileStream(path + targetFileName, FileMode.Open))
                {
                    if(sourceStream.ReadByte() != targetStream.ReadByte())
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
            return true;
        }
    }
}
