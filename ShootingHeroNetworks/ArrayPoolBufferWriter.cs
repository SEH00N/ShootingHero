using System;
using System.Buffers;

namespace ShootingHero.Networks
{
    public sealed class ArrayPoolBufferWriter : IBufferWriter<byte>, IDisposable
    {
        private const int DEFAULT_INITIAL_CAPACITY = 256;

        private readonly ArrayPool<byte> pool = null;
        private byte[] buffer = null;
        private int writtenCount = 0;
        private bool isDisposed = false;

        public int WrittenCount => writtenCount;
        public ArraySegment<byte> WrittenSegment => new ArraySegment<byte>(buffer, 0, writtenCount);

        public ArrayPoolBufferWriter(int initialCapacity = DEFAULT_INITIAL_CAPACITY, ArrayPool<byte> pool = null)
        {
            if (initialCapacity <= 0)
                throw new ArgumentOutOfRangeException(nameof(initialCapacity));

            this.pool = pool ?? ArrayPool<byte>.Shared;
            buffer = this.pool.Rent(initialCapacity);
        }

        public void Advance(int count)
        {
            ThrowIfDisposed();

            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            if (writtenCount > buffer.Length - count)
                throw new InvalidOperationException("Cannot advance past the end of the current buffer.");

            writtenCount += count;
        }

        public Memory<byte> GetMemory(int sizeHint = 0)
        {
            ThrowIfDisposed();
            EnsureCapacity(sizeHint);
            return buffer.AsMemory(writtenCount);
        }

        public Span<byte> GetSpan(int sizeHint = 0)
        {
            ThrowIfDisposed();
            EnsureCapacity(sizeHint);
            return buffer.AsSpan(writtenCount);
        }

        public void Write(ReadOnlySpan<byte> source)
        {
            ThrowIfDisposed();

            if (source.Length <= 0)
                return;

            EnsureCapacity(source.Length);
            source.CopyTo(buffer.AsSpan(writtenCount));
            writtenCount += source.Length;
        }

        public void Dispose()
        {
            if (isDisposed == true)
                return;

            isDisposed = true;

            if (buffer != null)
            {
                pool.Return(buffer);
                buffer = null;
            }

            writtenCount = 0;
        }

        private void EnsureCapacity(int sizeHint)
        {
            if (sizeHint < 0)
                throw new ArgumentOutOfRangeException(nameof(sizeHint));

            if (sizeHint == 0)
                sizeHint = 1;

            int requiredSize = writtenCount + sizeHint;
            if (requiredSize <= buffer.Length)
                return;

            int newSize = buffer.Length;
            while (newSize < requiredSize)
                newSize *= 2;

            byte[] oldBuffer = buffer;
            buffer = pool.Rent(newSize);
            oldBuffer.AsSpan(0, writtenCount).CopyTo(buffer);
            pool.Return(oldBuffer);
        }

        private void ThrowIfDisposed()
        {
            if (isDisposed == true)
                throw new ObjectDisposedException(nameof(ArrayPoolBufferWriter));
        }
    }
}
