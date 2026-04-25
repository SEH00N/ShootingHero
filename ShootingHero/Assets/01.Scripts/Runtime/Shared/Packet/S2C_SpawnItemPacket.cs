using MemoryPack;
using ShootingHero.Networks;

namespace ShootingHero.Shared
{
    [Packet((ushort)EPacketType.S2C_SpawnItemPacket)]
    [MemoryPackable]
    public partial class S2C_SpawnItemPacket : IPacket
    {
        public string ItemUUID { get; set; }
        public ItemDataDTO ItemData { get; set; }
    }
}