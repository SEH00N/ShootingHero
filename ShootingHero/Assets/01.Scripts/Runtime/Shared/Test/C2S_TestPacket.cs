using MemoryPack;
using ShootingHero.Networks;

namespace ShootingHero.Shared
{
    [Packet(1)]
    [MemoryPackable]
    public partial class C2S_TestPacket : IPacket
    {
        public string Message { get; set; }
    }
}