using ShootingHero.Networks;
using ShootingHero.Servers;
using ShootingHero.Shared;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ShootingHero.Clients
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField]
        private GameManager gameManager = null;

        [SerializeField]
        private DataTableManager dataTableManager = null;

        private void Awake()
        {
            gameManager.Initialize();
        }

        public void AddServer()
        {
            GameServer gameServer = new GameObject("GameServer").AddComponent<GameServer>();
            DontDestroyOnLoad(gameServer.gameObject);

            UnityPacketDispatcher unityPacketDispatcher = gameServer.gameObject.AddComponent<UnityPacketDispatcher>();
            gameServer.Initialize(dataTableManager, unityPacketDispatcher);

            unityPacketDispatcher.Initialize(gameServer.Server);
            gameServer.Listen(9999);

            SceneManager.LoadSceneAsync("Game", LoadSceneMode.Single);
        }

        public void StartGame()
        {
            Session session = new Session();
            session.OnOpenedEvent += session => {
                session.SendAsync(new C2S_EnterGameRequestPacket());
                gameManager.SetSession(session);
            };

            UnityPacketDispatcher unityPacketDispatcher = gameManager.gameObject.AddComponent<UnityPacketDispatcher>();
            Client client = new ClientBuilder(session, unityPacketDispatcher)
                .AddSingleton<GameManager>(gameManager)
                .AddSingleton<DataTableManager>(dataTableManager)
                .Build(typeof(Bootstrap).Assembly, typeof(GameDefine).Assembly);

            unityPacketDispatcher.Initialize(client);
            client.Connect("127.0.0.1", 9999);
        }
    }
}
