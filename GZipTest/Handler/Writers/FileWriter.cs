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

        void IWriter.Write(Block processedBlock)
        {
            resultStream.Write(processedBlock.Data, 0, processedBlock.Size);
            TotalBlockWrite++;
        }

        public void Dispose()
        {
            resultStream.Close();
        }
    }
}
