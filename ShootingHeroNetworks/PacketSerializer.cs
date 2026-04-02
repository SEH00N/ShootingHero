using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using MemoryPack;

namespace ShootingHero.Networks
{
    public partial class PacketSerializer
    {
        private Dictionary<Type, ushort> packetIDMap = null;
        private Dictionary<ushort, Func<ArraySegment<byte>, IPacket>> factories = null;

        private PacketSerializer()
        {
        }

        public ArrayPoolBufferWriter Serialize(IPacket packet)
        {
            if (packet == null)
                throw new ArgumentNullException(nameof(packet));

            Type packetType = packet.GetType();
            if(packetIDMap.TryGetValue(packetType, out ushort packetID) == false)
                throw new InvalidOperationException($"{packetType.FullName} PacketID not found.");

            ArrayPoolBufferWriter writer = new ArrayPoolBufferWriter();

            try
            {
                Span<byte> packetSizeSpan = writer.GetSpan(NetworkDefine.PACKET_SIZE_HEADER);
                BinaryPrimitives.WriteUInt16LittleEndian(packetSizeSpan, 0);
                writer.Advance(NetworkDefine.PACKET_SIZE_HEADER);

                Span<byte> packetIdSpan = writer.GetSpan(NetworkDefine.PACKET_ID_HEADER);
                BinaryPrimitives.WriteUInt16LittleEndian(packetIdSpan, packetID);
                writer.Advance(NetworkDefine.PACKET_ID_HEADER);

                MemoryPackSerializer.Serialize(packetType, writer, packet);

                int packetSize = writer.WrittenCount;
                if (packetSize > NetworkDefine.PACKET_MAX_SIZE)
                    throw new InvalidOperationException($"Packet is too large. Size: {packetSize}, Max: {NetworkDefine.PACKET_MAX_SIZE}");

                ArraySegment<byte> packetSegment = writer.WrittenSegment;
                BinaryPrimitives.WriteUInt16LittleEndian(packetSegment.AsSpan(0, NetworkDefine.PACKET_SIZE_HEADER), (ushort)packetSize);

                return writer;
            }
            catch
            {
                writer.Dispose();
                throw;
            }
        }

        public IPacket Deserialize(ArraySegment<byte> packetData)
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