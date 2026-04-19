using MemoryPack;
using ShootingHero.Networks;
using UnityEngine;

namespace ShootingHero.Shared
{
    [Packet((ushort)EPacketType.S2C_UnitRespawnPacket)]
    [MemoryPackable]
    public partial class S2C_UnitRespawnPacket : IPacket
    {
        public string PlayerID { get; set; }
        public Vector2 Position { get; set; }
        public int Height { get; set; }
    }
}