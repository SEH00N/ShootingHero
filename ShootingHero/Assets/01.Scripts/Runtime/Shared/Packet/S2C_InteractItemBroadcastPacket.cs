using MemoryPack;
using ShootingHero.Networks;

namespace ShootingHero.Shared
{
    [Packet((ushort)EPacketType.S2C_InteractItemBroadcastPacket)]
    [MemoryPackable]
    public partial class S2C_InteractItemBroadcastPacket : IPacket
    {
        public string PlayerID { get; set; }
        public string ItemUUID { get; set; }
    }
}