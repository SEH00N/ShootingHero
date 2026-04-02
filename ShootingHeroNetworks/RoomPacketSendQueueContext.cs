using System;
using System.Threading;

namespace ShootingHero.Networks
{
    public class RoomPacketSendQueueContext : ISendQueueContext
    {
        private readonly ArrayPoolBufferWriter bufferWriter = null;
        private readonly ArraySegment<byte> data;
        private int remainingReferenceCount = 0;
        private int isDisposed = 0;

        public RoomPacketSendQueueContext(PacketSerializer packetSerializer, IPacket packet, int referenceCount)
        {
            if (referenceCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(referenceCount));

            bufferWriter = packetSerializer.Serialize(packet);
            data = bufferWriter.WrittenSegment;
            remainingReferenceCount = referenceCount;
        }

        public ArraySegment<byte> GetData()
        {
            if (Volatile.Read(ref isDisposed) != 0)
                throw new ObjectDisposedException(nameof(RoomPacketSendQueueContext));

            return data;
        }

        public void AddReference()
        {
            if (Volatile.Read(ref isDisposed) != 0)
                return;
            
            Interlocked.Increment(ref remainingReferenceCount);
        }

        public void Dispose()
        {
            int current = Interlocked.Decrement(ref remainingReferenceCount);
            if (current > 0)
                return;

            if (Interlocked.Exchange(ref isDisposed, 1) != 0)
                return;

            bufferWriter.Dispose();
        }
    }
}
