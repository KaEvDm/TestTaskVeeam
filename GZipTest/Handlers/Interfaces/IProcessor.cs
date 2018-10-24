namespace GZipTest
{
    public interface IProcessor
    {
        bool TryProcess(Block block, out Block processedBlock);
        int TotalBlockProcessed { get; }
    }
}
