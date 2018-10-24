using System;

namespace GZipTest
{
    public interface IReader : IDisposable
    {
        bool TryRead(out Block block);
        int TotalBlockRead { get; }
    }
}
