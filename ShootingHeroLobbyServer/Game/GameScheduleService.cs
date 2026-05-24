using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace ShootingHero.LobbyServer
{
    public class GameScheduleService : BackgroundService
    {
        private readonly GameInstanceManager gameInstanceManager = null;
        private readonly IPortQueue portQueue = null;
        private readonly IGameInstanceLauncher gameInstanceLauncher = null;

        public GameScheduleService(GameInstanceManager gameInstanceManager, IPortQueue portQueue, IGameInstanceLauncher gameInstanceLauncher)
        {
            this.gameInstanceManager = gameInstanceManager;
            this.portQueue = portQueue;
            this.gameInstanceLauncher = gameInstanceLauncher;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (PeriodicTimer timer = new PeriodicTimer(TimeSpan.FromSeconds(0.5f)))
            {
                while (await timer.WaitForNextTickAsync(stoppingToken))
                {
                    await ScheduleGame();
                }
            }
        }

        private async Task ScheduleGame()
        {
            List<string> gamePublishQueue = await gameInstanceManager.FlushGamePublishQueueAsync();
            foreach(string gameUUID in gamePublishQueue)
            {
                int port = await portQueue.RentAsync();
                if(port == -1)
                {
                    gameInstanceManager.UnregisterGame(gameUUID);
                    continue;
                }

                GameInstanceHandle gameInstanceHandle = await gameInstanceLauncher.LaunchAsync(gameUUID, port);
                if(gameInstanceHandle == null)
                {
                    gameInstanceManager.UnregisterGame(gameUUID);
                    continue;
                }
                
                gameInstanceManager.RegisterGame(gameUUID, gameInstanceHandle);
            }
        }
    }
}