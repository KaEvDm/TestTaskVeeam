using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GZipTest
{
    public class ProcessorCreator
    {
        public IProcessor CreateProcessor(ProcessMode mode)
        {
            IProcessor processor;
            switch (mode)
            {
                case ProcessMode.compress:
                {
                    processor = new Compressor();
                    break;
                }
                case ProcessMode.decompress:
                {
                    processor = new Decompressor();
                    break;
                }
                default:
                {
                    throw new Exception("Неверный режим работы программы!");
                }
            }
            return processor;
        }
    }
}
