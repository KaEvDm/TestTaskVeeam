using System;

namespace GZipTest
{
    public interface IWriter : IDisposable
    {
        int TotalBlockWrite { get; }
        bool TryWrite(Block processedBlock);
    }
}
