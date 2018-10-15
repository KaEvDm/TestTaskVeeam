using System;
using System.IO;

namespace GZipTest
{
    class Program
    {
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += ProcessException;
            Parameters.Parse(args);
            Run();
        }

        static void Run()
        {
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
        }

        static void ProcessException(object sender, UnhandledExceptionEventArgs args)
        {
            Console.WriteLine((args.ExceptionObject as Exception).Message);
            Environment.Exit(1);
        }
    }
}
