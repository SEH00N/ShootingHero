namespace ShootingHero.Networks
{
    public class ClientBuilder : NetworkObjectBuilder<Client>
    {
        public ClientBuilder(Session session) : base()
        {
            AddSingleton<Session>(session);
        }

        protected override Client OnBuild()
        {
            return new Client(this);
        }
    }
}