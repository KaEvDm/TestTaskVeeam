using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GZipTest
{
    public interface IReader : IDisposable
    {
        bool Read(out Block block);
        int TotalBlockRead { get; }
    }
}
