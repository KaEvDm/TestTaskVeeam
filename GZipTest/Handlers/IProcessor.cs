namespace GZipTest
{
    public interface IProcessor
    {
        bool Process(Block block, out Block processedBlock);
        int TotalBlockProcessed { get; }
    }
}
