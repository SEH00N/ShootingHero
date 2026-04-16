using MemoryPack;
using ShootingHero.Networks;
using UnityEngine;

namespace ShootingHero.Shared
{
    [Packet((ushort)EPacketType.S2C_SpawnItemPacket)]
    [MemoryPackable]
    public partial class S2C_SpawnItemPacket : IPacket
    {
        public int ItemID { get; set; }
        public string ItemUUID { get; set; }
        public Vector2 Position { get; set; }
    }
}