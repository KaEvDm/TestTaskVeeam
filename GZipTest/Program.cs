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



            Parameters.Parse(args);
            new Program().Run();

            Console.WriteLine(TestFileManager.Compare(TestFile2, TestDecomp2, true));
            Console.ReadKey();
        }

        void Run()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            var fileSize = (new FileInfo(Parameters.PathToSourceFile)).Length;
            int processors = Environment.ProcessorCount;

            Console.WriteLine("Ждите!");

            if (fileSize < 2 * processors * Parameters.Megabyte)
            {
                MultiThreading.Run(1);
                MultiThreading.Stop();
            }
            else
            {
                MultiThreading.Run(2 * processors);
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
