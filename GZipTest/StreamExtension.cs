using System;
using System.IO;


namespace GZipTest
{
    public static class StreamExtension
    {
        public static void CopyTo(this Stream sourceStream, Stream destinationStream)
        {
            if (destinationStream == null)
                throw new ArgumentNullException("destinationStream не на что не указывает");

            if (!sourceStream.CanRead && !sourceStream.CanWrite)
                throw new ObjectDisposedException("sourceStream недоступен для чтения или записи");

            if (!destinationStream.CanRead && !destinationStream.CanWrite)
                throw new ObjectDisposedException("destinationStream недоступен для чтения или записи");

            var byfferSize = 4096;
            var buffer = new byte[byfferSize];

            int readByteCount;

            while ((readByteCount = sourceStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                destinationStream.Write(buffer, 0, readByteCount);
            }
        }
    }
}
