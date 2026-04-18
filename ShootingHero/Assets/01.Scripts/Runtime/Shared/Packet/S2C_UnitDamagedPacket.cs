using MemoryPack;
using ShootingHero.Networks;
using UnityEngine;

namespace ShootingHero.Shared
{
    [Packet((ushort)EPacketType.S2C_UnitDamagedPacket)]
    [MemoryPackable]
    public partial class S2C_UnitDamagedPacket : IPacket
    {
        public string PlayerID { get; set; }
        public int Damage { get; set; }
    }
}