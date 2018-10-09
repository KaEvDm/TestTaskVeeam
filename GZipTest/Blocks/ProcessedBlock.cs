namespace GZipTest
{
    public sealed class ProcessedBlock : BaseBlock
    {
        public ProcessedBlock(byte[] data, int number)
        {
            Number = number;

            Size = Data.Length;
        }
    }
}
