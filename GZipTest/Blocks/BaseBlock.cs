namespace GZipTest
{
    public abstract class BaseBlock
    {
        public virtual byte[] Data { get; protected set; }
        public int Number { get; set; }
        public int Size { get; protected set; }
    }
}
