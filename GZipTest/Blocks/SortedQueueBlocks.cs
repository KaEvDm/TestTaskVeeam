using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace GZipTest
{
    public class SortedQueueBlocks<T> where T : BaseBlock
    {
        readonly object mutex = new object();

        private bool IsDead = false; 

        Queue<T> blocks = new Queue<T>();

        public int Count { get => blocks.Count; }
        public bool IsEmpty { get => Count == 0; }
        public bool CanEnqueue { get => !IsDead; }
        public bool CanDequeue { get => !IsDead || !IsEmpty; }

        public void Enqueue(T block)
        {
            if (!(block is T))
                throw new ArgumentNullException("В очередь попал пустой блок.");

            if (!CanEnqueue)
                throw new InvalidOperationException("Очередь остановлена. В неё нельзя добавлять блоки.");

            lock (mutex)
            {
                while(block.Number != blocks.Peek().Number + 1)
                    Monitor.Wait(mutex);

                blocks.Enqueue(block);

                Monitor.Pulse(mutex);
            }
        }

        public BaseBlock Dequeue()
        {
            lock (mutex)
            {
                //пока очередь пустая, но в неё можно добавлять элементы
                while (IsEmpty && CanEnqueue)
                    Monitor.Wait(mutex);

                if (!CanDequeue)
                    throw new InvalidOperationException("Очередь остановлена и пуста." +
                        "Из неё нельзя извлекать блоки.");

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
