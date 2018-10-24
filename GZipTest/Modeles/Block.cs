using System;
using System.IO;

namespace GZipTest
{
    public sealed class Block
    {
        public byte[] Data { get; }
        public int Number { get; }
        public int Size { get => Data.Length; }

        public Block(byte[] data, int number)
        {
            Number = number;
            Data = data;
        }
    }
}
