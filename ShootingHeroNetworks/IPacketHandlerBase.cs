using System.Threading.Tasks;

namespace ShootingHero.Networks
{
    public interface IPacketHandlerBase
    {
        ValueTask HandlePacket(Session session, IPacket packet);
    }
}