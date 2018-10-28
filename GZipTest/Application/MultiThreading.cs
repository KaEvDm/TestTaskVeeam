using System;
using System.Collections.Generic;
using System.Threading;

namespace GZipTest
{
    public class MultiThreading
    {
        private List<Thread> threads = new List<Thread>();
        private readonly object mutex = new object();

        public void Run(int threadCount, Handler handler)
        {
            var threadRead = CreateThread(handler.Reading, "Read");
            threadRead.Start();

            for (int i = 0; i < threadCount - 2; i++)
            {
                var thread = CreateThread(handler.Processing, $"Processing {i}");
                threads.Add(thread);
            }

            foreach (var t in threads)
            {
                t.Start();
            }

            // В основном потоке
            handler.Writing();
        }

        public void Stop()
        {
            foreach (var t in threads)
            {
                t.Join();
            }
        }

        private Thread CreateThread(Action threadAction, string name)
        {
            return new Thread(() => TryRunAction(threadAction))
            {
                Name = name
            };
        }

        private void TryRunAction(Action threadAction)
        {
            try
            {
                threadAction();
            }
            catch (Exception e)
            {
                lock (mutex)
                {
                    Console.WriteLine($"В потоке \"{Thread.CurrentThread.Name}\" возникло исключение:");
                    Console.WriteLine(e.Message);
                    Environment.Exit(1);
                }
            }
        }
    }
}