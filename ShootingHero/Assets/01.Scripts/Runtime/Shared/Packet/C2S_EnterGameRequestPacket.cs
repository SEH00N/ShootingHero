using MemoryPack;
using ShootingHero.Networks;

namespace ShootingHero.Shared
{
    [Packet((ushort)EPacketType.C2S_EnterGameRequestPacket)]
    [MemoryPackable]
    public partial class C2S_EnterGameRequestPacket : IPacket
    {
        
    }
}