using System;

namespace GZipTest
{
    public interface IHandler : IDisposable
    {
        void Reading();
        void Processing();
        void Writing();
    }
}
