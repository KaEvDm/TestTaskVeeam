using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GZipTest
{
    public interface IWriter : IDisposable
    {
        bool Write(Block processedBlock);
        int TotalBlockWrite { get; }
    }
}
