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
                    processor = new GZipCompressor();
                    break;
                }
                case ProcessMode.decompress:
                {
                    processor = new GZipDecompressor();
                    break;
                }
                case ProcessMode.DeflateCompress:
                {
                    throw new NotImplementedException();
                }
                case ProcessMode.DeflateDecompress:
                {
                    throw new NotImplementedException();
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
