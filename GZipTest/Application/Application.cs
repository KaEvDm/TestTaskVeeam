using System;

namespace GZipTest
{
    public class Application
    {
        public Parameters parameters { get; }

        public Application(Parameters parameters)
        {
            this.parameters = parameters;
        }

        public void Run()
        {
            Console.WriteLine("Ждите.");

            if (parameters.IsNeedMultithreading)
            {
                var multithreading = new MultiThreading(parameters.handler);
                multithreading.Run(parameters.ProcessorCount * 2);
                multithreading.Stop();
            }
            else
            {
                parameters.handler.Reading();
                parameters.handler.Processing();
                parameters.handler.Writing();
            }

            Console.WriteLine("Успешно!");
        }
    }
}
