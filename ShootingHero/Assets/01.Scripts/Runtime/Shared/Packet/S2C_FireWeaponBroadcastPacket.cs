using MemoryPack;
using ShootingHero.Networks;
using UnityEngine;

namespace ShootingHero.Shared
{
    [Packet((ushort)EPacketType.S2C_FireWeaponBroadcastPacket)]
    [MemoryPackable]
    public partial class S2C_FireWeaponBroadcastPacket : IPacket
    {
        public string PlayerID { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Direction { get; set; }
    }
}