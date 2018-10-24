using System;

namespace GZipTest
{
    public class Application
    {
        public Parameters Parameters { get; }

        public Application(Parameters parameters)
        {
            Parameters = parameters;
        }

        public void Run()
        {
            Console.WriteLine("Ждите.");

            if (Parameters.IsNeedMultithreading)
            {
                var multithreading = new MultiThreading(Parameters.Handler);
                multithreading.Run(Parameters.ProcessorCount * 2);
                multithreading.Stop();
            }
            else
            {
                Parameters.Handler.Reading();
                Parameters.Handler.Processing();
                Parameters.Handler.Writing();
            }

            Console.WriteLine("Успешно!");
        }
    }
}
