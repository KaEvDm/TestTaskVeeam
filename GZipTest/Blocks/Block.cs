using System;
using System.IO;

namespace GZipTest
{
    public sealed class Block : BaseBlock
    {
        private static int ExistBlockCount = 0;

        public Block(Stream stream)
        {
            Number = ExistBlockCount++;

            Size = DetermineBlockSize(stream);
            Data = new byte[Size];
            stream.Read(Data, 0, Data.Length);
        }

        private int DetermineBlockSize(Stream stream)
        {
            switch (Parameters.Mode)
            {
                case ProcessMode.compress:
                {
                    if (Parameters.Megabyte > stream.Length - stream.Position)
                    {
                        return (int)(stream.Length - stream.Position);
                    }
                    return Parameters.Megabyte;
                }
                case ProcessMode.decompress:
                {
                    return GZipTools.GetSizeInfo(stream);
                }
                default:
                {
                    throw new ArgumentException("Неправильный режим работы программы. " +
                            "Ожидалось compress/decompress.");
                } 
            }
        }
    }
}
