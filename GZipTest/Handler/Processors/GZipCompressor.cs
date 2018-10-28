using System.IO;
using System.IO.Compression;

namespace GZipTest
{
    public class GZipCompressor : IProcessor
    {
        public int TotalBlockProcessed { get; private set; } = 0;

        bool IProcessor.TryProcess(Block block, out Block processedBlock)
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
