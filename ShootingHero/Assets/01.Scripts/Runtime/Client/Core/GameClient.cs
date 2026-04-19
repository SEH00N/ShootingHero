using ShootingHero.Networks;
using ShootingHero.Shared;

namespace ShootingHero.Clients
{
    public class GameClient
    {
        private Session session = null;
        private Client client = null;

        public void Intialize(DataTableManager dataTableManager, GameManager gameManager)
        {
            GameInstance.PlayMode = EPlayMode.Client;
            GameInstance.DataTableManager = dataTableManager;
            ClientInstance.GameClient = this;

            session = new Session();
            session.OnOpenedEvent += session => {
                session.SendAsync(new C2S_EnterGameRequestPacket());
            };

            UnityPacketDispatcher unityPacketDispatcher = gameManager.gameObject.AddComponent<UnityPacketDispatcher>();
            client = new ClientBuilder(session, unityPacketDispatcher)
                .AddSingleton<GameClient>(this)
                .AddSingleton<GameManager>(gameManager)
                .AddSingleton<DataTableManager>(dataTableManager)
                .Build(typeof(ClientInstance).Assembly, typeof(GameInstance).Assembly);

            unityPacketDispatcher.Initialize(client);
        }

        public void Connect(string ip, int port)
        {
            client.Connect(ip, port);
        }

        public void Send(IPacket packet)
        {
            session.SendAsync(packet);
        }
    }
}