namespace GZipTest
{
    public interface IProcessor
    {
        int TotalBlockProcessed { get; }
        Block Process(Block block);
    }
}
