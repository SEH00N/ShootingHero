using MemoryPack;
using ShootingHero.Networks;

namespace ShootingHero.Shared
{
    [Packet((ushort)EPacketType.S2C_UnitDeadPacket)]
    [MemoryPackable]
    public partial class S2C_UnitDeadPacket : IPacket
    {
        public string PlayerID { get; set; }
    }
}