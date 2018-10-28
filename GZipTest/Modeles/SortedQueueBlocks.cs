using System;
using System.Collections.Generic;
using System.Threading;

namespace GZipTest
{
    public class SortedQueueBlocks
    {
        private readonly object mutex = new object();
        private bool IsDead = false;
        private Queue<Block> blocks = new Queue<Block>();
        private int currentBlockNumber = 0;

        public int Capacity { get; set; }
        public int Count { get => blocks.Count; }
        public bool IsEmpty { get => Count == 0; }
        public bool CanEnqueue { get => !IsDead; }
        public bool CanDequeue { get => CanEnqueue || !IsEmpty; }

        public SortedQueueBlocks(int capacity = 20)
        {
            Capacity = capacity;
        }

        public void Enqueue(Block block)
        {
            if (block is null)
            {
                throw new ArgumentNullException("В очередь попал пустой блок.");
            }

            if (!CanEnqueue)
            {
                throw new InvalidOperationException("Очередь остановлена. В неё нельзя добавлять блоки." +
                    $"Count = {Count}, block number = {block.Number}");
            }

            lock (mutex)
            {
                while (Count >= Capacity)
                    Monitor.Wait(mutex, 1);

                //Сортировка
                while (block.Number != currentBlockNumber)
                    Monitor.Wait(mutex, 1);

                blocks.Enqueue(block);
                currentBlockNumber++;

                Monitor.Pulse(mutex);
            }
        }

        public Block Dequeue()
        {
            lock (mutex)
            {
                //пока очередь пустая, но в неё можно добавлять элементы
                while (IsEmpty && CanEnqueue)
                    Monitor.Wait(mutex, 1);

                if (!CanDequeue)
                {
                    return null;
                }
                return blocks.Dequeue();
            }
        }

        public void Stop()
        {
            lock (mutex)
            {
                IsDead = true;
                Monitor.PulseAll(mutex);
            }
        }
    }
}
