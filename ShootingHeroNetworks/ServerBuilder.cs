namespace ShootingHero.Networks
{
    public class ServerBuilder : NetworkObjectBuilder<Server>
    {
        public ServerBuilder(ISessionFactory sessionFactory) : base()
        {
            AddSingleton<ISessionFactory>(sessionFactory);

            RoomManager roomManager = new RoomManager(diContainer);
            AddSingleton<ISessionDispatcher>(roomManager);
            AddSingleton<IPacketDispatcher>(roomManager);
            AddSingleton<IRoomManager>(roomManager);
        }

        protected override Server OnBuild()
        {
            return new Server(this);
        }
    }
}