using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GZipTest
{
    public class FileHandler : IHandler
    {
        private SortedQueueBlocks queueForProcessing;
        private SortedQueueBlocks queueForWriting;

        private readonly IReader reader;
        private readonly IProcessor processor;
        private readonly IWriter writer;

        private ProgressBar readProgressBar;
        private ProgressBar processProgressBar;
        private ProgressBar writeProgressBar;

        public FileHandler(IReader reader, IProcessor processor, IWriter writer)
        {
            queueForProcessing = new SortedQueueBlocks();
            queueForWriting = new SortedQueueBlocks();

            this.reader = reader;
            this.processor = processor;
            this.writer = writer;

            readProgressBar = new ProgressBar("Reading", ConsoleColor.DarkRed);
            readProgressBar.Run();

            processProgressBar = new ProgressBar("Processing", ConsoleColor.DarkYellow);
            processProgressBar.Run();

            writeProgressBar = new ProgressBar("Writing", ConsoleColor.DarkGreen);
            writeProgressBar.Run();
        }

        void IHandler.Reading()
        {
            while (reader.Read(out Block block))
            {
                queueForProcessing.Enqueue(block);
                readProgressBar.Update(reader.TotalBlockRead);
            }
            queueForProcessing.Stop();
        }

        void IHandler.Processing()
        {
            while (queueForProcessing.CanDequeue || reader.TotalBlockRead != processor.TotalBlockProcessed)
            {
                Block block = queueForProcessing.Dequeue();

                if (!(block is null))
                {
                    if (processor.Process(block, out Block processedBlock))
                    {
                        queueForWriting.Enqueue(processedBlock);
                    }
                }
                processProgressBar.Update(processor.TotalBlockProcessed);
            }
            queueForWriting.Stop();
        }

        void IHandler.Writing()
        {
            while (queueForWriting.CanDequeue || processor.TotalBlockProcessed != writer.TotalBlockWrite)
            {
                Block processedBlock = queueForWriting.Dequeue();

                if (!(processedBlock is null))
                {
                    writer.Write(processedBlock);
                }
                writeProgressBar.Update(writer.TotalBlockWrite);
            }

            if(writer.TotalBlockWrite != reader.TotalBlockRead)
            {
                throw new Exception("Не все считанные блоки были записанны в итоговый файл!");
            }
        }

        public void Dispose()
        {
            reader.Dispose();
            writer.Dispose();
        }
    }
}
