using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace GZipTest
{
    public static class MultiThreading
    {
        private static List<Thread> threads = new List<Thread>();
        static SortedQueueBlocks<Block> queueBlocks = new SortedQueueBlocks<Block>();
        static SortedQueueBlocks<ProcessedBlock> queueProcessedBlocks = new SortedQueueBlocks<ProcessedBlock>();
        private static int totalBlockRead = 0;
        private static int totalBlockProcessed = 0;
        private static int totalBlockWrite = 0;

        public static void Run(int threadCount)
        {
            var threadRead = new Thread(() => ReadingBlocksIntoFile(Parameters.PathToSourceFile))
            {
                Name = "Read Thread"
            };
            threadRead.Start();

            for (int i = 0; i < threadCount - 2; i++)
            {
                var thread = new Thread(ProcessingBlocks)
                {
                    Name = "Process Thread" + i.ToString()
                };
                threads.Add(thread);
            }

            foreach (var t in threads)
                t.Start();

            Thread.CurrentThread.Name = "Write Thread";
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

                    totalBlockRead++;

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
            while (queueBlocks.CanDequeue || totalBlockRead != totalBlockProcessed)
            {
                Block block = (Block)queueBlocks.Dequeue();

                if(!(block is null))
                {

                    queueProcessedBlocks.Enqueue(Parameters.Process(block));

                    totalBlockProcessed++;

                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine($"блок {block.Number} обработан");
                    Console.BackgroundColor = ConsoleColor.Black;
                }
            }
            queueProcessedBlocks.Stop();
        }

        public static void WritingBlocksToFile(string pathToResultFile)
        {
            using (var resutStream = new FileStream(pathToResultFile, FileMode.OpenOrCreate))
            {
                while (queueProcessedBlocks.CanDequeue || totalBlockProcessed != totalBlockWrite)
                {
                    ProcessedBlock block = (ProcessedBlock)queueProcessedBlocks.Dequeue();

                    if(!(block is null))
                    {
                        resutStream.Write(block.Data, 0, block.Size);
                        totalBlockWrite++;

                        Console.BackgroundColor = ConsoleColor.Green;
                        Console.WriteLine($"блок {block.Number} записан");
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                }
            }
            queueProcessedBlocks.Stop();
        }
    }
}