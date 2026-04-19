using System.Collections.Generic;
using MemoryPack;
using ShootingHero.Networks;

namespace ShootingHero.Shared
{
    [Packet((ushort)EPacketType.S2C_EnterGameResponsePacket)]
    [MemoryPackable]
    public partial class S2C_EnterGameResponsePacket : IPacket
    {
        public string PlayerID { get; set; }
        public Dictionary<string, UnitDataDTO> Players { get; set; }
        public Dictionary<string, ItemDataDTO> Items { get; set; }
    }
}