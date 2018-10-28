using System.IO;
using System.IO.Compression;

namespace GZipTest
{
    public class Compressor : IProcessor
    {
        public int TotalBlockProcessed { get; private set; } = 0;

        Block IProcessor.Process(Block block)
        {
            Block processedBlock;
            using (var compressedDataStream = new MemoryStream())
            {
                using (var GZipStream = new GZipStream(compressedDataStream, CompressionMode.Compress))
                {
                    GZipStream.Write(block.Data, 0, block.Size);
                }

                processedBlock = new Block(GZipTools.AddSizeInfo(compressedDataStream.ToArray()), block.Number);
            }
            return processedBlock;
        }
    }
}
