using MemoryPack;
using ShootingHero.Networks;

namespace ShootingHero.Shared
{
    [Packet((ushort)EPacketType.C2S_InteractItemPacket)]
    [MemoryPackable]
    public partial class C2S_InteractItemPacket : IPacket
    {
        public string ItemUUID { get; set; }
    }
}