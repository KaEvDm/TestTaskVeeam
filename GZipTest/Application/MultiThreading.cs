using System;
using System.Collections.Generic;
using System.Threading;

namespace GZipTest
{
    public class MultiThreading
    {
        private List<Thread> threads = new List<Thread>();
        private readonly object sync = new object();
        private IHandler handler;

        public MultiThreading(IHandler handler)
        {
            this.handler = handler;
        }

        public void Run(int threadCount)
        {
            var threadRead = CreateThread(handler.Reading, "Read");
            threadRead.Start();

            for (int i = 0; i < threadCount - 2; i++)
            {
                var thread = CreateThread(handler.Processing, $"Processing {i}");
                threads.Add(thread);
            }

            foreach (var t in threads)
                t.Start();

            // В основном потоке
            handler.Writing();
        }

        public void Stop()
        {
            foreach (var t in threads)
                t.Join();
            handler.Dispose();
        }

        Thread CreateThread(Action threadAction, string name)
        {
            return new Thread(() => TryRun(threadAction))
            {
                Name = name
            };
        }

        void TryRun(Action threadAction)
        {
            try
            {
                threadAction();
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e.Message);
                threadAction();
            }
            catch (Exception e)
            {
                lock (sync)
                {
                    Console.WriteLine($"В потоке \"{Thread.CurrentThread.Name}\" возникло исключение:");
                    Console.WriteLine(e.Message);
                    Environment.Exit(1);
                }
            }
        }
    }
}