namespace GZipTest
{
    public interface IProcessor
    {
        int TotalBlockProcessed { get; }
        bool TryProcess(Block block, out Block processedBlock);
    }
}
