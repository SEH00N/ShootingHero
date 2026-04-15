using MemoryPack;
using ShootingHero.Networks;

namespace ShootingHero.Shared
{
    [Packet((ushort)EPacketType.S2C_TestPacket)]
    [MemoryPackable]
    public partial class S2C_TestPacket : IPacket
    {
        public string Message { get; set; }
    }
}