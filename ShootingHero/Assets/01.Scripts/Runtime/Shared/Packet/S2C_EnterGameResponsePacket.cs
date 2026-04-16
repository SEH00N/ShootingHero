using System.Collections.Generic;
using MemoryPack;
using ShootingHero.Networks;
using UnityEngine;

namespace ShootingHero.Shared
{
    [Packet((ushort)EPacketType.S2C_EnterGameResponsePacket)]
    [MemoryPackable]
    public partial class S2C_EnterGameResponsePacket : IPacket
    {
        public string PlayerID { get; set; }
        public Dictionary<string, Vector2> Players { get; set; }
        public Dictionary<string, (int, Vector2)> Items { get; set; }
    }
}