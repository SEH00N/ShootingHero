namespace ShootingHero.Networks
{
    public class ServerBuilder : NetworkObjectBuilder<Server>
    {
        public ServerBuilder(ISessionFactory sessionFactory, ISessionDispatcher sessionDispatcher, IPacketDispatcher packetDispatcher) : base()
        {
            AddSingleton<ISessionFactory>(sessionFactory);
            AddSingleton<ISessionDispatcher>(sessionDispatcher);
            AddSingleton<IPacketDispatcher>(packetDispatcher);
            AddSingleton<IRoomManager>(new RoomManager());
        }

        protected override Server OnBuild()
        {
            return new Server(this);
        }
    }
}