using System.IO;
using System.IO.Compression;


namespace GZipTest
{
    public static class Compressor
    {
        public static void CompressFileToFile(string sourceFile, string compressedFile)
        {
            using (FileStream sourceStream = new FileStream(sourceFile, FileMode.OpenOrCreate))
            using (FileStream targetStream = File.Create(compressedFile))
            using (GZipStream compressionStream = new GZipStream(targetStream, CompressionMode.Compress))
            {
                sourceStream.CopyTo(compressionStream);
            }
        }

        public static void DecompressFileToFile(string compressedFile, string decompressedFile)
        {
            using (FileStream sourceStream = new FileStream(compressedFile, FileMode.OpenOrCreate))
            using (FileStream targetStream = File.Create(decompressedFile))
            using (GZipStream decompressionStream = new GZipStream(sourceStream, CompressionMode.Decompress))
            {
                decompressionStream.CopyTo(targetStream);
            }
        }
    }
}
