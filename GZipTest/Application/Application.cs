using System;
using System.IO;

namespace GZipTest
{
    public class Application
    {
        public Parameters Parameters { get; }
        public Handler Handler;
        public bool UseMultithreading;

        public Application(Parameters parameters)
        {
            Parameters = parameters;

            var sourceFileSize = new FileInfo(parameters.PathToSourceFile).Length;
            if (Environment.ProcessorCount == 1 || sourceFileSize < 2 * Environment.ProcessorCount * Constants.Megabyte)
            {
                UseMultithreading = false;
            }
            else
            {
                UseMultithreading = true;
            }

            IHandlerCreator handlerCreator = new HandlerCreator(parameters.PathToSourceFile, parameters.PathToResultFile);
            Handler = handlerCreator.CreateHandler(parameters.Mode, TypeInfo.File, TypeInfo.File);
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
