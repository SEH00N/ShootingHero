using MemoryPack;
using ShootingHero.Networks;
using UnityEngine;

namespace ShootingHero.Shared
{
    [Packet(6)]
    [MemoryPackable]
    public partial class S2C_MoveInputBroadcastPacket : IPacket
    {
        public string PlayerID { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 MoveInput { get; set; }
    }
}