using System.IO;
using System.IO.Compression;

namespace GZipTest
{
    public class Decompressor : IProcessor
    {
        public int TotalBlockProcessed { get; private set; } = 0;

        bool IProcessor.TryProcess(Block block, out Block processedBlock)
        {
            using (var decompressedDataStream = new MemoryStream())
            using (var compressedDataStream = new MemoryStream(block.Data))
            using (GZipStream GZipStream = new GZipStream(compressedDataStream, CompressionMode.Decompress))
            {
                GZipStream.CopyTo(decompressedDataStream);
                processedBlock = new Block(decompressedDataStream.ToArray(), block.Number);
            }

            if (!(processedBlock is null))
            {
                TotalBlockProcessed++;
                return true;
            }
            return false;
        }
    }
}
