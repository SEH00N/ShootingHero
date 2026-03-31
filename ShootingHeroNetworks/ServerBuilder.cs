namespace ShootingHero.Networks
{
    public class ServerBuilder : NetworkObjectBuilder<Server>
    {
        public ServerBuilder(ISessionFactory sessionFactory, ISessionDispatcher sessionDispatcher, IPacketDispatcher packetDispatcher) : base()
        {
            AddSingleton(sessionFactory);
            AddSingleton(sessionDispatcher);
            AddSingleton(packetDispatcher);
        }

        protected override Server OnBuild()
        {
            return new Server(this);
        }
    }
}