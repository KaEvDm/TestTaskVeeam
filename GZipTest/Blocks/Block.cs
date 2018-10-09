using System.IO;

namespace GZipTest
{
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
