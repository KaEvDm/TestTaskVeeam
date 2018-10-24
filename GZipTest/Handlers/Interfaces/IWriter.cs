using System;

namespace GZipTest
{
    public interface IWriter : IDisposable
    {
        bool TryWrite(Block processedBlock);
        int TotalBlockWrite { get; }
    }
}
