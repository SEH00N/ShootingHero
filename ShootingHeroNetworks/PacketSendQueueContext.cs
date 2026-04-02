using System;
using System.Threading;

namespace ShootingHero.Networks
{
    public class PacketSendQueueContext : ISendQueueContext
    {
        private readonly ArrayPoolBufferWriter bufferWriter = null;
        private readonly ArraySegment<byte> data;
        private int isDisposed = 0;

        public PacketSendQueueContext(PacketSerializer packetSerializer, IPacket packet)
        {
            bufferWriter = packetSerializer.Serialize(packet);
            data = bufferWriter.WrittenSegment;
        }

        public ArraySegment<byte> GetData()
        {
            if (Volatile.Read(ref isDisposed) != 0)
                throw new ObjectDisposedException(nameof(PacketSendQueueContext));

            return data;
        }

        public void Dispose()
        {
            if (Interlocked.Exchange(ref isDisposed, 1) != 0)
                return;

            bufferWriter.Dispose();
        }
    }
}
