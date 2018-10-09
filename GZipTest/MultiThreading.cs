using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace GZipTest
{
    class MultiThreading
    {
        static SortedQueueBlocks<Block> queueBlocks = new SortedQueueBlocks<Block>();
        static SortedQueueBlocks<ProcessedBlock> queueProcessedBlocks = new SortedQueueBlocks<ProcessedBlock>();

        private static List<Thread> threads = new List<Thread>();

        public static void Run(int threadCount)
        {
            var threadRead = new Thread(() => ReadingBlocksIntoFile(Parameters.PathToSourceFile));
            threadRead.Start();

            for (int i = 0; i < threadCount - 2; i++)
            {
                var thread = new Thread(ProcessingBlocks);
                threads.Add(thread);
            }

            foreach (var t in threads)
                t.Start();

            // В основном потоке
            WritingBlocksToFile(Parameters.PathToResultFile);
        }

        public static void Stop()
        {
            foreach (var t in threads)
                t.Join();
        }

        public static void ReadingBlocksIntoFile(string pathToSourceFile)
        {
            using (var sourceStream = new FileStream(pathToSourceFile, FileMode.Open))
            {
                while (sourceStream.Length > sourceStream.Position)
                {
                    var block = new Block(sourceStream);
                    queueBlocks.Enqueue(block);

                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"блок {block.Number} cчитан");
                    Console.BackgroundColor = ConsoleColor.Black;
                }
            }
            queueBlocks.Stop();

            // После окончания чтения поток переключается на обработку
            threads.Add(Thread.CurrentThread);
            ProcessingBlocks();
        }

        private static void ProcessingBlocks()
        {
            while (queueBlocks.CanDequeue)
            {
                Block block = (Block)queueBlocks.Dequeue();

                queueProcessedBlocks.Enqueue(Parameters.Process(block));
#if DEBUG
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine($"блок {block.Number} обработан");
                Console.BackgroundColor = ConsoleColor.Black;
#endif
            }
            queueProcessedBlocks.Stop();
        }

        public static void WritingBlocksToFile(string pathToResultFile)
        {
            using (var resutStream = new FileStream(pathToResultFile, FileMode.OpenOrCreate))
            {
                while (queueProcessedBlocks.CanDequeue)
                {
                    ProcessedBlock block = (ProcessedBlock)queueProcessedBlocks.Dequeue();

                    resutStream.Write(block.Data, 0, block.Size);
#if DEBUG
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.WriteLine($"блок {block.Number} записан");
                    Console.BackgroundColor = ConsoleColor.Black;
#endif
                }
            }
            queueProcessedBlocks.Stop();
        }
    }
}