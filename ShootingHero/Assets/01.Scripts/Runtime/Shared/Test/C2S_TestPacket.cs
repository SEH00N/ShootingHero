using MemoryPack;
using ShootingHero.Networks;

namespace ShootingHero.Shared
{
    [Packet((ushort)EPacketType.C2S_TestPacket)]
    [MemoryPackable]
    public partial class C2S_TestPacket : IPacket
    {
        public string Message { get; set; }
    }
}