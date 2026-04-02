using System;
using MemoryPack;

namespace ShootingHero.Networks
{
    public class PacketSendQueueContext : ISendQueueContext
    {
        public PacketSendQueueContext(IPacket packet)
        {
            MemoryPackSerializer.Serialize(packet.GetType());
        }

        public ArraySegment<byte> GetData()
        {
            
        }

        public void Dispose()
        {
            
        }
    }
}