using System;
using System.Collections.Generic;

namespace ShootingHero.Networks
{
    public partial class PacketFactory
    {
        private Dictionary<ushort, Func<ArraySegment<byte>, IPacket>> factories = null;

        private PacketFactory()
        {
        }

        public IPacket Create(ArraySegment<byte> packetData)
        {
            if(packetData.Count < NetworkDefine.PACKET_ID_HEADER)
                return null;

            ushort packetID = BitConverter.ToUInt16(packetData.Array, packetData.Offset);
            if(factories.TryGetValue(packetID, out Func<ArraySegment<byte>, IPacket> factory) == false)
                return null;
            
            IPacket packet = factory(packetData);
            return packet;
        }
    }
}