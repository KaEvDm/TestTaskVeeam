using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GZipTest
{
    public static class GZipTools
    {
        private static readonly byte[] GZipDefaultHeader = { 0x1f, 0x8b, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x00 };
        private static readonly int sizeInfoLength = 4;

        public static bool IsGZipStream(Stream stream)
        {
            var temp = stream.Position;
            stream.Position += sizeInfoLength;

            var header = new byte[GZipDefaultHeader.Length];
            stream.Read(header, 0, header.Length);

            stream.Position = temp;

            if (header == GZipDefaultHeader)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static byte[] AddSizeInfo(byte[] data)
        {
            var a = data.Length + sizeInfoLength;
            var resultData = new byte[a];
            var size = BitConverter.GetBytes(data.Length);

            size.CopyTo(resultData, 0);
            data.CopyTo(resultData, 4);

            return data;
        }

        public static int GetSizeInfo(Stream stream)
        {
            if (!GZipTools.IsGZipStream(stream))
            {
                throw new ArgumentException("Неправильный заголовок GZip. " +
                    "Возможно, исходный файл не сжат или сжат при помощи другой программы.");
            }

            var sizeInfo = new byte[sizeInfoLength];
            stream.Read(sizeInfo, 0, sizeInfo.Length);
            return BitConverter.ToInt32(sizeInfo, 0);
        }
    }
}