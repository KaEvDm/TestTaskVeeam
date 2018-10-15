﻿using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace GZipTest
{
    public static class MultiThreading
    {
        private static List<Thread> threads = new List<Thread>();
        private static SortedQueueBlocks<Block> queueBlocks = new SortedQueueBlocks<Block>();
        private static SortedQueueBlocks<ProcessedBlock> queueProcessedBlocks = new SortedQueueBlocks<ProcessedBlock>();
        private static int totalBlockRead = 0;
        private static int totalBlockProcessed = 0;
        private static int totalBlockWrite = 0;

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

                    totalBlockRead++;
                }
            }
            queueBlocks.Stop();

            // После окончания чтения, поток переключается на обработку
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
                    }
                }
            }
            queueProcessedBlocks.Stop();
        }
    }
}