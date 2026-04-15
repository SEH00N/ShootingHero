using UnityEngine;
using MemoryPack;
using ShootingHero.Networks;

namespace ShootingHero.Shared
{
    [Packet((ushort)EPacketType.S2C_EnterGameBroadcastPacket)]
    [MemoryPackable]
    public partial class S2C_EnterGameBroadcastPacket : IPacket
    {
        public string PlayerID { get; set; }
        public Vector2 Position { get; set; }
    }
}