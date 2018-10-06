using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GZipTest.Blocks
{
    //обработанный блок
    public sealed class ProcessedBlock : BaseBlock
    {
        public ProcessedBlock(byte[] data, int number)
        {
            Number = number;

            Size = Data.Length;
        }
    }
}
