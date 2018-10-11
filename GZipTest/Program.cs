using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace GZipTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var TestFile1 = TestFileManager.path + "Test-Clastering_4-Size_200.txt";
            var TestComp1 = TestFileManager.path + "Test-Clastering_4-Size_200-zip.gz";
            var TestDecomp1 = TestFileManager.path + "Test-Clastering_4-Size_200-unzip.txt";

            var TestFile2 = TestFileManager.path + "Test-Clastering_8-Size_2000.txt";
            var TestComp2 = TestFileManager.path + "Test-Clastering_8-Size_2000-zip.gz";
            var TestDecomp2 = TestFileManager.path + "Test-Clastering_8-Size_2000-unzip.txt";

            var TestFile3 = TestFileManager.path + "Test-Clastering_2-Size_1000.txt";
            var TestComp3 = TestFileManager.path + "Test-Clastering_2-Size_1000-zip.gz";
            var TestDecomp3 = TestFileManager.path + "Test-Clastering_2-Size_1000-unzip.txt";

            var TestFile4 = TestFileManager.path + "Test-Clastering_4-Size_10000.txt";
            var TestComp4 = TestFileManager.path + "Test-Clastering_4-Size_10000-zip.gz";
            var TestDecomp4 = TestFileManager.path + "Test-Clastering_4-Size_10000-unzip.txt";

            //TestFileManager.CreateFile(500 * Parameters.Megabyte, 8, TestFile2, true);
            //Console.WriteLine("2 Успешно");
            //TestFileManager.CreateFile(1000 * Parameters.Megabyte, 2, TestFile3, true);
            //Console.WriteLine("3 Успешно");
            //long size = 5000;
            //long BigSize = size * Parameters.Megabyte;
            //TestFileManager.CreateFile(BigSize, 4, TestFile4, true);
            //Console.WriteLine("4 Успешно");



            //Parameters.Parse(args);
            Parameters.Mode = ProcessMode.compress;
            Parameters.ProcessСhoice();

            //new Program().Run(TestFile1, TestComp1);
            //Console.WriteLine("1 Успешно");
            new Program().Run(TestFile2, TestComp2);
            Console.WriteLine("2 Успешно");
            //new Program().Run(TestFile3, TestComp3);
            //Console.WriteLine("3 Успешно");
            //new Program().Run(TestFile4, TestComp4);
            //Console.WriteLine("4 Успешно");
        }

        void Run(string sourceFile, string resultFile)
        {
            Parameters.PathToSourceFile = sourceFile;
            Parameters.PathToResultFile = resultFile;

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            var fileSize = (new FileInfo(sourceFile)).Length;

            Console.WriteLine("Ждите!");

            if (fileSize < Parameters.Megabyte && Parameters.Mode == ProcessMode.compress)
            {
                Compressor.CompressFileToFile(sourceFile, resultFile);
            }
            else
            {
                int processors = Environment.ProcessorCount;
                MultiThreading.Run(processors * 2);

                MultiThreading.Stop();
            }

            Console.WriteLine("Успешно!");

            stopWatch.Stop();

            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);
            Console.WriteLine("RunTime " + elapsedTime);

            Console.ReadLine();
        }

        static void ProcessException(object sender, UnhandledExceptionEventArgs args)
        {
            Console.WriteLine((args.ExceptionObject as Exception).Message);
            Environment.Exit(1);
        }
    }
}
