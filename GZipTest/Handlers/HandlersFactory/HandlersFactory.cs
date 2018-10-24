using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GZipTest
{
    public abstract class HandlersFactory
    {
        public abstract IHandler CreateHandler();

        public abstract IHandler CreateDehandler();
    }
}