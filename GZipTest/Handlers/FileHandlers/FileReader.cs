using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GZipTest
{
    public class FileReader : IReader
    {
        private readonly FileStream sourceStream;

        public int TotalBlockRead { get; private set; }

        public FileReader(string pathToSourceFile)
        {
            sourceStream = new FileStream(pathToSourceFile, FileMode.Open);
            TotalBlockRead = 0;
        }

        bool IReader.Read(out Block block)
        {
            if(sourceStream.Length > sourceStream.Position)
            {
                var buffer = new byte[GZipTools.GetSizeInfo(sourceStream)];
                sourceStream.Read(buffer, 0, buffer.Length);

                block = new Block(buffer, TotalBlockRead++);

                return true;
            }

            block = null;
            return false;
        }

        public void Dispose()
        {
            sourceStream.Close();
        }
    }
}
