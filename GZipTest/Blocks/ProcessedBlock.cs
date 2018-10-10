namespace GZipTest
{
    public sealed class ProcessedBlock : BaseBlock
    {
        public ProcessedBlock(byte[] data, int number)
        {
            Number = number;
            Data = data;
            Size = Data.Length;
        }

        public override byte[] Data
        {
            get => base.Data;
            protected set
            {
                switch (Parameters.Mode)
                {
                    case ProcessMode.compress:
                    {
                        base.Data = GZipTools.AddSizeInfo(value);
                        break;
                    }
                    case ProcessMode.decompress:
                    {
                        base.Data = value;
                        break;
                    }
                }
            }
        }
    }
}
