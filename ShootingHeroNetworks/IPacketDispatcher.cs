namespace ShootingHero.Networks
{
    public interface IPacketDispatcher
    {
        void Dispatch(IPacket packet);
    }
}