namespace ShootingHero.Networks
{
    public interface IPacketHandlerBase
    {
        void HandlePacket(Session session, IPacket packet);
    }
}