using MemoryPack;
using ShootingHero.Networks;

namespace ShootingHero.Shared
{
    [Packet((ushort)EPacketType.C2S_ReloadWeaponPacket)]
    [MemoryPackable]
    public partial class C2S_ReloadWeaponPacket : IPacket
    {
    }
}