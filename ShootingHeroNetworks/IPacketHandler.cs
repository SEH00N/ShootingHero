namespace ShootingHero.Networks
{
    public interface IPacketHandler<TPacket> : IPacketHandlerBase where TPacket : class, IPacket
    {
        protected void HandlePacket(Session session, TPacket packet);
        void IPacketHandlerBase.HandlePacket(Session session, IPacket packet)
        {
            HandlePacket(session, packet as TPacket);
        }
    }
}