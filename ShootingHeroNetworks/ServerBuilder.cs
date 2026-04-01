namespace ShootingHero.Networks
{
    public class ServerBuilder : NetworkObjectBuilder<Server>
    {
        public ServerBuilder(ISessionFactory sessionFactory, ISessionDispatcher sessionDispatcher) : base()
        {
            AddSingleton<ISessionFactory>(sessionFactory);
            AddSingleton<ISessionDispatcher>(sessionDispatcher);
        }

        protected override Server OnBuild()
        {
            return new Server(this);
        }
    }
}