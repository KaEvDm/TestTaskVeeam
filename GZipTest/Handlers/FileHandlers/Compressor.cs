using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace GZipTest
{
    public class Compressor : IProcessor
    {
        public int TotalBlockProcessed { get; private set; } = 0;

        bool IProcessor.Process(Block block, out Block processedBlock)
        {
            using (var compressedDataStream = new MemoryStream())
            {
                using (var GZipStream = new GZipStream(compressedDataStream, CompressionMode.Compress))
                {
                    GZipStream.Write(block.Data, 0, block.Size);
                }

                processedBlock = new Block(GZipTools.AddSizeInfo(compressedDataStream.ToArray()), block.Number);
            }

            if(!(processedBlock is null))
            {
                TotalBlockProcessed++;
                return true;
            }
            return false;
        }
    }
}
