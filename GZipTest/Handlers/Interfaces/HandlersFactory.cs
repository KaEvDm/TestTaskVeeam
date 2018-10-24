namespace GZipTest
{
    public abstract class HandlersFactory
    {
        public abstract IHandler CreateHandler();
        public abstract IHandler CreateDehandler();
    }
}