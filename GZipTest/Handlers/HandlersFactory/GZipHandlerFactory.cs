using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GZipTest
{
    public class ArchiverFactory : HandlersFactory
    {
        public override IHandler CreateHandler()
        {
            var reader = new FileReader(Parameters.PathToSourceFile);
            var processor = new Compressor();
            var writer = new FileWriter(Parameters.PathToResultFile);

            return new FileHandler(reader, processor, writer);
        }

        public override IHandler CreateDehandler()
        {
            var reader = new FileReader(Parameters.PathToSourceFile);
            var processor = new Decompressor();
            var writer = new FileWriter(Parameters.PathToResultFile);

            return new FileHandler(reader, processor, writer);
        }
    }
}