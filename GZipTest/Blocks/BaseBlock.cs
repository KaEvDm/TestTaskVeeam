using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GZipTest.Blocks
{
    public abstract class BaseBlock
    {
        public byte[] Data { get; protected set; }
        public int Number { get; set; }
        public int Size { get; protected set; }
    }
}
