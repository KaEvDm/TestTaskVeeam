using System.IO;
using System.IO.Compression;

namespace GZipTest
{
    public static class Compressor
    {
        public static ProcessedBlock CompressBlock(Block block)
        {
            ProcessedBlock compressedBlock;
            using (var compressedDataStream = new MemoryStream())
            {
                using (var GZipStream = new GZipStream(compressedDataStream, CompressionMode.Compress))
                    GZipStream.Write(block.Data, 0, block.Size);

                compressedBlock = new ProcessedBlock(compressedDataStream.ToArray(), block.Number);
            }
            return compressedBlock;
        }

        public static ProcessedBlock DecompressBlock(Block block)
        {
            ProcessedBlock decompressedBlock;
            using (var decompressedDataStream = new MemoryStream())
            using (var compressedDataStream = new MemoryStream(block.Data))
            using (GZipStream GZipStream = new GZipStream(compressedDataStream, CompressionMode.Decompress))
            {
                GZipStream.CopyTo(decompressedDataStream);
                decompressedBlock = new ProcessedBlock(decompressedDataStream.ToArray(), block.Number);
            }
            return decompressedBlock;
        }
    }
}
