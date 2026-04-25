using ShootingHero.Shared;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ShootingHero.Servers
{
    public class ServerBootstrap : MonoBehaviour
    {
        [SerializeField]
        private GameManager gameManager = null;

        [SerializeField]
        private DataTableManager dataTableManager = null;

        [SerializeField]
        private ServerDataTableManager serverDataTableManager = null;

        public async void StartServer()
        {
            gameManager.Initialize();
            
            GameServer gameServer = new GameServer();
            gameServer.Initialize(dataTableManager, serverDataTableManager, gameManager);
            gameServer.Listen(9999);

            await SceneManager.LoadSceneAsync("Game", LoadSceneMode.Single);

            ItemSpawnManager itemSpawnManager = new ItemSpawnManager(
                dataTableManager.gameConfigTable, 
                dataTableManager.itemTable, 
                serverDataTableManager.itemSpawnPositionTable, 
                gameServer, 
                gameManager
            );
            itemSpawnManager.Initialize();
        }
    }
}
