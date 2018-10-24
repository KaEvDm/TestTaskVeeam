using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GZipTest
{
    public interface IHandler : IDisposable
    {
        void Reading();
        void Processing();
        void Writing();
    }
}
