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

        [SerializeField]
        private ServerDataTableManager serverDataTableManager = null;

        private void Awake()
        {
            gameManager.Initialize();
            GameInstance.DataTableManager = dataTableManager;
        }

        public void AddServer()
        {
            GameInstance.PlayMode = EPlayMode.Server;

            GameServer gameServer = new GameObject("GameServer").AddComponent<GameServer>();
            DontDestroyOnLoad(gameServer.gameObject);

            UnityPacketDispatcher unityPacketDispatcher = gameServer.gameObject.AddComponent<UnityPacketDispatcher>();
            gameServer.Initialize(dataTableManager, serverDataTableManager, unityPacketDispatcher);

            unityPacketDispatcher.Initialize(gameServer.Server);
            gameServer.Listen(9999);

            SceneManager.LoadSceneAsync("Game", LoadSceneMode.Single);
        }

        public void StartGame()
        {
            GameInstance.PlayMode = EPlayMode.Client;

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
