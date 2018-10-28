using System;

namespace GZipTest
{
    public interface IReader : IDisposable
    {
        int TotalBlockRead { get; }
        bool TryRead(out Block block);
    }
}
