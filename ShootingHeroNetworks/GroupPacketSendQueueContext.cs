using System;
using System.Threading;

namespace ShootingHero.Networks
{
    public class GroupPacketSendQueueContext : ISendQueueContext
    {
        private readonly ArrayPoolBufferWriter bufferWriter = null;
        private readonly ArraySegment<byte> data;
        private int remainingReferenceCount = 0;

        public GroupPacketSendQueueContext(IPacket packet, int referenceCount)
        {
            if (referenceCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(referenceCount));

            bufferWriter = PacketSerializer.Serialize(packet);
            data = bufferWriter.WrittenSegment;
            remainingReferenceCount = referenceCount;
        }

        public ArraySegment<byte> GetData()
        {
            if (Volatile.Read(ref remainingReferenceCount) <= 0)
                throw new ObjectDisposedException(nameof(GroupPacketSendQueueContext));

            return data;
        }

        public void Dispose()
        {
            int current = Interlocked.Decrement(ref remainingReferenceCount);
            if (current == 0)
                bufferWriter.Dispose();
        }
    }
}
