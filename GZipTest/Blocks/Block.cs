using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GZipTest
{
    //считаный блок
    public sealed class Block : BaseBlock
    {
        private static int ExistBlockCount = 0;

        public Block(Stream stream)
        {
            Number = ExistBlockCount++;

            Data = new byte[Size];

            stream.Read(Data, 0, Size);
        }
    }
}
