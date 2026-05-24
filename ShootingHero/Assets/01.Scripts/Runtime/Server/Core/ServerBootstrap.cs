using System;
using Cysharp.Threading.Tasks;
using ShootingHero.LobbyServer;
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

        private async void Start()
        {
            GameInstance.PlayMode = EPlayMode.Server;
            GameInstance.DataTableManager = dataTableManager;
            ServerInstance.ServerDataTableManager = serverDataTableManager;

            gameManager.Initialize();

            GetServerCommandLineArguments getServerCommandLineArguments = new GetServerCommandLineArguments(Environment.GetCommandLineArgs());
            if(getServerCommandLineArguments.IsValid == false)
                return;
            
            await StartServer(getServerCommandLineArguments.Port);
            await BroadcastServerReady(getServerCommandLineArguments.UUID);
        }

        private async UniTask StartServer(int port)
        {
            GameServer gameServer = new GameServer();
            gameServer.Initialize(dataTableManager, serverDataTableManager, gameManager);
            gameServer.Listen(port);

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

        private async UniTask BroadcastServerReady(string gameUUID)
        {
            GameInstanceReadyRequest request = new GameInstanceReadyRequest() { GameUUID = gameUUID };
            await new LobbyGameRequest<GameInstanceReadyRequest, GameInstanceReadyResponse>(request).RequestAsync();
        }
    }
}
