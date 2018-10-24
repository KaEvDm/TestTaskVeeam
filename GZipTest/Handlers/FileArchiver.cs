using System;

namespace GZipTest
{
    public class FileArchiver : IHandler
    {
        private SortedQueueBlocks queueForProcessing;
        private SortedQueueBlocks queueForWriting;

        private readonly IReader reader;
        private readonly IProcessor processor;
        private readonly IWriter writer;

        public FileArchiver(IReader reader, IProcessor processor, IWriter writer)
        {
            queueForProcessing = new SortedQueueBlocks();
            queueForWriting = new SortedQueueBlocks();

            this.reader = reader;
            this.processor = processor;
            this.writer = writer;
        }

        void IHandler.Reading()
        {
            while (reader.TryRead(out Block block))
            {
                queueForProcessing.Enqueue(block);
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
                    if (processor.TryProcess(block, out Block processedBlock))
                    {
                        queueForWriting.Enqueue(processedBlock);
                    }
                }
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
                    writer.TryWrite(processedBlock);
                }
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
