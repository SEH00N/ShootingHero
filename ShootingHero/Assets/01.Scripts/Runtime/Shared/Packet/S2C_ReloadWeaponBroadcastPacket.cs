using MemoryPack;
using ShootingHero.Networks;

namespace ShootingHero.Shared
{
    [Packet((ushort)EPacketType.S2C_ReloadWeaponBroadcastPacket)]
    [MemoryPackable]
    public partial class S2C_ReloadWeaponBroadcastPacket : IPacket
    {
        public string PlayerID { get; set; }
    }
}