using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace ShootingHero.LobbyServer
{
    public sealed class GameInstanceShutdownService : IHostedService
    {
        private readonly GameInstanceManager gameInstanceManager;

        public GameInstanceShutdownService(GameInstanceManager gameInstanceManager)
        {
            this.gameInstanceManager = gameInstanceManager;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return gameInstanceManager.StopAllAsync();
        }
    }
}
