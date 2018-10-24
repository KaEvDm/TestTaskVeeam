using System;
namespace GZipTest
{
    public class FileArchiversFactory : HandlersFactory
    {
        private readonly string sourceFile;
        private readonly string resultFile;

        public FileArchiversFactory(string sourceFile, string resultFile)
        {
            this.sourceFile = sourceFile;
            this.resultFile = resultFile;
        }

        public override IHandler CreateHandler()
        {
            var reader = new FileReader(sourceFile);
            var processor = new Compressor();
            var writer = new FileWriter(resultFile);

            return new FileArchiver(reader, processor, writer);
        }

        public override IHandler CreateDehandler()
        {
            var reader = new FileReader(sourceFile);
            var processor = new Decompressor();
            var writer = new FileWriter(resultFile);

            return new FileArchiver(reader, processor, writer);
        }
    }
}