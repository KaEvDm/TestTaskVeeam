namespace GZipTest
{
    public interface IHandlerCreator
    {
        Handler CreateHandler(ProcessMode mode, TypeInfo sourceType, TypeInfo resultType);
    }
}