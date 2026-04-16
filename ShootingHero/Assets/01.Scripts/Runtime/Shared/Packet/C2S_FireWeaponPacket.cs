using MemoryPack;
using ShootingHero.Networks;
using UnityEngine;

namespace ShootingHero.Shared
{
    [Packet((ushort)EPacketType.C2S_FireWeaponPacket)]
    [MemoryPackable]
    public partial class C2S_FireWeaponPacket : IPacket
    {
        public Vector2 Direction { get; set; }
    }
}