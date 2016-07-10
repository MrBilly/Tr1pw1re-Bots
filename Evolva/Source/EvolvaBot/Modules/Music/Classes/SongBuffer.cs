using System;
using System.Threading;
using System.Threading.Tasks;

namespace EvolvaBot.Modules.Music
{
    public class SongBuffer
    {
        private readonly byte[] ringBuffer;

        public int WritePosition { get; private set; }
        public int ReadPosition { get; private set; }

        public int ContentLength => (WritePosition >= ReadPosition ?
                                     WritePosition - ReadPosition :
                                     (BufferSize - ReadPosition) + WritePosition);

        public int BufferSize { get; }

        private readonly object readWriteLock = new object();

        public SongBuffer(int size)
        {
            if (size <= 0)
                throw new ArgumentException();
            BufferSize = size;
            ringBuffer = new byte[size];

        }

        public int Read(byte[] buffer, int count)
        {
            if (buffer.Length < count)
                throw new ArgumentException();
            lock(readWriteLock)
            {
                if (count > ContentLength)
                    count = ContentLength;

                if (WritePosition == ReadPosition)
                    return 0;

                if(WritePosition > ReadPosition)
                {
                    Buffer.BlockCopy(ringBuffer, ReadPosition, buffer, 0, count);
                    ReadPosition += count;
                    return count;
                }

                if(count + ReadPosition <= BufferSize)
                {
                    Buffer.BlockCopy(ringBuffer, ReadPosition, buffer, 0, count);
                    ReadPosition += count;
                    return count;
                }

                var readNormally = BufferSize - ReadPosition;
                Buffer.BlockCopy(ringBuffer, ReadPosition, buffer, 0, readNormally);

                var readFromStart = count - readNormally;
                Buffer.BlockCopy(ringBuffer, 0, buffer, readNormally, readFromStart);

                ReadPosition = readFromStart;
                return count;
            }
        }

        public async Task WriteAsync(byte[] buffer, int count, CancellationToken cancelToken)
        {
            if (count > buffer.Length)
                throw new ArgumentException();
            while(ContentLength + count > BufferSize)
            {
                await Task.Delay(20, cancelToken).ConfigureAwait(false);
                if (cancelToken.IsCancellationRequested)
                    return;
            }

            lock(readWriteLock)
            {
                if(WritePosition + count < BufferSize)
                {
                    Buffer.BlockCopy(buffer, 0, ringBuffer, WritePosition, count);
                    WritePosition += count;
                    return;
                }

                var wroteNormally = BufferSize - WritePosition;
                Buffer.BlockCopy(buffer, 0, ringBuffer, WritePosition, count);

                var wroteFromStart = count - wroteNormally;
                Buffer.BlockCopy(buffer, wroteNormally, ringBuffer, 0, wroteFromStart);

                WritePosition = wroteFromStart;
            }
        }
    }
}