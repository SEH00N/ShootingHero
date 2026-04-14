using MemoryPack;
using ShootingHero.Networks;

namespace ShootingHero.Shared
{
    [Packet(2)]
    [MemoryPackable]
    public partial class C2S_EnterGameRequestPacket : IPacket
    {
        
    }
}