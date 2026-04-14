using UnityEngine;
using MemoryPack;
using ShootingHero.Networks;

namespace ShootingHero.Shared
{
    [Packet(4)]
    [MemoryPackable]
    public partial class S2C_EnterGameBroadcastPacket : IPacket
    {
        public string PlayerID { get; set; }
        public Vector2 Position { get; set; }
    }
}