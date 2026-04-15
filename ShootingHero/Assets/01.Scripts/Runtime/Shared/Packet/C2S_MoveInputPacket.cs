using MemoryPack;
using ShootingHero.Networks;
using UnityEngine;

namespace ShootingHero.Shared
{
    [Packet(5)]
    [MemoryPackable]
    public partial class C2S_MoveInputPacket : IPacket
    {
        public Vector2 MoveInput { get; set; }
    }
}