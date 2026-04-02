using System.Threading.Tasks;

namespace ShootingHero.Networks
{
    public interface IPacketDispatcher
    {
        ValueTask Dispatch(Session session, IPacket packet);
    }
}