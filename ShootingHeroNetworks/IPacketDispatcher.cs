namespace ShootingHero.Networks
{
    public interface IPacketDispatcher
    {
        void Dispatch(Session session, IPacket packet);
    }
}