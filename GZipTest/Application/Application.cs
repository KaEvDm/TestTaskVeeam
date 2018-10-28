using System;
using System.IO;

namespace GZipTest
{
    public class Application
    {
        public Handler Handler;
        public bool UseMultithreading;

        public Application(Parameters parameters)
        {
            var sourceFileSize = new FileInfo(parameters.PathToSourceFile).Length;
            if(sourceFileSize == 0)
            {
                throw new Exception("Исходный файл пуст!");
            }

            if (Environment.ProcessorCount == 1 || sourceFileSize < 2 * Environment.ProcessorCount * Constants.Megabyte)
            {
                UseMultithreading = false;
            }
            else
            {
                UseMultithreading = true;
            }

            Handler = new Handler(parameters);
        }

        public void Run()
        {
            Console.WriteLine("Ждите.");

            if (UseMultithreading)
            {
                var multithreading = new MultiThreading();
                multithreading.Run(Environment.ProcessorCount * 2, Handler);
                multithreading.Stop();
            }
            else
            {
                Handler.Reading();
                Handler.Processing();
                Handler.Writing();
            }

            Console.WriteLine("Успешно!");
        }
    }
}
