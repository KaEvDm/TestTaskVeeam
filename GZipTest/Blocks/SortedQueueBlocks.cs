using System;
using System.Collections.Generic;
using System.Threading;

namespace GZipTest
{
    public class SortedQueueBlocks<T> where T : BaseBlock
    {
        private readonly object mutex = new object();
        private bool IsDead = false;
        private Queue<T> blocks = new Queue<T>();
        private int currentBlockNumber = 0;

        public int Count { get => blocks.Count; }
        public bool IsEmpty { get => Count == 0; }
        public bool CanEnqueue { get => !IsDead; }
        public bool CanDequeue { get => !IsDead || !IsEmpty; }

        public void Enqueue(T block)
        {
            if (block is null)
            {
                throw new ArgumentNullException("В очередь попал пустой блок.");
            }
            if (!CanEnqueue)
            {
                throw new InvalidOperationException("Очередь остановлена. В неё нельзя добавлять блоки.");
            }

            lock (mutex)
            {
                //Сортировка
                while (block.Number != currentBlockNumber)
                    Monitor.Wait(mutex, 1);

                blocks.Enqueue(block);
                currentBlockNumber++;

                Monitor.Pulse(mutex);
            }
        }

        public T Dequeue()
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
