using System.IO;

namespace GZipTest
{
    public class FileWriter : IWriter
    {
        private readonly FileStream resultStream;
        public int TotalBlockWrite { get; private set; }

        public FileWriter(string pathToResultFile)
        {
            resultStream = new FileStream(pathToResultFile, FileMode.OpenOrCreate);
            TotalBlockWrite = 0;
        }

        bool IWriter.TryWrite(Block processedBlock)
        {
            resultStream.Write(processedBlock.Data, 0, processedBlock.Size);
            TotalBlockWrite++;
            return true;
        }

        public void Dispose()
        {
            resultStream.Close();
        }
    }
}
