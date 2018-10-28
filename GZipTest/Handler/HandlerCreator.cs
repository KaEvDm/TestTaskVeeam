using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GZipTest
{
    class HandlerCreator : IHandlerCreator
    {
        private readonly string source;
        private readonly string result;

        public HandlerCreator(string source, string result)
        {
            this.source = source;
            this.result = result;
        }

        Handler IHandlerCreator.CreateHandler(ProcessMode mode, TypeInfo sourceType, TypeInfo resultType)
        {
            IReader reader = new ReaderCreator().CreateReader(source, sourceType);
            IWriter writer = new WriterCreator().CreateWriter(result, resultType);
            IProcessor processor = new ProcessorCreator().CreateProcessor(mode);

            return new Handler(reader, processor, writer);
        }
    }
}
