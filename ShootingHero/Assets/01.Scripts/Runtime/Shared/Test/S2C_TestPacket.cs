using MemoryPack;
using ShootingHero.Networks;

namespace ShootingHero.Shared
{
    [Packet(0)]
    [MemoryPackable]
    public partial class S2C_TestPacket : IPacket
    {
        public string Message { get; set; }
    }
}