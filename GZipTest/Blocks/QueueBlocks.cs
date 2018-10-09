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

        Queue<T> blocks = new Queue<T>();

        public int Count { get => blocks.Count; }
        public bool IsEmpty { get => Count == 0; }
        public bool CanEnqueue { get; private set; } = true;

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

                if (IsEmpty && !CanEnqueue)
                    throw new InvalidOperationException("Очередь остановлена и пуста." +
                        "Из неё нельзя извлекать блоки.");

                return blocks.Dequeue();
            }
        }

        public void Stop()
        {
            lock (mutex)
            {
                CanEnqueue = false;
                Monitor.PulseAll(mutex);
            }
        }
    }
}
