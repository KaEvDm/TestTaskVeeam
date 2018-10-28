using System;

namespace GZipTest
{
    public interface IWriter : IDisposable
    {
        int TotalBlockWrite { get; }
        void Write(Block processedBlock);
    }
}
